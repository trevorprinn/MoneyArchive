using MoneyArchiveDb;
using MoneyArchiveDb.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MoneyArchiveApp {
    public partial class FormMain : Form {

        readonly ArchiveDb _db;

        readonly Settings _settings;

        public FormMain() {
            InitializeComponent();

            _settings = Settings.Load();
            var db = getDb(_settings.QifFolder);
            if (db == null) Close();
            _db = db!;
        }

        ArchiveDb? getDb(string? folder) {
            ArchiveDb? db = null;
            if (!string.IsNullOrWhiteSpace(folder) && Directory.Exists(folder)) {
                try {
                    db = DbLoader.LoadQifDirectory(folder);
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Failed to load", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (db != null) return db;
            dlgFolder.SelectedPath = _settings.QifFolder;
            if (dlgFolder.ShowDialog() == DialogResult.OK) {
                try {
                    db = DbLoader.LoadQifDirectory(dlgFolder.SelectedPath);
                    _settings.QifFolder = dlgFolder.SelectedPath;
                    _settings.Save();
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Failed to load", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return db;
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            gridTransactions.AutoGenerateColumns = false;
            loadUI();
        }

        void loadUI() {
            gridTransactions.DataSource = null;
            listAccounts.DataSource = _db.Accounts.OrderBy(a => a.Name).Select(a => new AccountItem(a)).ToArray();
        }

        class AccountItem {
            public Account Account { get; private set; }
            public override string ToString() => Account.Name;
            public AccountItem(Account account) {
                Account = account;
            }
        }

        private void listAccounts_SelectedIndexChanged(object sender, EventArgs e) {
            Account? account = (listAccounts.SelectedItem as AccountItem)?.Account;
            if (account == null) return;
            loadTransactions(account.Transactions);
        }

        TransactionItem[]? _currentTransactions;

        void loadTransactions(IEnumerable<Transaction> transactions) {
            var cursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try {
                gridTransactions.DataSource = null;
                _currentTransactions = null;
                textSearch.Text = null;
                _currentTransactions = transactions.OrderBy(t => t.Date).Select(t => new TransactionItem(t)).SetRunningTotals();
                gridTransactions.DataSource = _currentTransactions;
                gridTransactions.AutoResizeColumns();
                gridTransactions.Focus();
            } finally {
                Cursor.Current = cursor;
            }
        }

        private void gridTransactions_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e) {
            e.ToolTipText = null;
            if (e.RowIndex < 0 || e.RowIndex >= gridTransactions.Rows.Count) return;
            var trans = gridTransactions.Rows[e.RowIndex]?.DataBoundItem as TransactionItem;
            if (!(trans?.Transaction.Splits?.Any() ?? false)) return;
            var splits = trans.Transaction.Splits;
            var lengths = new int[] { splits.Max(s => s.Category?.Value?.Length ?? 0), splits.Max(s => s.Amount.ToString().Length), splits.Max(s => s.Memo?.Length ?? 0) };
            var texts = splits.Select(s => $"{(s.Category?.Value ?? "").PadRight(lengths[0])} | {s.Amount.ToString().PadLeft(lengths[1])} | {(s.Memo ?? "").PadRight(lengths[2])}");
            e.ToolTipText = string.Join(Environment.NewLine, texts);
        }

        private void textSearch_TextChanged(object sender, EventArgs e) {
            if (_currentTransactions == null) return;
            var text = textSearch.Text?.ToLower();
            if (string.IsNullOrEmpty(text)) {
                gridTransactions.DataSource = _currentTransactions;
            } else {
                gridTransactions.DataSource = _currentTransactions.Where(t => t.ContainsText(text)).ToArray();
            }
        }
    }

    class TransactionItem {
        public Transaction Transaction { get; private set; }
        public DateTime Date => Transaction.Date;
        public string? Payee => Transaction.Payee?.Name;
        public decimal Amount => Transaction.Amount;
        public string? Transfer => Transaction.TransferAccount?.Name;
        public string? Memo => Transaction.Memo;
        public int? ChequeNumber => Transaction.ChequeNumber;
        public string? Category => Transaction.Category?.Value;
        public decimal RunningTotal { get; set; }
        public bool HasSplit => Transaction.Splits?.Any() ?? false;

        public bool ContainsText(string text) =>
            (Transaction.Payee?.Name.ToLower().Contains(text) ?? false)
            || (Transaction.Memo?.ToLower().Contains(text) ?? false);

        public TransactionItem(Transaction transaction) {
            Transaction = transaction;
        }
    }

    static class TransactionItemExtensions {
        public static TransactionItem[] SetRunningTotals(this IEnumerable<TransactionItem> items) {
            decimal tot = 0m;
            var aitems = items.ToArray();
            foreach (var item in aitems) {
                tot += item.Amount;
                item.RunningTotal = tot;
            }
            return aitems;
        }
    }
}

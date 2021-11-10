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

        internal enum SelectionTypes {
            Accounts = 0, Payees = 1, Categories = 2
        }

        public FormMain() {
            InitializeComponent();

            _settings = Settings.Load();
            var db = getDb(_settings.QifFolder);
            if (db == null) Close();
            _db = db!;

            cboListType.Items.AddRange(Enum.GetNames(typeof(SelectionTypes)));
            cboListType.SelectedIndex = (int)SelectionTypes.Accounts;
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

        class AccountItem {
            public Account Account { get; private set; }
            public override string ToString() => Account.Name;
            public AccountItem(Account account) {
                Account = account;
            }
        }

        class CategoryItem {
            public Category Category { get; private set; }
            override public string ToString() => Category.Value;
            public CategoryItem(Category category) {
                Category = category;
            }
        }

        class PayeeItem {
            public Payee Payee { get; private set; }
            public override string ToString() => Payee.Name;
            public PayeeItem(Payee payee) {
                Payee = payee;
            }
        }

        private void listSelection_SelectedIndexChanged(object sender, EventArgs e) {
            switch (selType) {
                case SelectionTypes.Accounts:
                    Account? account = (listSelection.SelectedItem as AccountItem)?.Account;
                    if (account == null) return;
                    loadTransactions(account.Transactions);
                    break;
                case SelectionTypes.Categories:
                    Category? category = (listSelection.SelectedItem as CategoryItem)?.Category;
                    if (category == null) return;
                    loadTransactions(category.Transactions);
                    break;
                case SelectionTypes.Payees:
                    Payee? payee = (listSelection.SelectedItem as PayeeItem)?.Payee;
                    if (payee == null) return;
                    loadTransactions(payee.Transactions);
                    break;
            }
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
                gridTransactions.AutoGenerateColumns = false;
                gridTransactions.Columns["Account"].Visible = selType != SelectionTypes.Accounts;
                gridTransactions.Columns["Payee"].Visible = selType != SelectionTypes.Payees;
                gridTransactions.Columns["Category"].Visible = selType != SelectionTypes.Categories;
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
                gridTransactions.DataSource = _currentTransactions.Where(t => t.ContainsText(selType, text)).ToArray();
            }
        }

        SelectionTypes selType => (SelectionTypes)cboListType.SelectedIndex;

        private void cboListType_SelectedIndexChanged(object sender, EventArgs e) {
            switch (selType) {
                case SelectionTypes.Accounts:
                    listSelection.DataSource = _db.Accounts.OrderBy(a => a.Name).Select(a => new AccountItem(a)).ToArray();
                    break;
                case SelectionTypes.Categories:
                    listSelection.DataSource = _db.Categories.OrderBy(a => a.Value).Select(c => new CategoryItem(c)).ToArray();
                    break;
                case SelectionTypes.Payees:
                    listSelection.DataSource = _db.Payees.OrderBy(p => p.Name).Select(p => new PayeeItem(p)).ToArray();
                    break;
                default:
                    listSelection.DataSource = null;
                    break;
            }
            if (listSelection.Items.Count > 0) listSelection.SelectedIndex = 0;
        }
    }

    class TransactionItem {
        public Transaction Transaction { get; private set; }
        public DateTime Date => Transaction.Date;
        public string? Payee => Transaction.Payee?.Name;
        public string? Account => Transaction.Account?.Name;
        public decimal Amount => Transaction.Amount;
        public string? Transfer => Transaction.TransferAccount?.Name;
        public string? Memo => Transaction.Memo;
        public int? ChequeNumber => Transaction.ChequeNumber;
        public string? Category => Transaction.Category?.Value;
        public decimal RunningTotal { get; set; }
        public bool HasSplit => Transaction.Splits?.Any() ?? false;

        public bool ContainsText(FormMain.SelectionTypes selType, string text) =>
            selType switch {
                FormMain.SelectionTypes.Accounts =>
                    (Transaction.Payee?.Name.ToLower().Contains(text) ?? false)
                    || (Transaction.Category?.Value.ToLower().Contains(text) ?? false)
                    || (Transaction.Memo?.ToLower().Contains(text) ?? false),
                FormMain.SelectionTypes.Categories =>
                    (Transaction.Payee?.Name.ToLower().Contains(text) ?? false)
                    || (Transaction.Account?.Name.ToLower().Contains(text) ?? false)
                    || (Transaction.Memo?.ToLower().Contains(text) ?? false),
                FormMain.SelectionTypes.Payees =>
                    (Transaction.Account?.Name.ToLower().Contains(text) ?? false)
                    || (Transaction.Category?.Value.ToLower().Contains(text) ?? false)
                    || (Transaction.Memo?.ToLower().Contains(text) ?? false),
                _ => false
            };

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

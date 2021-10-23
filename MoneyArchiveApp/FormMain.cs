using MoneyArchiveDb;
using MoneyArchiveDb.Database;
using System;
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
            gridTransactions.DataSource = account.Transactions.OrderBy(t => t.Date).Select(t => new TransactionItem(t)).ToArray();
        }

        class TransactionItem {
            public Transaction Transaction {  get; private set; }
            public DateTime Date => Transaction.Date;
            public string? Payee => Transaction.Payee?.Name;
            public decimal Amount => Transaction.Amount;
            public string? Transfer => Transaction.TransferAccount?.Name;

            public TransactionItem(Transaction transaction) {
                Transaction = transaction;
            }

        }
    }
}

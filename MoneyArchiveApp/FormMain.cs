using Microsoft.EntityFrameworkCore;
using MoneyArchiveDb.Database;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace MoneyArchiveApp {
    public partial class FormMain : Form {

        string connString = "Data Source=10.17.23.2;Database=MoneyArchive;User Id=sa;Password=autoluck;MultipleActiveResultSets=true";

        public FormMain() {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            gridTransactions.AutoGenerateColumns = false;

            using var cx = new ArchiveContext(connString);
            listAccounts.DataSource = cx.Accounts.OrderBy(a => a.Name).Select(a => new AccountItem(a)).ToArray();
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
            using var cx = new ArchiveContext(connString);
            var data = cx.Transactions.Where(t => t.AccountId == account.Id)
                .OrderBy(t => t.Date)
                .Include(t => t.Payee).Include(t => t.Category).Include(t => t.TransferAccount)
                .Select(t => new TransactionItem(t))
                .ToArray();
            gridTransactions.DataSource = data;
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

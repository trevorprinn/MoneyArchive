using MoneyArchiveDb.Database;
using MoneyArchiveDb.QifImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyArchiveDb {
    public static class DbLoader {

        public static ArchiveDb Load(params QifFile[] qifs) {
            var db = new ArchiveDb {
                Accounts = qifs.Select(q => new Account(q.AccountName)).ToArray()
            };

            foreach (var qif in qifs) {
				var account = db.Accounts.Single(a => a.Name == qif.AccountName);
				foreach (var qt in qif.Transactions) {
					var sorder = 0;
                    var t = Transaction.Create();

                    t.Account = account;
                    t.AccountId = account.Id;
                    t.Account.Transactions.Add(t);

                    t.Amount = qt.Amount;

                    t.Category = getCat(qt.Category);
                    t.CategoryId = t.Category?.Id;
                    t.Category?.Transactions.Add(t);

                    t.ChequeNumber = qt.ChequeNumber;
                    t.Date = qt.Date;
                    t.Memo = qt.Memo;

                    t.Payee = getPayee(qt.Payee);
                    t.PayeeId = t.Payee?.Id;
                    t.Payee?.Transactions.Add(t);

                    t.Status = qt.Status == 0 ? ' ' : qt.Status;
                    t.Splits = qt.Splits?.Select(s => {
                        var cat = getCat(s.Category);
                        var tacc = !string.IsNullOrWhiteSpace(s.Transfer) ? db.Accounts.SingleOrDefault(a => a.Name == s.Transfer) : null;
                        var sp = new TransactionSplit {
                            Amount = s.Amount,
                            Category = cat,
                            CategoryId = cat?.Id,
                            TransferAccount = tacc,
                            TransferAccountId = tacc?.Id,
                            Memo = s.Memo,
                            Order = sorder++
                        };
                        return sp;
                    }).ToArray();

                    t.TransferAccount = !string.IsNullOrWhiteSpace(qt.Transfer) ? db.Accounts.SingleOrDefault(a => a.Name == qt.Transfer) : null;
                    t.TransferAccountId = t.TransferAccount?.Id;
                    t.TransferAccount?.TransferTransactions.Add(t);
                    
                    t.TransType = qt.TransType;

                    db.Transactions.Add(t);
				}
			}
            matchTransfers(db);
            return db;

			Payee getPayee(string n) {
				if (string.IsNullOrWhiteSpace(n)) return null;
                var payee = db.Payees.SingleOrDefault(p => p.Name == n);
                if (payee == null) db.Payees.Add(payee = new Payee(n));
				return payee;
			}
			Category getCat(string v) {
				if (string.IsNullOrWhiteSpace(v)) return null;
                var cat = db.Categories.SingleOrDefault(c => c.Value == v);
                if (cat == null) db.Categories.Add(cat = new Category(v));
				return cat;
			}
		}

		public static ArchiveDb Load(params string[] qifFilenames) {
			var qifs = qifFilenames.Select(f => QifFile.Load(f)).Where(f => f.Type != "Invst").ToArray();
			return Load(qifs);
		}

		public static ArchiveDb LoadQifDirectory(string qifDirectory) =>
			Load(Directory.GetFiles(Environment.ExpandEnvironmentVariables(qifDirectory), "*.qif"));

		static void matchTransfers(ArchiveDb db) {
			foreach (var account in db.Accounts) matchTransfers(account);
        }

		static void matchTransfers(Account account) {
			var transfers = account.Transactions.Where(t => t.TransferAccountId != null && t.TransferMatchId == null);
			foreach (var transfer in transfers) {
				var match = transfer.TransferAccount.Transactions.FirstOrDefault(m => m.Date == transfer.Date && m.TransferAccountId == account.Id && m.Amount == -transfer.Amount);
				if (match != null) {
					transfer.TransferMatch = match;
					match.TransferMatch = transfer;
                    transfer.TransferMatchId = match.Id;
                    match.TransferMatchId = transfer.Id;
                }
            }
		}
	}
}

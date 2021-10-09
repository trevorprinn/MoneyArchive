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

        public static async Task Load(string connectionString, params QifFile[] qifs) {
            using var cx = new ArchiveContext(connectionString);
            await cx.Database.EnsureCreatedAsync();

            if (cx.Accounts.Any() || cx.Categories.Any() || cx.Transactions.Any() || cx.Payees.Any()) throw new Exception("Database is not empty");

			var payees = new Dictionary<string, Payee>();
			var cats = new Dictionary<string, Category>();

			var accounts = qifs.Select(q => new Account { Name = q.AccountName }).ToArray();
			cx.Accounts.AddRange(accounts);

			var transactions = new List<Transaction>();

			foreach (var qif in qifs) {
				var account = accounts.Single(a => a.Name == qif.AccountName);
				foreach (var t in qif.Transactions) {
					var sorder = 0;
					transactions.Add(new Transaction {
						Account = account,
						Amount = t.Amount,
						Category = getCat(t.Category),
						ChequeNumber = t.ChequeNumber,
						Date = t.Date,
						Memo = t.Memo,
						Payee = getPayee(t.Payee),
						Status = t.Status == 0 ? ' ' : t.Status,
						Splits = t.Splits?.Select(s => new TransactionSplit {
							Amount = s.Amount,
							Category = getCat(s.Category),
							TransferAccount = !string.IsNullOrWhiteSpace(s.Transfer) ? accounts.SingleOrDefault(a => a.Name == s.Transfer) : null,
							Memo = s.Memo,
							Order = sorder++
						}).ToArray(),
						TransferAccount = !string.IsNullOrWhiteSpace(t.Transfer) ? accounts.SingleOrDefault(a => a.Name == t.Transfer) : null,
						TransType = t.TransType
					});
				}
			}
			cx.Transactions.AddRange(transactions);
			await cx.SaveChangesAsync();

			Payee getPayee(string n) {
				if (string.IsNullOrWhiteSpace(n)) return null;
				if (payees.TryGetValue(n, out Payee p)) return p;
				var payee = new Payee { Name = n };
				payees.Add(n, payee);
				return payee;
			}
			Category getCat(string v) {
				if (string.IsNullOrWhiteSpace(v)) return null;
				if (cats.TryGetValue(v, out Category c)) return c;
				var cat = new Category { Value = v };
				cats.Add(v, cat);
				return cat;
			}
		}

		public static Task Load(string connectionString, params string[] qifFilenames) {
			var qifs = qifFilenames.Select(f => QifFile.Load(f)).ToArray();
			return Load(connectionString, qifs);
		}

		public static Task LoadQifDirectory(string connectionString, string qifDirectory) =>
			Load(connectionString, Directory.GetFiles(qifDirectory, "*.qif"));
	}
}

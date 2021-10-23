using System.Collections.Generic;

namespace MoneyArchiveDb.Database {
    public class ArchiveDb {
        public Account[] Accounts { get; set; }

        public List<Category> Categories { get; set; } = new();

        public List<Payee> Payees { get; set; } = new();

        public List<Transaction> Transactions { get; set; } = new();
    }
}

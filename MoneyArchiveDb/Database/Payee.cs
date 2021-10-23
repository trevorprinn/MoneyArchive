using System.Collections.Generic;

namespace MoneyArchiveDb.Database {
    public class Payee {
        static int _nextId = 1;

        public Payee(string name) {
            Id = _nextId++;
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<Transaction> Transactions { get; set; } = new();
    }
}

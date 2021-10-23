using System.Collections.Generic;

namespace MoneyArchiveDb.Database {
    public class Category {
        static int _nextId = 1;

        public Category(string value) {
            Id = _nextId++;
            Value = value;
        }
        public int Id { get; set; }

        public string Value { get; set; }

        public List<Transaction> Transactions { get; set; } = new();
    }
}

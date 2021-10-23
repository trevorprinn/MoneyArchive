using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyArchiveDb.Database {
    public class Transaction {
        static int _nextId = 1;

        public static Transaction Create() => new() { Id = _nextId++ };

        public int Id { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public char Status { get; set; }
        public string Memo { get; set; }
        public int? ChequeNumber { get; set; }
        public string TransType { get; set; }

        public int? PayeeId { get; set; }
        public Payee Payee { get; set; }

        public int? TransferAccountId { get; set; }
        public Account TransferAccount { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public TransactionSplit[] Splits { get; set; }

        public int? TransferMatchId { get; set; }
        public Transaction TransferMatch { get; set; }
    }
}

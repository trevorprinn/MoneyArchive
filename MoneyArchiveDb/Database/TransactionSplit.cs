namespace MoneyArchiveDb.Database {
    
    public class TransactionSplit {
        public int Order { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public int? TransferAccountId { get; set; }
        public Account TransferAccount { get; set; }

        public string Memo { get; set; }

        public decimal Amount { get; set; }
    }
}

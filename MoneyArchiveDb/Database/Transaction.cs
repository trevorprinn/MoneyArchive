using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyArchiveDb.Database {
    [Table("Transaction")]
    [Index(nameof(AccountId), nameof(Date))]
    [Index(nameof(PayeeId), nameof(Date))]
    [Index(nameof(CategoryId), nameof(Date))]
    public class Transaction {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public char Status { get; set; }
        public string Memo { get; set; }
        public int? ChequeNumber { get; set; }
        public string TransType { get; set; }

        public int? PayeeId { get; set; }
        public virtual Payee Payee { get; set; }

        public int? TransferAccountId { get; set; }
        public virtual Account TransferAccount { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<TransactionSplit> Splits { get; set; }

        public int? TransferMatchId { get; set; }
        public virtual Transaction TransferMatch { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyArchiveDb.Database {
    [Table("TransactionSplit")]
    [Index(nameof(CategoryId))]
    public class TransactionSplit {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }

        public int Order { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int? TransferAccountId { get; set; }
        public virtual Account TransferAccount { get; set; }

        public string Memo { get; set; }

        public decimal Amount { get; set; }
    }
}

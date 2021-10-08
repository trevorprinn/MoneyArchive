using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyArchiveDb.Database {
    [Table("Transaction")]
    public class Transaction {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Date {  get; set; }
        public decimal Amount { get; set; }
        public char Status { get; set; }
        public string Memo { get; set; }
        public int? ChequeNumber { get; set; }
        public string TransType { get; set; }
    }
}

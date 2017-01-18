using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalTracker.Domain
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }
        public Account Account { get; set; }

        [Required]
        public int PayeeId { get; set; }
        public Payee Payee { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public Decimal Amount { get; set; }

        [MaxLength(100)]
        public string Reference { get; set; }

        [MaxLength(20)]
        public string Number { get; set; }

        [MaxLength(200)]
        public string Memo { get; set; }
    }
}

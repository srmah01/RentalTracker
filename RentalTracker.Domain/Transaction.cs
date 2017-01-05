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
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }

        public Account Account { get; set; }

        [Required]
        public int PayeeId { get; set; }

        public Payee Payee { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public DateTime Date { get; set; }

        public Decimal Amount { get; set; }

        public string Reference { get; set; }

        public string Number { get; set; }

        public string Memo { get; set; }
    }
}

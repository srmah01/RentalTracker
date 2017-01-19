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

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required, DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }

        // Amount is positive if it represents Income, or negative if it represents Expenditure
        [Required, DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{0:c}")]
        public Decimal Amount { get; set; }

        [MaxLength(30)]
        public string Reference { get; set; }

        [MaxLength(200)]
        public string Memo { get; set; }
    }
}

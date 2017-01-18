using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalTracker.Domain
{
    public class Account
    {
        public Account()
        {
           Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public String Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public Decimal OpeningBalance { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}

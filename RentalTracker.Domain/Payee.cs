using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalTracker.Domain
{
    public class Payee
    {
        public Payee()
        {
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int DefaultCategoryId { get; set; }

        [Display(Name = "Default Category")]
        public Category DefaultCategory { get; set; }

        [MaxLength(200)]
        public string Memo { get; set; }

        public virtual ICollection<Transaction> Transactions { get; private set; }
    }
}

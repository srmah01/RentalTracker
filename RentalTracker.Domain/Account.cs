﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [DisplayName("Opening Balance")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public Decimal OpeningBalance { get; set; }

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public Decimal Balance { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}

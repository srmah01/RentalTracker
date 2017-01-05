using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalTracker.Domain
{
    public class Account
    {
        public Account()
        {
           Transactions = new List<Transaction>();
        }

        public int Id { get; set; }

        public String Name { get; set; }

        public Decimal OpeningBalance { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}

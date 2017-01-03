using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalTrackerDomain
{
    public class Transaction
    {
        public int ID { get; set; }

        public String Account { get; set; }

        public String Payee { get; set; }

        public String Category { get; set; }

        public DateTime Date { get; set; }

        public Decimal Amount { get; set; }

        public String Reference { get; set; }

        public String Number { get; set; }

        public String Memo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalTrackerDomain
{
    public class Payee
    {
        public int ID { get; set; }

        public String Name { get; set; }

        public String DefaultCategory { get; set; }

        public String Memo { get; set; }
    }
}

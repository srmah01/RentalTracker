using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalTracker.Domain
{
    public class Payee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DefaultCategory { get; set; }

        public string Memo { get; set; }
    }
}

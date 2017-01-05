using RentalTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalTracker.DAL
{
    public interface IRentalTracker
    {
        Account SaveNewAccount(Account account);

        Account UpdateAccount(Account account);
    }
}

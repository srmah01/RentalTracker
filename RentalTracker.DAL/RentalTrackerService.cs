using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalTracker.Domain;
using System.Data.Entity;

namespace RentalTracker.DAL
{
    public class RentalTrackerService : IRentalTrackerService
    {
        private IRentalTrackerRepository repository;

        public RentalTrackerService(IRentalTrackerRepository repository)
        {
            this.repository = repository;
        }

        public ICollection<Account> GetAllAccounts()
        {
            return repository.GetAllAccounts();
        }

        public void AddAccount(Account account)
        {
            throw new NotImplementedException();
        }

        public Account FindAccount(int? id)
        {
            throw new NotImplementedException();
        }
    }
}

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
        #region Dashboard
        public int GetNumberOfAccounts()
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Accounts.Count();
            }
        }

        public int GetNumberOfCategories()
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Categories.Count();
            }
        }

        public int GetNumberOfPayees()
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Payees.Count();
            }
        }

        public int GetNumberOfTransactions()
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Transactions.Count();
            }
        }

        public Decimal GetTotalOfAccountBalances()
        {
            using (var context = new RentalTrackerContext())
            {
                var balance = 0.0m;

                foreach (var account in context.Accounts)
                {
                    balance += GetAccountBalance(account.Id);
                }

                return balance;
            }
        }

        #endregion

        #region Accounts

        public ICollection<Account> GetAllAccounts()
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Accounts.AsNoTracking().ToList();
            }
        }

        public Account FindAccount(int? id)
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Accounts.AsNoTracking()
                                       .SingleOrDefault(a => a.Id == id);
            }
        }

        public Account FindAccountWithTransactions(int? id)
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Accounts.AsNoTracking()
                                       .Include(a => a.Transactions).AsNoTracking()
                                       .SingleOrDefault(a => a.Id == id);
            }
        }

        public void SaveNewAccount(Account account)
        {
            using (var context = new RentalTrackerContext())
            {
                context.Accounts.Add(account);
                context.SaveChanges();
            }
        }

        public void SaveUpdatedAccount(Account account)
        {
            using (var context = new RentalTrackerContext())
            {
                context.Entry(account).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public Decimal GetAccountBalance(int? id)
        {
            // Making Transaction.Amount positive for income values and negative for expenditure values
            // makes the calculation of the account balance trivial. Simply, just sum the Amount column
            // and add it to the OpeningBalance.
            // TODO: Revist these to see if SQL could be improved.
            if (id != null)
            {
                using (var context = new RentalTrackerContext())
                {
                    var openingBalance = context.Accounts.SingleOrDefault(a => a.Id == id).OpeningBalance;

                    var x = context.Accounts.Include("Transactions").SingleOrDefault(a => a.Id == id).Transactions.Sum(t => t.Amount);
                    return (openingBalance + x);
                }
            }
            return 0.00m;
        }

        #endregion

        #region Categories
        public ICollection<Category> GetAllCategories()
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Categories.AsNoTracking().ToList();
            }
        }

        public Category FindCategory(int? id)
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Categories.AsNoTracking()
                                        .SingleOrDefault(c => c.Id == id);
            }
        }

        public Category FindCategoryWithTransactions(int? id)
        {
            using (var context = new RentalTrackerContext())
            {
                var category = context.Categories
                                        .Include(x => x.Transactions)
                                        .SingleOrDefault(c => c.Id == id);

                if (category != null)
                {
                    foreach (var transaction in category.Transactions)
                    {
                        // Explicitly load the Account reference
                        context.Entry(transaction).Reference(t => t.Account).Load();
                    }
                }

                return category;
            }
        }

        public void SaveNewCategory(Category category)
        {
            using (var context = new RentalTrackerContext())
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
        }

        public void SaveUpdatedCategory(Category category)
        {
            using (var context = new RentalTrackerContext())
            {
                context.Entry(category).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        #endregion

        #region Payees

        public ICollection<Payee> GetAllPayees()
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Payees.AsNoTracking().ToList();
            }
        }

        public Payee FindPayee(int? id)
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Payees.AsNoTracking()
                                       .SingleOrDefault(p => p.Id == id);
            }
        }

        public Payee FindPayeeWithTransactions(int? id)
        {
            using (var context = new RentalTrackerContext())
            {
                var payee = context.Payees
                                     .Include(a => a.Transactions)
                                     .SingleOrDefault(p => p.Id == id);

                if (payee != null)
                {
                    foreach (var transaction in payee.Transactions)
                    {
                        // Explicitly load the Account reference
                        context.Entry(transaction).Reference(t => t.Account).Load();
                    }
                }

                return payee;
            }
        }

        public void SaveNewPayee(Payee payee)
        {
            using (var context = new RentalTrackerContext())
            {
                context.Payees.Add(payee);
                context.SaveChanges();
            }
        }

        public void SaveUpdatedPayee(Payee payee)
        {
            using (var context = new RentalTrackerContext())
            {
                context.Entry(payee).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        #endregion

        #region Transactions

        public ICollection<Transaction> GetAllTransactionsWithAccountAndCategory()
        {
            using (var context = new RentalTrackerContext())
            {
                var transactions = context.Transactions.AsNoTracking()
                                                       .Include(t => t.Account)
                                                       .Include(t => t.Category);
                return transactions.ToList();
            }
        }

        public Transaction FindTransaction(int? id)
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Transactions.AsNoTracking()
                                           .SingleOrDefault(p => p.Id == id);
            }
        }

        public void SaveNewTransaction(Transaction transaction)
        {
            using (var context = new RentalTrackerContext())
            {
                context.Transactions.Add(transaction);
                context.SaveChanges();
            }
        }

        public void SaveUpdatedTransaction(Transaction transaction)
        {
            using (var context = new RentalTrackerContext())
            {
                context.Entry(transaction).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void RemoveTransaction(int id)
        {
            using (var context = new RentalTrackerContext())
            {
                Transaction transaction = context.Transactions.Find(id);
                context.Transactions.Remove(transaction);
                context.SaveChanges();
            }
        }

        #endregion
    }
}

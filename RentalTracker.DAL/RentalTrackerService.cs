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
                var accounts = context.Accounts.AsNoTracking().ToList();
                foreach (var account in accounts)
                {
                    account.Balance = GetAccountBalance(account.Id);
                }

                return accounts;
            }
        }

        public Account FindAccount(int? id)
        {
            using (var context = new RentalTrackerContext())
            {
                var account = context.Accounts.AsNoTracking()
                                       .SingleOrDefault(a => a.Id == id);
                if (account != null)
                {
                    account.Balance = GetAccountBalance(id);
                }

                return account;
            }
        }

        public Account FindAccountWithTransactions(int? id, DateTime? from = null, DateTime? to = null, bool ascending = true)
        {
            using (var context = new RentalTrackerContext())
            {
                var account = context.Accounts
                                     .SingleOrDefault(a => a.Id == id);

                if (account != null)
                {
                    context.Accounts.Attach(account);
                    var transactions = context.Entry(account)
                                    .Collection(a => a.Transactions)
                                    .Query()
                                    .Include(t => t.Payee)
                                    .Include(t => t.Category)
                                    .OrderBy(t => t.Date)
                                    .ThenBy(t => t.Id);

                    CalculateTransactionBalances(transactions, account.OpeningBalance);

                    if (from == null)
                    {
                        from = DateTime.MinValue;
                    }

                    if (to == null)
                    {
                        to = DateTime.MaxValue;
                    }

                    if (ascending)
                    {
                        account.Transactions = transactions
                                               .Where(t => t.Date >= from && t.Date <= to)
                                               .ToList();
                    }
                    else
                    {
                        account.Transactions = transactions
                                               .Where(t => t.Date >= from && t.Date <= to)
                                               .OrderByDescending(t => t.Date)
                                               .ThenByDescending(t => t.Id)
                                               .ToList();
                    }

                    account.Balance = GetAccountBalance(id);
                }

                return account;
            }
        }

        private void CalculateTransactionBalances(IQueryable<Transaction> transactions, Decimal openingBalance)
        {
            var balance = openingBalance;

            foreach (var transaction in transactions)
            {
                balance += (transaction.Amount * (transaction.Category.Type == CategoryType.Income ? 1 : -1));
                transaction.Balance = balance;
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
            using (var context = new RentalTrackerContext())
            {
                var openingBalance = (from a in context.Accounts
                                      where a.Id == id
                                      select a.OpeningBalance).FirstOrDefault();

                // Generates SQL that gets only the specific column
                var amounts = (from t in context.Transactions
                               where t.AccountId == id
                               select new { Amount = t.Amount, CategoryType = t.Category.Type }).ToList();

                var income = amounts.Where(a => a.CategoryType == CategoryType.Income).ToList().Sum(a => a.Amount);

                var expense = amounts.Where(a => a.CategoryType == CategoryType.Expense).ToList().Sum(a => a.Amount);

                return (openingBalance + income - expense);
            }
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

        public Category FindCategoryWithTransactions(int? id, DateTime? from = null, DateTime? to = null, bool ascending = true)
        {
            using (var context = new RentalTrackerContext())
            {
                var category = context.Categories
                                     .SingleOrDefault(a => a.Id == id);

                if (category != null)
                {
                    context.Categories.Attach(category);

                    if (from == null)
                    {
                        from = DateTime.MinValue;
                    }

                    if (to == null)
                    {
                        to = DateTime.MaxValue;
                    }

                    if (ascending)
                    {
                        context.Entry(category)
                               .Collection(c => c.Transactions)
                               .Query()
                               .Include(t => t.Account)
                               .Include(t => t.Payee)
                               .Where(t => t.Date >= from && t.Date <= to)
                               .OrderBy(t => t.Date)
                               .ThenBy(t => t.Id)
                               .Load();
                    }
                    else
                    {
                        context.Entry(category)
                               .Collection(c => c.Transactions)
                               .Query()
                               .Include(t => t.Account)
                               .Include(t => t.Payee)
                               .Where(t => t.Date >= from && t.Date <= to)
                               .OrderByDescending(t => t.Date)
                               .ThenByDescending(t => t.Id)
                               .Load();
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
                return context.Payees
                    .AsNoTracking()
                    .Include(p => p.DefaultCategory)
                    .AsNoTracking()
                    .ToList();
            }
        }

        public Payee FindPayee(int? id)
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Payees.AsNoTracking()
                                     .Include(p => p.DefaultCategory)
                                     .SingleOrDefault(p => p.Id == id);
            }
        }

        public Payee FindPayeeWithTransactions(int? id, DateTime? from = null, DateTime? to = null, bool ascending = true)
        {
            using (var context = new RentalTrackerContext())
            {
                var payee = context.Payees
                                   .Include(p => p.DefaultCategory)
                                   .SingleOrDefault(a => a.Id == id);

                if (payee != null)
                {
                    context.Payees.Attach(payee);

                    if (from == null)
                    {
                        from = DateTime.MinValue;
                    }

                    if (to == null)
                    {
                        to = DateTime.MaxValue;
                    }

                    if (ascending)
                    {
                        context.Entry(payee)
                               .Collection(c => c.Transactions)
                               .Query()
                               .Include(t => t.Account)
                               .Include(t => t.Category)
                               .Where(t => t.Date >= from && t.Date <= to)
                               .OrderBy(t => t.Date)
                               .ThenBy(t => t.Id)
                               .Load();
                    }
                    else
                    {
                        context.Entry(payee)
                               .Collection(c => c.Transactions)
                               .Query()
                               .Include(t => t.Account)
                               .Include(t => t.Category)
                               .Where(t => t.Date >= from && t.Date <= to)
                               .OrderByDescending(t => t.Date)
                               .ThenByDescending(t => t.Id)
                               .Load();
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

        public ICollection<Transaction> GetAllTransactionsWithAccountAndPayeeAndCategory(
            String account = null, String payee = null, String category = null,
            DateTime? from = null, DateTime? to = null, bool ascending = true)
        {
            using (var context = new RentalTrackerContext())
            {
                var transactions = context.Transactions.AsNoTracking()
                                                       .Include(t => t.Account)
                                                       .Include(t => t.Payee)
                                                       .Include(t => t.Category)
                                                       .AsQueryable();

                if (from == null)
                {
                    from = DateTime.MinValue;
                }

                if (to == null)
                {
                    to = DateTime.MaxValue;
                }

                if (account != null)
                {
                    transactions = transactions.Where(t => t.Account.Name.ToLower().Contains(account.ToLower()));
                }

                if (payee != null)
                {
                    transactions = transactions.Where(t => t.Payee.Name.ToLower().Contains(payee.ToLower()));
                }

                if (category != null)
                {
                    transactions = transactions.Where(t => t.Category.Name.ToLower().Contains(category.ToLower()));
                }

                if (ascending)
                {
                    transactions = transactions
                                    .Where(t => t.Date >= from && t.Date <= to)
                                    .OrderBy(t => t.Date)
                                    .ThenBy(t => t.Id);
                }
                else
                {
                    transactions = transactions
                                    .Where(t => t.Date >= from && t.Date <= to)
                                    .OrderByDescending(t => t.Date)
                                    .ThenByDescending(t => t.Id);
                }


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

        public Transaction FindTransactionWithAccountAndPayeeAndCategory(int? id)
        {
            using (var context = new RentalTrackerContext())
            {
                return context.Transactions.AsNoTracking()
                                           .Include(t => t.Account)
                                           .Include(t => t.Payee)
                                           .Include(t => t.Category)
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

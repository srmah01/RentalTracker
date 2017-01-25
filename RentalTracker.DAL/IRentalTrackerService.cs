using RentalTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalTracker.DAL
{
    public interface IRentalTrackerService
    {
        #region Dashboard
        int GetNumberOfAccounts();

        int GetNumberOfCategories();

        int GetNumberOfPayees();

        int GetNumberOfTransactions();

        Decimal GetTotalOfAccountBalances();

        #endregion

        #region Accounts

        ICollection<Account> GetAllAccounts();

        Decimal GetAccountBalance(int? id);

        Account FindAccount(int? id);

        Account FindAccountWithTransactions(int? id);

        void SaveNewAccount(Account account);

        void SaveUpdatedAccount(Account account);

        #endregion

        #region Categories

        ICollection<Category> GetAllCategories();

        Category FindCategory(int? id);

        Category FindCategoryWithTransactions(int? id);

        void SaveNewCategory(Category category);

        void SaveUpdatedCategory(Category category);

        #endregion

        #region Payees

        ICollection<Payee> GetAllPayees();

        Payee FindPayee(int? id);

        Payee FindPayeeWithTransactions(int? id);

        void SaveNewPayee(Payee payee);

        void SaveUpdatedPayee(Payee payee);

        #endregion

        #region Transactions

        ICollection<Transaction> GetAllTransactionsWithAccountAndPayeeAndCategory();

        Transaction FindTransaction(int? id);

        void SaveNewTransaction(Transaction transaction);

        void SaveUpdatedTransaction(Transaction transaction);

        void RemoveTransaction(int id);

        #endregion
    }
}

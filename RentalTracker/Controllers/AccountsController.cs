using System.Data;
using System.Net;
using System.Web.Mvc;
using RentalTracker.DAL;
using RentalTracker.Domain;
using RentalTracker.Models;
using RentalTracker.Utilities;
using RentalTracker.DAL.Exceptions;
using RentalTracker.Enums;
using System.Web.Helpers;

namespace RentalTracker.Controllers
{
    /// <summary>
    /// Represents the controller for the Account entity related pages.
    /// </summary>
    public class AccountsController : Controller
    {
        /// <summary>
        /// The Rental Tracker DAL service.
        /// </summary>
        private IRentalTrackerService rentalTrackerService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rentalTrackerService">The instance of the RentalTrackerService.</param>
        public AccountsController(IRentalTrackerService rentalTrackerService)
        {
            this.rentalTrackerService = rentalTrackerService;
        }

        /// <summary>
        /// Gets the Index view.
        /// </summary>
        /// <returns>The Index view with all Accounts listed.</returns>
        // GET: Accounts
        public ActionResult Index()
        {
            return View(rentalTrackerService.GetAllAccounts());
        }

        /// <summary>
        /// Get the details view of a given Account with a list of it's Transactions.
        /// </summary>
        /// <param name="id">The id of the Account.</param>
        /// <param name="dateFilter">The value of the DateFilter selector.</param>
        /// <param name="fromDate">The from date of the Transactions.</param>
        /// <param name="toDate">The to date of the Transactions.</param>
        /// <param name="sortOrder">The order inn which to display the Transactions.</param>
        /// <returns>The Details view of an Account.</returns>
        // GET: Accounts/Details/5
        public ActionResult Details(int? id,
            DateFilterSelector dateFilter = DateFilterSelector.AllDates,
            string fromDate = null, string toDate = null, 
            SortDirection sortOrder = SortDirection.Ascending)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dateFilterViewModel = new DateFilterViewModel();
            dateFilterViewModel.SetDateFilter(dateFilter, fromDate, toDate, sortOrder);

            Account account = rentalTrackerService.FindAccountWithTransactions(id, dateFilterViewModel.FromDate,
                 dateFilterViewModel.ToDate, (sortOrder == SortDirection.Ascending));

            if (account == null)
            {
                return HttpNotFound();
            }

            var accountViewModel = new EntityDetailsViewModel<Account>()
            {
                Entity = account,
                Filter = dateFilterViewModel
            };

            foreach (var item in account.Transactions)
            {
                var transactionViewModel = new TransactionsListViewModel();

                transactionViewModel.Date = item.Date;
                transactionViewModel.Payee = item.Payee.Name; 
                transactionViewModel.Category = item.Category.Name;
                if (item.Category.Type == CategoryType.Income)
                {
                    transactionViewModel.Income = item.Amount;
                    transactionViewModel.Expense = null;
                }
                else
                {
                    transactionViewModel.Income = null;
                    transactionViewModel.Expense = item.Amount;   // Alway display a posivive amount
                }
                transactionViewModel.Taxable = item.Taxable;
                transactionViewModel.Balance = item.Balance;
                transactionViewModel.Reference = item.Reference;
                transactionViewModel.Memo = item.Memo;
                accountViewModel.Transactions.Add(transactionViewModel);
            }

            return View(accountViewModel);
        }

        /// <summary>
        /// Gets the empty Create view for a new Account.
        /// </summary>
        /// <returns></returns>
        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles the submit of a new Account entity.
        /// </summary>
        /// <param name="account">The new Account.</param>
        /// <returns>The Index view if successful, otherwise the Create view with errors displayed.</returns>
        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,OpeningBalance")] Account account)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    rentalTrackerService.SaveNewAccount(account);
                    return RedirectToAction("Index");
                }
                catch (RentalTrackerServiceValidationException ex)
                {
                    HandleValidationErrors.AddErrorsToModel(this, ex.ValidationResults);
                }
                catch (DataException ex)
                {
                    HandleValidationErrors.AddExceptionError(this, ex);
                }
            }

            return View(account);
        }

        /// <summary>
        /// Get the Edit view of the specified Account entity.
        /// </summary>
        /// <param name="id">The specified id.</param>
        /// <returns>The Edit view with the entities fields filled in.</returns>
        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = rentalTrackerService.FindAccount(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        /// <summary>
        /// Handles the submit of an edited Account entity.
        /// </summary>
        /// <param name="account">The updated Account.</param>
        /// <returns>The Index view if successful, otherwise the Edit view with errors displayed.</returns>
        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,OpeningBalance")] Account account)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    rentalTrackerService.SaveUpdatedAccount(account);
                    return RedirectToAction("Index");
                }
                catch (RentalTrackerServiceValidationException ex)
                {
                    HandleValidationErrors.AddErrorsToModel(this, ex.ValidationResults);
                }
                catch (DataException ex)
                {
                    HandleValidationErrors.AddExceptionError(this, ex);
                }
            }
            return View(account);
        }

    }
}

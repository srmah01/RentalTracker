using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using RentalTracker.DAL;
using RentalTracker.Domain;
using RentalTracker.Models;
using RentalTracker.DAL.Exceptions;
using RentalTracker.Utilities;
using System.Web.Helpers;
using RentalTracker.Enums;

namespace RentalTracker.Controllers
{
    /// <summary>
    /// Represents the controller for the Transaction entity related pages.
    /// </summary>
    public class TransactionsController : Controller
    {
        /// <summary>
        /// The Rental Tracker DAL service.
        /// </summary>
        private IRentalTrackerService rentalTrackerService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rentalTrackerService">The instance of the RentalTrackerService.</param>
        public TransactionsController(IRentalTrackerService rentalTrackerService)
        {
            this.rentalTrackerService = rentalTrackerService;
        }

        /// <summary>
        /// Gets the Index view.
        /// </summary>
        /// <returns>The Index view with all matching Transactions listed.</returns>
        /// <param name="account">The string to match against the Transaction's Account name</param>
        /// <param name="payee">The string to match against the Transaction's Payee name</param>
        /// <param name="category">The string to match against the Transaction's Category name></param>
        /// <param name="dateFilter">The value of the DateFilter selector.</param>
        /// <param name="fromDate">The from date of the Transactions.</param>
        /// <param name="toDate">The to date of the Transactions.</param>
        /// <param name="sortOrder">The order inn which to display the Transactions.</param>
        /// <returns>The Index view of the matchin Transactions.</returns>
        // GET: Transactions
        public ActionResult Index(string account = null, string payee = null, string category = null,
            DateFilterSelector dateFilter = DateFilterSelector.AllDates,
            string fromDate = null, string toDate = null,
            SortDirection sortOrder = SortDirection.Ascending)
        {
            var searchFilterViewModel = new SearchFilterViewModel();
            searchFilterViewModel.SetSearchFilter(account, payee, category, dateFilter, fromDate, toDate, sortOrder);

            var transactions = rentalTrackerService.GetAllTransactionsWithAccountAndPayeeAndCategory(
                account, payee, category,
                searchFilterViewModel.FromDate, searchFilterViewModel.ToDate, sortOrder == SortDirection.Ascending);
            var indexViewModel = new EntityDetailsViewModel<Transaction>()
            {
                Filter = searchFilterViewModel
            };

            // Add last search terms to the ViewBag so they can be passed through
            // again if a sort order change is requested
            ViewBag.Account = account;
            ViewBag.Payee = payee;
            ViewBag.Category = category;
            
            foreach (var item in transactions)
            {
                var transactionViewModel = new TransactionsListViewModel();

                transactionViewModel.Id = item.Id;
                transactionViewModel.Date = item.Date;
                transactionViewModel.Account = item.Account.Name;
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
                    transactionViewModel.Expense = item.Amount;
                }
                transactionViewModel.Taxable = item.Taxable;
                transactionViewModel.Reference = item.Reference;
                transactionViewModel.Memo = item.Memo;
                indexViewModel.Transactions.Add(transactionViewModel);
            }

            return View(indexViewModel);
        }

        /// <summary>
        /// Gets the Create view for a new Transaction.
        /// </summary>
        /// <returns></returns>
        // GET: Transactions/Create
        public ActionResult Create()
        {
            GetReferenceData();
            return View();
        }

        /// <summary>
        /// Handles the submit of a new Transaction entity.
        /// </summary>
        /// <param name="account">The new Transaction.</param>
        /// <returns>The Index view if successful, otherwise the Create view with errors displayed.</returns>
        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,AccountId,PayeeId,CategoryId,Taxable,Amount,Reference,Memo")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    rentalTrackerService.SaveNewTransaction(transaction);
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

            GetReferenceData();
            return View(transaction);
        }

        /// <summary>
        /// Get the Edit view of the specified Transaction entity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The Edit view with the entities fields filled in.</returns>
        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = rentalTrackerService.FindTransaction(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }

            GetReferenceData();
            return View(transaction);
        }

        /// <summary>
        /// Handles the submit of an edited Transaction entity.
        /// </summary>
        /// <param name="transaction">The updated Transaction.</param>
        /// <returns>The Index view if successful, otherwise the Edit view with errors displayed.</returns>
        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,AccountId,PayeeId,CategoryId,Amount,Taxable,Reference,Memo")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    rentalTrackerService.SaveUpdatedTransaction(transaction);
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

            GetReferenceData();
            return View(transaction);
        }

        /// <summary>
        /// Gets the Delete view for a Transaction.
        /// </summary>
        /// <param name="id">The specified id.</param>
        /// <returns>The Delete view of the Transaction with the fields filled in if successful,
        /// otherwise the HttpNotFound error.</returns>
        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = rentalTrackerService.FindTransactionWithAccountAndPayeeAndCategory(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        /// <summary>
        /// Handles the submit of a Delete action for a Transaction.
        /// </summary>
        /// <param name="id">The specified id.</param>
        /// <returns>The Index view if successful, otherwise the HttpNotFound error.</returns>
        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                rentalTrackerService.RemoveTransaction(id);
                return RedirectToAction("Index");
            }
            catch (ArgumentNullException)
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Returns the reference data for the selection lists in the ViewBag.
        /// </summary>
        private void GetReferenceData()
        {
            ViewBag.AccountId = new SelectList(rentalTrackerService.GetAllAccounts(), "Id", "Name");
            ViewBag.CategoryId = new SelectList(rentalTrackerService.GetAllCategories(), "Id", "Name");
            var allPayees = rentalTrackerService.GetAllPayees();
            ViewBag.PayeeId = new SelectList(allPayees, "Id", "Name");
            ViewBag.PayeeCategoryMap = allPayees.AsEnumerable()
                                                .Select(p => new { PayeeId = p.Id,
                                                    DefaultCategoryId = p.DefaultCategoryId
                                                }).ToList();
        }
    }
}

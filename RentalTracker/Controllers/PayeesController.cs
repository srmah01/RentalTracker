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
    /// Represents the controller for the Payee entity related pages.
    /// </summary>
    public class PayeesController : Controller
    {
        /// <summary>
        /// The Rental Tracker DAL service.
        /// </summary>
        private IRentalTrackerService rentalTrackerService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rentalTrackerService">The instance of the RentalTrackerService.</param>
        public PayeesController(IRentalTrackerService rentalTrackerService)
        {
            this.rentalTrackerService = rentalTrackerService;
        }

        /// <summary>
        /// Gets the Index view.
        /// </summary>
        /// <returns>The Index view with all Payees listed.</returns>
        // GET: Payees
        public ActionResult Index()
        {
            return View(rentalTrackerService.GetAllPayees());
        }

        /// <summary>
        /// Get the details view of a given Payee with a list of it's Transactions.
        /// </summary>
        /// <param name="id">The id of the Payee.</param>
        /// <param name="dateFilter">The value of the DateFilter selector.</param>
        /// <param name="fromDate">The from date of the Transactions.</param>
        /// <param name="toDate">The to date of the Transactions.</param>
        /// <param name="sortOrder">The order inn which to display the Transactions.</param>
        /// <returns>The Details view of a Payee.</returns>
        // GET: Payees/Details/5
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

            Payee payee = rentalTrackerService.FindPayeeWithTransactions(id, dateFilterViewModel.FromDate,
                 dateFilterViewModel.ToDate, (sortOrder == SortDirection.Ascending));

            if (payee == null)
            {
                return HttpNotFound();
            }
            var payeeViewModel = new EntityDetailsViewModel<Payee>()
            {
                Entity = payee,
                Filter = dateFilterViewModel
            };

            foreach (var item in payee.Transactions)
            {
                var transactionViewModel = new TransactionsListViewModel();

                transactionViewModel.Date = item.Date;
                transactionViewModel.Account = item.Account.Name;
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
                payeeViewModel.Transactions.Add(transactionViewModel);
            }

            return View(payeeViewModel);
        }

        /// <summary>
        /// Gets the Create view for a new Payee.
        /// </summary>
        /// <returns></returns>
        // GET: Payees/Create
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles the submit of a new Payee entity.
        /// </summary>
        /// <param name="payee">The new Payee.</param>
        /// <returns>The Index view if successful, otherwise the Create view with errors displayed.</returns>
        // POST: Payees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Memo")] Payee payee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    rentalTrackerService.SaveNewPayee(payee);
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

            return View(payee);
        }

        /// <summary>
        /// Get the Edit view of the specified Payee entity.
        /// </summary>
        /// <param name="id">The specified id.</param>
        /// <returns>The Edit view with the entities fields filled in.</returns>
        // GET: Payees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payee payee = rentalTrackerService.FindPayee(id);
            if (payee == null)
            {
                return HttpNotFound();
            }
            return View(payee);
        }

        /// <summary>
        /// Handles the submit of an edited Payee entity.
        /// </summary>
        /// <param name="payee">The updated Payee.</param>
        /// <returns>The Index view if successful, otherwise the Edit view with errors displayed.</returns>
        // POST: Payees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Memo")] Payee payee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    rentalTrackerService.SaveUpdatedPayee(payee);
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
            return View(payee);
        }
    }
}

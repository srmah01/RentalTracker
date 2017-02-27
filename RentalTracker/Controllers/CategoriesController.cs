using System.Data;
using System.Net;
using System.Web.Mvc;
using RentalTracker.DAL;
using RentalTracker.Domain;
using RentalTracker.Models;
using RentalTracker.DAL.Exceptions;
using RentalTracker.Utilities;
using RentalTracker.Enums;
using System.Web.Helpers;

namespace RentalTracker.Controllers
{
    /// <summary>
    /// Represents the controller for the Category entity related pages.
    /// </summary>
    public class CategoriesController : Controller
    {
        /// <summary>
        /// The Rental Tracker DAL service.
        /// </summary>
        private IRentalTrackerService rentalTrackerService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rentalTrackerService">The instance of the RentalTrackerService.</param>
        public CategoriesController(IRentalTrackerService rentalTrackerService)
        {
            this.rentalTrackerService = rentalTrackerService;
        }

        /// <summary>
        /// Gets the Index view.
        /// </summary>
        /// <returns>The Index view with all Categories listed.</returns>
        // GET: Categories
        public ActionResult Index()
        {
            return View(rentalTrackerService.GetAllCategories());
        }

        /// <summary>
        /// Get the details view of a given Category with a list of it's Transactions.
        /// </summary>
        /// <param name="id">The id of the Category.</param>
        /// <param name="dateFilter">The value of the DateFilter selector.</param>
        /// <param name="fromDate">The from date of the Transactions.</param>
        /// <param name="toDate">The to date of the Transactions.</param>
        /// <param name="sortOrder">The order inn which to display the Transactions.</param>
        /// <returns>The Details view of a Category.</returns>
        // GET: Categories/Details/5
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

            Category category = rentalTrackerService.FindCategoryWithTransactions(id, dateFilterViewModel.FromDate,
                 dateFilterViewModel.ToDate, (sortOrder == SortDirection.Ascending));

            if (category == null)
            {
                return HttpNotFound();
            }

            var categoryViewModel = new EntityDetailsViewModel<Category>()
            {
                Entity = category,
                Filter = dateFilterViewModel
            };

            foreach (var item in category.Transactions)
            {
                var transactionViewModel = new TransactionsListViewModel();

                transactionViewModel.Date = item.Date;
                transactionViewModel.Account = item.Account.Name;
                transactionViewModel.Payee = item.Payee.Name;
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
                categoryViewModel.Transactions.Add(transactionViewModel);
            }

            return View(categoryViewModel);
        }

        /// <summary>
        /// Gets the Create view for a new Category.
        /// </summary>
        /// <returns></returns>
        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles the submit of a new Category entity.
        /// </summary>
        /// <param name="category">The new Category.</param>
        /// <returns>The Index view if successful, otherwise the Create view with errors displayed.</returns>
        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Type")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    rentalTrackerService.SaveNewCategory(category);
                return RedirectToAction("Index");
                }
                catch (RentalTrackerServiceValidationException ex)
                {
                    HandleValidationErrors.AddErrorsToModel(this, ex.ValidationResults);
                }
            }

            return View(category);
        }

        /// <summary>
        /// Get the Edit view of the specified Category entity.
        /// </summary>
        /// <param name="id">The specified id.</param>
        /// <returns>The Edit view with the entities fields filled in.</returns>
        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = rentalTrackerService.FindCategory(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        /// <summary>
        /// Handles the submit of an edited Category entity.
        /// </summary>
        /// <param name="category">The updated Category.</param>
        /// <returns>The Index view if successful, otherwise the Edit view with errors displayed.</returns>
        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Type")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    rentalTrackerService.SaveUpdatedCategory(category);
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
            return View(category);
        }
    }
}

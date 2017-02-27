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
    public class PayeesController : Controller
    {
        private IRentalTrackerService rentalTrackerService;

        public PayeesController(IRentalTrackerService rentalTrackerService)
        {
            this.rentalTrackerService = rentalTrackerService;
        }

        // GET: Payees
        public ActionResult Index()
        {
            return View(rentalTrackerService.GetAllPayees());
        }

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

        // GET: Payees/Create
        public ActionResult Create()
        {
            return View();
        }

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

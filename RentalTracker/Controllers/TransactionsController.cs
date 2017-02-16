using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
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
    public class TransactionsController : Controller
    {
        private IRentalTrackerService rentalTrackerService;

        public TransactionsController(IRentalTrackerService rentalTrackerService)
        {
            this.rentalTrackerService = rentalTrackerService;
        }

        // GET: Transactions
        public ActionResult Index(string account = null, string payee = null, string category = null,
            DateFilterSelector dateFilter = DateFilterSelector.AllDates,
            string fromDate = null, string toDate = null,
            SortDirection sortOrder = SortDirection.Ascending)
        {
            var searchFilterViewModel = new SearchFilterViewModel();
            searchFilterViewModel.SetSearchFilter(account, payee, category, dateFilter, fromDate, toDate, sortOrder);

            var transactions = rentalTrackerService.GetAllTransactionsWithAccountAndPayeeAndCategory();
            var indexViewModel = new EntityDetailsViewModel<Transaction>()
            {
                Filter = searchFilterViewModel
            };
            
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

        // GET: Transactions/Create
        public ActionResult Create()
        {
            GetReferenceData();
            return View();
        }

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

        private void GetReferenceData()
        {
            ViewBag.AccountId = new SelectList(rentalTrackerService.GetAllAccounts(), "Id", "Name");
            ViewBag.CategoryId = new SelectList(rentalTrackerService.GetAllCategories(), "Id", "Name");
            ViewBag.PayeeId = new SelectList(rentalTrackerService.GetAllPayees(), "Id", "Name");
        }
    }
}

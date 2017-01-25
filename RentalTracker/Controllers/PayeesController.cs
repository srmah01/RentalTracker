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
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payee payee = rentalTrackerService.FindPayeeWithTransactions(id);
            if (payee == null)
            {
                return HttpNotFound();
            }
            var payeeViewModel = new EntityDetailsViewModel<Payee>()
            {
                Entity = payee
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
                    transactionViewModel.Expense = item.Amount * -1;   // Alway display a posivive amount
                }
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
                rentalTrackerService.SaveNewPayee(payee);
                return RedirectToAction("Index");
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
                rentalTrackerService.SaveUpdatedPayee(payee);
                return RedirectToAction("Index");
            }
            return View(payee);
        }

        // GET: Payees/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Payees/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Payee payee = db.Payees.Find(id);
        //    db.Payees.Remove(payee);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

    }
}

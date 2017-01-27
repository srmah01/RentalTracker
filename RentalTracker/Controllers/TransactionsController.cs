﻿using System;
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
    public class TransactionsController : Controller
    {
        private IRentalTrackerService rentalTrackerService;

        public TransactionsController(IRentalTrackerService rentalTrackerService)
        {
            this.rentalTrackerService = rentalTrackerService;
        }

        // GET: Transactions
        public ActionResult Index()
        {
            var transactions = rentalTrackerService.GetAllTransactionsWithAccountAndPayeeAndCategory();
            var indexViewModel = new EntityDetailsViewModel<Transaction>();

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
                    transactionViewModel.Expense = item.Amount * -1;   // Alway display a posivive amount
                }
                transactionViewModel.Reference = item.Reference;
                transactionViewModel.Memo = item.Memo;
                indexViewModel.Transactions.Add(transactionViewModel);
            }

            return View(indexViewModel);
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            // For now make Details page unreachable - it might be removed later
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = rentalTrackerService.FindTransaction(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            var transactionViewModel = new TransactionViewModel();
            transactionViewModel.Accounts = rentalTrackerService.GetAllAccounts().Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            });

            transactionViewModel.Categories = rentalTrackerService.GetAllCategories().Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            });
            transactionViewModel.Payees = rentalTrackerService.GetAllPayees().Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            });

            return View(transactionViewModel);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,Date,Amount,Reference,Number,Memo")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                rentalTrackerService.SaveNewTransaction(transaction);
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(rentalTrackerService.GetAllAccounts(), "Id", "Name");
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
            ViewBag.AccountId = new SelectList(rentalTrackerService.GetAllAccounts(), "Id", "Name");
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,Date,Amount,Reference,Number,Memo")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                rentalTrackerService.SaveUpdatedTransaction(transaction);
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(rentalTrackerService.GetAllAccounts(), "Id", "Name");
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
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
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            rentalTrackerService.RemoveTransaction(id);
            return RedirectToAction("Index");
        }
    }
}

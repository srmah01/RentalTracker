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
using System.Data.Entity.Validation;
using RentalTracker.Utilities;
using RentalTracker.DAL.Exceptions;

namespace RentalTracker.Controllers
{
    public class AccountsController : Controller
    {
        private IRentalTrackerService rentalTrackerService;

        public AccountsController(IRentalTrackerService rentalTrackerService)
        {
            this.rentalTrackerService = rentalTrackerService;
        }

        // GET: Accounts
        public ActionResult Index()
        {
            return View(rentalTrackerService.GetAllAccounts());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id,
            DateFilterSelector? filterSelector = null, string filterFromDate = null, string filterToDate = null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account;
            if (filterSelector == null || filterSelector == DateFilterSelector.AllDates)
            {
                account = rentalTrackerService.FindAccountWithTransactions(id);
            }
            else
            {
                var dateFilter = new DateFilter();
                dateFilter.SetDateFilter(filterSelector.Value, filterFromDate, filterToDate);
                account = rentalTrackerService.FindAccountWithDateFilteredTransactions(id, dateFilter.From, dateFilter.To);
            }

            if (account == null)
            {
                return HttpNotFound();
            }

            var accountViewModel = new EntityDetailsViewModel<Account>()
            {
                Entity = account
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

        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

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

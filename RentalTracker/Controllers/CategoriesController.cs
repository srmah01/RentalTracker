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

namespace RentalTracker.Controllers
{
    public class CategoriesController : Controller
    {
        private IRentalTrackerService rentalTrackerService;

        public CategoriesController(IRentalTrackerService rentalTrackerService)
        {
            this.rentalTrackerService = rentalTrackerService;
        }

        // GET: Categories
        public ActionResult Index()
        {
            return View(rentalTrackerService.GetAllCategories());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = rentalTrackerService.FindCategoryWithTransactions(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var categoryViewModel = new EntityDetailsViewModel<Category>()
            {
                Entity = category
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

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

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

using RentalTracker.DAL;
using RentalTracker.DAL.Exceptions;
using RentalTracker.Domain;
using RentalTracker.Models;
using RentalTracker.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentalTracker.Controllers
{
    public class TaxReportController : Controller
    {
        /// <summary>
        /// The Rental Tracker DAL service.
        /// </summary>
        private IRentalTrackerService rentalTrackerService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rentalTrackerService">The instance of the RentalTrackerService.</param>
        public TaxReportController(IRentalTrackerService rentalTrackerService)
        {
            this.rentalTrackerService = rentalTrackerService;
        }

        /// <summary>
        /// Gets the Generate Report view.
        /// </summary>
        // GET: TaxReport/Generate
        public ActionResult Generate()
        {
            GetReferenceData();
            return View();
        }

        /// <summary>
        ///  Handles the request to generate a new Tax Report.
        /// </summary>
        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Generate([Bind(Include = "AccountId,Year")] TaxReportViewModel taxReport)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = rentalTrackerService.FindAccount(taxReport.AccountId);

                    if (account == null)
                    {
                        return HttpNotFound();
                    }

                    return RedirectToAction("Report", new { AccountId = taxReport.AccountId, AccountName = account.Name,
                        Year = taxReport.Year });
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
            return View();
        }

        /// <summary>
        /// Gets the Report view.
        /// </summary>
        // GET: TaxReport/Report
        public ActionResult Report(TaxReportViewModel taxReport)
        {
            var account = rentalTrackerService.FindAccountWithTransactions(taxReport.AccountId);
            if (account != null)
            {
                var yearStart = DateTime.ParseExact("06/04/" + (taxReport.Year - 1), "dd/MM/yyyy", new CultureInfo("en-GB"));
                var yearEnd = DateTime.ParseExact("05/04/" + taxReport.Year, "dd/MM/yyyy", new CultureInfo("en-GB"));

                taxReport.Income = account.Transactions.AsQueryable()
                                                       .Where(t => t.Date >= yearStart && t.Date <= yearEnd)
                                                       .Where(t => t.Category.Type == CategoryType.Income && t.Taxable)
                                                       .ToList();

                taxReport.Expenses = account.Transactions.AsQueryable()
                                                         .Where(t => t.Date >= yearStart && t.Date <= yearEnd)
                                                         .Where(t => t.Category.Type == CategoryType.Expense && t.Taxable)
                                                         .ToList();

                taxReport.TotalIncome = taxReport.Income.Sum(t => t.Amount);
                taxReport.TotalExpense = taxReport.Expenses.Sum(t => t.Amount);
                taxReport.Profit = taxReport.TotalIncome - taxReport.TotalExpense;
                taxReport.TaxLiability = taxReport.Profit * 0.20m;  // 20%
            }

            GetReferenceData();
            return View(taxReport);
        }


        /// <summary>
        /// Returns the reference data for the selection lists in the ViewBag.
        /// </summary>
        private void GetReferenceData()
        {
            ViewBag.AccountId = new SelectList(rentalTrackerService.GetAllAccounts(), "Id", "Name");
            //ViewBag.Years = allPayees.AsEnumerable()
            //                                    .Select(p => new {
            //                                        PayeeId = p.Id,
            //                                        DefaultCategoryId = p.DefaultCategoryId
            //                                    }).ToList();

        }
    }
}
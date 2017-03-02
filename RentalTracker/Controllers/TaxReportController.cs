using RentalTracker.DAL;
using RentalTracker.DAL.Exceptions;
using RentalTracker.Models;
using RentalTracker.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
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
        /// Gets the Index view.
        /// </summary>
        // GET: TaxReport
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
                    //rentalTrackerService.SaveNewTransaction(transaction);
                    //return RedirectToAction("Report", new { TaxReport = taxReport });
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
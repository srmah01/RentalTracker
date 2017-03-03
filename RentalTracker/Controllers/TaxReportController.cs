using RentalTracker.DAL;
using RentalTracker.DAL.Exceptions;
using RentalTracker.Service;
using RentalTracker.Utilities;
using System.Data;
using System.Web.Mvc;

namespace RentalTracker.Controllers
{
    /// <summary>
    /// Represents the controller for the TaxReport related pages.
    /// </summary>
    public class TaxReportController : Controller
    {
        /// <summary>
        /// The Tax Report service.
        /// </summary>
        private ITaxReportService taxReportService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="taxReportService">The instance of the TaxReportService.</param>
        public TaxReportController(ITaxReportService taxReportService)
        {
            this.taxReportService = taxReportService;
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
        /// <param name="accountId">The Id of the account to report on.</param>
        /// <param name="year">The year for the report.</param>
        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Generate([Bind(Include = "AccountId,Year")] int? accountId, int? year)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return RedirectToAction("Report", new { accountId = accountId, year = year });
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
        /// <param name="accountId">The Id of the account to report on.</param>
        /// <param name="year">The year for the report.</param>
        /// <returns></returns>
        // GET: TaxReport/Report
        public ActionResult Report(int? accountId, int? year)
        {
            if (!accountId.HasValue || !year.HasValue)
            {
                return HttpNotFound();
            }

            var taxReportViewModel = taxReportService.GenerateReport(accountId.Value, year.Value);

            if (taxReportViewModel == null)
            {
                return HttpNotFound();
            }

            GetReferenceData();
            return View(taxReportViewModel);
        }


        /// <summary>
        /// Returns the reference data for the selection lists in the ViewBag.
        /// </summary>
        private void GetReferenceData()
        {
            IRentalTrackerService rentalTrackerService = taxReportService.RentalTrackerService;
            ViewBag.AccountId = new SelectList(rentalTrackerService.GetAllAccounts(), "Id", "Name");
        }
    }
}
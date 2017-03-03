using RentalTracker.DAL;
using RentalTracker.Models;

namespace RentalTracker.Service
{
    /// <summary>
    /// The definition for the Tax Report Servivce.
    /// </summary>
    public interface ITaxReportService
    {
        /// <summary>
        /// Get the instance of the IRentalTrackerService used by the service.
        /// <seealso cref="IRentalTrackerService"/>
        /// </summary>
        IRentalTrackerService RentalTrackerService { get; }

        /// <summary>
        /// Geneartes a Tax Report for a specified account and year.
        /// </summary>
        /// <param name="accountId">The Id of the account to report on.</param>
        /// <param name="year">The year for the report.</param>
        /// <returns>A view model containing the details of the report and account tranascations.</returns>
        TaxReportViewModel GenerateReport(int accountId, int year);
    }
}
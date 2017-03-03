using RentalTracker.DAL;
using RentalTracker.Domain;
using RentalTracker.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RentalTracker.Service
{
    /// <summary>
    /// Class representing the implemetation of the ITaxReportService service.
    /// <see cref="ITaxReportService"/>
    /// </summary>
    public class TaxReportService : ITaxReportService
    {
        // Tax Liability Rate of 20% of the Profit.
        private const Decimal TAX_LIABILITY_RATE = 0.20m;

        /// <summary>
        /// <see cref="ITaxReportService.RentalTrackerService"/>
        /// </summary>
        public IRentalTrackerService RentalTrackerService { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rentalTrackerService">The instance of the RentalTrackerService.</param>
        public TaxReportService(IRentalTrackerService rentalTrackerService)
        {
            RentalTrackerService = rentalTrackerService;
        }

        /// <summary>
        /// <see cref="ITaxReportService.GenerateReport(int, int)"/>
        /// </summary>
        public TaxReportViewModel GenerateReport(int accountId, int year)
        {
            var account = RentalTrackerService.FindAccountWithTransactions(accountId);
            if (account != null)
            {
                TaxReportViewModel taxReport = new TaxReportViewModel()
                {
                    AccountId = account.Id,
                    AccountName = account.Name,
                    Year = year
                };

                taxReport.Income = GetTransactions(account.Transactions, taxReport.Year, CategoryType.Income);
                taxReport.Expenses = GetTransactions(account.Transactions, taxReport.Year, CategoryType.Expense);

                taxReport.TotalIncome = taxReport.Income.Sum(t => t.Amount);
                taxReport.TotalExpense = taxReport.Expenses.Sum(t => t.Amount);
                taxReport.Profit = taxReport.TotalIncome - taxReport.TotalExpense;
                taxReport.TaxLiability = taxReport.Profit * TAX_LIABILITY_RATE;

                return taxReport;  
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="year"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private ICollection<Transaction> GetTransactions(ICollection<Transaction> transactions, int year, CategoryType type)
        {
            var yearStart = DateTime.ParseExact("06/04/" + (year - 1), "dd/MM/yyyy", new CultureInfo("en-GB"));
            var yearEnd = DateTime.ParseExact("05/04/" + year, "dd/MM/yyyy", new CultureInfo("en-GB"));

            return transactions.AsQueryable()
                               .Where(t => t.Date >= yearStart && t.Date <= yearEnd)
                               .Where(t => t.Category.Type == type && t.Taxable)
                               .ToList();
        }
    }
}
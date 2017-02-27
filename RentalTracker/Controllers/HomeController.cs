using RentalTracker.DAL;
using RentalTracker.Models;
using System.Web.Mvc;

namespace RentalTracker.Controllers
{
    /// <summary>
    /// Represents the controller for the Dashboard page.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The Rental Tracker DAL service.
        /// </summary>
        private IRentalTrackerService rentalTrackerService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rentalTrackerService">The instance of the RentalTrackerService.</param>
        public HomeController(IRentalTrackerService rentalTrackerService)
        {
            this.rentalTrackerService = rentalTrackerService;
        }

        /// <summary>
        /// Gets the Dashboard view .
        /// </summary>
        /// <returns>The Dashboard view with a summary of the database entities.</returns>
        // GET: Index
        public ActionResult Index()
        {
            var viewModel = new DashboardViewModel()
            {
                NetTotal = rentalTrackerService.GetTotalOfAccountBalances(),
                NumberOfAccounts = rentalTrackerService.GetNumberOfAccounts(),
                NumberOfCategories = rentalTrackerService.GetNumberOfCategories(),
                NumberOfPayess = rentalTrackerService.GetNumberOfPayees(),
                NumberOfTransactions = rentalTrackerService.GetNumberOfTransactions()
            };

            return View(viewModel);
        }
    }
}
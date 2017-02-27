using RentalTracker.DAL;
using RentalTracker.Models;
using System.Web.Mvc;

namespace RentalTracker.Controllers
{
    public class HomeController : Controller
    {
        private IRentalTrackerService rentalTrackerService;

        public HomeController(IRentalTrackerService rentalTrackerService)
        {
            this.rentalTrackerService = rentalTrackerService;
        }

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
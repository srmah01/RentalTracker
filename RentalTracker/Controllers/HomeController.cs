using RentalTracker.DAL;
using RentalTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            //var summary = rentalTrackerService.GetSummary();
            var viewModel = new DashboardViewModel()
            {
                NetTotal = 1000.00m,
                NumberOfAccounts = 3,
                NumberOfCategories = 4,
                NumberOfPayess = 6,
                NumberOfTransactions = 5
            };

            return View(viewModel);
        }
    }
}
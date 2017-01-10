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

namespace RentalTracker.Controllers
{
    public class AccountsController : Controller
    {
        //private RentalTrackerContext db = new RentalTrackerContext();

        private IRentalTracker rentalTrackerService;

        public AccountsController(IRentalTracker rentalTrackerService)
        {
            this.rentalTrackerService = rentalTrackerService;
        }

        // GET: Accounts
        public ActionResult Index()
        {
            return View(rentalTrackerService.getAllAccounts());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
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
                rentalTrackerService.AddAccount(account);
                return RedirectToAction("Index");
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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Name,OpeningBalance")] Account account)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(account).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(account);
        //}

        // GET: Accounts/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Account account = db.Accounts.Find(id);
        //    if (account == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(account);
        //}

        // POST: Accounts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Account account = db.Accounts.Find(id);
        //    db.Accounts.Remove(account);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

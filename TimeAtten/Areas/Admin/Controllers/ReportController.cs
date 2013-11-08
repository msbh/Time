using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAtten.Models;

namespace TimeAtten.Areas.Admin.Controllers
{
    public class ReportController : Controller
    {
        private TimeAttenEntities1 db = new TimeAttenEntities1();

        //
        // GET: /Admin/Report/

        public ActionResult Index()
        {
            return View(db.ACTIVEEMPRPT.ToList());
        }

        //
        // GET: /Admin/Report/Details/5

        public ActionResult Details(string id = null)
        {
            ACTIVEEMPRPT activeemprpt = db.ACTIVEEMPRPT.Find(id);
            if (activeemprpt == null)
            {
                return HttpNotFound();
            }
            return View(activeemprpt);
        }

        //
        // GET: /Admin/Report/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Report/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ACTIVEEMPRPT activeemprpt)
        {
            if (ModelState.IsValid)
            {
                db.ACTIVEEMPRPT.Add(activeemprpt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(activeemprpt);
        }

        //
        // GET: /Admin/Report/Edit/5

        public ActionResult Edit(string id = null)
        {
            ACTIVEEMPRPT activeemprpt = db.ACTIVEEMPRPT.Find(id);
            if (activeemprpt == null)
            {
                return HttpNotFound();
            }
            return View(activeemprpt);
        }

        //
        // POST: /Admin/Report/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ACTIVEEMPRPT activeemprpt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activeemprpt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(activeemprpt);
        }

        //
        // GET: /Admin/Report/Delete/5

        public ActionResult Delete(string id = null)
        {
            ACTIVEEMPRPT activeemprpt = db.ACTIVEEMPRPT.Find(id);
            if (activeemprpt == null)
            {
                return HttpNotFound();
            }
            return View(activeemprpt);
        }

        //
        // POST: /Admin/Report/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ACTIVEEMPRPT activeemprpt = db.ACTIVEEMPRPT.Find(id);
            db.ACTIVEEMPRPT.Remove(activeemprpt);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
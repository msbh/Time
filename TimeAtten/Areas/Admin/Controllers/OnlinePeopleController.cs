using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAtten.Models;
using TimeAtten.Services.Services;

namespace TimeAtten.Areas.Admin.Controllers
{
    public class OnlinePeopleController : Controller
    {
        UserServices UserServc = new UserServices();
        BreadcrumbService BreadServc = new BreadcrumbService();
        private TimeAttenEntities1 db = new TimeAttenEntities1();

        //
        // GET: /Admin/OnlinePeople/

        public ActionResult Index()
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Online People";
            string Subtitle = "Online People";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Online People", title, Subtitle);

            return View(db.Online.ToList());
        }

        //
        // GET: /Admin/OnlinePeople/Details/5

        public ActionResult Details(int id = 0)
        {
            Online online = db.Online.Find(id);
            if (online == null)
            {
                return HttpNotFound();
            }
            return View(online);
        }

        //
        // GET: /Admin/OnlinePeople/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/OnlinePeople/Create

        [HttpPost]
        public ActionResult Create(Online online)
        {
            if (ModelState.IsValid)
            {
                db.Online.Add(online);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(online);
        }

        //
        // GET: /Admin/OnlinePeople/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Online online = db.Online.Find(id);
            if (online == null)
            {
                return HttpNotFound();
            }
            return View(online);
        }

        //
        // POST: /Admin/OnlinePeople/Edit/5

        [HttpPost]
        public ActionResult Edit(Online online)
        {
            if (ModelState.IsValid)
            {
                db.Entry(online).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(online);
        }

        //
        // GET: /Admin/OnlinePeople/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Online online = db.Online.Find(id);
            if (online == null)
            {
                return HttpNotFound();
            }
            return View(online);
        }

        //
        // POST: /Admin/OnlinePeople/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Online online = db.Online.Find(id);
            db.Online.Remove(online);
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
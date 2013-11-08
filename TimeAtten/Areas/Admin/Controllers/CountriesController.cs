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
    public class CountriesController : Controller
    {

        UserServices UserServc = new UserServices();
        BreadcrumbService BreadServc = new BreadcrumbService();
        private TimeAttenEntities1 db = new TimeAttenEntities1();

        //
        // GET: /Admin/Countries/

        public ActionResult Index()
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 ))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Country";
            string Subtitle = "Country";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Country", title, Subtitle);
            return View(db.NATION.ToList().Where(item => item.IsActive == true));
        }

        //
        // GET: /Admin/Countries/Details/5

        public ActionResult Details(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Country Detail";
            string Subtitle = "Country Detail";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Country Detail", title, Subtitle);
            NATION nation = db.NATION.Find(id);
            if (nation == null)
            {
                return HttpNotFound();
            }
            return View(nation);
        }

        //
        // GET: /Admin/Countries/Create

        public ActionResult Create()
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Country Create";
            string Subtitle = "Country Create";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Country Create", title, Subtitle);
            return View();
        }

        //
        // POST: /Admin/Countries/Create

        [HttpPost]
        public ActionResult Create(NATION nation)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 ))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Country";
            string Subtitle = "Country";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Country", title, Subtitle);
            nation.IsActive = true;
            nation.CreatedBy = SessionService.Current.LoginId;
            nation.CreatedDate = DateTime.Now.ToString();
            //if (ModelState.IsValid)
            {
                db.NATION.Add(nation);
                db.SaveChanges();
                UserServc.AuditLogEntry("Created", "", Convert.ToInt32(SessionService.Current.LoginId), "Country id =" + nation.Nation_Code + " Created ", "Countries");
                return RedirectToAction("Index");
            }
            //  return View(nation);
        }

        //
        // GET: /Admin/Countries/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Country Edit";
            string Subtitle = "Country Edit";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Country Edit", title, Subtitle);
            NATION nation = db.NATION.Find(id);
            if (nation == null)
            {
                return HttpNotFound();
            }
            return View(nation);
        }

        //
        // POST: /Admin/Countries/Edit/5

        [HttpPost]
        public ActionResult Edit(NATION nation)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Country";
            string Subtitle = "Country";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Country", title, Subtitle);
            nation.IsActive = true;
            nation.EditedBy = SessionService.Current.LoginId;
            nation.EditedDate = DateTime.Now.ToString();
            if (ModelState.IsValid)
            {
                db.Entry(nation).State = EntityState.Modified;
                db.SaveChanges();
                UserServc.AuditLogEntry("Edited", "", Convert.ToInt32(SessionService.Current.LoginId), "Country id =" + nation.Nation_Code + " edited ", "Countries");
                return RedirectToAction("Index");
            }
            return View(nation);
        }

        //
        // GET: /Admin/Countries/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Country Delete";
            string Subtitle = "Country Delete";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Country Delete", title, Subtitle);

            NATION nation = db.NATION.Find(id);
            if (nation == null)
            {
                return HttpNotFound();
            }
            return View(nation);
        }

        //
        // POST: /Admin/Countries/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 ))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Country";
            string Subtitle = "Country";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Country", title, Subtitle);
            NATION nation = db.NATION.Find(id);
            nation.IsActive = false;
            nation.EditedBy = SessionService.Current.LoginId;
            nation.EditedDate = DateTime.Now.ToString();
            db.Entry(nation).State = EntityState.Modified;
            db.SaveChanges();
            UserServc.AuditLogEntry("Deleted", "", Convert.ToInt32(SessionService.Current.LoginId), "Country id =" + nation.Nation_Code + " deleted ", "Countries");
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
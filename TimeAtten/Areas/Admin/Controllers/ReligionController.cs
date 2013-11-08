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
    public class ReligionController : Controller
    {
        UserServices UserServc = new UserServices();
        BreadcrumbService BreadServc = new BreadcrumbService();
        private TimeAttenEntities1 db = new TimeAttenEntities1();

        //
        // GET: /Admin/Religion/

        public ActionResult Index()
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Religion";
            string Subtitle = "Religion";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Religion", title, Subtitle);
            return View(db.RELIGIONS.ToList().Where(item => item.IsActive == true));
        }

        //
        // GET: /Admin/Religion/Details/5

        public ActionResult Details(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Religion Detail";
            string Subtitle = "Religion Detail";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Religion Detail", title, Subtitle);
            RELIGIONS religions = db.RELIGIONS.Find(id);
            if (religions == null)
            {
                return HttpNotFound();
            }
            return View(religions);
        }

        //
        // GET: /Admin/Religion/Create

        public ActionResult Create()
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Religion Create";
            string Subtitle = "Religion Create";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Religion Create", title, Subtitle);
            return View();
        }

        //
        // POST: /Admin/Religion/Create

        [HttpPost]
        public ActionResult Create(RELIGIONS religions)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Religion";
            string Subtitle = "Religion ";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Religion ", title, Subtitle);
            religions.IsActive = true;
            religions.CreatedBy = SessionService.Current.LoginId;
            religions.CreatedDate = DateTime.Now.ToString();
            // if (ModelState.IsValid)
            {
                db.RELIGIONS.Add(religions);
                db.SaveChanges();
                UserServc.AuditLogEntry("Created", "", Convert.ToInt32(SessionService.Current.LoginId), "Religion id =" + religions.Relig_Code + " Created ", "Religion");
                return RedirectToAction("Index");
            }

            //  return View(religions);
        }

        //
        // GET: /Admin/Religion/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Religion Edit";
            string Subtitle = "Religion Edit";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Religion Edit", title, Subtitle);
            RELIGIONS religions = db.RELIGIONS.Find(id);
            if (religions == null)
            {
                return HttpNotFound();
            }
            return View(religions);
        }

        //
        // POST: /Admin/Religion/Edit/5

        [HttpPost]
        public ActionResult Edit(RELIGIONS religions)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Religion";
            string Subtitle = "Religion";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Religion", title, Subtitle);
            religions.IsActive = true;
            religions.EditedBy = SessionService.Current.LoginId;
            religions.EditedDate = DateTime.Now.ToString();
            if (ModelState.IsValid)
            {
                db.Entry(religions).State = EntityState.Modified;
                db.SaveChanges();
                UserServc.AuditLogEntry("Edited", "", Convert.ToInt32(SessionService.Current.LoginId), "Religion id =" + religions.Relig_Code + " edited ", "Religion");
                return RedirectToAction("Index");
            }
            return View(religions);
        }

        //
        // GET: /Admin/Religion/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Religion Delete";
            string Subtitle = "Religion Delete";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Religion Delete", title, Subtitle);

            RELIGIONS religions = db.RELIGIONS.Find(id);

            if (religions == null)
            {
                return HttpNotFound();
            }
            return View(religions);
        }

        //
        // POST: /Admin/Religion/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Religion";
            string Subtitle = "Religion";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Religion", title, Subtitle);
            RELIGIONS religions = db.RELIGIONS.Find(id);
            religions.IsActive = false;
            religions.EditedBy = SessionService.Current.LoginId;
            religions.EditedDate = DateTime.Now.ToString();
            db.Entry(religions).State = EntityState.Modified;
            db.SaveChanges();
            UserServc.AuditLogEntry("Deleted", "", Convert.ToInt32(SessionService.Current.LoginId), "Religion id =" + religions.Relig_Code + " deleted ", "Religion");
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
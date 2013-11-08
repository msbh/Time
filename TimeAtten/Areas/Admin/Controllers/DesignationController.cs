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
    public class DesignationController : Controller
    {
        UserServices UserServc = new UserServices();
        BreadcrumbService BreadServc = new BreadcrumbService();
        private TimeAttenEntities1 db = new TimeAttenEntities1();
        //
        // GET: /Admin/Designation/

        public ActionResult Index()
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Designation";
            string Subtitle = "Designation";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Designation", title, Subtitle);
            return View(db.DESIGS.ToList().Where(item => item.IsActive == true));
        }

        //
        // GET: /Admin/Designation/Details/5

        public ActionResult Details(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Designation Detail";
            string Subtitle = "Designation Detail";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Designation Detail", title, Subtitle);
            DESIGS desigs = db.DESIGS.Find(id);
            if (desigs == null)
            {
                return HttpNotFound();
            }
            if (desigs.IsActive == false)
            { return HttpNotFound(); }
            return View(desigs);
        }

        //
        // GET: /Admin/Designation/Create

        public ActionResult Create()
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Designation Create";
            string Subtitle = "Designation Create";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Designation Create", title, Subtitle);
            return View();
        }

        //
        // POST: /Admin/Designation/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DESIGS desigs)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Designation";
            string Subtitle = "Designation";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Designation", title, Subtitle);
            desigs.IsActive = true;
            desigs.CreatedBy = SessionService.Current.LoginId;
            desigs.CreatedDate = DateTime.Now.ToString();
            //if (ModelState.IsValid)
            {
                db.DESIGS.Add(desigs);
                db.SaveChanges();
                UserServc.AuditLogEntry("Created", "", Convert.ToInt32(SessionService.Current.LoginId), "Designation id =" + desigs.Desigs_Code + " Created ", "Designation");
                return RedirectToAction("Index");
            }

            // return View(desigs);
        }

        //
        // GET: /Admin/Designation/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Designation Edit";
            string Subtitle = "Designation Edit";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Designation Edit", title, Subtitle);
            DESIGS desigs = db.DESIGS.Find(id);
            if (desigs == null)
            {
                return HttpNotFound();
            }
            return View(desigs);
        }

        //
        // POST: /Admin/Designation/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DESIGS desigs)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Designation";
            string Subtitle = "Designation";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Designation", title, Subtitle);
            desigs.IsActive = true;
            desigs.EditedBy = SessionService.Current.LoginId;
            desigs.EditedDate = DateTime.Now.ToString();
            if (ModelState.IsValid)
            {
                db.Entry(desigs).State = EntityState.Modified;
                db.SaveChanges();
                UserServc.AuditLogEntry("Edited", "", Convert.ToInt32(SessionService.Current.LoginId), "Designation id =" + desigs.Desigs_Code + " edited ", "Designation");
                return RedirectToAction("Index");
            }
            return View(desigs);
        }

        //
        // GET: /Admin/Designation/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Designation Delete";
            string Subtitle = "Designation Delete";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Designation Delete", title, Subtitle);

            DESIGS desigs = db.DESIGS.Find(id);
            if (desigs == null)
            {
                return HttpNotFound();
            }
            return View(desigs);
        }

        //
        // POST: /Admin/Designation/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Designation";
            string Subtitle = "Designation";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Designation", title, Subtitle);
            DESIGS desigs = db.DESIGS.Find(id);
            desigs.IsActive = false;
            desigs.EditedBy = SessionService.Current.LoginId;
            desigs.EditedDate = DateTime.Now.ToString();
            db.Entry(desigs).State = EntityState.Modified;
            db.SaveChanges();
            UserServc.AuditLogEntry("Deleted", "", Convert.ToInt32(SessionService.Current.LoginId), "Designation id =" + desigs.Desigs_Code + " deleted ", "Designation");
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
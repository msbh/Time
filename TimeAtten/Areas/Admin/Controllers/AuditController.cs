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
    public class AuditController : Controller
    {
        private TimeAttenEntities1 db = new TimeAttenEntities1();
        UserServices UserServc = new UserServices();
        BreadcrumbService BreadServc = new BreadcrumbService();
        //
        // GET: /Admin/Audit/

        public ActionResult Index()
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl = HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Audit Log";
            string Subtitle = "Every Act is been Noted";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Audit Log", title, Subtitle);
            return View(db.AuditLog.ToList());
        }

        //
        // GET: /Admin/Audit/Details/5

        public ActionResult Details(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl = HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Audit Log Detail";
            string Subtitle = "Every Act is been Noted";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Audit Log Detail", title, Subtitle);

            AuditLog auditlog = db.AuditLog.Find(id);
            if (auditlog == null)
            {
                return HttpNotFound();
            }
            return View(auditlog);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
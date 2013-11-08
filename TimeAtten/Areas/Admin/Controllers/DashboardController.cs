using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAtten.Services.Services;

namespace TimeAtten.Areas.Admin.Controllers
{      

    public class DashboardController : Controller
    {
        BreadcrumbService BreadServc = new BreadcrumbService();
        //
        // GET: /Admin/Dashboard/
        //    [Authorize]
        
        public ActionResult Index()
        {
            if (SessionService.Current.LoginId == 0)
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Dashboard";
            string Subtitle = "Dashboard";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Dashboard", title, Subtitle);
           
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

    }
}

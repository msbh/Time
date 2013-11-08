using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAtten.Models;
using TimeAtten.Framework.Admin;
using TimeAtten.Services.Services;
using TimeAtten.Services.Utilities;

namespace TimeAtten.Areas.Admin.Controllers
{
    public class OwnerCreateController : Controller
    {
        UserServices UserServc = new UserServices();
        BreadcrumbService BreadServc = new BreadcrumbService();
        private TimeAttenEntities1 db = new TimeAttenEntities1();

        //
        // GET: /Admin/OwnerCreate/

        public ActionResult Index()
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 ))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Owner";
            string Subtitle = "Owner";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Owner", title, Subtitle);

            IEnumerable<Company> own = UserServc.GetOwnerCompanies();
            IEnumerable<User> usr = UserServc.GetOwnersList();
            var bothList = (from L1 in own
                            join L2 in usr
                         on L1.userId equals L2.ID
                            select new { L1, L2 });
            OwnerMultipleList tempList = new OwnerMultipleList();
            OwnerModal tempOwner = new OwnerModal();
            foreach (var vals in bothList)
            {
                tempOwner.id = vals.L2.ID.ToString();
                tempOwner.Username = vals.L2.UserName;
                tempOwner.Email = vals.L2.email;
                tempList.Company.Add(vals.L1);
                tempList.OwnerModal.Add(tempOwner);
                tempOwner = new OwnerModal();
            }
            return View(tempList);
        }

        //
        // GET: /Admin/OwnerCreate/Details/5

        public ActionResult Details(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            User user = db.User.Find(id);
            int userid = user.ID;
            Company Comp = UserServc.GetOwnerCompanyByUserId(userid);
            EMP_REC usrProf = UserServc.GetOwnerProfileByUserId(userid);
            OwnerCreation own = new OwnerCreation();
            own.Company = Comp;
            own.User = user;
            own.UserProfile = usrProf;
            #region Viewbags
            string title = "Owner Detail";
            string Subtitle = "Owner Detail";
            ViewBag.BreadCrums = BreadServc.DetailBreadCrumb("Owner Detail", title, Subtitle);
            try
            {
                ViewBag.Nation = UserServc.GetCountryById(Convert.ToInt32(usrProf.Nation_Code));
                ViewBag.Religion = UserServc.GetReligionById(Convert.ToInt32(usrProf.Relig_Code));
            }
            catch (Exception ert) { }
            #endregion
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(own);
        }

        //
        // GET: /Admin/OwnerCreate/Create

        public ActionResult Create()
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            #region Viewbags of BreadCrumb
            string title = "Create Owner";
            string Subtitle = "Create Owner";
            ViewBag.BreadCrums = BreadServc.EditBreadCrumb("Owner", title, Subtitle);
            #endregion
            #region Viewbags of Dropdowns
            ViewBag.countries = new CountriesModel();
            ViewBag.Religion = new ReligionModel();
            ViewBag.Qualification = new QualificationModel();
            #endregion
            return View();
        }

        //
        // POST: /Admin/OwnerCreate/Create

        [HttpPost]
        public ActionResult Create(OwnerCreation Owner)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            #region Viewbags of BreadCrumb
            string title = "Create Owner";
            string Subtitle = "Create Owner";
            ViewBag.BreadCrums = BreadServc.EditBreadCrumb("Owner", title, Subtitle);
            #endregion
            #region Viewbags of Dropdowns
            ViewBag.countries = new CountriesModel();
            ViewBag.Religion = new ReligionModel();
            ViewBag.Qualification = new QualificationModel();
            #endregion
            UserServices UserServ = new UserServices();
            User UserMod = new User();

            UserMod = UserServ.GetUser(Owner.User.UserName);

            if (UserServ.UserExists(Owner.User.UserName))
            {
                TempData["ErrorMessage"] = "UserName Already Exisit";
                return View();
            }
            else if (UserServ.EmailInUse(Owner.User.email))
            {
                TempData["ErrorMessage"] = "Email Already Exisit";
                return View();
            }
            else
            {
                try
                {
                    UserServ.CreateOwnerWithCompany(Owner);
                    return RedirectToAction("Index");
                }
                catch (Exception ee)
                {
                    TempData["ErrorMessage"] = "Some Problem Occured";
                    return View(Owner);
                }
            }
            return View(Owner);
        }

        //
        // GET: /Admin/OwnerCreate/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            User user = db.User.Find(id);
            int userid = user.ID;
            #region Viewbags of BreadCrumb
            string title = "Edit Owner";
            string Subtitle = "Edit Owner";
            ViewBag.BreadCrums = BreadServc.EditBreadCrumb("Owner", title, Subtitle);
            #endregion
            #region Viewbags of Dropdowns
            ViewBag.countries = new CountriesModel();
            ViewBag.Religion = new ReligionModel();
            ViewBag.Qualification = new QualificationModel();
            #endregion

            Company Comp = UserServc.GetOwnerCompanyByUserId(userid);
            EMP_REC usrProf = UserServc.GetOwnerProfileByUserId(userid);

            OwnerCreation own = new OwnerCreation();
            own.Company = Comp;
            own.User = user;
            own.UserProfile = usrProf;

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(own);
        }

        //
        // POST: /Admin/OwnerCreate/Edit/5

        [HttpPost]
        public ActionResult Edit(OwnerCreation owner)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            //if (ModelState.IsValid)
            User UserMod = new User();
            UserServices UserServ = new UserServices();

            UserMod = UserServ.GetUser(owner.User.UserName);

            if (UserServ.UserExists(owner.User.UserName))
            {
                TempData["ErrorMessage"] = "UserName Already Exisit";
                return View();
            }
            else if (UserServ.EmailInUse(owner.User.email))
            {
                TempData["ErrorMessage"] = "Email Already Exisit";
                return View();
            }
            else
            {
                string error = UserServc.EditOwner(owner);
                if (error != "1") { TempData["ErrorMessage"] = error; return View(owner); }
                return RedirectToAction("Index");
            }

        }

        //
        // GET: /Admin/OwnerCreate/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Owner Delete";
            string Subtitle = "Owner Delete";
            ViewBag.BreadCrums = BreadServc.DeleteBreadCrumb("Owner Delete", title, Subtitle);

            User user = db.User.Find(id);
            int userid = user.ID;
            Company Comp = UserServc.GetOwnerCompanyByUserId(userid);
            EMP_REC usrProf = UserServc.GetOwnerProfileByUserId(userid);

            OwnerCreation own = new OwnerCreation();
            own.Company = Comp;
            own.User = user;
            own.UserProfile = usrProf;

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(own);
        }

        //
        // POST: /Admin/OwnerCreate/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Owner";
            string Subtitle = "Owner";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Owner", title, Subtitle);

            UserServc.OwnerDelete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
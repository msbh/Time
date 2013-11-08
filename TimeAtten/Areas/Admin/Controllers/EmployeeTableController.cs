using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAtten.Framework.Admin;
using TimeAtten.Models;
using TimeAtten.Services.Services;

namespace TimeAtten.Areas.Admin.Controllers
{
    public class EmployeeTableController : Controller
    {
        UserServices UserServc = new UserServices();
        BreadcrumbService BreadServc = new BreadcrumbService();
        private TimeAttenEntities1 db = new TimeAttenEntities1();
        //
        // GET: /Admin/OwnerCreate/
        UserServices UserServ = new UserServices();
        public ActionResult Index()
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0))
            {
                if (!UserServc.CheckLogin())
                { return RedirectToAction("Index", "Dashboard"); }
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl = HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Employee";
            string Subtitle = "Employee";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Employee", title, Subtitle);

            IEnumerable<Company> own = UserServc.GetOwnerCompanies();
            IEnumerable<User> usr = UserServc.GetEmployeeList();

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
            User user = db.User.Find(id);
            int userid = user.ID;
            int compnyId = UserServ.GetCompnayIdByUserID(userid);
            Company Comp = db.Company.Where(item => item.CompanyId == compnyId).SingleOrDefault();
            EMP_REC usrProf = db.EMP_REC.Where(item => item.UserId == userid && item.IsActive == true).SingleOrDefault();

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
        // GET: /Admin/OwnerCreate/Create

        public ActionResult Create()
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl = HttpContext.Request.Url.AbsolutePath });
            }
            #region Viewbags of BreadCrumb
            string title = "Employee Entry";
            string Subtitle = "Employee Entry";
            ViewBag.BreadCrums = BreadServc.EditBreadCrumb("Employee", title, Subtitle);
            #endregion
            #region Viewbags of Dropdowns
            ViewBag.countries = new CountriesModel();
            ViewBag.Religion = new ReligionModel();
            ViewBag.Qualification = new QualificationModel();
            ViewBag.Companies = new CompaniesSelect();
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
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl = HttpContext.Request.Url.AbsolutePath });
            }
            #region Viewbags of BreadCrumb
            string title = "Employee Entry";
            string Subtitle = "Employee Entry";
            ViewBag.BreadCrums = BreadServc.EditBreadCrumb("Employee", title, Subtitle);
            #endregion
            #region Viewbags of Dropdowns
            ViewBag.countries = new CountriesModel();
            ViewBag.Religion = new ReligionModel();
            ViewBag.Qualification = new QualificationModel();
            ViewBag.Companies = new CompaniesSelect();
            #endregion
            User UserMod = new User();

            UserMod = UserServ.GetUser(Owner.User.UserName);

            if (UserMod.UserName == Owner.User.UserName)
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
                    UserServ.CreateEmployee(Owner);
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
            User user = db.User.Find(id);
            int userid = user.ID;
            int compnyId = UserServ.GetCompnayIdByUserID(userid);
            Company Comp = db.Company.Where(item => item.CompanyId == compnyId).SingleOrDefault();
            EMP_REC usrProf = db.EMP_REC.Where(item => item.UserId == userid && item.IsActive == true).SingleOrDefault();

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

            if (ModelState.IsValid)
            {
                db.Entry(owner.User).State = EntityState.Modified;
                db.Entry(owner.UserProfile).State = EntityState.Modified;
                db.Entry(owner.Company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(owner);
        }

        //
        // GET: /Admin/OwnerCreate/Delete/5

        public ActionResult Delete(int id = 0)
        {
            User user = db.User.Find(id);
            int userid = user.ID;
            int compnyId = UserServ.GetCompnayIdByUserID(userid);
            Company Comp = db.Company.Where(item => item.CompanyId == compnyId).SingleOrDefault();
            EMP_REC usrProf = db.EMP_REC.Where(item => item.UserId == userid && item.IsActive == true).SingleOrDefault();

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
            User user = db.User.Find(id);
            int userid = user.ID;
            int compnyId = UserServ.GetCompnayIdByUserID(userid);
            Company Comp = db.Company.Where(item => item.CompanyId == compnyId).SingleOrDefault();
            EMP_REC usrProf = db.EMP_REC.Where(item => item.UserId == userid && item.IsActive == true).SingleOrDefault();
            user.IsActive = false;
            usrProf.IsActive = false;

            db.Entry(user).State = EntityState.Modified;
            db.Entry(usrProf).State = EntityState.Modified;
            //db.User.Remove(user);
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
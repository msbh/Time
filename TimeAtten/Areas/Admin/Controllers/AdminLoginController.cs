using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAtten.Framework.Admin;
using TimeAtten.Services.Services;
using TimeAtten.Models;
using TimeAtten.Areas.Admin.Models;
using TimeAtten.Repositories;

namespace TimeAtten.Areas.Admin.Controllers
{
    public class AdminLoginController : Controller
    {
        //
        // GET: /Admin/AdminLogin/
        TimeAttenEntities1 db = new TimeAttenEntities1();
        BreadcrumbService BreadServc = new BreadcrumbService();
        UserServices _userServices = new UserServices();
        AdminLoginRepository rep = new AdminLoginRepository();

        //public AdminLoginController(
        //     UserServices userServices
        //    )
        // {
        //     _userServices = userServices;
        //  }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult delte()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register(string ReturnUrl)
        {
            if (_userServices.CheckLogin())
            {
                if (ReturnUrl != null)
                {
                    if (ReturnUrl != null)
                    {
                        return Redirect(ReturnUrl);
                    }
                }
                return RedirectToAction("Index", "Dashboard");
            }
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(BothLoginHeader registerV)
        {
           string ReturnUrl ="";try{ReturnUrl= TempData["ReturnUrl"].ToString();}catch(Exception ert){}
            TempData["ReturnUrl"] = ReturnUrl;
            if (_userServices.CheckLogin())
            {
                if (ReturnUrl != null)
                {
                    if (ReturnUrl != null)
                    {
                        return Redirect(ReturnUrl);
                    }
                }
                return RedirectToAction("Index", "Dashboard");
            }
            UserServices UserServ = new UserServices();
            User UserMod = new User();
            UserMod = UserServ.CheckUser(registerV.LoginViewModel.Username, registerV.LoginViewModel.Password);
            if (UserMod.UserName == registerV.LoginViewModel.Username)
            {
                UserServ.AuditLogEntry("Login", "", Convert.ToInt32(UserMod.ID), "User Login as Admin", "Login"); //Audit log
                UserServ.UserSession(UserMod);  //Sesions
                int pin = Convert.ToInt32(UserMod.PinCode);
                UserServ.LoginRoutine(pin);
                if (ReturnUrl != null)
                {
                    if (ReturnUrl != null)
                    {
                        return Redirect(ReturnUrl);
                    }
                }
                return Redirect(Url.Action("Index", "Dashboard"));
            }
            TempData["ErrorMessage"] = "Wrong User Credentials";
            TempData["ErrorMessage"] = "Wrong User Credentials";
            return RedirectToAction("Register", "AdminLogin", new { ReturnUrl = ReturnUrl });
        }
        public ActionResult Logout()
        {
           string ReturnUrl ="";try{ReturnUrl= TempData["ReturnUrl"].ToString();}catch(Exception ert){}
            TempData["ReturnUrl"] = ReturnUrl;
            UserServices UserServ = new UserServices();
            int pin = UserServ.UserSessionOut();
            UserServ.LogoutRoutine(pin);
            TempData["ErrorMessage"] = "Log out Successfull";
            UserServ.AuditLogEntry("Logout", "", Convert.ToInt32(SessionService.Current.LoginId), "User Logout as Super Admin", "Logout"); //Audit log                  
            return RedirectToAction("Register", "AdminLogin", new { ReturnUrl = ReturnUrl });
        }
        [HttpPost]
        public ActionResult forgot(BothLoginHeader registerV)
        {
           string ReturnUrl ="";try{ReturnUrl= TempData["ReturnUrl"].ToString();}catch(Exception ert){}
            TempData["ReturnUrl"] = ReturnUrl;
            UserServices UserServ = new UserServices();
            User UserMod = new User();
            if (UserServ.UserExists(registerV.ForgotPasswordViewModel.UsernameOrEmail.ToString()))
            {

                UserMod = UserServ.GetUser(registerV.ForgotPasswordViewModel.UsernameOrEmail);
                string newpassword = UserServ.forgotPasword(Convert.ToInt32(UserMod.PinCode));
                TempData["ErrorMessage"] = "New Password has been mailed to you email.";

                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl = ReturnUrl });
            }
            TempData["ErrorMessage"] = "Wrong User Credentials";
            return RedirectToAction("Register", "AdminLogin", new { ReturnUrl = ReturnUrl });
        }

        [HttpPost]
        public ActionResult Register(BothLoginHeader registerV)
        {
           string ReturnUrl ="";try{ReturnUrl= TempData["ReturnUrl"].ToString();}catch(Exception ert){}
            TempData["ReturnUrl"] = ReturnUrl;
            if (_userServices.CheckLogin())
            {
                if (ReturnUrl != null)
                {
                    if (ReturnUrl != null)
                    {
                        return Redirect(ReturnUrl);
                    }
                }
                return RedirectToAction("Index", "Dashboard");
            }
            //   db.User.FirstOrDefault();
            UserServices UserServ = new UserServices();
            User UserMod = new User();
            UserMod = UserServ.GetUser(registerV.RegisterViewModel.Username);

            if (UserMod.UserName == registerV.RegisterViewModel.Username)
            {
                TempData["ErrorMessage"] = "UserName Already Exisit";
                return View();
            }
            else if (UserServ.EmailInUse(registerV.RegisterViewModel.Email))
            {
                TempData["ErrorMessage"] = "Email Already Exisit";
                return View();
            }
            else
            {
                try
                {
                    User UserCreate = new User();
                    EMP_REC userProfile = new EMP_REC();
                    UserCreate.Password = UserServ.CreateMD5Hash(registerV.RegisterViewModel.Password);
                    UserCreate.UserName = registerV.RegisterViewModel.Username;
                    UserCreate.email = registerV.RegisterViewModel.Email;
                    UserCreate.IsActive = true;
                    UserCreate.CreatedDate = DateTime.Now.ToString();
                    UserCreate.IsSuperAdmin = 1;
                    UserCreate.IsAdmin = 0;
                    UserCreate.CreatedBy = "Super Admin";  //Add Here user who s making this admin

                    userProfile.First_Name = registerV.RegisterViewModel.FirstName;
                    userProfile.Last_Name = registerV.RegisterViewModel.LastName;
                    userProfile.Gender = registerV.RegisterViewModel.Sex;
                    userProfile.Email = registerV.RegisterViewModel.Email;
                    userProfile.City = registerV.RegisterViewModel.City;
                    //userProfile.Nation_Code = registerV.RegisterViewModel.City;

                    try
                    {
                        userProfile.B_Date = Convert.ToDateTime(registerV.RegisterViewModel.BirthdayDate).ToString();
                    }
                    catch (Exception ee) { userProfile.B_Date = registerV.RegisterViewModel.BirthdayDate; }
                    long Pincode = UserServ.GetLastProfileId();

                    userProfile.Pin_Code = Pincode;
                    int UserId = UserServ.GetLastUserId();
                    userProfile.UserId = UserId;
                    string pin = UserServ.AddUserProfile(userProfile);

                    UserCreate.PinCode = (long)Convert.ToDouble(Pincode);

                    UserCreate.ID = UserId;
                    UserServ.AddUser(UserCreate);
                    UserServ.AuditLogEntry("Register", "", Convert.ToInt32(UserId), "User registered as Super Admin", "Register");
                    string body = "Welcome Dear " + userProfile.First_Name + " to Zultime . Kindly enjoy using it";
                    UserServ.emailSend(userProfile.Email, "Registration Succesfull", body);

                    UserServ.AuditLogEntry("Login", "", Convert.ToInt32(UserMod.ID), "User Login as Super Admin", "Login"); //Audit log
                    UserServ.UserSession(UserCreate);  //Sesions


                    string title = "Dashboard";
                    string Subtitle = "Dashboard";
                    ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Dashboard", title, Subtitle);
                    if (ReturnUrl != null)
                    {
                        if (ReturnUrl != null)
                        {
                            return Redirect(ReturnUrl);
                        }
                    }
                    return Redirect(Url.Action("Index", "Dashboard"));
                }
                catch (Exception ee)
                {
                    TempData["ErrorMessage"] = "Some Problem Occured";
                    return View();
                }
            }
            //return View();
        }
    }
}

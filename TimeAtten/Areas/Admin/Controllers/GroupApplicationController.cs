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
    public class GroupApplicationController : Controller
    {
        UserServices UserServc = new UserServices();
        BreadcrumbService BreadServc = new BreadcrumbService();
        private TimeAttenEntities1 db = new TimeAttenEntities1();

        #region Group Role
        public ActionResult GroupCreate()
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Permission Group Create";
            string Subtitle = "Permission Group Create";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Permission Group Create", title, Subtitle);
            #region Viewbags of Dropdowns
            ViewBag.employees = new EmployeeSelect();
            ViewBag.userapplication = new UserApplicationSelect();
            ViewBag.companies = new CompaniesSelect();
            ViewBag.permissions = new PermissionSelect();
            #endregion
            return View();
        }
        //
        // POST: /Admin/GroupApplication/Create

        [HttpPost]
        public ActionResult GroupCreate(GroupRoleModel groupRoleModal, FormCollection formComp)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            #region forms Collection
            var employeselect = formComp["employeetype"];
            var AppSelect = formComp["basetype"];
            List<string> tempEmp = new List<string>(employeselect.Split(','));
            List<string> tempAppGrp = new List<string>(AppSelect.Split(','));
            int tempInt = 0;
            #endregion
            string title = "Permission Group";
            string Subtitle = "Permission Group";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Permission Group", title, Subtitle);
            #region Viewbags of Dropdowns
            ViewBag.employees = new EmployeeSelect();
            ViewBag.userapplication = new UserApplicationSelect();
            ViewBag.companies = new CompaniesSelect();
            ViewBag.permissions = new PermissionSelect();
            TempData["ErrorMessage"] = "";
            #endregion

            groupRoleModal.groupRole.IsActive = true;
            groupRoleModal.groupRole.CreatedBy = SessionService.Current.LoginId;
            groupRoleModal.groupRole.CreatedDate = DateTime.Now.ToString();
            groupRoleModal.groupRole.CompanyId = UserServc.GetParentCompaniesByEmployeeId(groupRoleModal.EmployeeId);

            //groupRoleModal.groupApplication.ApplicationId = groupRoleModal.ApplicationId;

            groupRoleModal.groupPermission.CreatedBy = SessionService.Current.LoginId;
            groupRoleModal.groupPermission.CreatedDate = DateTime.Now.ToString();
            groupRoleModal.groupPermission.IsActive = true;
            groupRoleModal.groupPermission.Permission = groupRoleModal.permission.ToString();

            groupRoleModal.groupUsers.CreatedBy = SessionService.Current.LoginId;
            groupRoleModal.groupUsers.CreatedDate = DateTime.Now.ToString();
            groupRoleModal.groupUsers.IsActive = true;

            if (ModelState.IsValid)
            {
                UserServc.CreateGroupRole(groupRoleModal.groupRole);
                int roleID = UserServc.GetLastGroupRoleID();
                int tempAppInt = 0;
                int tempIntCheckUser = 0;

                groupRoleModal.groupPermission.GroupRoleId = roleID;
                groupRoleModal.groupApplication.GroupRoleId = roleID;
                groupRoleModal.groupUsers.GroupID = roleID;

                UserServc.CreateGroupPermision(groupRoleModal);
                foreach (string selectVal in tempAppGrp)
                {
                    try
                    {
                        tempInt = Convert.ToInt32(selectVal);
                    }
                    catch (Exception e) { }
                    UserServc.CreateGroupUserApplication(roleID, tempInt);
                }

                foreach (string selectVal in tempEmp)
                {
                    try
                    {
                        tempInt = Convert.ToInt32(selectVal);
                    }
                    catch (Exception e) { }

                    foreach (string selectVal3 in tempAppGrp)
                    {
                        tempIntCheckUser = UserServc.CheckUserPermissionExists(tempInt, tempAppInt);
                        try
                        {
                            tempAppInt = Convert.ToInt32(selectVal3);
                        }

                        catch (Exception e) { tempAppInt = 0; tempIntCheckUser = 0; }
                        if (tempIntCheckUser == 1)
                        {
                            TempData["ErrorMessage"] = TempData["ErrorMessage"] + UserServc.GetUsername(tempInt) + "Group Permission Already Exisit for Derpatment Id=" + tempInt + ",";
                        }
                        else if (tempIntCheckUser == 0)
                        {
                            UserServc.CreateGroupUsers(groupRoleModal, tempInt);
                            UserServc.AuditLogEntry("Created " + groupRoleModal.permission.ToString(), "", tempInt, "Permission Group Created by Admin group id " + roleID, "Permission Group");
                        }
                    }
                }

                UserServc.AuditLogEntry("Created " + groupRoleModal.permission.ToString(), "", Convert.ToInt32(SessionService.Current.LoginId), "Permission Group Created by Admin group id " + roleID, "Permission Group");
                UserServc.AuditLogEntry("Created " + groupRoleModal.permission.ToString(), "", groupRoleModal.CompanyId, "Permission Group Created by Admin group id " + roleID, "Permission Group");
                return RedirectToAction("IndexGroup");
            }
            title = "Permission Group Create";
            Subtitle = "Permission Group Create";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Permission Group Create", title, Subtitle);
            return View(groupRoleModal);
        }
        public ActionResult IndexGroup()
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
               string dasd= HttpContext.Request.Url.AbsolutePath;
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Permission Group ";
            string Subtitle = "Permission Group ";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Permission Group ", title, Subtitle);
            GroupRoleModel groupRoleModal = new GroupRoleModel();
            UserServc.GetGroupUserPermissionforIndex(groupRoleModal);
            if (groupRoleModal != null)
            {
                return View(groupRoleModal);
            }
            else
            { return RedirectToAction("IndexGroup", "Dashboard"); }
        }
        public ActionResult GroupEdit(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Permission Group Edit";
            string Subtitle = "Permission Group Edit";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Permission Group Edit", title, Subtitle);
            #region Viewbags of Dropdowns
            ViewBag.employees = new EmployeeSelect();
            ViewBag.userapplication = new UserApplicationSelect();
            ViewBag.companies = new CompaniesSelect();
            ViewBag.permissions = new PermissionSelect();
            #endregion
            #region forms Collection

            ViewData["employeetype"] = UserServc.GetGroupUserbyGroupRoleId(1);
            // var AppSelect = formComp["basetype"];

            #endregion
            GroupRoleModel groupRoleModal = new GroupRoleModel();
            groupRoleModal.groupRole = UserServc.GetGroupRolebyId(id);
            groupRoleModal.groupPermission = UserServc.GetGroupPermissionbyGroupRoleId(id);
            if (groupRoleModal.groupPermission != null)
                groupRoleModal.permission = groupRoleModal.groupPermission.GroupPermissionId;
            if (groupRoleModal.groupRole == null)
            {
                return HttpNotFound();
            }
            return View(groupRoleModal);
        }

        //
        // POST: /Admin/GroupApplication/Edit/5
        [HttpPost]
        public ActionResult GroupEdit(int id, FormCollection formComp)
        {

            //groupRoleModal.groupApplication=
            #region forms Collection
            var employeselect = formComp["employeetype"];
            var AppSelect = formComp["basetype"];
            List<string> tempEmp = new List<string>(employeselect.Split(','));
            List<string> tempAppGrp = new List<string>(AppSelect.Split(','));
            int tempInt = 0;
            #endregion
            string title = "Permission Group";
            string Subtitle = "Permission Group";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Permission Group", title, Subtitle);
            #region Viewbags of Dropdowns
            ViewBag.employees = new EmployeeSelect();
            ViewBag.userapplication = new UserApplicationSelect();
            ViewBag.companies = new CompaniesSelect();
            ViewBag.permissions = new PermissionSelect();
            TempData["ErrorMessage"] = "";
            #endregion

            GroupRoleModel groupRoleModal = new GroupRoleModel();
            groupRoleModal.groupRole = UserServc.GetGroupRolebyId(id);
            groupRoleModal.groupPermission = UserServc.GetGroupPermissionbyGroupRoleId(id);


            groupRoleModal.groupRole.EditedBy = SessionService.Current.LoginId;
            groupRoleModal.groupRole.EditedDate = DateTime.Now.ToString();
            groupRoleModal.groupRole.CompanyId = UserServc.GetParentCompaniesByEmployeeId(groupRoleModal.EmployeeId);

            //groupRoleModal.groupApplication.ApplicationId = groupRoleModal.ApplicationId;

            groupRoleModal.groupPermission.EditedBy = SessionService.Current.LoginId;
            groupRoleModal.groupPermission.EditedDate = DateTime.Now.ToString();
            groupRoleModal.groupPermission.Permission = groupRoleModal.permission.ToString();

            groupRoleModal.groupUsers.EditedBy = SessionService.Current.LoginId;
            groupRoleModal.groupUsers.EditedDate = DateTime.Now.ToString();

            if (ModelState.IsValid)
            {
                UserServc.EditGroupRole(groupRoleModal.groupRole);
                int tempAppInt = 0;
                int tempIntCheckUser = 0;

                groupRoleModal.groupApplication.GroupRoleId = id;
                groupRoleModal.groupUsers.GroupID = id;

                UserServc.EditGroupPermision(groupRoleModal);

                foreach (string selectVal in tempAppGrp)
                {
                    try
                    {
                        tempInt = Convert.ToInt32(selectVal);
                    }
                    catch (Exception e) { }
                    UserServc.CreateGroupUserApplication(id, tempInt);
                }

                foreach (string selectVal in tempEmp)
                {
                    try
                    {
                        tempInt = Convert.ToInt32(selectVal);
                    }
                    catch (Exception e) { }

                    foreach (string selectVal3 in tempAppGrp)
                    {
                        tempIntCheckUser = UserServc.CheckUserPermissionExists(tempInt, tempAppInt);
                        try
                        {
                            tempAppInt = Convert.ToInt32(selectVal3);
                        }

                        catch (Exception e) { tempAppInt = 0; tempIntCheckUser = 0; }
                        if (tempIntCheckUser == 1)
                        {
                            TempData["ErrorMessage"] = TempData["ErrorMessage"] + UserServc.GetUsername(tempInt) + "Group Permission Already Exisit for Derpatment Id=" + tempInt + ",";
                        }
                        else if (tempIntCheckUser == 0)
                        {
                            UserServc.CreateGroupUsers(groupRoleModal, tempInt);
                            UserServc.AuditLogEntry("Edited " + groupRoleModal.permission.ToString(), "", tempInt, "Permission Group Edited by Admin group id " + id, "Permission Group");
                        }
                    }
                }

                UserServc.AuditLogEntry("Edited " + groupRoleModal.permission.ToString(), "", Convert.ToInt32(SessionService.Current.LoginId), "Permission Group Edited by Admin group id " + id, "Permission Group");
                UserServc.AuditLogEntry("Edited " + groupRoleModal.permission.ToString(), "", groupRoleModal.CompanyId, "Permission Group Edited by Admin group id " + id, "Permission Group");
                return RedirectToAction("Index");
            }
            title = "Permission Group Edit";
            Subtitle = "Permission Group Edit";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Permission Group Edit", title, Subtitle);
            return View(groupRoleModal);
        }

        //
        // GET: /Admin/GroupApplication/Delete/5
        public ActionResult GroupDelete(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Permission Group Delete";
            string Subtitle = "Permission Group Delete";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Permission Group Delete", title, Subtitle);
            GroupRoleModel groupRoleModal = new GroupRoleModel();
            groupRoleModal=UserServc.GetGroupUserDetails(id);

            if (groupRoleModal.groupRole == null)
            {
                return HttpNotFound();
            }
            return View(groupRoleModal);
        }

        //
        // POST: /Admin/GroupApplication/Delete/5

        [HttpPost, ActionName("DeleteGroup")]
        public ActionResult GroupDeleteConfirmed(int id)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            GroupRole group_role = UserServc.GetGroupRolebyId(id);

            group_role.IsActive = false;

            GroupPermission grpPerm = UserServc.GetGroupPermissionbyGroupRoleId(id);


            grpPerm.IsActive = false;
            UserServc.DeleteGroupRole(group_role);
            UserServc.DeleteGroupPermision(grpPerm);

            return RedirectToAction("IndexGroup");
        }

        public ActionResult GroupDetails(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 || SessionService.Current.Admin == 1))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Permission Group Detail";
            string Subtitle = "Permission Group Detail";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Permission Group Detail", title, Subtitle);
            GroupRoleModel groupRoleModal = new GroupRoleModel();
           groupRoleModal= UserServc.GetGroupUserDetails(id);
            if (groupRoleModal.groupRole == null)
            {
                return HttpNotFound();
            }
            return View(groupRoleModal);
        }
        #endregion

        #region Application Modules
        //
        // GET: /Admin/GroupApplication/
        public ActionResult Index()
        {
            if (!UserServc.CheckLogin())
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            if (SessionService.Current.Admin == 0)
            {
                string title = "Applications";
                string Subtitle = "Applications";
                ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Applications", title, Subtitle);
                return View(db.User_Application.ToList().Where(item => item.IsActive == true));
            }
            else
            { return RedirectToAction("Index", "Dashboard"); }
        }
        //
        // GET: /Admin/GroupApplication/Details/5

        public ActionResult Details(int id = 0)
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0 ))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Applications";
            string Subtitle = "Applications";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Applications", title, Subtitle);
            User_Application user_application = db.User_Application.Find(id);
            if (user_application == null)
            {
                return HttpNotFound();
            }
            return View(user_application);
        }

        //
        // GET: /Admin/GroupApplication/Create

        public ActionResult Create()
        {
            if (!UserServc.CheckLogin() || !(SessionService.Current.Admin == 0))
            {
                return RedirectToAction("Register", "AdminLogin", new { ReturnUrl= HttpContext.Request.Url.AbsolutePath });
            }
            string title = "Applications Create";
            string Subtitle = "Applications Create";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Applications Create", title, Subtitle);
            return View();
        }


        //
        // POST: /Admin/GroupApplication/Create

        [HttpPost]
        public ActionResult Create(User_Application user_application)
        {
            string title = "Applications";
            string Subtitle = "Applications";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Applications", title, Subtitle);
            user_application.IsActive = true;
            user_application.CreatedBy = SessionService.Current.LoginId;
            user_application.CreatedDate = DateTime.Now.ToString();
            if (ModelState.IsValid)
            {
                db.User_Application.Add(user_application);
                db.SaveChanges();
                UserServc.AuditLogEntry("Created", "", Convert.ToInt32(SessionService.Current.LoginId), "Application Created by Admin", "Application");
                return RedirectToAction("Index");
            }
            return View(user_application);
        }

        //
        // GET: /Admin/GroupApplication/Edit/5

        public ActionResult Edit(int id = 0)
        {
            string title = "Applications Edit";
            string Subtitle = "Applications Edit";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Applications Edit", title, Subtitle);

            User_Application user_application = db.User_Application.Find(id);
            if (user_application == null)
            {
                return HttpNotFound();
            }
            return View(user_application);
        }

        //
        // POST: /Admin/GroupApplication/Edit/5

        [HttpPost]
        public ActionResult Edit(User_Application user_application)
        {
            string title = "Applications";
            string Subtitle = "Applications";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Applications", title, Subtitle);
            user_application.IsActive = true;
            user_application.EditedBy = SessionService.Current.LoginId;
            user_application.EditedDate = DateTime.Now.ToString();
            if (ModelState.IsValid)
            {
                db.Entry(user_application).State = EntityState.Modified;
                db.SaveChanges();
                UserServc.AuditLogEntry("Edited", "", Convert.ToInt32(SessionService.Current.LoginId), "Application id =" + user_application.ApplicationId + " edited ", "Application");
                return RedirectToAction("Index");
            }
            return View(user_application);
        }

        //
        // GET: /Admin/GroupApplication/Delete/5

        public ActionResult Delete(int id = 0)
        {
            string title = "Applications Delete";
            string Subtitle = "Applications Delete";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Applications Delete", title, Subtitle);

            User_Application user_application = db.User_Application.Find(id);
            if (user_application == null)
            {
                return HttpNotFound();
            }
            return View(user_application);
        }

        //
        // POST: /Admin/GroupApplication/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            string title = "Applications";
            string Subtitle = "Applications";
            ViewBag.BreadCrums = BreadServc.IndexBreadCrumb("Applications", title, Subtitle);
            User_Application user_application = db.User_Application.Find(id);
            user_application.IsActive = false;
            user_application.EditedBy = SessionService.Current.LoginId;
            user_application.EditedDate = DateTime.Now.ToString();
            db.Entry(user_application).State = EntityState.Modified;
            db.SaveChanges();
            UserServc.AuditLogEntry("Deleted", "", Convert.ToInt32(SessionService.Current.LoginId), "Application id =" + user_application.ApplicationId + " Deleted ", "Application");
            return RedirectToAction("Index");
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
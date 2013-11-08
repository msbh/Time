using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using TimeAtten.Models;
using TimeAtten.Services.Services;

using TimeAtten.Repositories;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Net;
using TimeAtten.Framework.Admin;
using System.Web.Mvc;

namespace TimeAtten.Services.Services
{
    public class UserServices
    {
        IRepository<User> _userRepository;
        IRepository<EMP_REC> _empRepository;

        TimeAttenEntities1 db = new TimeAttenEntities1();

        private string GetUserIP()
        {
            try
            {
                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip))
                {
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                return ip;
            }
            catch (Exception ee) { return "No ip Found"; }
        }

        public bool CheckLogin()
        {
            if (SessionService.Current.LoginId != 0)
            {
                return true;
            }
            //LogoutRoutine();
            return false;
        }

        public bool emailSend(string email, string subject, string body)
        {
            string smtpUrl = "";
            string smtpUser = "";
            string smtpPwd = "";
            string smtpFrom = "";
            try
            {
                smtpUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["smtpUrl"];
                smtpUser = System.Web.Configuration.WebConfigurationManager.AppSettings["smtpUser"];
                smtpPwd = System.Web.Configuration.WebConfigurationManager.AppSettings["smtpPwd"];
                smtpFrom = System.Web.Configuration.WebConfigurationManager.AppSettings["smtpFrom"];

            }
            catch (Exception ee)
            {
                smtpUrl = "";
                smtpUser = "";
                smtpPwd = "";
                smtpFrom = "";
            }
            try
            {
                var client = new SmtpClient(smtpUrl, 587)
                    {
                        Credentials = new NetworkCredential(smtpUser, smtpPwd),
                        EnableSsl = true
                    };
                client.Send(smtpFrom, email, subject, body);
                return true;
            }
            catch (Exception eee)
            {
                return false;
            }
        }
        public IEnumerable<EMP_REC> GetBirthdays(string date)
        {
            UserProfile userP = new UserProfile();
            return db.EMP_REC.Where(item => item.B_Date == date).Select(item => item);

        }
        public int GetLastAuditId()
        {
            try
            {
                int lastProductId = db.AuditLog.Max(item => item.Id);
                return lastProductId + 1;
            }
            catch (Exception ee) { return 1; }
        }
        public int GetCompnayIdByUserID(int userid)
        {
            try
            {
                int compnyId = db.Company.First(item => item.userId.Equals(userid)).CompanyId;
                return compnyId;
            }
            catch (Exception ee) { return 1; }
        }
        public bool AuditLogEntry(string Action, string obj, int userid, string desc, string Models)
        {
            try
            {
                AuditLog logEntry = new AuditLog();
                int id = GetLastAuditId();
                logEntry.Id = id;
                logEntry.Date = DateTime.Now.ToString();
                logEntry.Action = Action;
                logEntry.Object = obj;
                logEntry.userId = userid;
                logEntry.Description = desc;
                logEntry.Module = Models;
                logEntry.ip = GetUserIP();
                logEntry.FirstName = GetUsername(userid);
                db.AuditLog.Add(logEntry);
                db.SaveChanges();
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        public void UserSession(User usr)
        {
            try
            {
                //int loginId = MySession.Current.LoginId;
                //string property1 = MySession.Current.Property1;
                //MySession.Current.Property1 = newValue;
                //DateTime myDate = MySession.Current.MyDate;
                //MySession.Current.MyDate = DateTime.Now;   
                SessionService.Current.UserPin = Convert.ToInt32(usr.PinCode);
                SessionService.Current.LoginName = usr.UserName;
                SessionService.Current.MyDate = DateTime.Now;
                SessionService.Current.LoginId = usr.ID;
                if (usr.IsSuperAdmin == 1)
                {
                    SessionService.Current.Admin = 0;
                }
                else if (usr.IsAdmin == 1)
                {
                    SessionService.Current.Admin = 1;
                }
                else
                {
                    SessionService.Current.Admin = 2;
                }
                SessionService.Current.CompanyId = GetCompnayIdByUserID(usr.ID);
            }
            catch (Exception ee) { }
        }
        public int UserSessionOut()
        {
            int pin = 0;
            try
            {
                pin = SessionService.Current.UserPin;
                SessionService.Current.UserPin = 0;
                SessionService.Current.LoginId = 0;
                SessionService.Current.CompanyId = 0;
                SessionService.Current.LoginName = "No";
                SessionService.Current.MyDate = DateTime.Now;
                return pin;
            }
            catch (Exception ee)
            {
            }
            return pin;
        }
        public bool AddUser(User useR)
        {
            try
            {
                User us = db.User.Add(useR);
                db.SaveChanges();
            }
            catch (Exception ee)
            { return false; }
            return true;
        }
        public string AddUserProfile(EMP_REC emp)
        {
            TimeAttenEntities1 db2 = new TimeAttenEntities1();
            string temPin = "";
            try
            {
                db2.EMP_REC.Add(emp);
                db2.SaveChanges();
            }
            catch (Exception ee)
            { return "0"; }
            try
            {
                EMP_REC empPin = db2.EMP_REC.Where(item => item.Email == emp.Email).FirstOrDefault();
                temPin = empPin.Pin_Code.ToString();
            }
            catch (Exception ew)
            {
                temPin = "0";
            }
            return temPin;
        }
        public bool ActivateUser(string userName, string activationCode)
        {
            //if (!UserExists(userName))
            //    return false;

            //User user = GetUser(userName);

            //if (string.IsNullOrEmpty(user.ActivationCode))
            //    return true;

            //if (user.ActivationCode == activationCode)
            //{
            //    user.ActivationCode = string.Empty;
            //    user.IsActive = true;
            //    _userRepository.Update(user);
            //    return true;
            //}
            //else
            return false;
        }

        public bool UserExists(string userNameorID)
        {
            if (string.IsNullOrWhiteSpace(userNameorID))
                return false;

            int userID;
            User user;
            bool IsInt = int.TryParse(userNameorID, out userID);
            if (IsInt)
                user = GetUser(userID);
            else
                try
                {
                    user = db.User.First(item => item.UserName.Equals(userNameorID));
                }
                catch (Exception e) { return false; }
            return user != null;
        }

        public bool ValidatePassword(string userName, string password)
        {
            User user = GetUser(userName);
            return ValidatePassword(user, password);

        }

        public bool ValidatePassword(int userID, string password)
        {
            User user = GetUser(userID);
            return ValidatePassword(user, password);
        }

        public bool ValidatePassword(User user, string password)
        {
            string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password + user.Password, "MD5");
            return user.Password == hashedPassword;
        }

        public bool EmailInUse(string email)
        {
            try
            {
                return db.User.First(item => item.email.Equals(email)) != null;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        //public bool GoogleEmailInUse(string email)
        //{
        //    return db.User.First(item => item.email.Equals(email) && item.IsGoogleUser == true) != null;
        //}
        public int GetLastOnlineId()
        {
            try
            {
                int lastProductId = db.Online.Max(item => item.id);
                return lastProductId + 1;
            }
            catch (Exception ee) { return 1; }
        }
        public void LoginRoutine(int userPin)
        {
            Online onlineUser = new Online();
            onlineUser.OnlineTime = DateTime.Now.ToString();
            onlineUser.ip = GetUserIP();
            onlineUser.OnlinePin = userPin;
            onlineUser.id = GetLastOnlineId();
            try
            {
                Online us = db.Online.Add(onlineUser);
                db.SaveChanges();
            }
            catch (Exception ee)
            { }
        }

        public void LogoutRoutine(int userPin)
        {
            try
            {
                Online onlineUser = db.Online.Where(item => item.OnlinePin == userPin).FirstOrDefault();
                Online us = db.Online.Remove(onlineUser);
                db.SaveChanges();
            }
            catch (Exception ee)
            { }

            //IEnumerable<ThreadViewStamp> views = _threadViewStampRepository.Where(item => item.UserID.Equals(userID));
            //_threadViewStampRepository.Delete(views);
            //OnlineUser onlineUser = _onlineUserRepository.First(item => item.UserID.Equals(userID));
            //if (onlineUser != null)
            //    _onlineUserRepository.Delete(onlineUser);
            //User user = GetUser(userID);
            //user.LastLogoutDate = DateTime.Now;
            //_userRepository.Update(user);
        }

        public User GetUser(int userID)
        {
            return db.User.Where(itm => itm.PinCode == userID).FirstOrDefault();
        }
        public User GetUser(string userNameOrEmail)
        {
            if (EmailInUse(userNameOrEmail))
                return db.User.First(item => item.email.Equals(userNameOrEmail));
            else
                try
                {
                    return db.User.First(item => item.UserName.Equals(userNameOrEmail));
                }
                catch (Exception e)
                {
                    User uu = new User();
                    return uu;
                }
        }
        public User CheckUser(string username, string password)
        {

            if (EmailInUse(username))
                return db.User.First(item => item.email.Equals(username));
            else
                try
                {
                    string tempPass = CreateMD5Hash(password);
                    return db.User.First(item => item.UserName.Equals(username) && item.Password.Equals(tempPass) && item.IsActive == true);
                }
                catch (Exception e)
                {
                    User uu = new User();
                    return uu;
                }
        }
        public string CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2")); 
            }
            return sb.ToString();
        }
        public int GetLastProfileId()
        {
            int lastProductId = 0;
            try
            {
                lastProductId = Convert.ToInt32(db.EMP_REC.Max(item => item.Pin_Code));
            }
            catch (Exception ee)
            {
            }
            return lastProductId + 1;
        }
        public int GetLastGroupRoleID()
        {
            int lastProductId = 0;
            try
            {
                lastProductId = Convert.ToInt32(db.GroupRole.Max(item => item.GroupId));
            }
            catch (Exception ee)
            {
            }
            return lastProductId;
        }
        public int GetLastUserId()
        {
            try
            {

                int lastProductId = db.User.Max(item => item.ID);
                return lastProductId + 1;
            }
            catch (Exception e)
            {
                if (db.User.Where(item => item.ID == 1).Single().ID != null)
                {
                    return 0;
                }
                else { return 1; }
            }
        }
        public int GetLastComapnyId()
        {
            try
            {
                int lastProductId = db.Company.Max(item => item.CompanyId);
                return lastProductId + 1;
            }
            catch (Exception e)
            {
                if (db.Company.Where(item => item.CompanyId == 1).Single().CompanyId != null)
                {
                    return 0;
                }
                else { return 1; }
            }
        }

        public string GetUsername(int userid)
        {
            return db.User.Where(itm => itm.ID == userid).FirstOrDefault().UserName;
        }

        #region Group Role

        public GroupRole GetGroupRolebyId(int groupRoleId)
        {
            return db.GroupRole.Find(groupRoleId);
        }

        public GroupPermission GetGroupPermissionbyGroupRoleId(int groupRoleId)
        {
            return db.GroupPermission.Where(item => item.GroupRoleId == groupRoleId).SingleOrDefault();
        }

        public List<int> GetGroupUserbyGroupRoleId(int groupRoleId)
        {
            try
            {
                return db.UserGroup.Where(item => item.GroupID == groupRoleId).Select(t => Convert.ToInt32(t.GroupID)).ToList();
            }
            catch (Exception err) { return null; }
        }

        public GroupRoleModel GetGroupUserPermissionforIndex(GroupRoleModel groupRoleModal)
        {
            try
            {
                if (SessionService.Current.Admin == 0)
                {
                    var usrGroup = db.GroupRole.Join(db.GroupPermission, g => g.GroupId, a => a.GroupRoleId, (g, a) => new { g = g, a = a });
                    groupRoleModal.IgroupRole = usrGroup.Select(item => item.g).Where(item2 => item2.IsActive == true);
                    groupRoleModal.IgroupPermision = usrGroup.Select(item => item.a).Where(item2 => item2.IsActive == true);
                    return groupRoleModal;
                }
                else if (SessionService.Current.Admin == 1)
                {
                    int compId = SessionService.Current.CompanyId;
                    //   var abc= db.GroupPermission.Join().ToList().Where(item => item.GroupPermissionId == compId);
                    var usrGroup = db.GroupRole.Join(db.GroupPermission, g => g.GroupId, a => a.GroupRoleId, (g, a) => new { g = g, a = a }).Where(ga => ga.g.CompanyId == compId);
                    groupRoleModal.IgroupRole = usrGroup.Select(item => item.g).Where(item2 => item2.IsActive == true);
                    groupRoleModal.IgroupPermision = usrGroup.Select(item => item.a).Where(item2 => item2.IsActive == true);
                    //                groupRoleModal.IgroupRole = db.GroupRole.ToList().Where(item => item.CompanyId == compId);
                    return groupRoleModal;
                }
                return groupRoleModal;
            }
            catch (Exception err) { return groupRoleModal; }
        }
        public GroupRoleModel GetGroupUserDetails(int GroupId)
        {
            GroupRoleModel groupRoleModal = new GroupRoleModel();
            try
            {
                groupRoleModal.groupRole = GetGroupRolebyId(GroupId);
                groupRoleModal.groupPermission = GetGroupPermissionbyGroupRoleId(GroupId);

                var tempo = db.User.Join(db.UserGroup, g => g.ID, a => a.UserID, (g, a) => new { g = g, a = a }).Where(eg => eg.a.GroupID == GroupId);
                groupRoleModal.IgroupUserDetail = tempo.Select(item => item.g).ToList();

                var tempo2 = db.User_Application.Join(db.GroupApplication, g => g.ApplicationId, a => a.ApplicationId, (g, a) => new { g = g, a = a }).Where(eg => eg.a.GroupRoleId == GroupId);
                groupRoleModal.IgroupAppDetail = tempo2.Select(item => item.g).ToList();

                groupRoleModal.IgroupApplication = db.GroupApplication.Where(g => g.GroupRoleId == GroupId);
                if (groupRoleModal.groupPermission != null)
                    groupRoleModal.permission = Convert.ToInt32(groupRoleModal.groupPermission.Permission);
                return groupRoleModal;
            }
            catch (Exception err) { return groupRoleModal; }
        }

        public void CreateGroupRole(GroupRole groupRole)
        {
            db.GroupRole.Add(groupRole);
            db.SaveChanges();
        }
        public void EditGroupRole(GroupRole groupRole)
        {
            try
            {
                db.Entry(groupRole).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ee)
            { }
        }
        public void DeleteGroupRole(GroupRole groupRole)
        {
            try
            {
                db.Entry(groupRole).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ee)
            { }
        }
        public void DeleteGroupPermision(GroupPermission groupPerm)
        {
            try
            {
                db.Entry(groupPerm).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ee)
            { }
        }
        public void EditGroupPermision(GroupRoleModel groupRoleM)
        {
            try
            {
                db.Entry(groupRoleM.groupPermission).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ee)
            { }
        }

        public void CreateGroupUserApplication(int roleId, int ApplicationId)
        {
            GroupApplication grpApp = new GroupApplication();
            grpApp.GroupRoleId = roleId;
            grpApp.ApplicationId = ApplicationId;
            db.GroupApplication.Add(grpApp);
            db.SaveChanges();
        }
        public void CreateGroupPermision(GroupRoleModel groupRoleM)
        {
            db.GroupPermission.Add(groupRoleM.groupPermission);
            db.SaveChanges();
        }
        public void CreateGroupUsers(GroupRoleModel groupRoleM, int userId)
        {
            groupRoleM.groupUsers.UserID = userId;
            db.UserGroup.Add(groupRoleM.groupUsers);
            db.SaveChanges();
        }
        public int CheckUserPermissionExists(int UserId, int appId)
        {
            var usrGroup = db.GroupRole.Join(db.GroupApplication, g => g.GroupId, a => a.GroupRoleId, (g, a) => new { g = g, a = a }).Where(ga => ga.a.ApplicationId == appId);
            if (usrGroup != null)
            {
                var usrGroup2 = usrGroup.Join(db.UserGroup, gt => gt.g.GroupId, u => u.GroupID, (gt, u) => new { gt = gt, u = u });
                if (usrGroup2 != null)
                {
                    var usrGroup3 = usrGroup2.Where(item => item.u.UserID == UserId).SingleOrDefault();
                    if (usrGroup3 != null)
                    {
                        return usrGroup3.u.GroupID.Value;
                    }
                }
            }
            return 0;
        }
        #endregion
        #region Owner Services
        //OwnerEdit Service
        public string EditOwner(OwnerCreation owner)
        {
            if (owner.OwnerModal.Password != null)
            {
                if (owner.OwnerModal.Password != "")
                {
                    string salt = random.CreateSalt();
                    string hashedPass = CreateMD5Hash(owner.OwnerModal.Password);

                    owner.User.Password = hashedPass;
                }
            }
            try
            {
                db.Entry(owner.User).State = EntityState.Modified;
                db.Entry(owner.UserProfile).State = EntityState.Modified;
                db.Entry(owner.Company).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ee)
            { return "Some error occured"; }


            string body = "Your Profile and Company has been Reseted at" + DateTime.Now + " by Admin";
            emailSend(owner.User.email, "Profile Edited", body);

            AuditLogEntry("Profile Edited", "", Convert.ToInt32(SessionService.Current.LoginId), "User " + owner.User.ID + " profile has been edited at " + DateTime.Now + "", "Profile Edited");
            AuditLogEntry("Profile Edited", "", Convert.ToInt32(owner.User.ID), "Profile Reseted as Admin at " + DateTime.Now + "", "Profile Edited");
            AuditLogEntry("Company Edited", "", Convert.ToInt32(SessionService.Current.LoginId), "User " + owner.User.ID + " Company has been edited at " + DateTime.Now + "", "Company Edited");
            AuditLogEntry("Company Edited", "", Convert.ToInt32(owner.User.ID), "Company Reseted as Admin at " + DateTime.Now + "", "Company Edited");

            return "1";
        }
        //Owner Index Services
        public IEnumerable<Company> GetOwnerCompanies()
        {
            IEnumerable<Company> temp = db.Company.Where(item => item.IsActive == true && item.ParentId == 0);
            return temp;
        }
        public IEnumerable<User> GetOwnersList()
        {
            IEnumerable<User> temp = db.User.ToList().Where(item => item.IsActive == true && item.IsAdmin == 1);
            return temp;
        }
        public IEnumerable<User> GetEmployeeList()
        {
            IEnumerable<User> temp;
            if (SessionService.Current.Admin == 0)
            {
                temp = db.User.ToList().Where(item => item.IsActive == true && item.IsAdmin == 0 && item.IsSuperAdmin == 0);
            }
            else if (SessionService.Current.Admin == 1)
            {

                int compId = GetCompaniesByUserId(SessionService.Current.LoginId).CompanyId;
                temp = db.User.ToList().Join(db.CompanyUser, s => s.ID, h => h.UserId, (s, h) => new { s, h }).Where(item => item.s.IsActive == true && item.s.IsAdmin == 0 && item.s.IsSuperAdmin == 0 && item.h.CompanyId == compId).Select(item2 => item2.s);
            }
            else
            {
                temp = null;
            }
            //IEnumerable<User> temp = db.User.ToList().Where(item => item.IsActive == true && item.IsAdmin == 0 && item.IsSuperAdmin==0);
            return temp;
        }
        public Company GetOwnerCompanyByUserId(int userid)
        {
            Company temp = db.Company.Where(item => item.userId == userid && item.ParentId == 0 && item.IsActive == true).SingleOrDefault();
            return temp;
        }
        public Company GetCompaniesByUserId(int userid)
        {
            Company temp = db.Company.Where(item => item.userId == userid && item.IsActive == true).SingleOrDefault();
            return temp;
        }
        public int GetParentCompaniesByEmployeeId(int userid)
        {
            try
            {
                var temp = db.CompanyUser.Join(db.Company, s => s.CompanyId, h => h.CompanyId, (s, h) => new { s, h }).Where(item => item.s.UserId == userid && item.s.IsActive == true && item.h.IsActive == true && item.h.ParentId == 0).SingleOrDefault();
                return temp.s.CompanyId.Value;
            }
            catch (Exception er) { return 0; }
        }
        public EMP_REC GetOwnerProfileByUserId(int userid)
        {
            EMP_REC temp = db.EMP_REC.Where(item => item.UserId == userid && item.IsActive == true).SingleOrDefault();
            return temp;
        }
        public void CreateOwnerWithCompany(OwnerCreation Owner)
        {
            User UserCreate = new User();
            EMP_REC userProfile = new EMP_REC();
            UserCreate.IsAdmin = 1;
            UserCreate.IsSuperAdmin = 0;
            UserCreate.CreatedBy = SessionService.Current.LoginId.ToString();

            UserCreate.CreatedDate = DateTime.Now.ToString();
            UserCreate.Password = CreateMD5Hash(Owner.OwnerModal.Password);
            UserCreate.UserName = Owner.User.UserName;
            UserCreate.email = Owner.User.email;
            UserCreate.IsActive = true;
            UserCreate.CreatedDate = DateTime.Now.ToString();

            userProfile = Owner.UserProfile;
            userProfile.Email = Owner.User.email;


            long Pincode = GetLastProfileId();

            userProfile.Pin_Code = Pincode;
            userProfile.IsActive = true;
            userProfile.Authority = 1;
            int UserId = GetLastUserId();
            userProfile.UserId = UserId;
            string pin = AddUserProfile(userProfile);

            UserCreate.PinCode = (long)Convert.ToDouble(pin);

            UserCreate.ID = UserId;
            AddUser(UserCreate);

            CreateMainCompany(Owner.Company, UserId);
            AuditLogEntry("Register", "", Convert.ToInt32(UserId), "User registered as Admin", "Register");
            string body = "Welcome Dear " + userProfile.First_Name + " to Zultime . Kindly enjoy using it";
            emailSend(userProfile.Email, "Registration Succesfull", body);
        }
        public void OwnerDelete(int id)
        {
            User user = db.User.Find(id);
            int userid = user.ID;
            Company Comp = GetOwnerCompanyByUserId(userid);
            EMP_REC usrProf = GetOwnerProfileByUserId(userid);

            user.IsActive = false;
            Comp.IsActive = false;
            usrProf.IsActive = false;

            db.Entry(user).State = EntityState.Modified;
            db.Entry(Comp).State = EntityState.Modified;
            db.Entry(usrProf).State = EntityState.Modified;
            //db.User.Remove(user);
            db.SaveChanges();

            string body = "Your Profile and you Company has been Deleted at" + DateTime.Now + " by Admin";
            emailSend(user.email, "Profile Deleted", body);

            AuditLogEntry("Profile Deleted", "", Convert.ToInt32(SessionService.Current.LoginId), "User " + user.ID + " profile has been deleted at " + DateTime.Now + "", "Profile Deleted");
            AuditLogEntry("Profile Deleted", "", Convert.ToInt32(user.ID), "Profile Deleted as Admin at " + DateTime.Now + "", "Profile Deleted");
            AuditLogEntry("Company Deleted", "", Convert.ToInt32(SessionService.Current.LoginId), "User " + user.ID + " Company has been deleted at " + DateTime.Now + "", "Company Deleted");
            AuditLogEntry("Company Deleted", "", Convert.ToInt32(user.ID), "Company Deleted as Admin at " + DateTime.Now + "", "Company Deleted");

        }
        public bool CreateMainCompany(Company Company, int UserId)
        {
            try
            {
                Company comp = new Company();
                int compId = GetLastComapnyId();
                comp.CompanyId = compId;
                comp.CompanyLogo = Company.CompanyLogo;
                comp.CompanyName = Company.CompanyName;
                comp.userId = UserId;
                comp.ParentId = 0;
                comp.IsActive = true;
                db.Company.Add(comp);
                db.SaveChanges();
                AuditLogEntry("Company", "", Convert.ToInt32(UserId), "Company Created by Super Admin for admin", "Comapny"); //Audit log

                return true;
            }
            catch (Exception ee) { return false; }
        }

        //OwnerServices End

        //Employee Services
        #region Employee
        public void CreateEmployee(OwnerCreation Owner)
        {
            User UserCreate = new User();
            EMP_REC userProfile = new EMP_REC();
            UserCreate.IsAdmin = 0;
            UserCreate.IsSuperAdmin = 0;
            UserCreate.CreatedBy = SessionService.Current.LoginId.ToString();

            UserCreate.CreatedDate = DateTime.Now.ToString();
            UserCreate.Password = CreateMD5Hash(Owner.OwnerModal.Password);
            UserCreate.UserName = Owner.User.UserName;
            UserCreate.email = Owner.User.email;
            UserCreate.IsActive = true;
            UserCreate.CreatedDate = DateTime.Now.ToString();

            userProfile.First_Name = Owner.UserProfile.First_Name;
            userProfile.Last_Name = Owner.UserProfile.Last_Name;
            userProfile.Gender = Owner.UserProfile.Gender;
            try
            {
                userProfile.B_Date = Convert.ToDateTime(Owner.UserProfile.B_Date).ToString();
            }
            catch (Exception ee) { userProfile.B_Date = Owner.UserProfile.B_Date; }
            long Pincode = GetLastProfileId();

            userProfile.Pin_Code = Pincode;

            string pin = AddUserProfile(userProfile);

            UserCreate.PinCode = (long)Convert.ToDouble(Pincode);
            int UserId = GetLastUserId();
            UserCreate.ID = UserId;

            AddUser(UserCreate);
            AuditLogEntry("Register", "", Convert.ToInt32(UserId), "User registered as Employee", "Register");
            AuditLogEntry("Employee Created", "", Convert.ToInt32(UserCreate.CreatedBy), "Admin registered " + UserId + " as Employee", "Employee");
            
            string body = "Welcome Dear " + userProfile.First_Name + " to Zultime . Kindly enjoy using it";
            emailSend(userProfile.Email, "Registration Succesfull", body);
            
            AddEmployeeToCompany(Owner.Company.CompanyId, UserId);
        }
        public bool AddEmployeeToCompany(int CompId, int UserId)
        {
            try
            {
                CompanyUser comp = new CompanyUser();
                comp.CompanyId = CompId;
                comp.UserId = UserId;
                comp.CreatedDate = DateTime.Now.ToString();
                comp.CreatedBy = SessionService.Current.LoginId;
                comp.IsActive = true;
                db.CompanyUser.Add(comp);
                db.SaveChanges();
                AuditLogEntry("Company", "", Convert.ToInt32(UserId), "User Added to Company " + CompId + " . Created by Admin", "Comapny"); //Audit log
                return true;
            }
            catch (Exception ee) { return false; }
        }
        #endregion
        //Emploeyee Services End

        //Framework Select Permission
        public List<Permisions> GetPermissionSelect()
        {
            List<Permisions> Permisions = new List<Permisions>();
            foreach (var temp in db.Permission.ToList())
            {
                Permisions.Add(new Permisions() { Value = Convert.ToInt32(temp.Value), PermisionName = temp.PermissionName });
            }

            return Permisions;
        }
        //Framework Select Companies
        public List<Compane> GetCompaniesSelect()
        {
            List<Compane> Compane = new List<Compane>();
            if (SessionService.Current.Admin == 0)
            {
                foreach (var temp in db.Company.ToList().Where(item => item.IsActive == true))
                {
                    Compane.Add(new Compane() { Value = temp.CompanyId, CompaneName = temp.CompanyName });
                }
            }
            else if (SessionService.Current.Admin == 1)
            {
                foreach (var temp2 in db.Company.ToList().Where(item => item.IsActive == true && item.userId == SessionService.Current.LoginId))
                {
                    Compane.Add(new Compane() { Value = temp2.CompanyId, CompaneName = temp2.CompanyName });
                }
            }
            else
            {
                Compane.Add(new Compane() { Value = 0, CompaneName = "No Company Found" });
            }
            return Compane;
        }
        //Framework Employee Select
        public List<Employees> GetEmployeeSelect()
        {
            List<Employees> Employees = new List<Employees>();
            if (SessionService.Current.Admin == 0)
            {
                foreach (var temp in db.User.ToList().Where(item => item.IsActive == true))
                {
                    Employees.Add(new Employees() { Value = temp.ID, EmployeesName = temp.UserName });
                }
            }
            else if (SessionService.Current.Admin == 1)
            {
                int companyId = Convert.ToInt32(db.CompanyUser.SingleOrDefault(item2 => item2.UserId == SessionService.Current.LoginId).CompanyId);
                //var tempo= db.User.Join(db.CompanyUser, s =>  s.ID,h =>  h.UserId ,(s, h) => new {s});
                foreach (var temp2 in db.User.Join(db.CompanyUser, s => s.ID, h => h.UserId, (s, h) => new { s }).Where(item => item.s.IsActive == true && item.s.IsSuperAdmin == 0).ToList())
                {
                    Employees.Add(new Employees() { Value = temp2.s.ID, EmployeesName = temp2.s.UserName });
                }
            }
            else
            {
                Employees.Add(new Employees() { Value = 0, EmployeesName = "No Employees Found" });
            }
            return Employees;
        }
        //Framework Country Select
        public List<Country> GetCountriesSelect()
        {
            List<Country> Country = new List<Country>();
            foreach (var temp in db.NATION.ToList().Where(item => item.IsActive == true))
            {
                Country.Add(new Country() { Value = temp.Nation_Code, CountryName = temp.Description });
            }
            return Country;
        }
        #endregion

        #region Form Services
        public IEnumerable<SelectListItem> IselectListEmployee()
        {
            return GetEmployeeSelect().Select(
                b => new SelectListItem { Value = b.Value.ToString(), Text = b.EmployeesName });
        }
        public IEnumerable<SelectListItem> IselectListUserApplication()
        {
            return db.User_Application.ToList().Select(
                    b => new SelectListItem { Value = b.ApplicationId.ToString(), Text = b.ApplicationName });
        }
        public string GetCountryById(int id)
        {
            string temp = db.NATION.Where(item => item.IsActive == true && item.Nation_Code == id).SingleOrDefault().Description.ToString();
            return temp;
        }
        public string GetReligionById(int id)
        {
            string temp = db.RELIGIONS.Where(item => item.IsActive == true && item.Relig_Code == id).SingleOrDefault().Description.ToString();
            return temp;
        }
        #endregion

        public string ResetPassword(int userID, string newPassword)
        {
            User user = GetUser(userID);
            string salt = random.CreateSalt();
            string hashedPass = CreateMD5Hash(newPassword);

            user.Password = hashedPass;

            UpdateUser(user);
            string body = "Your Password Reseted at" + DateTime.Now + "";
            emailSend(user.email, "Password Reset", body);

            AuditLogEntry("Reset", "", Convert.ToInt32(user.ID), "User reset his Password as Admin at " + DateTime.Now + "", "Reset");

            return newPassword;
        }
        public string forgotPasword(int userID)
        {
            User user = GetUser(userID);
            string newPassword = random.RandomPassword();
            string salt = random.CreateSalt();
            string hashedPass = CreateMD5Hash(newPassword);

            user.Password = hashedPass;

            UpdateUser(user);
            string body = "Your Password Reseted at" + DateTime.Now + "and your new password is " + newPassword + "";
            emailSend(user.email, "Password Reset", body);

            AuditLogEntry("Forgot", "", Convert.ToInt32(user.ID), "User Forgot and  reset his Password as Admin at " + DateTime.Now + "", "Forgot Password");

            return newPassword;
        }

        public bool UpdateUser(User useR)
        {
            try
            {
                User us = db.User.Where(itm => itm.PinCode == useR.PinCode).FirstOrDefault();
                us.Password = useR.Password;
                db.SaveChanges();
            }
            catch (Exception ee)
            { return false; }
            return true;
        }

        //public User UpdatePassword(int userID, string newPassword)
        //{
        //    string newSalt = random.CreateSalt();
        //    string newHashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword + newSalt, "MD5");

        //    User user = GetUser(userID);

        //    user.Password = newHashedPassword;
        //   // user.PasswordSalt = newSalt;
        //  //  _userRepository.Update(user);


        //    db.Entry(user).State = EntityState.Modified;
        //    db.SaveChanges();

        //    return user;
        //}

        //    public UserProfile UpdateSignature(int userID, string signature)
        //    {
        //        var userProfile = _userProfileRepository.Get(userID);
        //        userProfile.Signature = signature;
        //        userProfile.ParsedSignature = _parseServices.ParseBBCodeText(signature);
        //        _userProfileRepository.Update(userProfile);
        //        return userProfile;
        //    }

        public User UpdateEmail(int userID, string newEmail)
        {
            var user = GetUser(userID);
            user.email = newEmail;
            // _userRepository.Update(user);
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return user;
        }
        //public User UpdateStatus(int userID, string Newstatus)
        //{
        //    var user = GetUser(userID);
        //    user.Mood = Newstatus;
        //    _userRepository.Update(user);
        //    return user;
        //}

        //    //public User UpdateGoogle(int userID)
        //    //{
        //    //    var user = _userRepository.Get(userID);
        //    //    user.IsGoogleUser = true;
        //    //    _userRepository.Update(user);
        //    //    return user;
        //    //}

        //    public UserProfile UpdateAvatarToNone(int userID)
        //    {
        //        return UpdateAvatar(userID, "None", string.Empty);
        //    }

        //    public UserProfile UpdateAvatarToUrl(int userID, string url)
        //    {
        //        return UpdateAvatar(userID, "Url", url);
        //    }

        //    public UserProfile UpdateAvatarToUpload(int userID, string uploadedFileName)
        //    {
        //        return UpdateAvatar(userID, "Upload", uploadedFileName);
        //    }

        //    //private UserProfile UpdateAvatar(int userID, string avatarType, string avatar)
        //    //{
        //    //    var userProfile = _userProfileRepository.Get(userID);
        //    //    userProfile.AvatarType = avatarType;
        //    //    userProfile.Avatar = avatar;
        //    //    _userProfileRepository.Update(userProfile);
        //    //    return userProfile;
        //    //}

        public void UpdateProfile(
            int userID,
            bool isShowSignature,
            bool isSubscribeToThread,
            string firstName,
            string lastName,
            string state,
            string city,
            string zipCode,
            string address,

            string location,
            int? themeID,
            int? defaultRankRole,
            string aim,
            string icq,
            string msn,
            string website,
            string email,
            DateTime? birthdate)
        {
            User user = db.User.First(item => item.PinCode == userID);
            EMP_REC UserPro = db.EMP_REC.First(item => item.Pin_Code == userID);

            UserPro.First_Name = firstName;
            UserPro.Last_Name = lastName;
            //   UserPro.StatCode = state;
            // UserPro.City = city;
            // user.ZipCode = zipCode;
            //  UserPro.Address = address;
            UserPro.Email = email;


            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            //  _userRepository.Update(user);

            //UserProfile userProfile = _userProfileRepository.First(item => item.UserID == userID);
            //userProfile.Location = location;
            //userProfile.ThemeID = themeID;
            //userProfile.DefaultRole = defaultRankRole;
            //userProfile.AIM = aim;
            //userProfile.ICQ = icq;
            //userProfile.MSN = msn;
            //userProfile.Website = website;
            //userProfile.Birthdate = birthdate;
            //userProfile.IsShowSignature = isShowSignature;
            //userProfile.IsSubscribeToThread = isSubscribeToThread;

            db.Entry(UserPro).State = EntityState.Modified;
            db.SaveChanges();
        }

        //    public User Register(string username, string password, string email, string state, string city, string zipCode, string address, string phone)
        //    {

        //        string activationType = SiteConfig.AccountActivation.Value;
        //        string salt = random.CreateSalt() + DateTime.Now.ToString();
        //        string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password + salt, "MD5");

        //        var user = new User()
        //        {
        //            Password = hashedPassword,
        //            PasswordSalt = salt,
        //            RegisterDate = DateTime.Now,
        //            RegisterIP = HttpContext.Current.Request.UserHostAddress,
        //            ActivationCode = activationType == "None" ? string.Empty : random.CleanGUID(),
        //            Status = false,
        //            LastLoginIP = null,
        //            Username = username,
        //            UsernameLower = username.ToLower(),
        //            LastLoginDate = DateTime.Now,
        //            LastLogoutDate = DateTime.Now,
        //            LastPostDate = DateTime.Now,
        //            Email = email,
        //            State = state,
        //            City = city,
        //            ZipCode = zipCode,
        //            Address = address,
        //            IsActive = true,
        //            IsFBUser = false,
        //            Phone = phone

        //        };

        //        user.UserProfile = new UserProfile()
        //        {
        //            IsShowSignature = true,
        //            IsSubscribeToThread = true,
        //            AvatarType = "None",
        //            ThemeID = SiteConfig.BoardTheme.ToInt()
        //        };

        //        var inRole = new InRole()
        //        {
        //            UserID = user.UserID,
        //            RoleID = SiteConfig.RegistrationRole.IntValue()
        //        };
        //        user.InRoles.Add(inRole);

        //        _userRepository.Add(user);

        //        return user;
        //    }
        //    #region Google Register
        //    public User RegisterGoogle(string username, string password, string firstname, string lastname, string email, string state, string city, string zipCode, string address, string phone, string bdayDate, string websitee, string fbid)
        //    {

        //        string activationType = "None";
        //        string salt = random.CreateSalt() + DateTime.Now.ToString();
        //        string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password + salt, "MD5");
        //        DateTime tempBdayDate = new DateTime();
        //        if (bdayDate != null)
        //        {
        //            tempBdayDate = Convert.ToDateTime(bdayDate);
        //        }
        //        var user = new User()
        //        {
        //            Password = hashedPassword,
        //            PasswordSalt = salt,
        //            RegisterDate = DateTime.Now,
        //            RegisterIP = HttpContext.Current.Request.UserHostAddress,
        //            ActivationCode = activationType == "None" ? string.Empty : random.CleanGUID(),
        //            Status = false,
        //            LastLoginIP = null,
        //            Username = username,
        //            UsernameLower = username.ToLower(),
        //            LastLoginDate = DateTime.Now,
        //            LastLogoutDate = DateTime.Now,
        //            LastPostDate = DateTime.Now,
        //            Email = email,
        //            State = state,
        //            City = city,
        //            ZipCode = zipCode,
        //            Address = address,
        //            IsActive = true,
        //            IsFBUser = false,
        //            IsGoogleUser = true,
        //            Phone = phone

        //        };

        //        user.UserProfile = new UserProfile()
        //        {
        //            IsShowSignature = true,
        //            IsSubscribeToThread = true,
        //            AvatarType = "None",
        //            ThemeID = SiteConfig.BoardTheme.ToInt(),
        //            Website = websitee,
        //            Birthdate = tempBdayDate,
        //            Picture = fbid
        //        };

        //        var inRole = new InRole()
        //        {
        //            UserID = user.UserID,
        //            RoleID = SiteConfig.RegistrationRole.IntValue()
        //        };
        //        user.InRoles.Add(inRole);

        //        _userRepository.Add(user);

        //        return user;
        //    }
        //    #endregion
        //    #region Msbh fb Login
        //    public bool IsFbUsers(string email)
        //    {
        //        var result = _userRepository.Where(item => item.Email == email && item.IsFBUser == true);
        //        if (result == null)
        //        { return false; }
        //        if (result.Count() == 0)
        //        { return false; }
        //        return true;
        //    }
        //    public User RegisterFB(string username, string password, string firstname, string lastname, string email, string state, string city, string zipCode, string address, string phone, string bdayDate, string websitee, string fbid)
        //    {

        //        string activationType = "None";
        //        string salt = random.CreateSalt() + DateTime.Now.ToString();
        //        string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password + salt, "MD5");
        //        DateTime tempBdayDate = new DateTime();
        //        if (bdayDate != null)
        //        {
        //            tempBdayDate = Convert.ToDateTime(bdayDate);
        //        }
        //        var user = new User()
        //        {
        //            Password = hashedPassword,
        //            PasswordSalt = salt,
        //            RegisterDate = DateTime.Now,
        //            RegisterIP = HttpContext.Current.Request.UserHostAddress,
        //            ActivationCode = activationType == "None" ? string.Empty : random.CleanGUID(),
        //            Status = false,
        //            LastLoginIP = null,
        //            Username = username,
        //            UsernameLower = username.ToLower(),
        //            LastLoginDate = DateTime.Now,
        //            LastLogoutDate = DateTime.Now,
        //            LastPostDate = DateTime.Now,
        //            Email = email,
        //            State = state,
        //            City = city,
        //            ZipCode = zipCode,
        //            Address = address,
        //            IsActive = true,
        //            IsFBUser = true,
        //            Phone = phone

        //        };

        //        user.UserProfile = new UserProfile()
        //        {
        //            IsShowSignature = true,
        //            IsSubscribeToThread = true,
        //            AvatarType = "None",
        //            ThemeID = SiteConfig.BoardTheme.ToInt(),
        //            Website = websitee,
        //            Birthdate = tempBdayDate,
        //            Picture = fbid
        //        };

        //        var inRole = new InRole()
        //        {
        //            UserID = user.UserID,
        //            RoleID = SiteConfig.RegistrationRole.IntValue()
        //        };
        //        user.InRoles.Add(inRole);

        //        _userRepository.Add(user);

        //        return user;
        //    }



        //    public User UpdateFbDetails(int userID, string username, string password, string firstname, string lastname, string email, string state, string city, string zipCode, string address, string phone, string bdayDate, string websitee, string fbid)
        //    {
        //        DateTime tempBdayDate = new DateTime();
        //        if (bdayDate != null)
        //        {
        //            tempBdayDate = Convert.ToDateTime(bdayDate);
        //        }
        //        if (email == null)
        //        { email = ""; }
        //        if (username == null)
        //        { username = ""; }
        //        if (fbid == null)
        //        { fbid = ""; }
        //        if (websitee == null)
        //        { websitee = ""; }
        //        if (firstname == null)
        //        { firstname = ""; }
        //        if (lastname == null)
        //        { lastname = ""; }
        //        if (username == null)
        //        { username = ""; }
        //        if (state == null)
        //        { state = ""; }
        //        if (city == null)
        //        { city = ""; }
        //        if (state == "None")
        //        { state = ""; }
        //        if (city == "None")
        //        { city = ""; }
        //        var user = _userRepository.Get(userID);
        //        if (user.Email != email && email != "")
        //        { user.Email = email; }
        //        if (user.State != state && state != "" && state != "none")
        //        {
        //            user.State = state;
        //        }
        //        if (user.City != city && city != "" && city != "none")
        //        {
        //            user.City = city;
        //        }
        //        if (user.FirstName != firstname && firstname != "")
        //        {
        //            user.FirstName = firstname;
        //        }
        //        if (user.LastName != lastname && lastname != "")
        //        {
        //            user.LastName = lastname;
        //        }
        //        if (user.UserProfile.Birthdate != tempBdayDate && bdayDate != "" && bdayDate != "1/1/2012")
        //        {
        //            user.UserProfile.Birthdate = tempBdayDate;
        //        }
        //        if (user.UserProfile.Website != websitee && websitee != "")
        //        {
        //            user.UserProfile.Website = websitee;
        //        }
        //        if (user.UserProfile.Location != city + ", " + state && city != "")
        //        {
        //            user.UserProfile.Location = city + ", " + state;
        //        }
        //        if (user.UserProfile.Picture != fbid && fbid != "")
        //        {
        //            user.UserProfile.Picture = fbid;
        //        }

        //        if (user.Username != username && username != "")
        //        {
        //            user.Username = username;
        //        }

        //        _userRepository.Update(user);
        //        return user;
        //    }

        //    public User UpdateGoogleDetails(int userID, string username, string password, string firstname, string lastname, string email, string state, string city, string zipCode, string address, string phone, string bdayDate)
        //    {
        //        DateTime tempBdayDate = new DateTime();
        //        if (bdayDate != null)
        //        {
        //            tempBdayDate = Convert.ToDateTime(bdayDate);
        //        }
        //        if (email == null)
        //        { email = ""; }
        //        if (username == null)
        //        { username = ""; }
        //        if (firstname == null)
        //        { firstname = ""; }
        //        if (lastname == null)
        //        { lastname = ""; }
        //        if (username == null)
        //        { username = ""; }
        //        if (state == null)
        //        { state = ""; }
        //        if (city == null)
        //        { city = ""; }
        //        if (state == "None")
        //        { state = ""; }
        //        if (city == "None")
        //        { city = ""; }
        //        var user = _userRepository.Get(userID);
        //        if (user.Email != email && email != "")
        //        { user.Email = email; }
        //        if (state != "none")
        //        {
        //            user.State = state;
        //        }
        //        if (city != "none")
        //        {
        //            user.City = city;
        //        }
        //        if (phone != "")
        //        {
        //            user.Phone = phone;
        //        }
        //        if (zipCode != "")
        //        {
        //            user.ZipCode = zipCode;
        //        }
        //        if (address != "")
        //        {
        //            user.Address = address;
        //        }
        //        if (firstname != "")
        //        {
        //            user.FirstName = firstname;
        //        }
        //        if (lastname != "")
        //        {
        //            user.LastName = lastname;
        //        }
        //        if (bdayDate != "1/1/2012" && bdayDate != "")
        //        {
        //            user.UserProfile.Birthdate = tempBdayDate;
        //        }

        //        if (user.UserProfile.Location != city + ", " + state && city != "")
        //        {
        //            user.UserProfile.Location = city + ", " + state;
        //        }

        //        if (user.Username != username && username != "")
        //        {
        //            user.Username = username;
        //        }

        //        _userRepository.Update(user);
        //        return user;
        //    }

        //    #endregion
        //    public string RequestPasswordReset(int userID)
        //    {
        //        PasswordResetRequest request = _passwordResetRequestRepository.Get(userID);

        //        if (request != null)
        //            _passwordResetRequestRepository.Delete(request);

        //        PasswordResetRequest pwrr = new PasswordResetRequest
        //        {
        //            UserID = userID,
        //            Token = random.CleanGUID(),
        //            Date = DateTime.Now
        //        };

        //        _passwordResetRequestRepository.Add(pwrr);

        //        string token = userID.ToString() + "-" + pwrr.Token;

        //        return token;
        //    }

        //    public bool ValidatePasswordResetRequest(string token)
        //    {
        //        if (string.IsNullOrWhiteSpace(token))
        //            return false;

        //        string[] split = token.Split('-');
        //        int userID = int.Parse(split[0]);
        //        string code = split[1];

        //        PasswordResetRequest resetRequest = _passwordResetRequestRepository.Get(userID);

        //        if ((DateTime.Now - resetRequest.Date).Minutes > 15)
        //            return false;

        //        return (resetRequest.UserID == userID && resetRequest.Token == code);
        //    }

        //    public void DeleteUser(int userID)
        //    {
        //        var messagesSent = _messageRepository.Where(item => item.FromUserID == userID);
        //        foreach (var message in messagesSent)
        //        {
        //            if (message.FromUserID == null && message.ToUserID == null)
        //                _messageRepository.Delete(message);
        //            else
        //            {
        //                message.FromUserID = null;
        //                _messageRepository.Update(message);
        //            }
        //        }

        //        var messagesReceived = _messageRepository.Where(item => item.ToUserID == userID);
        //        foreach (var message in messagesSent)
        //        {
        //            if (message.FromUserID == null && message.ToUserID == null)
        //                _messageRepository.Delete(message);
        //            else
        //            {
        //                message.ToUserID = null;
        //                _messageRepository.Update(message);
        //            }
        //        }

        //        _userRepository.Delete(userID);
        //    }

        //    public void DeletePasswordResetRequest(int userID)
        //    {
        //        _passwordResetRequestRepository.Delete(userID);
        //    }
        //    public List<UserModel> GetFriendSuggetion(string city = "", string state = "", int forumid = 0, int userid = 0, int categoryid = 0)
        //    {

        //        return _commonrepository.GetFriendSuggestion(state, city, forumid, userid, categoryid);

        //    }

        //    public List<UserModel> GetPagedFriends(List<UserModel> PageFriend, int userid, int pageNumber, int pageSize, OrderBy orderBy = OrderBy.Date, Direction direction = Direction.Descending)
        //    {

        //        PageFriend = PageFriend.OrderByDescending(item => item.username).ThenByDescending(x => x.state).ToList();

        //        if (direction == Direction.Ascending)
        //            PageFriend.Reverse();

        //        PageFriend = PageFriend.TakePage(pageNumber, pageSize).ToList();
        //        return PageFriend;
        //    }
    }
}

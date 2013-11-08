using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAtten.Models;
using TimeAtten.Services.Services;

namespace TimeAtten.Framework.Admin
{
    public class Application
    {
        public string ApplicationName { get; set; }
        public int Value { get; set; }
    }

    public class UserApplicationSelect
    {
        private TimeAttenEntities1 db = new TimeAttenEntities1();
        UserServices UserServc = new UserServices();
        public IEnumerable<Application> Applications { get; set; }
        public SelectList ApplicationSelectList { get; set; }
        public Application Application { get; set; }

        public IEnumerable<SelectListItem> ISelectItems { get; set; } 
           

        private List<Application> GetApplications()
        {
            List<Application> Application = new List<Application>();
            foreach (var temp in db.User_Application.ToList().Where(item => item.IsActive == true))
            {
                Application.Add(new Application() { Value = temp.ApplicationId, ApplicationName = temp.ApplicationName });
            }
            return Application;
        }

        public UserApplicationSelect()
        {
            this.Applications = GetApplications();
            this.ApplicationSelectList = new SelectList(Applications, "Value", "ApplicationName");
            this.ISelectItems = UserServc.IselectListUserApplication(); ;
        }
    }
    
}
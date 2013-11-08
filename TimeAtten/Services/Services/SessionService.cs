using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeAtten.Services.Services
{
    public class SessionService
    {
        // private constructor
        private SessionService()
        {
            Property1 = "default value";
            LoginName = "null";
        }
        // Gets the current session.
        public static SessionService Current
        {
            get
            {
                SessionService session =
                (SessionService)HttpContext.Current.Session["__MySession__"];
                if (session == null)
                {
                    session = new SessionService();
                    HttpContext.Current.Session["__MySession__"] = session;
                }
                return session;
            }
        }
        // **** add your session properties here, e.g like this:
        public string Property1 { get; set; }
        public string LoginName { get; set; }
        public int UserPin { get; set; }
        public int CompanyId { get; set; }
        public int Admin { get; set; }
        public DateTime MyDate { get; set; }
        public int LoginId { get; set; }
    }
}
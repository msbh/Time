using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAtten.Services.Services;

namespace TimeAtten.Framework.Admin
{
      public class Permisions
    {
        public string PermisionName { get; set; }
        public int Value { get; set; }
    }
    public class PermissionSelect
    {
         UserServices UserServc = new UserServices();
         public IEnumerable<Permisions> PermissionI { get; set; }
        public SelectList PermisionSelectList { get; set; }
        public Permisions Permisions { get; set; }

        public PermissionSelect()
        {
            this.PermissionI = UserServc.GetPermissionSelect();
            this.PermisionSelectList = new SelectList(PermissionI, "Value", "PermisionName");
        }
    }
}
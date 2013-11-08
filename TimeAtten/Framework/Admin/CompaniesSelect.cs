using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAtten.Models;
using TimeAtten.Services.Services;

namespace TimeAtten.Framework.Admin
{
    public class Compane
    {
        public string CompaneName { get; set; }
        public int Value { get; set; }
    }

    public class CompaniesSelect
    {
        UserServices UserServc = new UserServices();
        public IEnumerable<Compane> Companies { get; set; }
        public SelectList ComapnySelectList { get; set; }
        public Compane Compane { get; set; }

        public CompaniesSelect()
        {
            this.Companies = UserServc.GetCompaniesSelect();
            this.ComapnySelectList = new SelectList(Companies, "Value", "CompaneName");
        }
    }
    //public class CompaniesSelect
    //{
    //}
}
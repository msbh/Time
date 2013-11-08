using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAtten.Services.Services;

namespace TimeAtten.Framework.Admin
{
    public class Employees
    {
        public string EmployeesName { get; set; }
        public int Value { get; set; }
    }

    public class EmployeeSelect
    {        
        UserServices UserServc = new UserServices();
        public IEnumerable<Employees> EmployeesI { get; set; }
        public SelectList EmployeeSelectList { get; set; }
        public Employees Employees { get; set; }
        public IEnumerable<SelectListItem> ISelectItems { get; set; } 

        public EmployeeSelect()
        {
            this.EmployeesI = UserServc.GetEmployeeSelect();
            this.EmployeeSelectList = new SelectList(EmployeesI, "Value", "EmployeesName");
            this.ISelectItems = UserServc.IselectListEmployee();
        }
    }    
}
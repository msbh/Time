using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAtten.Models;

namespace TimeAtten.Framework.Admin
{
    public class Designation
    {
        public string DesignationName { get; set; }
        public int Value { get; set; }
    }

    public class DesignationsModel
    {
        private TimeAttenEntities1 db = new TimeAttenEntities1();

        public IEnumerable<Designation> Designations { get; set; }
        public SelectList DesignationSelectList { get; set; }
        public Designation Designation { get; set; }

        private List<Designation> GetDesignations()
        {
            List<Designation> Designation = new List<Designation>();
            foreach (var temp in db.DESIGS.ToList().Where(item=>item.IsActive==true))
            {
                Designation.Add(new Designation() { Value = temp.Desigs_Code, DesignationName = temp.Description });
            }
            return Designation;
        }

        public DesignationsModel()
        {
            this.Designations = GetDesignations();
            this.DesignationSelectList = new SelectList(Designations, "Value", "DesignationName");
        }
    }
}

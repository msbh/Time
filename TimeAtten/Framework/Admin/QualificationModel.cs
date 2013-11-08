using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAtten.Models;

namespace TimeAtten.Framework.Admin
{
    public class Qualification
    {
        public string QualificationName { get; set; }
        public int Value { get; set; }
    }

    public class QualificationModel
    {
        private TimeAttenEntities1 db = new TimeAttenEntities1();

        public IEnumerable<Qualification> Qualifications { get; set; }
        public SelectList QualificationSelectList { get; set; }
        public Qualification Qualification { get; set; }

        private List<Qualification> GetQualifications()
        {
            List<Qualification> Qualification = new List<Qualification>();
            foreach (var temp in db.Qualification.ToList().Where(item=>item.IsActive==true))
            {
                Qualification.Add(new Qualification() { Value = temp.QualCode, QualificationName = temp.QualDesc });
            }
            return Qualification;
        }

        public QualificationModel()
        {
            this.Qualifications = GetQualifications();
            this.QualificationSelectList = new SelectList(Qualifications, "Value", "QualificationName");
        }
    }
}

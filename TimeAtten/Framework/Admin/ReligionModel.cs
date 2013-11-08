using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAtten.Models;

namespace TimeAtten.Framework.Admin
{
    public class Religion
    {
        public string ReligionName { get; set; }
        public int Value { get; set; }
    }

    public class ReligionModel
    {
        private TimeAttenEntities1 db = new TimeAttenEntities1();

        public IEnumerable<Religion> Religions { get; set; }
        public SelectList ReligionSelectList { get; set; }
        public Religion Religion { get; set; }

        private List<Religion> GetReligions()
        {
            List<Religion> Religion = new List<Religion>();
            foreach (var temp in db.RELIGIONS.ToList().Where(item=>item.IsActive==true))
            {
                Religion.Add(new Religion() { Value = temp.Relig_Code, ReligionName = temp.Description });
            }
            return Religion;
        }

        public ReligionModel()
        {
            this.Religions = GetReligions();
            this.ReligionSelectList = new SelectList(Religions, "Value", "ReligionName");
        }
    }
}

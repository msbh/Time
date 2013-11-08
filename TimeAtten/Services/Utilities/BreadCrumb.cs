using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeAtten.Services.Utilities
{
    public class BreadCrumb
    {
        public string BreadCrumbName { get; set; }
        public string url { get; set; }      

        public BreadCrumb()
        {
            BreadCrumbName="" ;
            url ="";            
        }
    }
    public class BreadCrumbList
    {
        public List<BreadCrumb> BreadCrumList { get; set; }
        public string CurrentTitle { get; set; }
        public string SubTitle { get; set; }

        public BreadCrumbList()
        {
            BreadCrumList = new List<BreadCrumb>();
            CurrentTitle = "";
            SubTitle = "";
        }
    }
}
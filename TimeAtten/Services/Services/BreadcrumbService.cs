using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeAtten.Services.Utilities;
using System.Web.Mvc;


namespace TimeAtten.Services.Services
{
    public class BreadcrumbService
    {
        UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        public BreadCrumbList IndexBreadCrumb(string Controller, string title, string subtitle)
        {
            #region Viewbags of BreadCrumb
            BreadCrumbList BreadCrums = new BreadCrumbList();
            BreadCrumb BreadCrum = new BreadCrumb();
            BreadCrum.BreadCrumbName = "Home";
            BreadCrum.url = helper.Action("Index", "Dashboard");
            //RedirectToAction("Index", "Dashboard").ToString();
            BreadCrums.BreadCrumList.Add(BreadCrum);
            BreadCrum = new BreadCrumb();
            BreadCrum.BreadCrumbName = Controller;
            BreadCrum.url = "#";
            BreadCrums.BreadCrumList.Add(BreadCrum);
            BreadCrums.CurrentTitle = title;
            BreadCrums.SubTitle = subtitle;
            return BreadCrums;
            #endregion
        }
        public BreadCrumbList EditBreadCrumb(string Controller, string title, string subtitle)
        {
            #region Viewbags of BreadCrumb
            BreadCrumbList BreadCrums = new BreadCrumbList();
            BreadCrumb BreadCrum = new BreadCrumb();
            BreadCrum.BreadCrumbName = "Home";
            BreadCrum.url = helper.Action("Index", "Dashboard");
            BreadCrums.BreadCrumList.Add(BreadCrum);
            BreadCrum = new BreadCrumb();
            BreadCrum.BreadCrumbName = Controller;
            BreadCrum.url = helper.Action("Index");
            BreadCrums.BreadCrumList.Add(BreadCrum);
            BreadCrum = new BreadCrumb();
            BreadCrum.BreadCrumbName = "Edit";
            BreadCrum.url = "#";
            BreadCrums.BreadCrumList.Add(BreadCrum);
            BreadCrums.CurrentTitle = title;
            BreadCrums.SubTitle = subtitle;
            return BreadCrums;
            #endregion
        }
        public BreadCrumbList CreateBreadCrumb(string Controller, string title, string subtitle)
        {
            #region Viewbags of BreadCrumb
            BreadCrumbList BreadCrums = new BreadCrumbList();
            BreadCrumb BreadCrum = new BreadCrumb();
            BreadCrum.BreadCrumbName = "Home";
            BreadCrum.url = helper.Action("Index", "Dashboard");
            BreadCrums.BreadCrumList.Add(BreadCrum);
            BreadCrum = new BreadCrumb();
            BreadCrum.BreadCrumbName = Controller;
            BreadCrum.url = helper.Action("Index");
            BreadCrums.BreadCrumList.Add(BreadCrum);
            BreadCrum = new BreadCrumb();
            BreadCrum.BreadCrumbName = "Create";
            BreadCrum.url = "#";
            BreadCrums.BreadCrumList.Add(BreadCrum);
            BreadCrums.CurrentTitle = title;
            BreadCrums.SubTitle = subtitle;
            return BreadCrums;
            #endregion
        }
        public BreadCrumbList DeleteBreadCrumb(string Controller, string title, string subtitle)
        {
            #region Viewbags of BreadCrumb
            BreadCrumbList BreadCrums = new BreadCrumbList();
            BreadCrumb BreadCrum = new BreadCrumb();
            BreadCrum.BreadCrumbName = "Home";
            BreadCrum.url = helper.Action("Index", "Dashboard");
            BreadCrums.BreadCrumList.Add(BreadCrum);
            BreadCrum = new BreadCrumb();
            BreadCrum.BreadCrumbName = Controller;
            BreadCrum.url = helper.Action("Index");
            BreadCrums.BreadCrumList.Add(BreadCrum);
            BreadCrum = new BreadCrumb();
            BreadCrum.BreadCrumbName = "Delete";
            BreadCrum.url = "#";
            BreadCrums.BreadCrumList.Add(BreadCrum);
            BreadCrums.CurrentTitle = title;
            BreadCrums.SubTitle = subtitle;
            return BreadCrums;
            #endregion
        }
        public BreadCrumbList DetailBreadCrumb(string Controller, string title, string subtitle)
        {
            #region Viewbags of BreadCrumb
            BreadCrumbList BreadCrums = new BreadCrumbList();
            BreadCrumb BreadCrum = new BreadCrumb();
            BreadCrum.BreadCrumbName = "Home";
            BreadCrum.url = helper.Action("Index", "Dashboard");
            BreadCrums.BreadCrumList.Add(BreadCrum);
            BreadCrum = new BreadCrumb();
            BreadCrum.BreadCrumbName = Controller;
            BreadCrum.url = helper.Action("Index");
            BreadCrums.BreadCrumList.Add(BreadCrum);
            BreadCrum = new BreadCrumb();
            BreadCrum.BreadCrumbName = "Detail";
            BreadCrum.url = "#";
            BreadCrums.BreadCrumList.Add(BreadCrum);
            BreadCrums.CurrentTitle = title;
            BreadCrums.SubTitle = subtitle;
            return BreadCrums;
            #endregion
        }
    }
}
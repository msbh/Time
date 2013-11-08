using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeAtten.Models;

namespace TimeAtten.Framework.Admin
{
    public class GroupRoleModel
    {
        public GroupRole groupRole { get; set; }
        public GroupApplication groupApplication { get; set; }
        public GroupPermission groupPermission { get; set; }
        public UserGroup groupUsers { get; set; }
        public IEnumerable<UserGroup> IgroupUser { get; set; }
        public IEnumerable<GroupApplication> IgroupApplication { get; set; }
        public IEnumerable<GroupRole> IgroupRole { get; set; }//for index
        public IEnumerable<GroupPermission> IgroupPermision { get; set; }//for index
        public IEnumerable<User> IgroupUserDetail { get; set; }//for Detail
        public IEnumerable<User_Application> IgroupAppDetail { get; set; }//for Detail
        public int ApplicationId { get; set; }
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public int permission { get; set; }
        public IEnumerable<string> SelectedStuff { get; set; }


        public GroupRoleModel()
        {
            groupRole = new GroupRole();
            groupApplication = new GroupApplication();
            groupPermission = new GroupPermission();
            groupUsers = new UserGroup();
            ApplicationId = 0;
            EmployeeId = 0;
            CompanyId = 0;
            permission = 000;
        }
    }
}
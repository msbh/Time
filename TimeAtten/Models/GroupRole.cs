//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimeAtten.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GroupRole
    {
        public GroupRole()
        {
            this.GroupApplication = new HashSet<GroupApplication>();
        }
    
        public Nullable<int> CompanyId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public Nullable<int> EditedBy { get; set; }
        public string EditedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual ICollection<GroupApplication> GroupApplication { get; set; }
    }
}

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
    
    public partial class Permission
    {
        public int Id { get; set; }
        public Nullable<int> Value { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
    }
}
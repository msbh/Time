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
    
    public partial class User
    {
        public int ID { get; set; }
        public Nullable<long> PinCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string email { get; set; }
        public Nullable<int> IsAdmin { get; set; }
        public Nullable<int> IsSuperAdmin { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }
}

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
    
    public partial class RELIGIONS
    {
        public int Relig_Code { get; set; }
        public string Description { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> EditedBy { get; set; }
        public string CreatedDate { get; set; }
        public string EditedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}

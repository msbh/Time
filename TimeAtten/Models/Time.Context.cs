﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TimeAttenEntities1 : DbContext
    {
        public TimeAttenEntities1()
            : base("name=TimeAttenEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<ACTIVEEMPRPT> ACTIVEEMPRPT { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<COMP_CALENDAR> COMP_CALENDAR { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<companyDesc> companyDesc { get; set; }
        public DbSet<DESIGS> DESIGS { get; set; }
        public DbSet<GRPCHILD> GRPCHILD { get; set; }
        public DbSet<HIERGRPS> HIERGRPS { get; set; }
        public DbSet<INDTSRPT> INDTSRPT { get; set; }
        public DbSet<JobStat> JobStat { get; set; }
        public DbSet<LEVELS> LEVELS { get; set; }
        public DbSet<NATION> NATION { get; set; }
        public DbSet<OVERTIME> OVERTIME { get; set; }
        public DbSet<Qualification> Qualification { get; set; }
        public DbSet<REASONS> REASONS { get; set; }
        public DbSet<RELIGIONS> RELIGIONS { get; set; }
        public DbSet<ROT_TZONE> ROT_TZONE { get; set; }
        public DbSet<SCHED_TZONE> SCHED_TZONE { get; set; }
        public DbSet<SUMRPT> SUMRPT { get; set; }
        public DbSet<sysdiagrams> sysdiagrams { get; set; }
        public DbSet<TEMP_TXT_DATA> TEMP_TXT_DATA { get; set; }
        public DbSet<TEMPREASONS> TEMPREASONS { get; set; }
        public DbSet<TEMPTMP> TEMPTMP { get; set; }
        public DbSet<TIME_SHEET> TIME_SHEET { get; set; }
        public DbSet<TIMEZONE> TIMEZONE { get; set; }
        public DbSet<TOT_REAS> TOT_REAS { get; set; }
        public DbSet<TSHEETDATES> TSHEETDATES { get; set; }
        public DbSet<TSMODLOG> TSMODLOG { get; set; }
        public DbSet<TZONERPT> TZONERPT { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<WeekPlanner> WeekPlanner { get; set; }
        public DbSet<EMP_REC> EMP_REC { get; set; }
        public DbSet<Online> Online { get; set; }
        public DbSet<User_Application> User_Application { get; set; }
        public DbSet<GroupApplication> GroupApplication { get; set; }
        public DbSet<GroupRole> GroupRole { get; set; }
        public DbSet<CompanyUser> CompanyUser { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<GroupPermission> GroupPermission { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
    }
}

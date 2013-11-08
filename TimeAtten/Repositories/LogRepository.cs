using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeAtten.Models;

namespace TimeAtten.Repositories
{
    public class LogRepository : ILogRepository
    {
        public User CurrentUser { get; private set; }
        private TimeAttenEntities1 context;

        public LogRepository(TimeAttenEntities1 dataContext)
        {
            this.context = dataContext;
        }

        public LogRepository()
        {
            
            // TODO: Complete member initialization
        }
        //public void AddLog( thread)
        //{
        //    context.audi.Attach(thread);
        //    context.ObjectStateManager.ChangeObjectState(thread, EntityState.Modifie
        //    context.SaveChanges();
        //}

    }
}
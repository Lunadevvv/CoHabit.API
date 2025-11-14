using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Dashboard;

namespace CoHabit.API.Helpers
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            
            // Production: chỉ cho admin truy cập
            // return httpContext.User.Identity?.IsAuthenticated == true && 
            //         httpContext.User.IsInRole("Admin");
            
            // Development: bỏ comment dòng dưới
            return true;
        }
    }
}
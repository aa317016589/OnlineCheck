using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 

namespace OnlineCheck.Web.Filters
{
    public class ErrorFilterAttribute :HandleErrorAttribute
    {
      
        public override void OnException(ExceptionContext filterContext)
        {
           

            base.OnException(filterContext);
        }
    }
}
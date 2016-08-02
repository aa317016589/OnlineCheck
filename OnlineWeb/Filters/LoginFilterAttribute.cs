using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OnlineCheck.Web.Filters
{
    public class LoginFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           // return;
            base.OnActionExecuting(filterContext);
            //var cacheId = CookieUtils.Get("Account");
            //var logger = ObjectContainer.Resolve<ILoggerFactory>().Create(GetType().Name);
            //var cacher = ObjectContainer.Resolve<ICacher>();
            ////if (string.IsNullOrEmpty(accountId))
            ////{
            ////    filterContext.Result =
            ////        new RedirectResult("/Home/Login?CallBackUrl=" +
            ////                           HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString()));
            ////}

            //var url = NCasServerSetting.GetServerAuthUrl(filterContext.RequestContext.HttpContext.Request.Url.ToString());
            //if (string.IsNullOrEmpty(cacheId))
            //{
            //    //加了这句就不再走后面的Action
            //    filterContext.Result = new RedirectResult(url);
            //}
            //var teacher = cacher.Get<TestletsStructTeacherDto>(cacheId);
            //if (teacher == null)
            //{
            //    //加了这句就不再走后面的Action
            //    filterContext.Result = new RedirectResult(url);
            //}

        }
    }
}
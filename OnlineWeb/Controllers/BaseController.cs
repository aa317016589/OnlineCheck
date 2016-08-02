using System;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace OnlineCheck.Web.Controllers
{
    public class BaseController : Controller
    {
        public Int32 TeacherId
        {
            get
            {
                if (Request.Cookies["TeacherId"] == null)
                {
                    return 0;
                }

                return Int32.Parse(Request.Cookies["TeacherId"].Value);

               
            }
        }

        public String QuestionGroupId
        {
            get { return Request.QueryString["QuestionGroupId"] ?? ""; }
        }
    }
}
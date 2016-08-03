using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace OnlineCheck.Web.Controllers
{
    public class BaseController : Controller
    {
        public Int32 teacherId;

        public Int32 TeacherId
        {
            get
            {
                if (Request.QueryString["teacherId"] != null)
                {
                    Int32 id = Int32.Parse(Request.QueryString["teacherId"]);

                    Response.Cookies.Add(new HttpCookie("TeacherId", id.ToString()));

                    return id;
                }


                if (Request.Cookies["TeacherId"] != null)
                {
                    return Int32.Parse(Request.Cookies["TeacherId"].Value);
                }

                return 0;
            }
        }

        private Teacher _teacherInfo;

        public Teacher TeacherInfo
        {
            get { return _teacherInfo ?? OnlineCheckManager.Instance.Teachers.SingleOrDefault(s => s.TeacherId == TeacherId); }
        }



        public String QuestionGroupId
        {
            get { return Request.QueryString["QuestionGroupId"] ?? ""; }
        }
    }
}
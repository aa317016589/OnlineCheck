using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OnlineCheck.Web.Filters;

namespace OnlineCheck.Web.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>评阅页面
        /// </summary>
        [HttpGet]

        public ActionResult Index(string questionGroupId, Int32 teacherId)
        {
            QuestionGroup questionGroup =
                OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(
                    s => s.QuestionGroupId.ToString() == questionGroupId);


            //if (testletsStructTeacherDto != null)
            //{
            //    查询教师的评阅份数
            //    ViewData["DistributionNumber"] = testletsStructTeacherDto.DistributionNumber;
            //}

            ViewData["QuestionGroup"] = questionGroup;

            //查询教师信息
            ViewData["Teacher"] = teacherId;

            Response.Cookies.Add(new HttpCookie("TeacherId", teacherId.ToString()));

            return View();
        }

        /// <summary>问题卷页面
        /// </summary>
        [HttpGet]

        public ActionResult Problematics(string id)
        {
            ////查询题组信息
            //var testletsStructInfo =
            //    _testletsStructQueryService.FindTestletsStructInfo(new TestletsStructQueryDto(examTestletsId: id));
            //if (testletsStructInfo == null)
            //{
            //    //跳转到题组不存在页面
            //    return Redirect("/Error");
            //}
            //var testletsStructTeacherDto = _teacherQueryService.FindTeacherDto(testletsStructInfo.TestletsStructId, OriTeacherId);
            ////查询教师的评阅份数
            //ViewData["DistributionNumber"] = testletsStructTeacherDto.DistributionNumber;
            ////评分点区域
            //ViewData["ScorePoint"] = _scorePointStructQueryService.QueryScorePointStructDto(testletsStructInfo.TestletsStructId); ;
            ////阈值
            //ViewData["TestletsStruct"] = testletsStructInfo;
            ////查询科目信息
            //ViewData["Subject"] = _subjectQueryService.FindSubjectInfoDto(testletsStructInfo.SubjectId);
            ////查询教师信息
            //ViewData["Teacher"] = base.Teacher;

            QuestionGroup questionGroup =
                OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(s => s.QuestionGroupId.ToString() == id);

            ViewData["Teacher"] = TeacherId;

            ViewData["QuestionGroup"] = questionGroup;


            return View();
        }

        /// <summary>仲裁卷页面
        /// </summary>
        [HttpGet]

        public ActionResult Arbitration(string id)
        {
            ////查询题组信息
            //var testletsStructInfo =
            //    _testletsStructQueryService.FindTestletsStructInfo(new TestletsStructQueryDto(examTestletsId: id));
            //var testletsStructTeacherDto = _teacherQueryService.FindTeacherDto(testletsStructInfo.TestletsStructId, OriTeacherId);
            ////查询教师的评阅份数
            //ViewData["DistributionNumber"] = testletsStructTeacherDto.DistributionNumber;
            ////评分点区域
            //ViewData["ScorePoint"] =
            //    _scorePointStructQueryService.QueryScorePointStructDto(testletsStructInfo.TestletsStructId);
            ////阈值
            //ViewData["TestletsStruct"] = testletsStructInfo;
            ////查询科目信息
            //ViewData["Subject"] = _subjectQueryService.FindSubjectInfoDto(testletsStructInfo.SubjectId);
            ////查询教师信息
            //ViewData["Teacher"] = base.Teacher;


            QuestionGroup questionGroup =
                OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(s => s.QuestionGroupId.ToString() == id);

            ViewData["Teacher"] = TeacherId;
            ViewData["QuestionGroup"] = questionGroup;

            return View();
        }

        /// <summary>回评页面
        /// </summary>
        [HttpGet]
        [LoginFilter]
        public ActionResult CallBack(string testletsStructId, string reviewId)
        {

            String questionGroupId = testletsStructId;
  
            QuestionGroup questionGroup =
                OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(
                    s => s.QuestionGroupId.ToString() == questionGroupId);


            ViewData["Teacher"] = TeacherId;
            ViewData["QuestionGroup"] = questionGroup;


            ////查询题组信息
            //var testletsStructInfo =
            //    _testletsStructQueryService.FindTestletsStructInfo(new TestletsStructQueryDto(testletsStructId));

            //var testletsStructTeacherDto = _teacherQueryService.FindTeacherDto(testletsStructId, OriTeacherId);

            //ViewData["ScorePoint"] = _scorePointStructQueryService.QueryScorePointStructDto(testletsStructId);

            //ViewData["TestletsStruct"] = testletsStructInfo;
            ////查询教师的评阅份数
            //ViewData["DistributionNumber"] = testletsStructTeacherDto.DistributionNumber;
            ////查询科目信息
            //ViewData["Subject"] = _subjectQueryService.FindSubjectInfoDto(testletsStructInfo.SubjectId);
            ////查询教师信息
            //ViewData["Teacher"] = base.Teacher;
            //var testlets =
            //    _testletsQueryService.QueryTestletsDtos(new TestletsQueryDto(testletsStructId)).FirstOrDefault();
            //ViewData["Testlets"] = testlets;
            return View();
        }

       /// <summary>
       /// 控制界面，
       /// </summary>
       /// <returns></returns>
        public ActionResult Ctrl()
        {
            OnlineCheckManager.Instance.AnswerSheets.ForEach(s =>
            {
                s.AnswerChecks.ForEach(a =>
                {
                  a.Set();
                });
            });

           ViewData["Finish"] = OnlineCheckManager.Instance.AnswerSheets;



            return View();
        }




        ///// <summary>临时登录页面
        ///// </summary>
        //[HttpGet]
        //public ActionResult Login()
        //{
        //    var model = new TempLogin()
        //    {
        //        CallBackUrl = RequestUtils.GetString("CallBackUrl")
        //    };
        //    return System.Web.UI.WebControls.View(model);
        //}

        //[HttpPost]
        //public ActionResult Login(TempLogin model)
        //{
        //    var teacherDot = _teacherQueryService.FindTeacherDtoById(model.TeacherId);
        //    if (teacherDot == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "该教师不存在");
        //        ViewData.Model = new TempLogin()
        //        {
        //            CallBackUrl = model.CallBackUrl
        //        };
        //        return View();
        //    }

        //    var teacher = _jsonSerializer.Serialize(teacherDot);
        //    CookieUtils.WriteCookie("Account", teacher, DateTime.MaxValue);
        //    return Redirect(model.CallBackUrl);
        //}

        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }
    }
}
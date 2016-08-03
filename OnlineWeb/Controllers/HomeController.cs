using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OnlineCheck.Web.Filters;
using OnlineCheck.Web.Models;

namespace OnlineCheck.Web.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>评阅页面
        /// </summary>
        [HttpGet]

        public ActionResult Review(string questionGroupId, Int32 teacherId)
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
            ViewData["Teacher"] = TeacherInfo;



            return View();
        }

        /// <summary>问题卷页面
        /// </summary>
        [HttpGet]

        public ActionResult Problematics(string questionGroupId, Int32 teacherId)
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
                OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(s => s.QuestionGroupId.ToString() == questionGroupId);

            ViewData["Teacher"] = TeacherInfo;

            ViewData["QuestionGroup"] = questionGroup;



            return View();
        }

        /// <summary>仲裁卷页面
        /// </summary>
        [HttpGet]

        public ActionResult Arbitration(string questionGroupId, Int32 teacherId)
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
                OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(s => s.QuestionGroupId.ToString() == questionGroupId);

            ViewData["Teacher"] = TeacherInfo;

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


            ViewData["Teacher"] = TeacherInfo;
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
        public ActionResult Finish()
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

        public ActionResult Progress()
        {
            ViewData["ReviewProgress"] = OnlineCheckManager.Instance.GetReviewProgress();

            return View();
        }


        public ActionResult Index()
        {
            if (OnlineCheckManager.Instance.IsTesting)
            {
                return RedirectToAction("List");
            }

       
            return View();
        }

        [HttpPost]
        public ActionResult Index(CtrlViewModel ctrl)
        {
            List<Teacher> teachers = new List<Teacher>();

            for (int i = 0; i < ctrl.TeacherCounts; i++)
            {
                teachers.Add(new Teacher()
                {
                    TeacherId = i + 1,
                    TeacherName = "第" + (i + 1).ToString() + "位教师"
                });
            }

            OnlineCheckManager.Instance.Teachers.AddRange(teachers);

            int x = 1;

            for (int i = 0; i < ctrl.QuestionGroupCounts; i++)
            {
                QuestionGroup questionGroup = new QuestionGroup((i + 1).ToString(), ctrl.JudgeMode, teachers);

                questionGroup.QuestionGroupName = "第" + (i + 1).ToString() + "题组";


                for (int j = 0; j < ctrl.QuestionCounts; j++)
                {
                    Question firstQuestion = new Question(questionGroup.QuestionGroupId,
                        ((i * ctrl.QuestionCounts) + j + 1).ToString(),
                        ctrl.Threshold, 25, new Random((int)DateTime.Now.Ticks * j).Next());

                    questionGroup.Questions.Add(firstQuestion);

                }

                OnlineCheckManager.Instance.QuestionGroups.Add(questionGroup);
            }


            for (int i = 0; i < ctrl.StudentCounts; i++)
            {

                AnswerSheet answerSheet = new AnswerSheet()
                {
                    MaxPicUrl = Guid.NewGuid().ToString(),
                    StudentName = "学生" + (i + 1).ToString(),
                    StudentSubjectId = new Random().Next(99999) / (i + 1)
                };

                List<AnswerCheck> answerChecks = new List<AnswerCheck>();

                for (int j = 0; j < ctrl.QuestionGroupCounts; j++)
                {
                    QuestionGroup qg = OnlineCheckManager.Instance.QuestionGroups[j];


                    answerChecks.Add(new AnswerCheck(qg.QuestionGroupId, qg.Questions.Select(s => new Answer(s, Guid.NewGuid().ToString())).ToList(), qg.JudgeMode));
                }
                answerSheet.AnswerChecks = answerChecks;

                OnlineCheckManager.Instance.AnswerSheets.Add(answerSheet);
            }

            OnlineCheckManager.Instance.IsTesting = true;

            return RedirectToAction("list");
        }

        public ActionResult EndTest()
        {
            OnlineCheckManager.Instance.IsTesting = false;

            OnlineCheckManager.Instance.QuestionGroups = new List<QuestionGroup>();
            OnlineCheckManager.Instance.Teachers = new List<Teacher>();
            OnlineCheckManager.Instance.AnswerSheets = new List<AnswerSheet>();

            return RedirectToAction("index");
        }

        public ActionResult List()
        {
            ViewData["QuestionGroups"] = OnlineCheckManager.Instance.QuestionGroups;

            ViewData["Teachers"] = OnlineCheckManager.Instance.Teachers;





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
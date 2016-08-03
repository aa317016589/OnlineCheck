using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using OnlineCheck.Web.Models;

namespace OnlineCheck.Web.Controllers
{
    public class OnlineApiController : Controller
    {


        /// <summary>根据原题组结构Id查询评阅进度
        /// </summary>
        [HttpGet]
        public ActionResult GetReviewInfo(string id)
        {
            //var testletsStruct =
            //     _testletsStructQueryService.FindTestletsStructInfo(new TestletsStructQueryDto(examTestletsId: id));
            //var data = _reviewService.QueryReviewProgress(testletsStruct.TestletsStructId);
            //return Json(ActionHandleResult.FromSuccess(data: data), JsonRequestBehavior.AllowGet);

 

            return Json(ActionHandleResult.FromSuccess(data: OnlineCheckManager.Instance.GetReviewProgress(id)), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 根据原题组结构Id检测是否有仲裁卷
        /// </summary>
        [HttpGet]
        public ActionResult HasArbitration(string id)
        {
            //bool hasArbitration = _testletsQueryService.QueryCount(new
            //{
            //    ArbitrationStatus = ReviewStatus.Produce.ToString(),
            //    ExamTestletsId = id,
            //    UseFlag = (int)UseFlag.Useable
            //}) > 0;
            //return Json(ActionHandleResult.FromSuccess(data: hasArbitration), JsonRequestBehavior.AllowGet);

            return null;
        }

        /// <summary>根据元题组结构Id检测是否有问题卷
        /// </summary>
        [HttpGet]
        public ActionResult HasProblem(string id)
        {
            //bool hasProblematics = _testletsQueryService.QueryCount(new
            //{
            //    ProblematicsStatus = ProblematicsStatus.CanReview.ToString(),
            //    ExamTestletsId = id,
            //    UseFlag = (int)UseFlag.Useable
            //}) > 0;
            //return Json(ActionHandleResult.FromSuccess(data: hasProblematics), JsonRequestBehavior.AllowGet);

            return null;
        }

        /// <summary>开始评阅
        /// </summary>
        [HttpGet]
        public ActionResult BeginQuestionGroup(string questionGroupId)
        {
            //根据当前题组Id，从数据库拿出该题组的相关数据填充到内存中。

            IList<Teacher> teachers = new List<Teacher>()
            {
               new Teacher()
               {
                    TeacherId =  1,
                    TeacherName = "沈笑菲"
               },
                new Teacher()
               {
                    TeacherId =  2,
                    TeacherName = "慕雨烟"
               },
               new Teacher()
               {
                    TeacherId =  3,
                    TeacherName = "凤舞九天"
               },
               new Teacher()
               {
                    TeacherId =  4,
                    TeacherName = "丢丢"
               },           
            };

            QuestionGroup questionGroup = new QuestionGroup(questionGroupId, JudgeModes.FourReview, teachers);

            Question firstQuestion = new Question(questionGroup.QuestionGroupId, "1", 4, 25, 1001);

            questionGroup.Questions.Add(firstQuestion);

            Question secondQuestion = new Question(questionGroup.QuestionGroupId, "2", 5, 15, 1002);

            questionGroup.Questions.Add(secondQuestion);


            OnlineCheckManager.Instance.QuestionGroups.Add(questionGroup);

            OnlineCheckManager.Instance.Teachers.AddRange(teachers);

            OnlineCheckManager.Instance.AnswerSheets = CreateAnswerSheets(questionGroup, firstQuestion, secondQuestion);


            return Json("准备已完成", JsonRequestBehavior.AllowGet);
        }

        private List<AnswerSheet> CreateAnswerSheets(QuestionGroup questionGroup, Question firstQuestion,
            Question secondQuestion)
        {
            List<AnswerSheet> answerSheets = new List<AnswerSheet>()
            {
                new AnswerSheet()
                {
                    MaxPicUrl = Guid.NewGuid().ToString(),
                    StudentSubjectId = new Random().Next(0, 999999),
                    AnswerChecks = new List<AnswerCheck>()
                    {
                        new AnswerCheck(firstQuestion.QuestionGroupId, new List<Answer>()
                        {
                            new Answer(firstQuestion, Guid.NewGuid().ToString()),
                            new Answer(secondQuestion, Guid.NewGuid().ToString())
                        }, questionGroup.JudgeMode)
                    }
                },
                new AnswerSheet()
                {
                    MaxPicUrl = Guid.NewGuid().ToString(),
                    StudentSubjectId = new Random().Next(0, 999999),
                    AnswerChecks = new List<AnswerCheck>()
                    {
                        new AnswerCheck(firstQuestion.QuestionGroupId, new List<Answer>()
                        {
                            new Answer(firstQuestion, Guid.NewGuid().ToString()),
                            new Answer(secondQuestion, Guid.NewGuid().ToString())
                        }, questionGroup.JudgeMode)
                    }
                },
                new AnswerSheet()
                {
                    MaxPicUrl = Guid.NewGuid().ToString(),
                    StudentSubjectId = new Random().Next(0, 999999),
                    AnswerChecks = new List<AnswerCheck>()
                    {
                        new AnswerCheck(firstQuestion.QuestionGroupId, new List<Answer>()
                        {
                            new Answer(firstQuestion, Guid.NewGuid().ToString()),
                            new Answer(secondQuestion, Guid.NewGuid().ToString())
                        }, questionGroup.JudgeMode)
                    }
                }
            };

            return answerSheets;
        }



        /// <summary>
        /// 回收某个教师的题组
        /// </summary>
        [HttpGet]
        public ActionResult Recovery(string examTestletsId, string oriTeacherId)
        {
            //var testletsStruct =
            //    _testletsStructQueryService.FindTestletsStructInfo(
            //        new TestletsStructQueryDto(examTestletsId: examTestletsId));
            //if (testletsStruct != null)
            //{
            //    var teacherInfoDto = _teacherQueryService.FindTeacherDto(testletsStruct.TestletsStructId, oriTeacherId);
            //    if (teacherInfoDto != null)
            //    {
            //        var message = new TeacherRecovery()
            //        {
            //            TestletsStructId = testletsStruct.TestletsStructId,
            //            TeacherId = teacherInfoDto.TestletsStructTeacherId
            //        };
            //        var result = Send(message);
            //        if (result.GetHandleStatus() == HandleStatus.Success)
            //        {
            //            return Json(ActionHandleResult.FromSuccess(message: "回收成功"), JsonRequestBehavior.AllowGet);
            //        }
            //        return Json(ActionHandleResult.FromFail(message: "回收失败"), JsonRequestBehavior.AllowGet);
            //    }
            //}
            //return Json(ActionHandleResult.FromFail(message: "出错了~~~"), JsonRequestBehavior.AllowGet);

            return null;
        }
    }
}
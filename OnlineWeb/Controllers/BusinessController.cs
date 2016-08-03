using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using OnlineCheck.Web.Filters;
using OnlineCheck.Web.Models;

namespace OnlineCheck.Web.Controllers
{
    public class BusinessController : BaseController
    {
        #region 基础

        /// <summary>拉取题组,此Id为现在系统的题组Id
        /// </summary>
        [HttpGet]
        public ActionResult PullTestlets(string id)
        {
            //string teacherId = _teacherService.GetTeacherId(id, OriTeacherId);
            //var result = Send(new GetTestlets()
            //{
            //    TeacherId = teacherId,
            //    TestletsStructId = id
            //});
            //if (result.GetHandleStatus()==HandleStatus.Success)
            //{
            //    var testlets = _sender.DecodeResponseMessage<TestletsInfo>(result.HandleResult.Body);
            //    if (testlets != null)
            //    {
            //        return Json(ActionHandleResult.FromSuccess(data: new
            //        {
            //            TestletsId = testlets.TestletsId,
            //            ImageUrl = testlets.ImageUrl
            //        }), JsonRequestBehavior.AllowGet);
            //    }
            //}
            //return Json(ActionHandleResult.FromFail(success: 1, message: "目前题组评阅状态已经完成,请等待继续"),
            //    JsonRequestBehavior.AllowGet);


            AnswerSheet answerSheet = OnlineCheckManager.Instance.AnswerSheets.FirstOrDefault(
                s =>
                    s.AnswerChecks.Any(
                        a => a.QuestionGroupId == id && a.TeacherCheckManagerx.IsGet(TeacherId)));

            if (answerSheet == null)
            {
                return Json(ActionHandleResult.FromFail(success: 1, message: "目前题组评阅状态已经完成,请等待继续"),
                    JsonRequestBehavior.AllowGet);
            }

            AnswerCheck answerCheck = answerSheet.AnswerChecks.SingleOrDefault(s => s.QuestionGroupId == id);

            answerCheck.TeacherCheckManagerx.AddTeacherChecks(new TeacherCheck()
            {
                TeacherId = TeacherId,
            });




            return Json(ActionHandleResult.FromSuccess(data: new
            {
                TestletsId = answerCheck.AnswerCheckId,
                ImageUrl = answerCheck.CombinationedPicUrl
            }), JsonRequestBehavior.AllowGet);
        }


        /// <summary>查询客户端被拿走的
        /// </summary>
        [HttpGet]
        public ActionResult QueryLoaded(string id)
        {
            //string teacherId = _teacherService.GetTeacherId(id, OriTeacherId);
            //var message = new GetTakeAways()
            //{
            //    TeacherId = teacherId,
            //    TestletsStructId = id
            //};
            //var result = Send(message);
            //if (result.GetHandleStatus()==HandleStatus.Success)
            //{
            //    var testletses = _sender.DecodeResponseMessage<TakeAwayTestlets>(result.HandleResult.Body).TestletsInfos ;
            //    if (testletses != null)
            //    {
            //        return Json(ActionHandleResult.FromSuccess(data: testletses.Select(x => new
            //        {
            //            TestletsId = x.TestletsId,
            //            ImageUrl = x.ImageUrl
            //        })), JsonRequestBehavior.AllowGet);
            //    }
            //}
            //return Json(ActionHandleResult.FromFail(message: result.GetErrorMessage()), JsonRequestBehavior.AllowGet);

            return Json(ActionHandleResult.FromSuccess(), JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<ActionResult> BeginTeacher(string testletsStructId)
        {
            //string teacherId = _teacherService.GetTeacherId(testletsStructId, OriTeacherId);
            //var command = new ActiveTeacher(testletsStructId, teacherId);
            //var result = await ExecuteCommandAsync(command);
            //return System.Web.Helpers.Json(result, JsonRequestBehavior.AllowGet);

            return null;
        }

        /// <summary>提交预评阅ReviewTestletsViewModel model
        /// </summary>
        [HttpPost]
        public ActionResult Review(ReviewViewModel reviewViewModel)
        {
            //string teacherId = _teacherService.GetTeacherId(model.TestletsStructId, OriTeacherId);
            //var reviewScorePointList = model.ReviewScores.Select(reviewScore =>
            //    new ReviewScorePointDto(
            //        reviewScore.ScorePointStructId,
            //        reviewScore.Score)).ToList();
            //var command = new ReviewTestlets(model.TestletsId, teacherId, reviewScorePointList);
            //var result = await ExecuteCommandAsync(command);
            //if (result.IsSuccess())
            //{
            //    return Json(ActionHandleResult.FromSuccess(message: "评阅成功"));
            //}
            //if (result.GetErrorCode() == 1)
            //{
            //    return Json(ActionHandleResult.FromFail(success: 1, message: result.GetErrorMessage()));
            //}
            //return Json(ActionHandleResult.FromFail(message: result.GetErrorMessage()));






            TeacherCheck teacherCheck = new TeacherCheck()
            {
                TeacherId = TeacherId,
                TeacherInfo = TeacherInfo,
                Score = reviewViewModel.Score
            };



            AnswerCheck answerCheck = OnlineCheckManager.Instance.AnswerSheets.SelectMany(s => s.AnswerChecks)
               .SingleOrDefault(s => s.AnswerCheckId == reviewViewModel.AnswerCheckId);

            answerCheck.TeacherCheckManagerx.UpdateTeacherChecks(teacherCheck);


            OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(s => s.QuestionGroupId == reviewViewModel.QuestionGroupId).PressReviewManager.Enqueue(teacherCheck.TeacherId, new PressCheck()
            {
                Score = teacherCheck.Score,
                AnswerCheckId = answerCheck.AnswerCheckId
            });


            return Json(ActionHandleResult.FromSuccess(message: "评阅成功"));
        }

        /// <summary>查看原卷
        /// </summary>
        [HttpGet]
        public ActionResult SourcePaper(string testletsId)
        {
            //var studentId = _testletsQueryService.QueryStudentId(testletsId);
            //var url = string.Format(@"{0}/api/EduPlatImage/GetPaper?StudentId={1}",
            //    AppSettingsUtils.GetString("WebBasic"), studentId);
            //return Redirect(url);

            return null;
        }

        /// <summary>查询当前教师的已阅数量
        /// </summary>
        [HttpGet]
        public ActionResult QueryReviewCount(string testletsStructId)
        {
            //var teacherId = _teacherService.GetTeacherId(testletsStructId, OriTeacherId);
            //var reviewCount = _teacherQueryService.QueryReviewCount(testletsStructId, teacherId);
            //return Json(reviewCount, JsonRequestBehavior.AllowGet);


            return null;
        }


        #endregion

        #region 仲裁卷
        /// <summary>获取仲裁卷
        /// </summary>
        [HttpGet]
        public ActionResult PullArbitration(string id)
        {

            //string teacherId = _teacherService.GetTeacherId(id, OriTeacherId);
            //var message = new GetArbitrationTestlets()
            //{
            //    TeacherId = teacherId,
            //    TestletsStructId = id
            //};
            //var result = Send(message);
            //if (result.GetHandleStatus() == HandleStatus.Success)
            //{
            //    var testlets = _sender.DecodeResponseMessage<TestletsInfo>(result.HandleResult.Body);
            //    if (testlets != null)
            //    {
            //        return Json(ActionHandleResult.FromSuccess(data: new
            //        {
            //            TestletsId = testlets.TestletsId,
            //            ImageUrl = testlets.ImageUrl
            //        }), JsonRequestBehavior.AllowGet);
            //    }
            //}
            //return Json(ActionHandleResult.FromFail(success: 1, message: "不存在仲裁卷"),
            //    JsonRequestBehavior.AllowGet);






            return null;
        }

        /// <summary>查询全部被拿走的仲裁题组
        /// </summary>
        [HttpGet]
        public ActionResult QueryAllArbitration(string testletsStructId)
        {
            //string teacherId = _teacherService.GetTeacherId(id, OriTeacherId);
            //var message = new GetAllArbitrationTestlets()
            //{
            //    TeacherId = teacherId,
            //    TestletsStructId = id
            //};
            //var result = Send(message);
            //if (result.GetHandleStatus() ==HandleStatus.Success)
            //{
            //    var testletses = _sender.DecodeResponseMessage<AllArbitrationTestlets>(result.HandleResult.Body).TestletsInfos;
            //    if (testletses != null)
            //    {
            //        return Json(ActionHandleResult.FromSuccess(data: testletses.Select(x => new
            //        {
            //            TestletsId = x.TestletsId,
            //            ImageUrl = x.ImageUrl
            //        }).ToList()), JsonRequestBehavior.AllowGet);
            //    }
            //}
            //return Json(ActionHandleResult.FromFail(success: 1, message: "不存在仲裁卷"),
            //    JsonRequestBehavior.AllowGet);
            QuestionGroup questionGroup =
    OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(s => s.QuestionGroupId == testletsStructId);

            List<Question> questions = questionGroup.Questions;


            IEnumerable<dynamic> answerChecks = OnlineCheckManager.Instance.AnswerSheets.SelectMany(s => s.AnswerChecks)
                .Where(s => s.QuestionGroupId == testletsStructId
                            && !s.TeacherCheckManagerx.IsAllFinish && s.TeacherCheckManagerx.IsArbitration)
                .Select(f => new
                {
                    TestletsId = f.AnswerCheckId,
                    ImageUrl = f.CombinationedPicUrl,
                    TeacherChecks = f.TeacherCheckManagerx.TeacherChecks.Select(s => new
                    {
                        TeacherName = s.TeacherInfo.TeacherName,
                        ScoreCheck = s.Score.ToDictionary(
                            k => questions.SingleOrDefault(a => a.QuestionId.ToString() == k.Key).QuestionNo, v => v.Value)
                    })
                });


            return Json(ActionHandleResult.FromSuccess(data: answerChecks), JsonRequestBehavior.AllowGet);
        }


        /// <summary>问题卷提交页面ReviewTestletsViewModel model
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> ReviewArbitration(ReviewViewModel reviewViewModel)
        {
            //string teacherId = _teacherService.GetTeacherId(model.TestletsId, Teacher.OriTeacherId);
            //var reviewScorePointList = model.ReviewScores.Select(reviewScore =>
            //    new ReviewScorePointDto(
            //        reviewScore.ScorePointStructId,
            //        reviewScore.Score)).ToList();
            //var command = new ReviewProblematics(model.TestletsId, teacherId, reviewScorePointList);
            //var result = await ExecuteCommandAsync(command);
            //if (result.IsSuccess())
            //{
            //    return Json(ActionHandleResult.FromSuccess(message: "问题卷评阅成功"));
            //}
            //return Json(ActionHandleResult.FromFail(message: result.GetErrorMessage()));



            TeacherCheck teacherCheck = new TeacherCheck()
            {
                TeacherId = TeacherId,
                TeacherInfo = TeacherInfo,
                CheckType = CheckTypes.Arbitration,
                Score = reviewViewModel.Score
            };

            AnswerCheck answerCheck = OnlineCheckManager.Instance.AnswerSheets.SelectMany(s => s.AnswerChecks)
   .SingleOrDefault(s => s.AnswerCheckId == reviewViewModel.AnswerCheckId);

            answerCheck.TeacherCheckManagerx.UpdateTeacherChecks(teacherCheck, CheckTypes.Arbitration);

            answerCheck.TeacherCheckManagerx.PressReturn();


            return Json(ActionHandleResult.FromSuccess(message: "评阅成功"));

        }
        #endregion

        #region 回评
        /// <summary>查询回评题组,回评直接查询数据库,查询出来进行回评
        /// </summary>
        [HttpGet]
        public ActionResult QueryCallBack(string id)
        {
            //string teacherId = _teacherService.GetTeacherId(id, OriTeacherId);
            //var data = _testletsReviewQueryService.QueryTestletsReviewListDtos(id, teacherId).Select(x => new
            //{
            //    ReviewId=x.ReviewId,
            //    ReviewDate=x.ReviewDate.ToString("hh:mm:ss"),
            //    TotalScore=x.TotalScore,
            //    TestletsId=x.TestletsId
            //});
            //return Json(ActionHandleResult.FromSuccess(data: data), JsonRequestBehavior.AllowGet);

            QuestionGroup questionGroup =
                OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(s => s.QuestionGroupId == id);

            List<Question> questions = questionGroup.Questions;

            IEnumerable<dynamic> queuesPressChecks =
              questionGroup.PressReviewManager.PressReview[TeacherId].Where(s => !s.IsPressed).Select(x => new
                {
                    ReviewId = x.Id,
                    ReviewDate = x.CreateDateTime.ToString("hh:mm:ss"),
                    TotalScore =
                        x.Score.ToDictionary(
                            k => questions.SingleOrDefault(s => s.QuestionId.ToString() == k.Key).QuestionNo, v => v.Value)
                });

            return Json(ActionHandleResult.FromSuccess(data: queuesPressChecks), JsonRequestBehavior.AllowGet);

        }

        /// <summary>查询回评题组,回评直接查询数据库,查询出来进行回评
        /// </summary>
        [HttpGet]
        public ActionResult QueryOnlyCallBack(string testletsStructId, string reviewId)
        {

            PressCheck pressCheck =
                OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(s => s.QuestionGroupId == testletsStructId)
                    .PressReviewManager.PressReview[TeacherId].SingleOrDefault(s => s.Id == reviewId.ToString());

            AnswerCheck answerCheck = OnlineCheckManager.Instance.AnswerSheets.SelectMany(s => s.AnswerChecks)
            .SingleOrDefault(s => s.AnswerCheckId == pressCheck.AnswerCheckId);


            if (pressCheck != null)
            {
                var data = new
                {
                    TestletsId = answerCheck.AnswerCheckId,
                    ReviewId = pressCheck.Id,
                    ReviewDate = pressCheck.CreateDateTime.ToString("hh:mm:ss"),
                    ReviewItems = pressCheck.Score,
                    ImageUrl = answerCheck.CombinationedPicUrl
                };
                return Json(ActionHandleResult.FromSuccess(message: "", data: data), JsonRequestBehavior.AllowGet);
            }


            //string teacherId = _teacherService.GetTeacherId(testletsStructId, OriTeacherId);
            //var review = _testletsReviewQueryService.QueryTestletsReviewDtos(new TestletsReviewQueryDto(teacherId)
            //{
            //    ReviewId = reviewId
            //}).FirstOrDefault();


            //if (review != null)
            //{
            //    //查询出题组下所有的评分点结构Id
            //    var scorePoints = _scorePointQueryService.QueryScorePointDtos(review.TestletsId).ToList();
            //    var testletsInfo =
            //        _testletsQueryService.QueryTestletsDtos(new TestletsQueryDto(testletsStructId, review.TestletsId))
            //            .FirstOrDefault();
            //    if (testletsInfo != null && scorePoints.Any())
            //    {
            //        var data = new
            //        {
            //            TestletsId = review.TestletsId,
            //            ReviewId = review.ReviewId,
            //            ReviewDate = review.ReviewDate.ToString("hh:mm:ss "),
            //            ReviewTeacherId = review.ReviewTeacherId,
            //            ReviewItems = review.Items.Select(x => new
            //            {
            //                Id=x.Id,
            //                ScorePointId=x.ScorePointId,
            //                Score=x.Score,
            //                ScorePointStructId=scorePoints.FirstOrDefault(y=>y.ScorePointId==x.ScorePointId).ScorePointStructId
            //            }),
            //            ImageUrl = testletsInfo.ImageUrl
            //        };
            //        return Json(ActionHandleResult.FromSuccess(message: "", data: data), JsonRequestBehavior.AllowGet);
            //    }
            //}
            //return Json(ActionHandleResult.FromSuccess(message: "题组或者评阅不存在"), JsonRequestBehavior.AllowGet);

            return null;
        }


        /// <summary>回评
        /// </summary>
        [HttpPost]
        public ActionResult CallBackReview(ReviewViewModel reviewViewModel)
        {

            TeacherCheck teacherCheck = new TeacherCheck()
            {
                TeacherId = TeacherId,
                Score = reviewViewModel.Score
            };


            AnswerCheck answerCheck = OnlineCheckManager.Instance.AnswerSheets.SelectMany(s => s.AnswerChecks)
             .SingleOrDefault(s => s.AnswerCheckId == reviewViewModel.AnswerCheckId);

            answerCheck.TeacherCheckManagerx.UpdateTeacherChecks(teacherCheck);


            OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(s => s.QuestionGroupId == reviewViewModel.QuestionGroupId).PressReviewManager.Press(teacherCheck.TeacherId, answerCheck.AnswerCheckId, teacherCheck.Score);



            return Json(ActionHandleResult.FromSuccess(message: "回评成功"));

            //string teacherId = _teacherService.GetTeacherId(model.TestletsStructId, Teacher.OriTeacherId);
            //var reviewScorePointList = model.ReviewScores.Select(reviewScore =>
            //    new ReviewScorePointDto(
            //        reviewScore.ScorePointStructId,
            //        reviewScore.Score) {Id = reviewScore.Id}).ToList();
            //var command = new ReviewBackTestlets(model.TestletsId, model.ReviewId, teacherId, reviewScorePointList);
            //var result = await ExecuteCommandAsync(command);
            //if (result.IsSuccess())
            //{
            //    return Json(ActionHandleResult.FromSuccess(message: "回评成功"));
            //}
            //return Json(ActionHandleResult.FromFail(message: result.GetErrorMessage()), JsonRequestBehavior.AllowGet);

        }


        public ActionResult ConfirmCallBack(string id)
        {
            OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(s => s.QuestionGroupId == id).PressReviewManager.Clear(TeacherId);

            return Json(ActionHandleResult.FromSuccess(), JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region 问题卷
        /// <summary>设置为问题卷
        /// </summary>
        [HttpGet]
        public ActionResult SetProblematics(string testletsId, string testletsStructId)
        {
            //string teacherId = _teacherService.GetTeacherId(testletsStructId, OriTeacherId);
            //var command = new SetProblematics(testletsId, teacherId);
            //var result = await ExecuteCommandAsync(command);
            //if (result.IsSuccess())
            //{
            //    return Json(ActionHandleResult.FromSuccess(message: "设置成功"), JsonRequestBehavior.AllowGet);
            //}
            //return Json(ActionHandleResult.FromFail(message: result.GetErrorMessage()), JsonRequestBehavior.AllowGet);


            TeacherCheck teacherCheck = new TeacherCheck()
            {
                TeacherId = TeacherId,
                CheckType = CheckTypes.Doubt,
                TeacherInfo = TeacherInfo,
                Score = new Dictionary<string, double>()
            };



            AnswerCheck answerCheck = OnlineCheckManager.Instance.AnswerSheets.SelectMany(s => s.AnswerChecks)
                .SingleOrDefault(s => s.AnswerCheckId == testletsId);


            answerCheck.TeacherCheckManagerx.UpdateTeacherChecks(teacherCheck);



            return Json(ActionHandleResult.FromSuccess(message: "设置成功"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>问题卷提交页面ReviewTestletsViewModel model
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> ReviewProblematics(ReviewViewModel reviewViewModel)
        {
            //string teacherId = _teacherService.GetTeacherId(model.TestletsId, Teacher.OriTeacherId);
            //var reviewScorePointList = model.ReviewScores.Select(reviewScore =>
            //    new ReviewScorePointDto(
            //        reviewScore.ScorePointStructId,
            //        reviewScore.Score)).ToList();
            //var command = new ReviewProblematics(model.TestletsId, teacherId, reviewScorePointList);
            //var result = await ExecuteCommandAsync(command);
            //if (result.IsSuccess())
            //{
            //    return Json(ActionHandleResult.FromSuccess(message: "问题卷评阅成功"));
            //}
            //return Json(ActionHandleResult.FromFail(message: result.GetErrorMessage()));



            TeacherCheck teacherCheck = new TeacherCheck()
            {
                TeacherId = TeacherId,
                CheckType = CheckTypes.SolveDoubt,
                TeacherInfo = TeacherInfo,
                Score = reviewViewModel.Score
            };

            AnswerCheck answerCheck = OnlineCheckManager.Instance.AnswerSheets.SelectMany(s => s.AnswerChecks)
   .SingleOrDefault(s => s.AnswerCheckId == reviewViewModel.AnswerCheckId);

            answerCheck.TeacherCheckManagerx.SetFinalScoreForDoubt(teacherCheck);


            return Json(ActionHandleResult.FromSuccess(message: "评阅成功"));

        }

        /// <summary>查询问题卷
        /// </summary>
        [HttpGet]
        public ActionResult GetProblematics(string testletsStructId)
        {
            //string teacherId = _teacherService.GetTeacherId(testletsStructId, Teacher.OriTeacherId);
            ////问题卷处理
            //var problematics = _testletsQueryService.QueryTestletsDtos(new TestletsQueryDto(testletsStructId,
            //    problematicsStatus: ProblematicsStatus.CanReview.ToString()));
            //var data = problematics.Select(x => new
            //{
            //    TestletsId = x.TestletsId,
            //    TestletsStructId = x.TestletsStructId,
            //    ReviewMode = x.ReviewMode,
            //    ImageUrl = x.ImageUrl,
            //    FirstReviewStatus = x.FirstReviewStatus,
            //    SecondReviewStatus = x.SecondReviewStatus,
            //    ThirdReviewStatus = x.ThirdReviewStatus,
            //    ArbitrationStatus = x.ArbitrationStatus,
            //    ProblematicsStatus = x.ProblematicsStatus,
            //    Status = x.Status,
            //    Reviews = x.Reviews.Select(y => new
            //    {
            //        ReviewId = y.ReviewId,
            //        ReviewDate = y.ReviewDate.ToString("hh:mm:ss"),
            //        ReviewTeacherId = y.ReviewTeacherId,
            //        TotalScore = y.TotalScore
            //    })
            //});

            IEnumerable<dynamic> answerChecks = OnlineCheckManager.Instance.AnswerSheets.SelectMany(s => s.AnswerChecks)
                    .Where(s => s.QuestionGroupId == testletsStructId && !s.TeacherCheckManagerx.IsAllFinish && s.TeacherCheckManagerx.IsDoubt).Select(f => new
                    {
                        TestletsId = f.AnswerCheckId,
                        ImageUrl = f.CombinationedPicUrl
                    });





            return Json(ActionHandleResult.FromSuccess(data: answerChecks), JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}

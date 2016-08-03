using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace OnlineCheck
{
    public class OnlineCheckManager
    {

        #region 单一实例
        private static OnlineCheckManager _singleton;

        private OnlineCheckManager()
        {
            QuestionGroups = new List<QuestionGroup>();

            AnswerSheets = new List<AnswerSheet>();

            Teachers = new List<Teacher>();

            IsTesting = false;
        }

        public static OnlineCheckManager Instance
        {
            get
            {
                if (_singleton == null)
                {
                    Interlocked.CompareExchange(ref _singleton, new OnlineCheckManager(), null);
                }
                return _singleton;
            }
        }
        #endregion

        public List<QuestionGroup> QuestionGroups;

        public List<AnswerSheet> AnswerSheets;

        public List<Teacher> Teachers;

        public ReviewProgress GetReviewProgress(String questionGroupId)
        {
            List<TeacherCheckManager> teacherCheckManagers =
           OnlineCheckManager.Instance.AnswerSheets.SelectMany(s => s.AnswerChecks).Where(s => s.QuestionGroupId == questionGroupId)
               .Select(s => s.TeacherCheckManagerx)
               .ToList();


            var tecaherCheckDictionary = teacherCheckManagers.Select(
                s => new { ThirdCounter = s.ThirdCounter, IsAllFinish = s.IsAllFinish, EnoughCount = s.EnoughCount });


            //var tecaherCheckDictionary = teacherCheckManagers.GroupBy(f => new { f.ThirdCounter, f.EnoughCount, f.IsAllFinish })
            //    .Select(
            //        s => new { ThirdCounter = s.Key.ThirdCounter, EnoughCount = s.Key.EnoughCount, TotalCount = s.Sum(a => a.EnoughCount), IsAllFinish = s.Key.IsAllFinish }).ToList();



            String testletsStructId = questionGroupId;
            String testletsNumber = QuestionGroups.SingleOrDefault(s => s.QuestionGroupId == questionGroupId).QuestionGroupName;

            Int32 totalCount = tecaherCheckDictionary.Sum(s => s.EnoughCount);
            Int32 completeCount = tecaherCheckDictionary.Where(s => s.IsAllFinish).Sum(f => f.EnoughCount);


            Int32 firstProduceCount = tecaherCheckDictionary.Count(s => s.EnoughCount >= 1);
            Int32 firstCompleteCount = tecaherCheckDictionary.Count(s => s.ThirdCounter >= 1);



            Int32 secondProduceCount = tecaherCheckDictionary.Count(s => s.EnoughCount >= 2);
            Int32 secondCompleteCount = tecaherCheckDictionary.Count(s => s.ThirdCounter >= 2);



            Int32 thirdProduceCount = tecaherCheckDictionary.Count(s => s.EnoughCount >= 3);
            Int32 thirdCompleteCount = tecaherCheckDictionary.Count(s => s.ThirdCounter >= 3);



            Int32 arbitrationProduceCount = tecaherCheckDictionary.Count(s => s.EnoughCount == 4);
            Int32 arbitrationCompleteCount = tecaherCheckDictionary.Count(s => s.IsAllFinish && s.EnoughCount == 4);

            Int32 problematicsProduceCount = teacherCheckManagers.Count(s => s.IsDoubt);
            Int32 problematicsCompleteCount = teacherCheckManagers.Count(s => s.IsAllFinish && s.IsDoubt);




            ReviewProgress reviewProgress = new ReviewProgress(
                testletsStructId, testletsNumber, totalCount, completeCount,firstProduceCount,
                firstCompleteCount, secondProduceCount, secondCompleteCount, thirdProduceCount,
                thirdCompleteCount, arbitrationProduceCount, arbitrationCompleteCount,
                problematicsProduceCount, problematicsCompleteCount
                );


            return reviewProgress;
        }

        public List<ReviewProgress> GetReviewProgress()
        {
            return QuestionGroups.Select(s => GetReviewProgress(s.QuestionGroupId)).ToList();
        }

        public Boolean IsTesting { get;   set; }

        // #region 回评

        // public Dictionary<Int32, Queue<PressCheck>> PressReview; // 回评

        // private readonly Int32 PressCount = 10;

        // #region 增加回评
        // /// <summary>
        // /// 增加回评
        // /// </summary>
        // /// <param name="teacherId"></param>
        // /// <param name="pressCheck"></param>
        // public void Enqueue(Int32 teacherId, PressCheck pressCheck)
        // {
        //     Queue<PressCheck> queue = PressReview[teacherId];

        //     PressCheck pc = queue.SingleOrDefault(s => s.Id == pressCheck.Id);

        //     if (pc != null)
        //     {
        //         pc.IsPressed = true;

        //         pc.Score = pressCheck.Score;

        //         return;
        //     }


        //     if (queue.Count == PressCount)
        //     {
        //         while (true)
        //         {
        //             PressCheck dpressCheck = queue.Dequeue();

        //             if (!dpressCheck.IsPressed)
        //             {
        //                 break;
        //             }

        //             CallBack(dpressCheck);
        //         }
        //     }

        //     queue.Enqueue(pressCheck);
        // }
        // #endregion

        // #region 手动回评
        // /// <summary>
        // /// 手动回评
        // /// </summary>
        // /// <param name="teacherId"></param>
        // /// <param name="answerCheckId"></param>
        // /// <param name="socre"></param>
        // public void Press(Int32 teacherId, String answerCheckId, Dictionary<String, Double> socre)
        // {
        //     PressCheck pressCheck = PressReview[teacherId].SingleOrDefault(s => s.AnswerCheckId == answerCheckId);

        //     if (pressCheck == null)
        //     {
        //         return;
        //     }

        //     pressCheck.IsPressed = true;

        //     pressCheck.Score = socre;

        //     CallBack(pressCheck);

        // }
        // #endregion

        // #region 清理回评
        // /// <summary>
        // /// 清理回评
        // /// </summary>
        // /// <param name="teacherId"></param>
        // public void Clear(Int32 teacherId)
        // {
        //     Queue<PressCheck> queue = PressReview[teacherId];

        //     while (queue.Any())
        //     {
        //         CallBack(queue.Dequeue());
        //     }

        // }
        // #endregion

        // private void CallBack(PressCheck pressCheck)
        // {
        //     AnswerCheck answerCheck = AnswerSheets.SelectMany(s => s.AnswerChecks)
        //.SingleOrDefault(s => s.AnswerCheckId == pressCheck.AnswerCheckId);

        //     answerCheck.TeacherCheckManagerx.PressReturn();
        // }

        // #endregion
    }
}
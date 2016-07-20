using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OnlineCheck
{
    public class OnlineCheckManager
    {

        #region 单一实例
        private static OnlineCheckManager _singleton;

        private OnlineCheckManager()
        {
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


        public IEnumerable<QuestionGroup> QuestionGroups;


        public void Init(IEnumerable<QuestionGroup> questionGroups, IEnumerable<Int32> teacherIds)
        {
            PressReview = new Dictionary<int, Queue<PressCheck>>();

            foreach (Int32 id in teacherIds)
            {
                PressReview.Add(id, new Queue<PressCheck>());
            }

            QuestionGroups = questionGroups;
        }

      

        #region 回评

        public Dictionary<Int32, Queue<PressCheck>> PressReview; // 回评

        private readonly Int32 PressCount = 10;

        #region 增加回评
        /// <summary>
        /// 增加回评
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="pressCheck"></param>
        public void Enqueue(Int32 teacherId, PressCheck pressCheck)
        {
            Queue<PressCheck> queue = PressReview[teacherId];

            PressCheck pc = queue.SingleOrDefault(s => s.Id == pressCheck.Id);

            if (pc != null)
            {
                pc.IsPressed = true;

                pc.Score = pressCheck.Score;

                return;
            }


            if (queue.Count == PressCount)
            {
                while (true)
                {
                    PressCheck dpressCheck = queue.Dequeue();

                    if (!dpressCheck.IsPressed)
                    {
                        break;
                    }
                }
            }

            queue.Enqueue(pressCheck);
        } 
        #endregion

        #region 手动回评
        /// <summary>
        /// 手动回评
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="questionCheckId"></param>
        /// <param name="socre"></param>
        public void Press(Int32 teacherId, String questionCheckId, Double socre)
        {
            PressCheck pressCheck = PressReview[teacherId].SingleOrDefault(s => s.QuestionCheckId == questionCheckId);

            if (pressCheck == null)
            {
                return;
            }

            pressCheck.IsPressed = true;

            pressCheck.Score = socre;

        } 
        #endregion

        #region 清理回评
        /// <summary>
        /// 清理回评
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="questionGroupId"></param>
        public void Clear(Int32 teacherId, String questionGroupId)
        {
            Queue<PressCheck> queue = PressReview[teacherId];

            while (queue.Any())
            {
                PressCheck pressCheck = queue.Dequeue();

                QuestionGroups.SingleOrDefault(s => s.QuestionGroupId == questionGroupId).Questions.SingleOrDefault(s => s.QuestionCheckId == pressCheck.QuestionCheckId).TeacherCheckManagerx.PressReturn();
            }

        }  
        #endregion

        #endregion
    }
}
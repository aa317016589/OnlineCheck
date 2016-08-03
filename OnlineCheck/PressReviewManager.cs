using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineCheck
{
    public class PressReviewManager
    {
        public Dictionary<Int32, Queue<PressCheck>> PressReview;

        private readonly Int32 _pressPressCount;

        public PressReviewManager(IEnumerable<Teacher> teachers, int pressCount)
        {
            PressReview = new Dictionary<Int32, Queue<PressCheck>>();


            foreach (Teacher teacher in teachers)
            {
                PressReview.Add(teacher.TeacherId, new Queue<PressCheck>());
            }

            _pressPressCount = pressCount;
        }



        public PressReviewManager(IEnumerable<Teacher> teachers)
            : this(teachers, 10)
        {

        }


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


            if (queue.Count == _pressPressCount)
            {
                while (true)
                {
                    PressCheck dpressCheck = queue.Dequeue();

                    if (!dpressCheck.IsPressed)
                    {
                        break;
                    }

                    CallBack(dpressCheck);
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
        /// <param name="answerCheckId"></param>
        /// <param name="socre"></param>
        public void Press(Int32 teacherId, String answerCheckId, Dictionary<String, Double> socre)
        {
            PressCheck pressCheck = PressReview[teacherId].SingleOrDefault(s => s.AnswerCheckId == answerCheckId);

            if (pressCheck == null)
            {
                return;
            }

            pressCheck.IsPressed = true;

            pressCheck.Score = socre;

            CallBack(pressCheck);

        }
        #endregion

        #region 清理回评
        /// <summary>
        /// 清理回评
        /// </summary>
        /// <param name="teacherId"></param>
        public void Clear(Int32 teacherId)
        {
            Queue<PressCheck> queue = PressReview[teacherId];

            while (queue.Any())
            {
                CallBack(queue.Dequeue());
            }

        }
        #endregion

        private void CallBack(PressCheck pressCheck)
        {
            AnswerCheck answerCheck = OnlineCheckManager.Instance.AnswerSheets.SelectMany(s => s.AnswerChecks)
       .SingleOrDefault(s => s.AnswerCheckId == pressCheck.AnswerCheckId);

            answerCheck.TeacherCheckManagerx.PressReturn();
        }
    }


    /// <summary>
    /// 回评
    /// </summary>
    public class PressCheck
    {
        public String Id { get; private set; }

        public String AnswerCheckId { get; set; }

        public Dictionary<String, Double> Score { get; set; }

        /// <summary>
        /// 表示已经被回评处理过，即手动选择回评列表进行操作的
        /// </summary>
        public Boolean IsPressed { get; set; }

        public DateTime CreateDateTime { get; set; }

        public PressCheck()
        {
            Id = Guid.NewGuid().ToString();

            IsPressed = false;

            CreateDateTime = DateTime.Now;
        }
    }
}
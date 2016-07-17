using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineCheck
{
    public abstract class TeacherCheckManager
    {
        protected List<TeacherCheck> TeacherChecks;

        protected Double Threshold;

        public Double FinalScore { get; protected set; }

        protected Boolean IsAllFinish { get; set; }

        public Boolean HaveDoubt()
        {
            return TeacherChecks.Any(s => s.IsDoubt);
        }

        public Boolean IsFinish(Int32 teacherId)
        {
            if (IsAllFinish)
            {
                return true;
            }

            return TeacherChecks.Any(s => s.TeacherId == teacherId && s.IsOver);
        }

        public Boolean IsFinish()
        {
            return IsAllFinish;
        }

        protected Boolean IsEnough
        {
            get
            {
                return TeacherChecks.Capacity == TeacherChecks.Count;
            }
        }

        public TeacherCheckManager(JudgeModes judgeMode, Double threshold)
        {
            TeacherChecks = new List<TeacherCheck>((Int32)judgeMode);

            IsAllFinish = false;

            FinalScore = 0;

            Threshold = threshold;
        }

        public TeacherCheckManager(JudgeModes judgeMode)
            : this(judgeMode, 0)
        {

        }

        /// <summary>
        /// 占位符，把题目分给老师
        /// </summary>
        /// <param name="teacherCheck"></param>
        public void AddTeacherChecks(TeacherCheck teacherCheck)
        {
            if (IsEnough || IsAllFinish)
            {
                return;
            }

            if (TeacherChecks.Any(s=>s.TeacherId == teacherCheck.TeacherId))
            {
                return;
            }

            TeacherChecks.Add(teacherCheck);
        }

        /// <summary>
        /// 增加老师打分数据
        /// </summary>
        /// <param name="readyTeacherCheck"></param>
        public void UpdateTeacherChecks(TeacherCheck readyTeacherCheck)
        {
            TeacherCheck teacherCheck =
                TeacherChecks.SingleOrDefault(s => s.TeacherId == readyTeacherCheck.TeacherId);

            teacherCheck.IsOver = true;

            teacherCheck.IsDoubt = readyTeacherCheck.IsDoubt;

            teacherCheck.Score = readyTeacherCheck.Score;


            if (ReadyCheck())
            {
                Statistics();
            }
        }

        /// <summary>
        /// 计算分数
        /// </summary>
        protected abstract void Statistics();

        /// <summary>
        /// 检测是否满足最后计算
        /// </summary>
        /// <returns></returns>
        protected virtual Boolean ReadyCheck()
        {
            return IsEnough;
        }
    }

    /// <summary>
    /// 1
    /// </summary>
    public class TeacherCheckManagerFirst : TeacherCheckManager
    {
        public TeacherCheckManagerFirst()
            : base(JudgeModes.SingleReview)
        {

        }



        protected override void Statistics()
        {
            FinalScore = TeacherChecks.SingleOrDefault().Score;

            IsAllFinish = true;
        }
    }

    /// <summary>
    /// 2
    /// </summary>
    public class TeacherCheckManagerSecond : TeacherCheckManager
    {
        public TeacherCheckManagerSecond()
            : base(JudgeModes.MultiReview)
        {

        }

        protected override void Statistics()
        {
            FinalScore = TeacherChecks.Average(s => s.Score);

            IsAllFinish = true;
        }


    }

    /// <summary>
    /// 2+1
    /// </summary>
    public class TeacherCheckManagerThird : TeacherCheckManager
    {
        public TeacherCheckManagerThird(Double threshold)
            : base(JudgeModes.ThirdReview, threshold)
        {

        }

        protected override Boolean ReadyCheck()
        {
            if (base.ReadyCheck())
            {
                return true;
            }

            if (TeacherChecks.Count == 2)
            {
                return OnlineHelper.GetMaxThreshold(TeacherChecks.Select(s => s.Score).ToArray()) < Threshold;
            }

            return false;
        }

        protected override void Statistics()
        {
            IsAllFinish = true;

            FinalScore = OnlineHelper.GetMiddleScore(TeacherChecks.Select(s => s.Score).ToArray());
        }


    }

    /// <summary>
    ///  2+1+1
    /// </summary>
    public class TeacherCheckManagerFourth : TeacherCheckManager
    {
        public TeacherCheckManagerFourth(Double threshold)
            : base(JudgeModes.FourReview, threshold)
        {

        }

        protected override Boolean ReadyCheck()
        {
            if (base.ReadyCheck())
            {
                return true;
            }

            if (TeacherChecks.Count > 1)
            {
                return OnlineHelper.GetMaxThreshold(TeacherChecks.Select(s => s.Score).ToArray()) < Threshold;
            }

            return false;
        }

        protected override void Statistics()
        {
            IsAllFinish = true;

            //Double average = TeacherChecks.Take(2).Average(s => s.Score);

            //if (average < Threshold)
            //{
            //    FinalScore = average; return;
            //}

            //average = TeacherChecks.Take(3).Average(s => s.Score);

            //if (average < Threshold)
            //{
            //    FinalScore = average; return;
            //}

            //FinalScore = TeacherChecks.Last().Score;


            FinalScore = IsEnough ? TeacherChecks.Last().Score : OnlineHelper.GetMiddleScore(TeacherChecks.Select(s => s.Score).ToArray());
        }
    }

    public static class TeacherCheckManagerFactory
    {
        public static TeacherCheckManager CreaTeacherCheckManager(JudgeModes judgeMode, Double Threshold)
        {
            TeacherCheckManager teacherCheckManager = null;

            switch (judgeMode)
            {
                case JudgeModes.SingleReview:

                    teacherCheckManager = new TeacherCheckManagerFirst();

                    break;
                case JudgeModes.MultiReview:

                    teacherCheckManager = new TeacherCheckManagerSecond();

                    break;
                case JudgeModes.ThirdReview:
                    teacherCheckManager = new TeacherCheckManagerThird(Threshold);

                    break;
                case JudgeModes.FourReview:

                    teacherCheckManager = new TeacherCheckManagerFourth(Threshold);

                    break;
            }

            return teacherCheckManager;
        }
    }
}
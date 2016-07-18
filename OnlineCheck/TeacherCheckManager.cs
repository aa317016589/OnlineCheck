using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineCheck
{
    public abstract class TeacherCheckManager
    {
        protected List<TeacherCheck> TeacherChecks;

        /// <summary>
        /// 阀值
        /// </summary>
        protected Double Threshold;

        /// <summary>
        /// 最后得分
        /// </summary>
        public Double FinalScore { get; protected set; }

        /// <summary>
        /// 是否完全结束
        /// </summary>
        public Boolean IsAllFinish { get; set; }

        /// <summary>
        /// 是否含有问题类
        /// </summary>
        /// <returns></returns>
        public Boolean IsDoubt { get; protected set; }


        /// <summary>
        /// 是否是仲裁类
        /// </summary>
        /// <returns></returns>
        public Boolean IsArbitration { get; protected set; }


        public Boolean IsX(Int32 teacherId)
        {
            if (IsAllFinish)
            {
                return false;
            }

            if (!IsAllow)
            {
                return false;
            }

            if (TeacherChecks.Count >= EnoughCount)
            {
                return false;
            }


            return !TeacherChecks.Any(s => s.TeacherId == teacherId && s.IsOver);
        }


        /// <summary>
        /// 打分集合上限数
        /// </summary>
        protected Int32 EnoughCount { get; set; }


        /// <summary>
        /// 是否允许获取
        /// </summary>
        protected Boolean IsAllow { get; set; }

        /// <summary>
        /// 已提交回评数
        /// </summary>
        protected Int32 ThirdCounter { get; set; }



        protected TeacherCheckManager(JudgeModes judgeMode, Double threshold)
        {
            TeacherChecks = new List<TeacherCheck>((Int32)judgeMode);

            IsAllFinish = false;

            IsArbitration = false;

            FinalScore = 0;

            ThirdCounter = 0;

            IsAllow = true;

            Threshold = threshold;
        }

        protected TeacherCheckManager(JudgeModes judgeMode)
            : this(judgeMode, 0)
        {

        }




        /// <summary>
        /// 占位符，把题目分给老师
        /// </summary>
        /// <param name="teacherCheck"></param>
        public void AddTeacherChecks(TeacherCheck teacherCheck)
        {
            if (TeacherChecks.Count >= EnoughCount || IsAllFinish)
            {
                return;
            }

            if (TeacherChecks.Any(s => s.TeacherId == teacherCheck.TeacherId && teacherCheck.CheckType == CheckTypes.Ordinary))
            {
                return;
            }

            TeacherChecks.Add(teacherCheck);
        }

        /// <summary>
        /// 增加老师打分数据
        /// </summary>
        /// <param name="readyTeacherCheck"></param>
        public void UpdateTeacherChecks(TeacherCheck readyTeacherCheck, CheckTypes checkType = CheckTypes.Ordinary)
        {
            TeacherCheck teacherCheck =
                TeacherChecks.SingleOrDefault(s => s.TeacherId == readyTeacherCheck.TeacherId && s.CheckType == checkType);

            teacherCheck.IsOver = true;

            teacherCheck.CheckType = readyTeacherCheck.CheckType;

            teacherCheck.Score = readyTeacherCheck.Score;

            if (teacherCheck.CheckType == CheckTypes.Doubt)
            {
                IsAllFinish = true;

                IsDoubt = true;
            }
        }

        public void SetFinalScoreForDoubt(TeacherCheck teacherCheck)
        {
            if (!IsDoubt)
            {
                return;
            }

            FinalScore = teacherCheck.Score;

            TeacherChecks.Add(teacherCheck);

            IsDoubt = false;
        }

        /// <summary>
        /// 计算分数,回评被移除的时候执行的方法
        /// </summary>
        protected abstract void Statistics();

        public void PressReturn()
        {
            ThirdCounter = ThirdCounter + 1;
            Statistics();
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
            EnoughCount = 1;
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
            EnoughCount = 2;
        }

        protected override void Statistics()
        {
            if (ThirdCounter == TeacherChecks.Capacity)
            {
                FinalScore = TeacherChecks.Average(s => s.Score);

                IsAllFinish = true;
            }
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
            EnoughCount = 2;
        }

        protected override void Statistics()
        {
            if (ThirdCounter < 2)
            {
                return;
            }

            if (ThirdCounter == 2)
            {
                IsAllow = OnlineHelper.GetMinThreshold(TeacherChecks.Select(s => s.Score).ToArray()) > Threshold;

                if (IsAllow)
                {
                    EnoughCount = EnoughCount + 1;

                    return;
                }
            }


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
            EnoughCount = 2;
        }


        protected override void Statistics()
        {
            if (ThirdCounter < 2)
            {
                return;
            }

            if (!IsArbitration)
            {
                IsAllow = OnlineHelper.GetMinThreshold(TeacherChecks.Select(s => s.Score).ToArray()) > Threshold;

                if (IsAllow)
                {
                    EnoughCount = EnoughCount + 1;

                    if (ThirdCounter == 3)
                    {
                        IsArbitration = true;

                        IsAllow = false;
                    }

                    return;
                }
            }

            IsAllFinish = true;

            FinalScore = TeacherChecks.Capacity == TeacherChecks.Count
                ? TeacherChecks.Last().Score
                : OnlineHelper.GetMiddleScore(TeacherChecks.Select(s => s.Score).ToArray());
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
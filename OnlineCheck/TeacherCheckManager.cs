using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineCheck
{
    public abstract class TeacherCheckManager
    {
        public List<TeacherCheck> TeacherChecks;

        protected List<Answer> ReadyCheckAnswers { get; set; }

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


        /// <summary>
        /// 普通获取
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public Boolean IsGet(Int32 teacherId)
        {
            if (IsAllFinish)
            {
                return false;
            }

            if (!IsAllow)
            {
                return false;
            }

            TeacherCheck teacherCheck = TeacherChecks.SingleOrDefault(s => s.TeacherId == teacherId);

            if (teacherCheck != null)
            {
                return !teacherCheck.IsOver;
            }


            if (TeacherChecks.Count >= EnoughCount)
            {
                return false;
            }


            return true;
        }


        /// <summary>
        /// 打分集合上限数
        /// </summary>
        public Int32 EnoughCount { get; set; }


        /// <summary>
        /// 是否允许获取
        /// </summary>
        protected Boolean IsAllow { get; set; }

        /// <summary>
        /// 已提交回评数
        /// </summary>
        public Int32 ThirdCounter { get; set; }



        protected TeacherCheckManager(JudgeModes judgeMode, List<Answer> answers)
        {
            TeacherChecks = new List<TeacherCheck>((Int32)judgeMode);

            IsAllFinish = false;

            IsArbitration = false;

            ThirdCounter = 0;

            IsAllow = true;

            ReadyCheckAnswers = answers;
        }




        /// <summary>
        /// 占位符，把题目分给老师
        /// </summary>
        /// <param name="teacherCheck"></param>
        public void AddTeacherChecks(TeacherCheck teacherCheck)
        {
            if (TeacherChecks.Count >= EnoughCount || IsAllFinish || IsDoubt)
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
        /// <param name="checkType"></param>
        public void UpdateTeacherChecks(TeacherCheck readyTeacherCheck, CheckTypes checkType = CheckTypes.Ordinary)
        {
            TeacherCheck teacherCheck =
                TeacherChecks.SingleOrDefault(s => s.TeacherId == readyTeacherCheck.TeacherId && s.CheckType == checkType);

            teacherCheck.IsOver = true;

            teacherCheck.Score = readyTeacherCheck.Score;

            teacherCheck.TeacherInfo = readyTeacherCheck.TeacherInfo;

            if (readyTeacherCheck.CheckType == CheckTypes.Doubt)
            {
                IsDoubt = true;

                IsAllow = false;
            }
        }

        /// <summary>
        /// 对问题卷进行处理
        /// </summary>
        /// <param name="teacherCheck"></param>
        public void SetFinalScoreForDoubt(TeacherCheck teacherCheck)
        {
            if (!IsDoubt && IsAllFinish)
            {
                return;
            }

            ReadyCheckAnswers.ForEach(s =>
            {
                s.FinalScore = teacherCheck.Score[s.QuestionInfo.QuestionId.ToString()];
            });

            TeacherChecks.Add(teacherCheck);

            IsAllFinish = true;

        }

        /// <summary>
        /// 计算分数,回评被移除的时候执行的方法
        /// </summary>
        protected abstract void Statistics();

        /// <summary>
        /// 执行回评之后的回调
        /// </summary>
        public void PressReturn()
        {
            ThirdCounter = ThirdCounter + 1;
            Statistics();
        }

        public Dictionary<String, Double> GetFinalScore()
        {
            if (IsDoubt || IsArbitration || !IsAllFinish )
            {
                return null;
            }

            return ReadyCheckAnswers.ToDictionary(k => k.AnswerId, v => v.FinalScore);
        }

    }

    /// <summary>
    /// 1
    /// </summary>
    public class TeacherCheckManagerFirst : TeacherCheckManager
    {
        public TeacherCheckManagerFirst(List<Answer> answers)
            : base(JudgeModes.SingleReview, answers)
        {
            EnoughCount = 1;
        }

        protected override void Statistics()
        {
            ReadyCheckAnswers.ForEach(s =>
            {
                s.FinalScore = TeacherChecks.SingleOrDefault().Score[s.QuestionInfo.QuestionId.ToString()];
            });

            IsAllFinish = true;
        }
    }

    /// <summary>
    /// 2
    /// </summary>
    public class TeacherCheckManagerSecond : TeacherCheckManager
    {
        public TeacherCheckManagerSecond(List<Answer> answers)
            : base(JudgeModes.MultiReview, answers)
        {
            EnoughCount = 2;
        }

        protected override void Statistics()
        {
            if (ThirdCounter != TeacherChecks.Capacity)
            {
                return;
            }

            ReadyCheckAnswers.ForEach(s =>
            {
                s.FinalScore = TeacherChecks.Average(a => a.Score[s.QuestionInfo.QuestionId.ToString()]);
            });

            IsAllFinish = true;
        }


    }

    /// <summary>
    /// 2+1
    /// </summary>
    public class TeacherCheckManagerThird : TeacherCheckManager
    {
        public TeacherCheckManagerThird(List<Answer> answers)
            : base(JudgeModes.ThirdReview, answers)
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
                Boolean flag = false;

                foreach (var readyCheckAnswer in ReadyCheckAnswers)
                {

                    Boolean a = (OnlineHelper.GetMinThreshold(
                             TeacherChecks.Select(s => s.Score[readyCheckAnswer.QuestionInfo.QuestionId.ToString()]).ToArray()) >
                          readyCheckAnswer.QuestionInfo.Threshold);
                    flag = flag || a;

                    if (flag)
                    {
                        break; ;
                    }
                }

                IsAllow = flag;

                //    IsAllow = OnlineHelper.GetMinThreshold(TeacherChecks.Select(s => s.Score).ToArray()) > Threshold;

                if (IsAllow)
                {
                    EnoughCount = EnoughCount + 1;

                    return;
                }
            }


            IsAllFinish = true;

            ReadyCheckAnswers.ForEach(s =>
            {
                s.FinalScore = OnlineHelper.GetMiddleScore(TeacherChecks.Select(a => a.Score[s.QuestionInfo.QuestionId.ToString()]).ToArray());
            });
            //FinalScore = OnlineHelper.GetMiddleScore(TeacherChecks.Select(s => s.Score).ToArray());
        }

    }

    /// <summary>
    ///  2+1+1
    /// </summary>
    public class TeacherCheckManagerFourth : TeacherCheckManager
    {
        public TeacherCheckManagerFourth(List<Answer> answers)
            : base(JudgeModes.FourReview, answers)
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
                //    IsAllow = OnlineHelper.GetMinThreshold(TeacherChecks.Select(s => s.Score).ToArray()) > Threshold;
                Boolean flag = false;

                foreach (var readyCheckAnswer in ReadyCheckAnswers)
                {

                    Double f =
                        OnlineHelper.GetMinThreshold(
                            TeacherChecks.Select(s => s.Score[readyCheckAnswer.QuestionInfo.QuestionId.ToString()]).ToArray());

                    flag = flag || f >
                     readyCheckAnswer.QuestionInfo.Threshold;

                    if (flag)
                    {
                        break;
                    }

                }

                IsAllow = flag;

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

            ReadyCheckAnswers.ForEach(s =>
            {
                s.FinalScore = TeacherChecks.Capacity == TeacherChecks.Count ? TeacherChecks.Last().Score[s.AnswerId] : OnlineHelper.GetMiddleScore(TeacherChecks.Select(a => a.Score[s.QuestionInfo.QuestionId.ToString()]).ToArray());
            });
            //FinalScore = TeacherChecks.Capacity == TeacherChecks.Count
            //    ? TeacherChecks.Last().Score
            //    : OnlineHelper.GetMiddleScore(TeacherChecks.Select(s => s.Score).ToArray());
        }
    }



    public static class TeacherCheckManagerFactory
    {
        public static TeacherCheckManager CreaTeacherCheckManager(JudgeModes judgeMode, List<Answer> answers)
        {
            TeacherCheckManager teacherCheckManager = null;

            switch (judgeMode)
            {
                case JudgeModes.SingleReview:

                    teacherCheckManager = new TeacherCheckManagerFirst(answers);

                    break;
                case JudgeModes.MultiReview:

                    teacherCheckManager = new TeacherCheckManagerSecond(answers);

                    break;
                case JudgeModes.ThirdReview:
                    teacherCheckManager = new TeacherCheckManagerThird(answers);

                    break;
                case JudgeModes.FourReview:

                    teacherCheckManager = new TeacherCheckManagerFourth(answers);

                    break;
            }

            return teacherCheckManager;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineCheck
{
    public abstract class TeacherCheckManager : ICloneable 
    {
        protected List<TeacherCheck> TeacherChecks;

        protected Int32 Capacity;

        public Int32 Threshold;

        public Double FinalScore { get; protected set; }

        public Boolean IsFinish { get; protected set; }

        public TeacherCheckManager(JudgeModes judgeMode)
        {
            TeacherChecks = new List<TeacherCheck>((Int32)judgeMode);

            Capacity = (Int32)judgeMode;

            IsFinish = false;

            FinalScore = 0;

            Threshold = 0;
        }

        public void AddTeacherChecks(TeacherCheck teacherCheck)
        {
            if (TeacherChecks.Capacity == TeacherChecks.Count)
            {
                return;
            }

            TeacherChecks.Add(teacherCheck);

            if (TeacherChecks.Capacity == TeacherChecks.Count)
            {
                Statistics();
            }
        }

        protected abstract void Statistics();

        public object Clone()
        {
            return this.Clone(); 
        }

        public Boolean HaveDoubt()
        {
            return TeacherChecks.Any(s => s.IsDoubt);
        }
    }

    public class TeacherCheckManagerFirst : TeacherCheckManager
    {
        public TeacherCheckManagerFirst()
            : base(JudgeModes.SingleReview)
        {

        }



        protected override void Statistics()
        {
            FinalScore = TeacherChecks.SingleOrDefault().Score;

            IsFinish = true;
        }
    }

    public class TeacherCheckManagerSecond : TeacherCheckManager
    {
        public TeacherCheckManagerSecond()
            : base(JudgeModes.MultiReview)
        {

        }

        protected override void Statistics()
        {
            FinalScore = TeacherChecks.Average(s => s.Score);

            IsFinish = true;
        }


    }

    public class TeacherCheckManagerThird : TeacherCheckManager
    {
        public TeacherCheckManagerThird() 
            : base(JudgeModes.ThirdReview)
        {
            
        }

        protected override void Statistics()
        {
            IsFinish = true;

            Double average = TeacherChecks.Take(2).Average(s => s.Score);

            if (average < Threshold)
            {
                FinalScore = average;
            }

            FinalScore = GetMiddleScore(TeacherChecks.Select(s => s.Score).ToArray());
        }

        /// <summary>
        /// 取N个分数中较接近的两个作为最终分数
        /// </summary>
        /// <param name="scores"></param>
        /// <returns></returns>
        private double GetMiddleScore(params Double[] scores)
        {
            Int32 length = scores.Length;

            if (length == 0)
            {
                return 0;
            }

            if (length == 1)
            {
                return scores[0];
            }

            if (length == 2)
            {
                return (scores[0] + scores[1]) / 2;
            }

            scores = scores.OrderByDescending(s => s).ToArray();

            Double difference = scores[0] - scores[1], nowDiff = 0;

            int j = 0;

            Boolean Ist = true; //等差数列

            for (int i = 1; i < length - 1; i++)
            {
                nowDiff = Math.Abs(scores[i] - scores[i + 1]);

                Ist = Ist && difference == nowDiff;

                if (difference > nowDiff)
                {
                    difference = nowDiff;

                    j = i;
                }
            }


            if (Ist)
            {
                return scores.Average();
            }

            return (scores[j + 1] + scores[j]) / 2;
        }
    }


    public class TeacherCheckManagerFourth : TeacherCheckManager
    {
        public TeacherCheckManagerFourth()
            : base(JudgeModes.FourReview)
        {

        }
        protected override void Statistics()
        {
            throw new NotImplementedException();
        }
    }

    public static class TeacherCheckManagerFactory
    {
        public static TeacherCheckManager CreaTeacherCheckManager(JudgeModes judgeMode)
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
                    teacherCheckManager = new TeacherCheckManagerThird();

                    break;
                case JudgeModes.FourReview:

                    teacherCheckManager = new TeacherCheckManagerFourth();

                    break;
            }

            return teacherCheckManager;
        }
    }
}
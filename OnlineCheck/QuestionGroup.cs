using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineCheck
{
    public class QuestionGroup
    {
        public Int32 QuestionGroupId { get; set; }


        public JudgeModes JudgeMode { get; set; }

        public Queue<Question> Questions { get; set; }

        //这里生成一个Check

    }

    public class Question
    {
        public String Id { get; private set; }

        public Int32 QuestionId { get; set; }

        public Int32 StudentSubjectId { get; set; }

        public String PicUrl { get; set; }

        public JudgeModes JudgeMode { get; set; }

        public Int32 FinalScore { get; set; }

        public Boolean IsFinish { get; set; }

        private List<TeacherCheck> _teacherChecks;

        public Question()
        {
            Id = Guid.NewGuid().ToString();
        }

        public void AddTeacherChecks(TeacherCheck teacherCheck)
        {
            Int32 len = _teacherChecks.Count;

            switch (JudgeMode)
            {
                case JudgeModes.SingleReview:
                    if (len > 0)
                    {
                        return;
                    }
                    break;
                case JudgeModes.MultiReview:

                    if (len > 1)
                    {
                        return;
                    }

                    break;
                case JudgeModes.ThirdReview:


                    break;
                case JudgeModes.FourReview:
                    break;
            }

            _teacherChecks.Add(teacherCheck);
        }

        public Boolean HaveDoubt()
        {
            return _teacherChecks.Any(s => s.IsDoubt);
        }


        public void Statistics()
        {
            //最后算分数
        }
    }

    public class TeacherCheck
    {
        public Int32 TeacherId { get; set; }

        public Int32 Score { get; set; }

        public Boolean IsDoubt { get; set; }

        public CheckModes CheckMode { get; set; }
    }


    public abstract class Checkx
    {
        private List<TeacherCheck> _teacherChecks;

        public Checkx(Int32 capacity)
        {
            _teacherChecks = new List<TeacherCheck>(capacity);
        }

        public abstract void AddTeacherChecks();

        public abstract void Statistics();



        //实现4种阅卷模式 将这种类对象包含到Question类中负责维护老师的打分和最终分数的计算
    }
}
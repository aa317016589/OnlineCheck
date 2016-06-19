using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace OnlineCheck
{
    public class QuestionGroup
    {
        public Int32 QuestionGroupId { get; set; }

        public JudgeModes JudgeMode { get; set; }

        public List<Question> Questions { get; private set; }

        public QuestionGroup(Int32 questionGroupId, JudgeModes judgeMode)
        {
            QuestionGroupId = questionGroupId;

            JudgeMode = judgeMode;

            teacherCheckManager = TeacherCheckManagerFactory.CreaTeacherCheckManager(JudgeMode);

            Questions = new List<Question>();
        }



        private TeacherCheckManager teacherCheckManager;





    }

    public class Question
    {
        public String QuestionCheckId { get; private set; }

        public Int32 QuestionId { get; private set; }

        public Int32 StudentSubjectId { get; private set; }

        public String PicUrl { get; private set; }
 
        /// <summary>
        /// 阀值
        /// </summary>
        public Int32 Threshold { get; private set; }

        public TeacherCheckManager TeacherCheckManagerx { get; private set; }
 
        public Question(Int32 questionId, Int32 studentSubjectId, TeacherCheckManager teacherCheckManagerx, Int32 threshold, String picUrl)
        {
            QuestionCheckId = Guid.NewGuid().ToString();

            Threshold = threshold;

            QuestionId = questionId;

            StudentSubjectId = studentSubjectId;

            PicUrl = picUrl;
        }

        //public void AddTeacherChecks(TeacherCheck teacherCheck)
        //{
        //    Int32 len = _teacherChecks.Count;

        //    switch (JudgeMode)
        //    {
        //        case JudgeModes.SingleReview:
        //            if (len > 0)
        //            {
        //                return;
        //            }
        //            break;
        //        case JudgeModes.MultiReview:

        //            if (len > 1)
        //            {
        //                return;
        //            }

        //            break;
        //        case JudgeModes.ThirdReview:


        //            break;
        //        case JudgeModes.FourReview:
        //            break;
        //    }

        //    _teacherChecks.Add(teacherCheck);
        //}

        //public Boolean HaveDoubt()
        //{
        //    return _teacherChecks.Any(s => s.IsDoubt);
        //}

    }

    public class TeacherCheck
    {
        public Int32 TeacherId { get; set; }

        public Double Score { get; set; }

        public Boolean IsDoubt { get; set; }

        public Int32 QuestionId { get; set; }

        public Boolean IsOver { get; set; }

        public CheckModes CheckMode { get; set; }
    }




    //public abstract class Checkx
    //{
    //    private List<TeacherCheck> _teacherChecks;

    //    public Checkx(Int32 capacity)
    //    {
    //        _teacherChecks = new List<TeacherCheck>(capacity);
    //    }

    //    public abstract void AddTeacherChecks();

    //    public abstract void Statistics();



    //    //实现4种阅卷模式 将这种类对象包含到Question类中负责维护老师的打分和最终分数的计算
    //}


}
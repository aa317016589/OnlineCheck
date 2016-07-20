using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineCheck
{
    public class QuestionGroup
    {
        public String QuestionGroupId { get; set; }

        public JudgeModes JudgeMode { get; set; }

        public List<Question> Questions { get; private set; }

        public QuestionGroup(String questionGroupId, JudgeModes judgeMode)
        {
            QuestionGroupId = questionGroupId;

            JudgeMode = judgeMode;

            Questions = new List<Question>();
        }

        public Question SelectQuestion(Int32 teacherId)
        {
            return Questions.FirstOrDefault(s => s.TeacherCheckManagerx.IsX(teacherId));
        }

        public Question SeleteDoubtQuestion()
        {
            return Questions.FirstOrDefault(s => s.TeacherCheckManagerx.IsDoubt && !s.TeacherCheckManagerx.IsAllFinish);
        }

        public Question SeleteArbitrationQuestion()
        {
            return Questions.FirstOrDefault(s => s.TeacherCheckManagerx.IsArbitration && !s.TeacherCheckManagerx.IsAllFinish);
        }
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

        public Question(Int32 questionId, Int32 studentSubjectId, Int32 threshold, String picUrl, JudgeModes judgeMode)
        {
            QuestionCheckId = Guid.NewGuid().ToString();

            Threshold = threshold;

            QuestionId = questionId;

            StudentSubjectId = studentSubjectId;

            PicUrl = picUrl;

            PicUrl = QuestionCheckId;

            TeacherCheckManagerx = TeacherCheckManagerFactory.CreaTeacherCheckManager(judgeMode, Threshold);
        }
    }

    public class TeacherCheck
    {
        public Int32 TeacherId { get; set; }

        public Double Score { get; set; }

        public CheckTypes CheckType { get; set; }

        public String QuestionCheckId { get; set; }

        /// <summary>
        /// 表示已经更新了老师打的分数
        /// </summary>
        public Boolean IsOver { get; set; }


        public TeacherCheck()
        {
            IsOver = false;

            CheckType = CheckTypes.Ordinary;
        }

        public override string ToString()
        {
            return String.Format("{0} 对 {1} 设 {2}", TeacherId.ToString(), QuestionCheckId, Score.ToString());
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class PressCheck
    {
        public String Id { get; private set; }

        public Double Score { get; set; }

        public String QuestionCheckId { get; set; }

        /// <summary>
        /// 表示已经被回评处理过，即手动选择回评列表进行操作的
        /// </summary>
        public Boolean IsPressed { get; set; }

        public PressCheck()
        {
            Id = Guid.NewGuid().ToString();

            IsPressed = false;
        }
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
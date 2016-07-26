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

    }

    public class Question
    {
        public String QuestionGroupId { get; set; }

        public Int32 QuestionId { get; private set; }

        public Double MaxScore { get; private set; }

        /// <summary>
        /// 阀值
        /// </summary>
        public Int32 Threshold { get; private set; }

        public Question(String questionGroupId, Int32 questionId, Int32 threshold, Double maxScore)
        {
            Threshold = threshold;

            QuestionId = questionId;

            MaxScore = maxScore;

            QuestionGroupId = questionGroupId;
        }

    }

    public class TeacherCheck
    {
        public Int32 TeacherId { get; set; }

        public Dictionary<String, Double> Score { get; set; }

        public CheckTypes CheckType { get; set; }

        /// <summary>
        /// 表示已经更新了老师打的分数
        /// </summary>
        public Boolean IsOver { get; set; }


        public TeacherCheck()
        {
            IsOver = false;

            CheckType = CheckTypes.Ordinary;
        }

        //public override string ToString()
        //{
        //    return String.Format("{0} 对 {1} 设 {2}", TeacherId.ToString(), AnswerId, Score.ToString());
        //}
    }

    public class Answer
    {
        public String AnswerId { get; private set; }

        public String SmallPicUrl { get; private set; }

        public Question QuestionInfo { get; private set; }

        public Double FinalScore { get; set; }

        public Answer(Question questionInfo, String smallPicUrl)
        {
            QuestionInfo = questionInfo;

            SmallPicUrl = smallPicUrl;

            AnswerId = Guid.NewGuid().ToString();
        }
    }

    /// <summary>
    /// 答案卷
    /// </summary>
    public class AnswerSheet
    {
        public String MaxPicUrl { get; set; }

        /// <summary>
        /// 考号，唯一标示符
        /// </summary>
        public Int32 StudentSubjectId { get; set; }

        public Double Score
        {
            get { return AnswerChecks.Average(s => s.Answers.Average(a => a.FinalScore)); }
        }

        public List<AnswerCheck> AnswerChecks { get; set; }

    }

    /// <summary>
    /// 
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

        public PressCheck()
        {
            Id = Guid.NewGuid().ToString();

            IsPressed = false;
        }
    }
}
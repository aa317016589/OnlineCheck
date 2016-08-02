using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCheck
{
    public class AnswerCheck
    {
        public String AnswerCheckId { get; private set; }

        public List<Answer> Answers { get; private set; }

        public TeacherCheckManager TeacherCheckManagerx { get; private set; }

        public String QuestionGroupId { get; private set; }

        public String CombinationedPicUrl { get; private set; }

        public AnswerCheck(String questionGroupId, List<Answer> answers, JudgeModes judgeMode)
        {
            AnswerCheckId = Guid.NewGuid().ToString();

            Answers = answers;

            QuestionGroupId = questionGroupId;

            TeacherCheckManagerx = TeacherCheckManagerFactory.CreaTeacherCheckManager(judgeMode, answers);

            CombinationedPicUrl = "http://pic39.nipic.com/20140226/18071023_162553457000_2.jpg";
        }

        public void Set()
        {
            Dictionary<String, Double> dic = TeacherCheckManagerx.GetFinalScore();

            if (dic == null)
            {
                return;               
            }

            Answers.ForEach(s=>
            {
                s.FinalScore = dic[s.AnswerId];
            });
        }
    }
}

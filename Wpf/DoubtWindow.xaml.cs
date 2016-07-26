using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OnlineCheck;

namespace Wpf
{
    /// <summary>
    /// DoubtWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DoubtWindow : Window
    {
        public DoubtWindow(String questionGroupId, Int32 teacherId)
        {
            InitializeComponent();

            QuestionGroupId = questionGroupId;

            TeacherId = teacherId;
        }

        private readonly String QuestionGroupId;

        private readonly Int32 TeacherId;


        /// <summary>
        /// 打分框
        /// </summary>
        private Double Score
        {
            get { return Double.Parse(FirstScoreText.Text); }
        }

        /// <summary>
        /// 打分框2
        /// </summary>
        private Double Score2
        {
            get { return Double.Parse(SecondScoreText.Text); }
        }

        /// <summary>
        /// 当前的题目
        /// </summary>
        private String AnswerId
        {
            get { return FirstContentLabel.Content.ToString(); }
        }


        /// <summary>
        /// 当前的题目2
        /// </summary>
        private String AnswerId2
        {
            get { return SecondContentLabel.Content.ToString(); }
        }

        private String AnswerCheckId
        {
            get { return  AnswerCheckLabel.Content.ToString(); }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            //    TeacherCheck teacherCheck = new TeacherCheck()
            //    {
            //        TeacherId = TeacherId,
            //        CheckType = CheckTypes.Doubt,
            //        AnswerId = QuestionCheckId,
            //        Score = Score
            //    };


            //    QuestionGroup questionGroup =
            //OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(s => s.QuestionGroupId.ToString() == QuestionGroupId);

            //    Question question = questionGroup.Questions.SingleOrDefault(s => s.QuestionCheckId == QuestionCheckId);
            //    question.TeacherCheckManagerx.SetFinalScoreForDoubt(teacherCheck);
            var score = new Dictionary<String, Double>();

            score.Add(AnswerId, Score);
            score.Add(AnswerId2, Score2);

            TeacherCheck teacherCheck = new TeacherCheck()
            {
                TeacherId = TeacherId,
                CheckType = CheckTypes.Doubt,
                Score = score
            };

            AnswerCheck answerCheck = OnlineCheckManager.Instance.AnswerSheets.SelectMany(s => s.AnswerChecks)
   .SingleOrDefault(s => s.AnswerCheckId == AnswerCheckId);

            answerCheck.TeacherCheckManagerx.SetFinalScoreForDoubt(teacherCheck);
        }




        private void Get_Click(object sender, RoutedEventArgs e)
        {
            AnswerSheet answerSheet = OnlineCheckManager.Instance.AnswerSheets.FirstOrDefault(
                       s => s.AnswerChecks.Any(a => a.QuestionGroupId == QuestionGroupId && a.TeacherCheckManagerx.IsDoubt && !a.TeacherCheckManagerx.IsAllFinish));

            if (answerSheet == null)
            {
                MessageBox.Show("木有了"); return;
            }

            AnswerCheck answerCheck = answerSheet.AnswerChecks.SingleOrDefault(s => s.QuestionGroupId == QuestionGroupId);

            answerCheck.TeacherCheckManagerx.AddTeacherChecks(new TeacherCheck()
            {
                TeacherId = TeacherId,
                CheckType = CheckTypes.Doubt,
                Score = new Dictionary<string, double>()
            });

            AnswerCheckLabel.Content = answerCheck.AnswerCheckId;

            FirstContentLabel.Content = answerCheck.Answers[0].AnswerId;
            SecondContentLabel.Content = answerCheck.Answers[1].AnswerId;




            //Question question =
            //    OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(s => s.QuestionGroupId == QuestionGroupId)
            //        .SeleteDoubtQuestion();

            //if (question == null)
            //{
            //    MessageBox.Show("this is over");
            //    return;
            //}

            //question.TeacherCheckManagerx.AddTeacherChecks(new TeacherCheck()
            //{
            //    TeacherId = TeacherId,
            //    AnswerId = question.QuestionCheckId,
            //    CheckType = CheckTypes.Doubt
            //});



            //QuestionCheckIdLabel.Content = question.QuestionCheckId;

        }
    }
}

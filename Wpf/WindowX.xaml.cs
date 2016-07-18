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
    /// WindowX.xaml 的交互逻辑
    /// </summary>
    public partial class WindowX : Window
    {
        public WindowX()
        {
            InitializeComponent();
        }

        private String QuestionGroupId
        {
            get { return ""; }
        }

        /// <summary>
        /// 打分框
        /// </summary>
        private Double Score
        {
            get { return Double.Parse(ScoreText.Text); }
        }


        private Int32 TeacherId
        {
            get { return 0; }
        }

        /// <summary>
        /// 当前的题目
        /// </summary>
        private String QuestionCheckId
        {
            get { return QuestionCheckIdLabel.Content.ToString(); }
        }


        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            TeacherCheck teacherCheck = new TeacherCheck()
            {
                TeacherId = TeacherId,
                IsDoubt = false,
                IsOver = true,
                QuestionCheckId = QuestionCheckId,
                Score = Score
            };


            QuestionGroup questionGroup =
        OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(
            s => s.QuestionGroupId.ToString() == QuestionGroupId);

            questionGroup.Questions.SingleOrDefault(s => s.QuestionCheckId == QuestionCheckId)
                .TeacherCheckManagerx.UpdateTeacherChecks(teacherCheck);
        }

        private void Get_Click(object sender, RoutedEventArgs e)
        {

            Question question = OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(s => s.QuestionGroupId == QuestionGroupId)
                .Questions.FirstOrDefault(s => s.TeacherCheckManagerx.IsArbitration);

            if (question == null)
            {
                MessageBox.Show("this is over");
                return;
            }

            QuestionCheckIdLabel.Content = question.QuestionCheckId;

        }
    }
}

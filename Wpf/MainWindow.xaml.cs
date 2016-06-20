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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OnlineCheck;

namespace Wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }


        private Int32 TeacherId
        {
            get { return (Int32)TeacherList.SelectedValue; }
        }


        private String QuestionCheckId
        {
            get { return QuestionCheckIdLabel.Content.ToString(); }
        }


        private String QuestionGroupId
        {
            get { return QuestionGroupList.SelectedValue.ToString(); }
        }



        private Double Score
        {
            get { return Double.Parse(ScoreText.Text); }
        }

        /// <summary>
        /// 打分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Submit_Click(object sender, RoutedEventArgs e)
        {

            Int32 teacherId = TeacherId;

            String questionCheckId = QuestionCheckId;

            Double score = Score;

            TeacherCheck teacherCheck = new TeacherCheck()
            {
                TeacherId = teacherId,
                IsDoubt = false,
                IsOver = true,
                QuestionCheckId = questionCheckId,
                Score = score
            };

            Log.Items.Add(teacherCheck.ToString());


            OnlineCheckManager.Instance.Enqueue(teacherId, new PressCheck()
            {
                QuestionCheckId = questionCheckId,
                Score = score
            });


            QuestionGroup questionGroup =
                OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(
                    s => s.QuestionGroupId.ToString() == QuestionGroupList.SelectedValue.ToString());

            questionGroup.Questions.SingleOrDefault(s => s.QuestionCheckId == questionCheckId)
                .TeacherCheckManagerx.UpdateTeacherChecks(teacherCheck);

        }




        private void Init()
        {
            IList<Int32> teachers = new List<int>() { 1, 2 };

            IList<Int32> questionGroupIds = new List<int>() { 3, 4 };



            List<QuestionGroup> questionGroups = new List<QuestionGroup>();


            foreach (var questionGroupId in questionGroupIds)
            {
                QuestionGroupList.Items.Add(questionGroupId);


                QuestionGroup questionGroup = new QuestionGroup(questionGroupId, JudgeModes.SingleReview);


                questionGroup.Questions.Add(new Question(1, 1, questionGroup.TeacherCheckManager, 0, ""));

                questionGroup.Questions.Add(new Question(1, 1, questionGroup.TeacherCheckManager, 0, ""));


                questionGroups.Add(questionGroup);



            }


            foreach (var teacher in teachers)
            {
                TeacherList.Items.Add(teacher);
            }





            OnlineCheckManager.Instance.Init(questionGroups, teachers);



        }

        /// <summary>
        /// 获取题目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Get_Click(object sender, RoutedEventArgs e)
        {
            QuestionGroup questionGroup =
                OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(
                    s => s.QuestionGroupId.ToString() == QuestionGroupList.SelectedValue.ToString());


            Int32 teacherId = (Int32)TeacherList.SelectedValue;

            Question question = questionGroup.Questions.FirstOrDefault(s => !s.TeacherCheckManagerx.IsFinish(teacherId));

            if (question == null)
            {
                MessageBox.Show("this is over");
                return;
            }


            QuestionCheckIdLabel.Content = question.QuestionCheckId;

            questionGroup.TeacherCheckManager.AddTeacherChecks(new TeacherCheck()
            {
                TeacherId = teacherId,
                QuestionCheckId = question.QuestionCheckId,
                IsOver = false
            });

        }


        /// <summary>
        /// 教师列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TeacherList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PressCheckList.Items.Clear();


            Queue<PressCheck> queuesPressChecks = OnlineCheckManager.Instance.PressReview[TeacherId];

            foreach (var pressCheck in queuesPressChecks)
            {
                PressCheckList.Items.Add(pressCheck.Id);
            }
        }


        /// <summary>
        /// 题组列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuestionGroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Int32 teacherId = (Int32)TeacherList.SelectedValue;


            OnlineCheckManager.Instance.PressReview[teacherId].Clear();


            PressCheckList.Items.Clear();
        }

        /// <summary>
        /// 回评
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PressCheckBtn_Click(object sender, RoutedEventArgs e)
        {

            String id = PressCheckList.SelectedValue.ToString();


            PressCheck pressCheck = OnlineCheckManager.Instance.PressReview[TeacherId].SingleOrDefault(s => s.Id == id);



            QuestionCheckIdLabel.Content = pressCheck.QuestionCheckId;


        }



        private void SetQuestion_Click(object sender, RoutedEventArgs e)
        {

            TeacherCheck teacherCheck = new TeacherCheck()
            {
                TeacherId = TeacherId,
                IsDoubt = true,
                IsOver = true,
                QuestionCheckId = QuestionCheckId,
                Score = 0
            };


            QuestionGroup questionGroup =
                OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(
                    s => s.QuestionGroupId.ToString() == QuestionGroupId);

            questionGroup.Questions.SingleOrDefault(s => s.QuestionCheckId == QuestionCheckId)
                .TeacherCheckManagerx.UpdateTeacherChecks(teacherCheck);

        }

        private void StatisticsBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
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

        /// <summary>
        /// 当前的教师
        /// </summary>
        private Int32 TeacherId
        {
            get { return (Int32)TeacherList.SelectedValue; }
        }

        /// <summary>
        /// 当前的题目
        /// </summary>
        private String QuestionCheckId
        {
            get { return QuestionCheckIdLabel.Content.ToString(); }
        }

        /// <summary>
        /// 选择的题组
        /// </summary>
        private String QuestionGroupId
        {
            get { return QuestionGroupList.SelectedValue.ToString(); }
        }


        /// <summary>
        /// 打分框
        /// </summary>
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
            Boolean over = IsOver.IsChecked == true;

            TeacherCheck teacherCheck = new TeacherCheck()
            {
                TeacherId = TeacherId,
                QuestionCheckId = QuestionCheckId,
                Score = Score
            };

            Log.Items.Add(teacherCheck.ToString());


            QuestionGroup questionGroup =
                OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(
                    s => s.QuestionGroupId.ToString() == QuestionGroupId);

            questionGroup.Questions.SingleOrDefault(s => s.QuestionCheckId == QuestionCheckId)
                .TeacherCheckManagerx.UpdateTeacherChecks(teacherCheck);

            if (!over)
            {
                OnlineCheckManager.Instance.Enqueue(teacherCheck.TeacherId, new PressCheck()
                {
                    QuestionCheckId = teacherCheck.QuestionCheckId,
                    Score = teacherCheck.Score
                });
            }
            else
            {
                OnlineCheckManager.Instance.Press(teacherCheck.TeacherId, teacherCheck.QuestionCheckId, teacherCheck.Score);
            }



            IsOver.IsChecked = false;

        }




        private void Init()
        {
            IList<Int32> teachers = new List<int>() { 1, 2, 3, 4 };

            IList<String> questionGroupIds = new List<String>() { "5", "6" };



            List<QuestionGroup> questionGroups = new List<QuestionGroup>();


            foreach (var questionGroupId in questionGroupIds)
            {
                QuestionGroupList.Items.Add(questionGroupId);


                QuestionGroup questionGroup = new QuestionGroup(questionGroupId, JudgeModes.ThirdReview);


                questionGroup.Questions.Add(new Question(1, 1, 5, "", JudgeModes.FourReview));

                questionGroup.Questions.Add(new Question(1, 2, 4, "", JudgeModes.FourReview));


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
                    s => s.QuestionGroupId.ToString() == QuestionGroupId);


            Question question = questionGroup.Questions.FirstOrDefault(s => s.TeacherCheckManagerx.IsX(TeacherId));

            if (question == null)
            {
                MessageBox.Show("this is over");
                return;
            }


            QuestionCheckIdLabel.Content = question.QuestionCheckId;

            question.TeacherCheckManagerx.AddTeacherChecks(new TeacherCheck()
            {
                TeacherId = TeacherId,
                QuestionCheckId = question.QuestionCheckId
            });

            ScoreText.Text = "";
        }


        /// <summary>
        /// 教师列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TeacherList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PressCheckList.Items.Clear();


            IEnumerable<PressCheck> queuesPressChecks =
                OnlineCheckManager.Instance.PressReview[TeacherId].Where(s => !s.IsPressed);

            foreach (var pressCheck in queuesPressChecks)
            {
                PressCheckList.Items.Add(pressCheck.QuestionCheckId);
            }
        }


        /// <summary>
        /// 题组列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuestionGroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnlineCheckManager.Instance.PressReview[TeacherId].Clear();


            PressCheckList.Items.Clear();



        }

        /// <summary>
        /// 回评
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PressCheckBtn_Click(object sender, RoutedEventArgs e)
        {

            if (PressCheckList.SelectedValue == null)
            {
                MessageBox.Show("选择回评"); return;
            }


            PressCheck pressCheck = OnlineCheckManager.Instance.PressReview[TeacherId].SingleOrDefault(s => s.QuestionCheckId == PressCheckList.SelectedValue.ToString());

            IsOver.IsChecked = true;

            QuestionCheckIdLabel.Content = pressCheck.QuestionCheckId;

            ScoreText.Text = pressCheck.Score.ToString();
        }


        /// <summary>
        /// 设置问题卷
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetQuestion_Click(object sender, RoutedEventArgs e)
        {

            TeacherCheck teacherCheck = new TeacherCheck()
            {
                TeacherId = TeacherId,
                CheckType = CheckTypes.Doubt,
                QuestionCheckId = QuestionCheckId,
                Score = 0
            };


            QuestionGroup questionGroup =
                OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(
                    s => s.QuestionGroupId.ToString() == QuestionGroupId);

            questionGroup.Questions.SingleOrDefault(s => s.QuestionCheckId == QuestionCheckId)
                .TeacherCheckManagerx.UpdateTeacherChecks(teacherCheck);

        }



        /// <summary>
        /// 展示最后的分数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatisticsBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var questionGroup in OnlineCheckManager.Instance.QuestionGroups)
            {
                var v2 = questionGroup.Questions.ToDictionary(k => k.QuestionCheckId, v => v.TeacherCheckManagerx.FinalScore);

                foreach (var d in v2)
                {
                    S1.Items.Add(String.Format("{0} {1}", d.Key, d.Value));
                }
            }
        }


        /// <summary>
        /// 确定按钮，提交回评，清空该老师的回评队列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            OnlineCheckManager.Instance.Clear(TeacherId, QuestionGroupId);
            PressCheckList.Items.Clear();
        }



        private void Open_Click(object sender, RoutedEventArgs e)
        {
            WindowX windowX = new WindowX(QuestionGroupId, TeacherId);
            windowX.Show();
        }

        private void OpenDoubt_Click(object sender, RoutedEventArgs e)
        {
            DoubtWindow doubtWindow = new DoubtWindow(QuestionGroupId, TeacherId);
            doubtWindow.Show();
        }
    }
}
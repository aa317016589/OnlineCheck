using System;
using System.Collections;
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


        /// <summary>
        /// 选择的题组
        /// </summary>
        private String QuestionGroupId
        {
            get { return QuestionGroupList.SelectedValue.ToString(); }
        }


        private String AnswerCheckId
        {
            get { return AnswerCheckLabel.Content.ToString(); }
        }


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
        /// 打分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Boolean over = IsOver.IsChecked == true;

            var score = new Dictionary<String, Double>();

            score.Add(AnswerId, Score);
            score.Add(AnswerId2, Score2);


            TeacherCheck teacherCheck = new TeacherCheck()
            {
                TeacherId = TeacherId,
                Score = score
            };







            AnswerCheck answerCheck = OnlineCheckManager.Instance.AnswerSheets.SelectMany(s => s.AnswerChecks)
               .SingleOrDefault(s => s.AnswerCheckId == AnswerCheckId);

            answerCheck.TeacherCheckManagerx.UpdateTeacherChecks(teacherCheck);


            Log.Items.Add(String.Format("{0} , {1}", TeacherId, answerCheck.AnswerCheckId));

            if (!over)
            {
                OnlineCheckManager.Instance.Enqueue(teacherCheck.TeacherId, new PressCheck()
                {
                    Score = teacherCheck.Score,
                    AnswerCheckId = answerCheck.AnswerCheckId
                });
            }
            else
            {
                OnlineCheckManager.Instance.Press(teacherCheck.TeacherId, answerCheck.AnswerCheckId, teacherCheck.Score);
            }



            IsOver.IsChecked = false;

        }


        #region Init
        private void Init()
        {
            IList<Int32> teachers = new List<int>() { 1, 2, 3, 4 };





            QuestionGroup questionGroup = new QuestionGroup("5", JudgeModes.FourReview);

            Question firstQuestion = new Question(questionGroup.QuestionGroupId, 1001, 4, 25);

            questionGroup.Questions.Add(firstQuestion);

            Question secondQuestion = new Question(questionGroup.QuestionGroupId, 1002, 5, 15);

            questionGroup.Questions.Add(secondQuestion);

            foreach (var teacher in teachers)
            {
                TeacherList.Items.Add(teacher);
            }


            QuestionGroupList.Items.Add(questionGroup.QuestionGroupId);


            OnlineCheckManager.Instance.Init(questionGroup, teachers);

            OnlineCheckManager.Instance.AnswerSheets = CreateAnswerSheets(questionGroup, firstQuestion, secondQuestion);

        }

        private List<AnswerSheet> CreateAnswerSheets(QuestionGroup questionGroup, Question firstQuestion, Question secondQuestion)
        {
            List<AnswerSheet> answerSheets = new List<AnswerSheet>()
            {
                new AnswerSheet()
                {
                    MaxPicUrl = Guid.NewGuid().ToString(),
                    StudentSubjectId = new Random().Next(0, 999999),
                    AnswerChecks = new List<AnswerCheck>()
                    {
                        new AnswerCheck(firstQuestion.QuestionGroupId, new List<Answer>()
                        {
                            new Answer(firstQuestion, Guid.NewGuid().ToString()),
                            new Answer(secondQuestion, Guid.NewGuid().ToString())
                        }, questionGroup.JudgeMode)
                    }
                },
                new AnswerSheet()
                {
                    MaxPicUrl = Guid.NewGuid().ToString(),
                    StudentSubjectId = new Random().Next(0, 999999),
                    AnswerChecks = new List<AnswerCheck>()
                    {
                        new AnswerCheck(firstQuestion.QuestionGroupId, new List<Answer>()
                        {
                            new Answer(firstQuestion, Guid.NewGuid().ToString()),
                            new Answer(secondQuestion, Guid.NewGuid().ToString())
                        }, questionGroup.JudgeMode)
                    }
                },
                new AnswerSheet()
                {
                    MaxPicUrl = Guid.NewGuid().ToString(),
                    StudentSubjectId = new Random().Next(0, 999999),
                    AnswerChecks = new List<AnswerCheck>()
                    {
                        new AnswerCheck(firstQuestion.QuestionGroupId, new List<Answer>()
                        {
                            new Answer(firstQuestion, Guid.NewGuid().ToString()),
                            new Answer(secondQuestion, Guid.NewGuid().ToString())
                        }, questionGroup.JudgeMode)
                    }
                }
            };

            return answerSheets;
        }

        #endregion

        /// <summary>
        /// 获取题目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Get_Click(object sender, RoutedEventArgs e)
        {
            AnswerSheet answerSheet = OnlineCheckManager.Instance.AnswerSheets.FirstOrDefault(
                     s => s.AnswerChecks.Any(a => a.QuestionGroupId == QuestionGroupId && a.TeacherCheckManagerx.IsGet(TeacherId)));

            if (answerSheet == null)
            {
                MessageBox.Show("木有了"); return;
            }

            AnswerCheck answerCheck = answerSheet.AnswerChecks.SingleOrDefault(s => s.QuestionGroupId == QuestionGroupId);

            answerCheck.TeacherCheckManagerx.AddTeacherChecks(new TeacherCheck()
             {
                 TeacherId = TeacherId,
             });

            AnswerCheckLabel.Content = answerCheck.AnswerCheckId;

            FirstContentLabel.Content = answerCheck.Answers[0].AnswerId;

            SecondContentLabel.Content = answerCheck.Answers[1].AnswerId;

            FirstScoreText.Text = "";
            SecondScoreText.Text = "";
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
                PressCheckList.Items.Add(pressCheck.AnswerCheckId);
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


            PressCheck pressCheck = OnlineCheckManager.Instance.PressReview[TeacherId].SingleOrDefault(s => s.AnswerCheckId == PressCheckList.SelectedValue.ToString());

            IsOver.IsChecked = true;

            FirstContentLabel.Content = pressCheck.Score.FirstOrDefault().Key;

            FirstScoreText.Text = pressCheck.Score.FirstOrDefault().Value.ToString();

            AnswerCheckLabel.Content = pressCheck.AnswerCheckId;

            SecondContentLabel.Content = pressCheck.Score.LastOrDefault().Key;

            SecondScoreText.Text = pressCheck.Score.LastOrDefault().Value.ToString();
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
                Score = new Dictionary<string, double>()
            };



            AnswerCheck answerCheck = OnlineCheckManager.Instance.AnswerSheets.SelectMany(s => s.AnswerChecks)
                .SingleOrDefault(s => s.AnswerCheckId == AnswerCheckId);


            answerCheck.TeacherCheckManagerx.UpdateTeacherChecks(teacherCheck);

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
                // var v2 = questionGroup.Questions.SelectMany(s=>s.).ToDictionary(k => k.QuestionCheckId, v => v.TeacherCheckManagerx.FinalScore);

                IEnumerable<AnswerCheck> answerChecks = OnlineCheckManager.Instance.AnswerSheets.SelectMany(s => s.AnswerChecks)
                      .Where(s => s.QuestionGroupId == questionGroup.QuestionGroupId);

                foreach (var answerCheck in answerChecks)
                {
                    S1.Items.Add(answerCheck.AnswerCheckId);
                }


                //OnlineCheckManager.Instance.AnswerSheets.SelectMany(s=>s.Answers)
                // foreach (var d in v2)
                // {
                //     S1.Items.Add(String.Format("{0} {1}", d.Key, d.Value));
                // }
            }
        }


        /// <summary>
        /// 确定按钮，提交回评，清空该老师的回评队列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            OnlineCheckManager.Instance.Clear(TeacherId);
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

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            AnswerCheck answerCheck = OnlineCheckManager.Instance.AnswerSheets.SelectMany(s => s.AnswerChecks)
                   .SingleOrDefault(s => s.AnswerCheckId == AnswerChckIdText.Text);
        }

        private void Log_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;

            AnswerChckIdText.Text = listBox.SelectedItem.ToString();
        }
    }
}
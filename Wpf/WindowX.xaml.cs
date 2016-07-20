﻿using System;
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

        public WindowX(String questionGroupId, Int32 teacherId)
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
            get { return Double.Parse(ScoreText.Text); }
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
                CheckType = CheckTypes.Arbitration,
                IsOver = true,
                QuestionCheckId = QuestionCheckId,
                Score = Score
            };


            QuestionGroup questionGroup =
        OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(s => s.QuestionGroupId.ToString() == QuestionGroupId);

            Question question = questionGroup.Questions.SingleOrDefault(s => s.QuestionCheckId == QuestionCheckId);
            question.TeacherCheckManagerx.UpdateTeacherChecks(teacherCheck, CheckTypes.Arbitration);
            question.TeacherCheckManagerx.PressReturn();
        }

        private void Get_Click(object sender, RoutedEventArgs e)
        {

            Question question =
                OnlineCheckManager.Instance.QuestionGroups.SingleOrDefault(s => s.QuestionGroupId == QuestionGroupId)
                    .SeleteArbitrationQuestion();

            if (question == null)
            {
                MessageBox.Show("this is over");
                return;
            }

            question.TeacherCheckManagerx.AddTeacherChecks(new TeacherCheck()
            {
                TeacherId = TeacherId,
                QuestionCheckId = question.QuestionCheckId,
                IsOver = false,
                CheckType = CheckTypes.Arbitration
            });



            QuestionCheckIdLabel.Content = question.QuestionCheckId;

        }
    }
}

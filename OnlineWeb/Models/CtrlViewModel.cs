using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineCheck.Web.Models
{
    public class CtrlViewModel
    {
        [DisplayName("考生数")]
        [Required]
        public Int32 StudentCounts { get; set; }

        [DisplayName("题组数")]
        [Required]
        public Int32 QuestionGroupCounts { get; set; }


        [DisplayName("老师数")]
        [Range(1,4)]
        [Required]
        public Int32 TeacherCounts { get; set; }

        [DisplayName("每个题组下题目数")]
        [Required]
        public Int32 QuestionCounts { get; set; }

        [DisplayName("阀值")]
        [Required]
        public Int32 Threshold { get; set; }


        [DisplayName("评阅模式")]
        [Required]
        public JudgeModes JudgeMode { get; set; }
    }
}
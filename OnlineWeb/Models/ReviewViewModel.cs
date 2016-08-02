using System;
using System.Collections.Generic;

namespace OnlineCheck.Web.Models
{
    public class ReviewViewModel
    {
        public String Id { get; set; }

        public String AnswerCheckId { get; set; }

        public String QuestionGroupId { get; set; }

        public Dictionary<String, Double> Score { get; set; } 
    }
}
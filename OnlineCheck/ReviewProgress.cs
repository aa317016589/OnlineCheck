using System;

namespace OnlineCheck
{
    public class ReviewProgress
    {
        public string TestletsStructId { get; set; }
        /// <summary>题组编号
        /// </summary>
        public string TestletsNumber { get; set; }

        /// <summary>总题组数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>完成数量
        /// </summary>
        public int CompleteCount { get; set; }

        /// <summary>出成绩百分比
        /// </summary>
        public decimal AchievementsPercent
        {
            get { return TotalCount == 0 ? 0 : Convert.ToDecimal((CompleteCount * 100 / TotalCount).ToString("f2")); }
        }

        /// <summary>未完成的数量
        /// </summary>
        public int UnCompleteCount
        {
            get { return TotalCount - CompleteCount; }
        }

        /// <summary>未完成百分比
        /// </summary>
        public decimal UnCompletePercent
        {
            get { return TotalCount == 0 ? 0 : Convert.ToDecimal((UnCompleteCount * 100 / TotalCount).ToString("f2")); }
        }


        public int FirstProduceCount { get; set; }
        /// <summary>一评阅完成数量
        /// </summary>
        public int FirstCompleteCount { get; set; }

        /// <summary>一评完成百分比
        /// </summary>
        public decimal FirstCompletePercent
        {
            get { return TotalCount == 0 ? 0 : Convert.ToDecimal((FirstCompleteCount * 100 / FirstProduceCount).ToString("f2")); }
        }

         
        /// <summary>一评未完成量
        /// </summary>
        public int FirstUnCompleteCount
        {
            get { return FirstProduceCount - FirstCompleteCount; }
        }

        /// <summary>一评未完成率
        /// </summary>
        public decimal FirstUnCompletePercent
        {
            get { return TotalCount == 0 ? 0 : Convert.ToDecimal((FirstUnCompleteCount * 100 / FirstProduceCount).ToString("f2")); }
        }




        /// <summary>二评产生量
        /// </summary>
        public int SecondProduceCount { get; set; }
        /// <summary>二评完成量
        /// </summary>
        public int SecondCompleteCount { get; set; }

        /// <summary>二评完成率
        /// </summary>
        public decimal SecondCompletePercent
        {
            get
            {
                return SecondProduceCount == 0
                    ? 0
                    : Convert.ToDecimal((SecondCompleteCount * 100 / SecondProduceCount).ToString("f2"));
            }
        }

        /// <summary>二评未完成量
        /// </summary>
        public int SecondUnCompleteCount
        {
            get { return SecondProduceCount - SecondCompleteCount; }
        }

        /// <summary>二评未完成率
        /// </summary>
        public decimal SecondUnCompletePercent
        {
            get
            {
                return SecondProduceCount == 0
                    ? 0
                    : Convert.ToDecimal((SecondUnCompleteCount * 100 / SecondProduceCount).ToString("f2"));
            }
        }









        /// <summary>三评产生量
        /// </summary>
        public int ThirdProduceCount { get; set; }

        /// <summary>三评产生率
        /// </summary>
        public decimal ThirdProducePercent
        {
            get { return TotalCount == 0 ? 0 : Convert.ToDecimal((ThirdProduceCount * 100 / TotalCount).ToString("f2")); }
        }

        /// <summary>三评完成量
        /// </summary>
        public int ThirdCompleteCount { get; set; }

        /// <summary>三评完成率
        /// </summary>
        public decimal ThirdCompletePercent
        {
            get
            {
                return ThirdProduceCount == 0
                    ? 0
                    : Convert.ToDecimal((ThirdCompleteCount * 100 / ThirdProduceCount).ToString("f2"));
            }
        }

        /// <summary>三评未完成量
        /// </summary>
        public int ThirdUnCompleteCount
        {
            get { return ThirdProduceCount - ThirdCompleteCount; }
        }

        /// <summary>三评未完成率
        /// </summary>
        public decimal ThirdUnCompletePercent
        {
            get
            {
                return ThirdProduceCount == 0
                    ? 0
                    : Convert.ToDecimal((ThirdUnCompleteCount * 100 / ThirdProduceCount).ToString("f2"));
            }
        }



        /// <summary>仲裁产生量
        /// </summary>
        public int ArbitrationProduceCount { get; set; }

        /// <summary>仲裁产生率
        /// </summary>
        public decimal ArbitrationProducePercent
        {
            get
            {
                return TotalCount == 0 ? 0 : Convert.ToDecimal((ArbitrationProduceCount * 100 / TotalCount).ToString("f2"));
            }
        }
        /// <summary>仲裁完成量
        /// </summary>
        public int ArbitrationCompleteCount { get; set; }

        /// <summary>仲裁完成率
        /// </summary>
        public decimal ArbitrationCompletePercent
        {
            get
            {
                return ArbitrationProduceCount == 0
                    ? 0
                    : Convert.ToDecimal((ArbitrationCompleteCount * 100 / ArbitrationProduceCount).ToString("f2"));
            }
        }

        /// <summary>仲裁未完成量
        /// </summary>
        public int ArbitrationUnCompleteCount
        {
            get { return ArbitrationProduceCount - ArbitrationCompleteCount; }
        }

        /// <summary>仲裁未完成率
        /// </summary>
        public decimal ArbitrationUnCompletePercent
        {
            get
            {
                return ArbitrationProduceCount == 0
                    ? 0
                    : Convert.ToDecimal((ArbitrationUnCompleteCount * 100 / ArbitrationProduceCount).ToString("f2"));
            }
        }

        /// <summary>问题卷产生量
        /// </summary>
        public int ProblematicsProduceCount { get; set; }

        /// <summary>问题卷完成量
        /// </summary>
        public int ProblematicsCompleteCount { get; set; }


        /// <summary>问题卷产生率
        /// </summary>
        public decimal ProblematicsProducePercent
        {
            get
            {
                return TotalCount == 0 ? 0 : Convert.ToDecimal((ProblematicsProduceCount * 100 / TotalCount).ToString("f2"));
            }
        }

        /// <summary>问题卷完成率
        /// </summary>
        public decimal ProblematicsCompletePercent
        {
            get
            {
                return ProblematicsProduceCount == 0
                    ? 0
                    : Convert.ToDecimal((ProblematicsCompleteCount * 100 / ProblematicsProduceCount).ToString("f2"));
            }
        }

        /// <summary>问题卷未完成量
        /// </summary>
        public int ProblematicsUnCompleteCount
        {
            get { return ProblematicsProduceCount - ProblematicsCompleteCount; }
        }
        /// <summary>问题卷未完成率
        /// </summary>
        public decimal ProblematicsUnCompletePercent
        {
            get
            {
                return ProblematicsProduceCount == 0
                    ? 0
                    : Convert.ToDecimal((ProblematicsUnCompleteCount * 100 / ProblematicsProduceCount).ToString("f2"));
            }
        }

        public ReviewProgress(string testletsStructId, string testletsNumber, int totalCount, int completeCount,int firstProduceCount,
            int firstCompleteCount, int secondProduceCount, int secondCompleteCount, int thirdProduceCount,
            int thirdCompleteCount, int arbitrationProduceCount, int arbitrationCompleteCount,
            int problematicsProduceCount, int problematicsCompleteCount)
        {
            TestletsStructId = testletsStructId;
            TestletsNumber = testletsNumber;
            TotalCount = totalCount;
            CompleteCount = completeCount;

            FirstProduceCount = firstProduceCount;
            FirstCompleteCount = firstCompleteCount;

            SecondProduceCount = secondProduceCount;
            SecondCompleteCount = secondCompleteCount;

            ThirdProduceCount = thirdProduceCount;
            ThirdCompleteCount = thirdCompleteCount;

            ArbitrationProduceCount = arbitrationProduceCount;
            ArbitrationCompleteCount = arbitrationCompleteCount;

            ProblematicsProduceCount = problematicsProduceCount;
            ProblematicsCompleteCount = problematicsCompleteCount;
        }

    }

}
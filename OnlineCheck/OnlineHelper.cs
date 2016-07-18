using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineCheck
{
    public static class OnlineHelper
    {

        /// <summary>
        /// 最大的阀值
        /// </summary>
        /// <param name="scores"></param>
        /// <returns></returns>
        public static Double GetMinThreshold(params Double[] scores)
        {
            Int32 length = scores.Length;

            if (length == 0)
            {
                return 0;
            }

            if (length == 1)
            {
                return scores[0];
            }

            scores = scores.OrderByDescending(s => s).ToArray();

            Double difference = scores[0] - scores[1], nowDiff = 0;

            if (length == 2)
            {
                return difference;
            }



            for (int i = 1; i < length - 1; i++)
            {
                nowDiff = Math.Abs(scores[i] - scores[i + 1]);

                if (difference > nowDiff)
                {
                    difference = nowDiff;
                }
            }


            return difference;
        }


        /// <summary>
        ///  取N个分数中较接近的两个作为最终分数
        /// </summary>
        /// <param name="scores"></param>
        /// <returns></returns>
        public static double GetMiddleScore(params Double[] scores)
        {
            Int32 length = scores.Length;

            if (length == 0)
            {
                return 0;
            }

            if (length == 1)
            {
                return scores[0];
            }

            if (length == 2)
            {
                return (scores[0] + scores[1])/2;
            }

            scores = scores.OrderByDescending(s => s).ToArray();

            Double difference = scores[0] - scores[1], nowDiff = 0;

            int j = 0;

            Boolean Ist = true; //等差数列

            for (int i = 1; i < length - 1; i++)
            {
                nowDiff = Math.Abs(scores[i] - scores[i + 1]);

                Ist = Ist && difference == nowDiff;

                if (difference > nowDiff)
                {
                    difference = nowDiff;

                    j = i;
                }
            }


            if (Ist)
            {
                return scores.Average();
            }

            return (scores[j + 1] + scores[j])/2;
        }
    }
}
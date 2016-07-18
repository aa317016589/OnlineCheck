using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace OnlineCheck
{
    /// <summary>
    /// 阅卷模式
    /// </summary>
    public enum JudgeModes
    {
        /// <summary>
        /// 单评
        /// </summary>
        [Description("单评")]
        SingleReview = 1,

        /// <summary>
        /// 双评
        /// </summary>
        [Description("双评")]
        MultiReview,

        /// <summary>
        /// 2+1 模式
        /// </summary>
        [Description("2+1 模式")]
        ThirdReview,

        /// <summary>
        /// 2+1+1 模式
        /// </summary>
        [Description("2+1+1 模式")]
        FourReview
    }

    public enum CheckTypes
    {
        [Description("普通")]
        Ordinary = 1,
        [Description("问题")]
        Doubt,
        [Description("仲裁")]
        Arbitration,
    }
}
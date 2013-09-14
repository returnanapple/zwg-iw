using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Client.Class
{
    /// <summary>
    /// 最新投注信息
    /// </summary>
    public class BettingRes
    {
        /// <summary>
        /// 单号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 玩法
        /// </summary>
        public string HowToPlay { get; set; }

        /// <summary>
        /// 投注内容
        /// </summary>
        public string Values { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public double Multiple { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public double Money { get; set; }
    }
}

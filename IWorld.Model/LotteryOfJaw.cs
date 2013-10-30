using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 大白鲨游戏的开奖记录
    /// </summary>
    public class LotteryOfJaw : ModelBase
    {
        #region 属性

        /// <summary>
        /// 期号
        /// </summary>
        public string Issue { get; set; }

        /// <summary>
        /// 开奖号码
        /// </summary>
        public int Value { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的大白鲨游戏的开奖记录
        /// </summary>
        public LotteryOfJaw()
        {
        }

        /// <summary>
        /// 实例化一个新的大白鲨游戏的开奖记录
        /// </summary>
        /// <param name="issue">期号</param>
        /// <param name="value">开奖号码</param>
        public LotteryOfJaw(string issue, int value)
        {
            this.Issue = issue;
            this.Value = value;
        }

        #endregion
    }
}

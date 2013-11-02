using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 大白鲨游戏的投注明细
    /// </summary>
    public class BettingDetailOfJaw : ModelBase
    {
        #region 属性

        /// <summary>
        /// 目标标识
        /// </summary>
        public virtual MarkOfJaw Mark { get; set; }

        /// <summary>
        /// 投注金额
        /// </summary>
        public double Sum { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的大白鲨游戏的投注明细
        /// </summary>
        public BettingDetailOfJaw()
        {
        }

        /// <summary>
        /// 实例化一个新的大白鲨游戏的投注明细
        /// </summary>
        /// <param name="mark">目标标识</param>
        /// <param name="sum">投注金额</param>
        public BettingDetailOfJaw(MarkOfJaw mark, double sum)
        {
            this.Mark = mark;
            this.Sum = sum;
        }

        #endregion
    }
}

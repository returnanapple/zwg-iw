using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 虚拟排行
    /// </summary>
    public class VirtualTop : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 对应的彩票
        /// </summary>
        public virtual LotteryTicket Ticket { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public double Sum { get; set; }
        
        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的虚拟排行
        /// </summary>
        public VirtualTop()
        {
        }

        /// <summary>
        /// 实例化一个新的虚拟排行
        /// </summary>
        /// <param name="ticket">对应的彩票</param>
        /// <param name="sum">金额</param>
        public VirtualTop(LotteryTicket ticket, double sum)
        {
            this.Ticket = ticket;
            this.Sum = sum;
        }
        
        #endregion
    }
}

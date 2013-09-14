using System;
using System.Collections.Generic;
using System.Linq;

namespace IWorld.Model
{
    /// <summary>
    /// 彩票
    /// </summary>
    public class LotteryTicket : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 下一期
        /// </summary>
        public string NextPhases { get; set; }

        /// <summary>
        /// 下一期开奖时间
        /// </summary>
        public DateTime NextLotteryTime
        {
            get
            {
                if (this.Times.Any(x => x.Time > DateTime.Now))
                {
                    return this.Times.Where(x => x.Time > DateTime.Now).Min(x => x.Time);
                }
                else
                {
                    return this.Times.Min(x => x.Time).AddDays(1);
                }
            }
        }

        /// <summary>
        /// 下辖的玩法标签
        /// </summary>
        public virtual List<PlayTag> Tags { get; set; }

        /// <summary>
        /// 开奖时间
        /// </summary>
        public virtual List<LotteryTime> Times { get; set; }

        /// <summary>
        /// 位
        /// </summary>
        public virtual List<LotteryTicketSeat> Seats { get; set; }

        /// <summary>
        /// 标识 | 不在前台显示
        /// </summary>
        public bool Hide { get; set; }

        /// <summary>
        /// 排序系数
        /// </summary>
        public int Order { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的彩票
        /// </summary>
        public LotteryTicket()
        {
        }

        /// <summary>
        /// 实例化一个新的彩票
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="times">开奖时间</param>
        /// <param name="seats">位</param>
        /// <param name="order">排序系数</param>
        public LotteryTicket(string name, List<LotteryTime> times, List<LotteryTicketSeat> seats, int order)
        {
            this.Name = name;
            this.NextPhases = "";
            this.Tags = new List<PlayTag>();
            this.Times = times;
            this.Seats = seats;
            this.Hide = false;
            this.Order = order;
        }

        #endregion
    }
}

using System.Collections.Generic;

namespace IWorld.Model
{
    /// <summary>
    /// 开奖记录
    /// </summary>
    public class Lottery : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 期数
        /// </summary>
        public string Phases { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public LotterySources Sources { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public virtual Administrator Operator { get; set; }

        /// <summary>
        /// 彩种
        /// </summary>
        public virtual LotteryTicket Ticket { get; set; }

        /// <summary>
        /// 位
        /// </summary>
        public virtual List<LotterySeat> Seats { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的开奖记录
        /// </summary>
        public Lottery()
        {
        }

        /// <summary>
        /// 实例化一个新的开奖记录
        /// </summary>
        /// <param name="phases">期数</param>
        /// <param name="sources">来源</param>
        /// <param name="_operator">操作人</param>
        /// <param name="ticket">彩种</param>
        /// <param name="seats">位</param>
        public Lottery(string phases, LotterySources sources, Administrator _operator, LotteryTicket ticket, List<LotterySeat> seats)
        {
            this.Phases = phases;
            this.Sources = sources;
            this.Operator = _operator;
            this.Ticket = ticket;
            this.Seats = seats;
        }

        #endregion
    }
}

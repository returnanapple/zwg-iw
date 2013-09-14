using System;
using System.Linq;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 开奖信息
    /// </summary>
    [DataContract]
    public class LotteryResult
    {
        /// <summary>
        /// 开奖信息的存储指针
        /// </summary>
        [DataMember]
        public int LotteryId { get; set; }

        /// <summary>
        /// 期数
        /// </summary>
        [DataMember]
        public string Phases { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [DataMember]
        public LotterySources Sources { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [DataMember]
        public string Operator { get; set; }

        /// <summary>
        /// 所属彩种的存储指针
        /// </summary>
        [DataMember]
        public int TicketId { get; set; }

        /// <summary>
        /// 所属彩种的名称
        /// </summary>
        [DataMember]
        public string TicketName { get; set; }

        /// <summary>
        /// 开奖号码
        /// </summary>
        [DataMember]
        public string Seats { get; set; }

        /// <summary>
        /// 开奖时间
        /// </summary>
        [DataMember]
        public DateTime Time { get; set; }

        /// <summary>
        /// 实例化一个新的开奖信息
        /// </summary>
        /// <param name="lottery">开奖信息的数据封装</param>
        public LotteryResult(Lottery lottery)
        {
            this.LotteryId = lottery.Id;
            this.Phases = lottery.Phases;
            this.Sources = lottery.Sources;
            this.Operator = lottery.Operator == null ? "系统采集" : lottery.Operator.Username;
            this.TicketId = lottery.Ticket.Id;
            this.TicketName = lottery.Ticket.Name;
            this.Seats = string.Join(",", lottery.Seats.OrderBy(x => x.Order).ToList().ConvertAll(x => x.Value));
            this.Time = lottery.CreatedTime;
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 彩票信息
    /// </summary>
    [DataContract]
    public class TicketResult
    {
        /// <summary>
        /// 存储指针
        /// </summary>
        [DataMember]
        public int TicketId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 本期
        /// </summary>
        [DataMember]
        public string Phases { get; set; }

        /// <summary>
        /// 本期开奖
        /// </summary>
        [DataMember]
        public string Lottery { get; set; }

        /// <summary>
        /// 下一期
        /// </summary>
        [DataMember]
        public string NextPhases { get; set; }

        /// <summary>
        /// 下一期开奖时间
        /// </summary>
        [DataMember]
        public DateTime NextLotteryTime { get; set; }

        /// <summary>
        /// 标识 | 不在前台显示
        /// </summary>
        [DataMember]
        public bool Hide { get; set; }

        /// <summary>
        /// 排序系数
        /// </summary>
        [DataMember]
        public int Order { get; set; }

        /// <summary>
        /// 下辖玩法标签数量
        /// </summary>
        [DataMember]
        public int CountOfPlayTag { get; set; }

        /// <summary>
        /// 下辖玩法数量
        /// </summary>
        [DataMember]
        public int CountOfHowToPlay { get; set; }

        /// <summary>
        /// 开奖时间
        /// </summary>
        [DataMember]
        public List<LotteryTimeResult> LotteryTimes { get; set; }

        /// <summary>
        /// 实例化一个新的彩票信息
        /// </summary>
        /// <param name="ticket">彩票信息的数据封装</param>
        /// <param name="lottery">当期开奖记录的数据封装</param>
        /// <param name="countOfPlayTag">下辖玩法标签数量</param>
        /// <param name="countOfHowToPlay">下辖玩法数量</param>
        public TicketResult(LotteryTicket ticket, Lottery lottery, int countOfPlayTag, int countOfHowToPlay)
        {
            this.TicketId = ticket.Id;
            this.Name = ticket.Name;
            this.Phases = lottery == null ? "" : lottery.Phases;
            this.Lottery = lottery == null 
                ? "" : string.Join(",", lottery.Seats.OrderBy(x => x.Order).ToList().ConvertAll(x => x.Value));
            this.NextPhases = ticket.NextPhases;
            this.NextLotteryTime = ticket.NextLotteryTime;
            this.Hide = ticket.Hide;
            this.Order = ticket.Order;
            this.CountOfPlayTag = countOfPlayTag;
            this.CountOfHowToPlay = countOfHowToPlay;
            this.LotteryTimes = ticket.Times.ConvertAll(x => new LotteryTimeResult(x));
        }
    }
}

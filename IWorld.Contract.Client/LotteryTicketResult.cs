using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 彩票信息
    /// </summary>
    [DataContract]
    public class LotteryTicketResult
    {
        #region 公开属性

        /// <summary>
        /// 彩票的数据库存储指针
        /// </summary>
        [DataMember]
        public int TicketId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 排序系数
        /// </summary>
        [DataMember]
        public int Order { get; set; }

        /// <summary>
        /// 当前期号
        /// </summary>
        [DataMember]
        public string Phases { get; set; }

        /// <summary>
        /// 当前开奖号码
        /// </summary>
        [DataMember]
        public List<string> Values { get; set; }

        /// <summary>
        /// 下一期
        /// </summary>
        [DataMember]
        public string NextPhases { get; set; }

        /// <summary>
        /// 下期截止时间
        /// </summary>
        [DataMember]
        public DateTime SurplusTime { get; set; }

        /// <summary>
        /// 服务器时间
        /// </summary>
        [DataMember]
        public DateTime TimeAtServer { get; set; }

        /// <summary>
        /// 玩法标签
        /// </summary>
        [DataMember]
        public List<PlayTagResult> Tags { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的彩票信息
        /// </summary>
        /// <param name="ticket">彩票信息的数据封装</param>
        /// <param name="lottery">最新一期开奖信息</param>
        /// <param name="false">一个布尔值 表示该对象是否是实体对象</param>
        public LotteryTicketResult(LotteryTicket ticket, Lottery lottery, bool isEntity = false)
        {
            this.TicketId = ticket.Id;
            this.Name = ticket.Name;
            this.Order = ticket.Order;
            this.Phases = lottery == null ? "" : lottery.Phases;
            this.Values = lottery == null ? new List<string>()
                : lottery.Seats.OrderBy(x => x.Order).ToList().ConvertAll(x => x.Value);
            this.NextPhases = ticket.NextPhases;
            this.SurplusTime = ticket.NextLotteryTime;
            this.TimeAtServer = DateTime.Now;
            if (isEntity)
            {
                this.Tags = ticket.Tags.ConvertAll(x => new PlayTagResult(x));
            }
        }

        #endregion
    }
}

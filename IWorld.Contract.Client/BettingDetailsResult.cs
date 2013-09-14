using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 投注明细
    /// </summary>
    [DataContract]
    public class BettingDetailsResult
    {
        #region 公开属性

        /// <summary>
        /// 投注记录的存储指针
        /// </summary>
        [DataMember]
        public int BettingId { get; set; }

        /// <summary>
        /// 彩票
        /// </summary>
        [DataMember]
        public string Ticket { get; set; }

        /// <summary>
        /// 玩法标签
        /// </summary>
        [DataMember]
        public string PlayTag { get; set; }

        /// <summary>
        /// 玩法
        /// </summary>
        [DataMember]
        public string HowToPlay { get; set; }

        /// <summary>
        /// 期号
        /// </summary>
        [DataMember]
        public string Phases { get; set; }

        /// <summary>
        /// 投注时间
        /// </summary>
        [DataMember]
        public DateTime Time { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [DataMember]
        public string Owner { get; set; }

        /// <summary>
        /// 总注数
        /// </summary>
        [DataMember]
        public int Sum { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        [DataMember]
        public double Multiple { get; set; }

        /// <summary>
        /// 返点
        /// </summary>
        [DataMember]
        public string RetutnPoints { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DataMember]
        public double Pay { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        [DataMember]
        public double Bonus { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        public BettingStatus Status { get; set; }

        /// <summary>
        /// 投注号码
        /// </summary>
        [DataMember]
        public string Values { get; set; }

        /// <summary>
        /// 开奖号码
        /// </summary>
        [DataMember]
        public string LotteryValues { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的投注明细
        /// </summary>
        /// <param name="betting">投注明细的数据封装</param>
        /// <param name="lottery">对应的开奖记录</param>
        public BettingDetailsResult(Betting betting, Lottery lottery = null)
        {
            this.BettingId = betting.Id;
            this.Ticket = betting.HowToPlay.Tag.Ticket.Name;
            this.PlayTag = betting.HowToPlay.Tag.Name;
            this.HowToPlay = betting.HowToPlay.Name;
            this.Phases = betting.Phases;
            this.Time = betting.CreatedTime;
            this.Owner = betting.Owner.Username;
            this.Pay = betting.Pay;
            this.Bonus = betting.Bonus;
            this.Status = betting.Status;
            this.Sum = betting.Sum;
            this.Multiple = betting.Multiple;
            if (betting.HowToPlay.Interface == LotteryInterface.任N不定位)
            {
                this.RetutnPoints = (betting.Owner.UncertainReturnPoints - betting.Points) + "%";
            }
            else
            {
                this.RetutnPoints = (betting.Owner.NormalReturnPoints - betting.Points) + "%";
            }
            switch (betting.HowToPlay.Interface)
            {
                case LotteryInterface.任N不定位:
                case LotteryInterface.任N组选:
                    this.Values = string.Join(",", betting.Seats.First().ValueList);
                    break;
                case LotteryInterface.任N直选:
                    if (betting.HowToPlay.Parameter3 == 0)
                    {
                        this.Values = string.Join(" ", betting.Seats.First().ValueList);
                    }
                    else
                    {
                        this.Values = string.Join(",", betting.Seats.ConvertAll(x => string.Join("", x.ValueList)));
                    }
                    break;
                default:
                    this.Values = string.Join(",", betting.Seats.ConvertAll(x => string.Join("", x.ValueList)));
                    break;
            }
            this.LotteryValues = lottery == null ? "" : string.Join(",", lottery.Seats.ConvertAll(x => x.Value));
        }

        #endregion
    }
}

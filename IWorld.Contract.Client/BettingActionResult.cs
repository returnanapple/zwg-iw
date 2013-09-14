using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 投注的操作结果
    /// </summary>
    [DataContract]
    public class BettingActionResult : OperateResult
    {
        /// <summary>
        /// 投注记录的存储指针
        /// </summary>
        [DataMember]
        public int BettingId { get; set; }

        /// <summary>
        /// 玩法
        /// </summary>
        [DataMember]
        public string HowToPlay { get; set; }

        /// <summary>
        /// 投注内容
        /// </summary>
        [DataMember]
        public string Values { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        [DataMember]
        public double Multiple { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DataMember]
        public double Money { get; set; }

        /// <summary>
        /// 实例化一个新的投注的操作结果（成功）
        /// </summary>
        /// <param name="betting">投注记录的数据封装</param>
        public BettingActionResult(Betting betting)
        {
            this.BettingId = betting.Id;
            this.HowToPlay = betting.HowToPlay.Name;
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
            this.Multiple = betting.Multiple;
            this.Money = betting.Pay;
        }

        /// <summary>
        /// 实例化一个新的投注的操作结果（失败）
        /// </summary>
        /// <param name="error">错误信息</param>
        public BettingActionResult(string error)
            : base(error)
        {
        }
    }
}

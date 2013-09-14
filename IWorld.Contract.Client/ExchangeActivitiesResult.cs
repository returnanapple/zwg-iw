using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 兑换活动信息
    /// </summary>
    [DataContract]
    public class ExchangeActivitiesResult
    {
        #region 公开属性

        /// <summary>
        /// 兑换活动的存储指针
        /// </summary>
        [DataMember]
        public int ExchangeId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 名额
        /// </summary>
        [DataMember]
        public int Places { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [DataMember]
        public double UnitPrice { get; set; }

        /// <summary>
        /// 每人每次允许兑换数量
        /// </summary>
        [DataMember]
        public int EachPersonCanExchangeTheNumberOfTimes { get; set; }

        /// <summary>
        /// 每人每天允许兑换次数
        /// </summary>
        [DataMember]
        public int EachPersonCanExchangeTheTimesOfDays { get; set; }

        /// <summary>
        /// 每人每天允许兑换总数量
        /// </summary>
        [DataMember]
        public int EachPersonCanExchangeTheNumberOfDays { get; set; }

        /// <summary>
        /// 每人允许参与总次数
        /// </summary>
        [DataMember]
        public int EachPersonCanExchangeTheTimesOfAll { get; set; }

        /// <summary>
        /// 每人允许兑换总数量
        /// </summary>
        [DataMember]
        public int EachPersonCanExchangeTheNumberOfAll { get; set; }

        /// <summary>
        /// 奖品
        /// </summary>
        [DataMember]
        public List<PrizesResult> Prizes { get; set; }

        /// <summary>
        /// 参与条件
        /// </summary>
        [DataMember]
        public List<ConditionResult> Conditions { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public DateTime EndTime { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的兑换活动信息
        /// </summary>
        /// <param name="exchange">兑换活动信息的数据封装</param>
        public ExchangeActivitiesResult(Exchange exchange)
        {
            this.ExchangeId = exchange.Id;
            this.Name = exchange.Name;
            this.Places = exchange.Places;
            this.UnitPrice = exchange.UnitPrice;
            this.EachPersonCanExchangeTheNumberOfTimes = exchange.EachPersonCanExchangeTheNumberOfTimes;
            this.EachPersonCanExchangeTheTimesOfDays = exchange.EachPersonCanExchangeTheTimesOfDays;
            this.EachPersonCanExchangeTheNumberOfDays = exchange.EachPersonCanExchangeTheNumberOfDays;
            this.EachPersonCanExchangeTheTimesOfAll = exchange.EachPersonCanExchangeTheTimesOfAll;
            this.EachPersonCanExchangeTheNumberOfAll = exchange.EachPersonCanExchangeTheNumberOfAll;
            this.Prizes = exchange.Prizes.ConvertAll(x => new PrizesResult(x));
            this.Conditions = exchange.Conditions.ConvertAll(x => new ConditionResult(x));
            this.BeginTime = exchange.BeginTime;
            this.EndTime = exchange.EndTime;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;

namespace IWorld.Model
{
    /// <summary>
    /// 兑换活动
    /// </summary>
    public class Exchange : RegularlyBase
    {
        #region 公开属性

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 名额
        /// </summary>
        public int Places { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public double UnitPrice { get; set; }

        /// <summary>
        /// 每人每次允许兑换数量
        /// </summary>
        public int EachPersonCanExchangeTheNumberOfTimes { get; set; }

        /// <summary>
        /// 每人每天允许兑换次数
        /// </summary>
        public int EachPersonCanExchangeTheTimesOfDays { get; set; }

        /// <summary>
        /// 每人每天允许兑换总数量
        /// </summary>
        public int EachPersonCanExchangeTheNumberOfDays { get; set; }

        /// <summary>
        /// 每人允许参与总次数
        /// </summary>
        public int EachPersonCanExchangeTheTimesOfAll { get; set; }

        /// <summary>
        /// 每人允许兑换总数量
        /// </summary>
        public int EachPersonCanExchangeTheNumberOfAll { get; set; }

        /// <summary>
        /// 奖品
        /// </summary>
        public virtual List<Prize> Prizes { get; set; }

        /// <summary>
        /// 参与条件
        /// </summary>
        public virtual List<ExchangeCondition> Conditions { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的兑换活动
        /// </summary>
        public Exchange()
        {
        }

        /// <summary>
        /// 实例化一个新的兑换活动
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="places">名额</param>
        /// <param name="unitPrice">单价</param>
        /// <param name="eachPersonCanExchangeTheNumberOfTimes">每人每次允许兑换数量</param>
        /// <param name="eachPersonCanExchangeTheTimesOfDays">每人每天允许兑换次数</param>
        /// <param name="eachPersonCanExchangeTheNumberOfDays">每人每天允许兑换总数量</param>
        /// <param name="eachPersonCanExchangeTheTimesOfAll">每人允许参与总次数</param>
        /// <param name="eachPersonCanExchangeTheNumberOfAll">每人允许兑换总数量</param>
        /// <param name="prizes">奖品</param>
        /// <param name="conditions">参与条件</param>
        /// <param name="beginTime">开始公示时间</param>
        /// <param name="days">持续天数</param>
        /// <param name="autoDelete">过期自动删除</param>
        public Exchange(string name, int places, double unitPrice, int eachPersonCanExchangeTheNumberOfTimes
            , int eachPersonCanExchangeTheTimesOfDays, int eachPersonCanExchangeTheNumberOfDays
            , int eachPersonCanExchangeTheTimesOfAll, int eachPersonCanExchangeTheNumberOfAll, List<Prize> prizes
            , List<ExchangeCondition> conditions, DateTime beginTime, int days, bool autoDelete)
            : base(beginTime, days, autoDelete)
        {
            this.Name = name;
            this.Places = places;
            this.UnitPrice = unitPrice;
            this.EachPersonCanExchangeTheNumberOfTimes = eachPersonCanExchangeTheNumberOfTimes;
            this.EachPersonCanExchangeTheTimesOfDays = eachPersonCanExchangeTheTimesOfDays;
            this.EachPersonCanExchangeTheNumberOfDays = eachPersonCanExchangeTheNumberOfDays;
            this.EachPersonCanExchangeTheTimesOfAll = eachPersonCanExchangeTheTimesOfAll;
            this.EachPersonCanExchangeTheNumberOfAll = eachPersonCanExchangeTheNumberOfAll;
            this.Prizes = prizes;
            this.Conditions = conditions;
        }

        #endregion
    }
}

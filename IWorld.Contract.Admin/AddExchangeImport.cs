using System.Collections.Generic;
using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于新建兑换活动的数据集
    /// </summary>
    [DataContract]
    public class AddExchangeImport
    {
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
        public List<EditPrizeImport> Prizes { get; set; }

        /// <summary>
        /// 参与条件
        /// </summary>
        [DataMember]
        public List<EditConditionImport> Conditions { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public string BeginTime { get; set; }

        /// <summary>
        /// 持续天数
        /// </summary>
        [DataMember]
        public int Days { get; set; }

        /// <summary>
        /// 过期自动删除
        /// </summary>
        [DataMember]
        public bool AutoDelete { get; set; }
    }
}

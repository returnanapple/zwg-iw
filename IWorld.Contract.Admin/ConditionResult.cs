using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 限制条件信息
    /// </summary>
    [DataContract]
    public class ConditionResult
    {
        /// <summary>
        /// 限制条件的存储指针
        /// </summary>
        [DataMember]
        public int ConditionId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DataMember]
        public ConditionType Type { get; set; }

        /// <summary>
        /// 下限
        /// </summary>
        [DataMember]
        public double Limit { get; set; }

        /// <summary>
        /// 上限
        /// </summary>
        [DataMember]
        public double Upper { get; set; }

        /// <summary>
        /// 实例化一个新的限制条件信息
        /// </summary>
        /// <param name="condition">限制条件信息的数据封装</param>
        public ConditionResult(ActivityCondition condition)
        {
            this.ConditionId = condition.Id;
            this.Type = condition.Type;
            this.Limit = condition.Limit;
            this.Upper = condition.Upper;
        }

        /// <summary>
        /// 实例化一个新的限制条件信息
        /// </summary>
        /// <param name="condition">限制条件信息的数据封装</param>
        public ConditionResult(ExchangeCondition condition)
        {
            this.ConditionId = condition.Id;
            this.Type = condition.Type;
            this.Limit = condition.Limit;
            this.Upper = condition.Upper;
        }
    }
}

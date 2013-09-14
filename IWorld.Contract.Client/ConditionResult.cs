using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 限制条件信息
    /// </summary>
    [DataContract]
    public class ConditionResult
    {
        #region 公开属性

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

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的限制条件信息
        /// </summary>
        /// <param name="condition">限制条件信息的数据封装</param>
        public ConditionResult(ActivityCondition condition)
        {
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
            this.Type = condition.Type;
            this.Limit = condition.Limit;
            this.Upper = condition.Upper;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 默认活动信息
    /// </summary>
    [DataContract]
    public class NormalActivitiesResult
    {
        #region 公开属性

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        [DataMember]
        public ActivityType Type { get; set; }

        /// <summary>
        /// 活动涉及指标的最小值
        /// </summary>
        [DataMember]
        public double MinRestrictionValue { get; set; }

        /// <summary>
        /// 活动涉及指标的最大值
        /// </summary>
        /// 
        [DataMember]
        public double MaxRestrictionValues { get; set; }

        /// <summary>
        /// 奖励类型
        /// </summary>
        [DataMember]
        public ActivityRewardType RewardType { get; set; }

        /// <summary>
        /// 奖励数额类型（绝对值/百分比）
        /// </summary>
        [DataMember]
        public bool RewardValueIsAbsolute { get; set; }

        /// <summary>
        /// 奖励数额
        /// </summary>
        [DataMember]
        public double Reward { get; set; }

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

        /// <summary>
        /// 限制条件
        /// </summary>
        [DataMember]
        public List<ConditionResult> Conditions { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的默认活动信息
        /// </summary>
        /// <param name="activity">默认活动信息的数据封装</param>
        public NormalActivitiesResult(Activity activity)
        {
            this.Title = activity.Title;
            this.Type = activity.Type;
            this.MinRestrictionValue = activity.MinRestrictionValue;
            this.MaxRestrictionValues = activity.MaxRestrictionValues;
            this.RewardType = activity.RewardType;
            this.RewardValueIsAbsolute = activity.RewardValueIsAbsolute;
            this.Reward = activity.Reward;
            this.BeginTime = activity.BeginTime;
            this.EndTime = activity.BeginTime.AddDays(activity.Days);
            this.Conditions = activity.Conditions.ConvertAll(x => new ConditionResult(x));
        }

        #endregion
    }
}

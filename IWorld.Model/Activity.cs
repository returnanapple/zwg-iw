using System;
using System.Collections.Generic;

namespace IWorld.Model
{
    /// <summary>
    /// 默认活动
    /// </summary>
    public class Activity : RegularlyBase
    {
        #region 公开属性

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        public ActivityType Type { get; set; }

        /// <summary>
        /// 活动涉及指标的最小值
        /// </summary>
        public double MinRestrictionValue { get; set; }

        /// <summary>
        /// 活动涉及指标的最大值
        /// </summary>
        public double MaxRestrictionValues { get; set; }

        /// <summary>
        /// 奖励类型
        /// </summary>
        public ActivityRewardType RewardType { get; set; }

        /// <summary>
        /// 奖励数额类型（绝对值/百分比）
        /// </summary>
        public bool RewardValueIsAbsolute { get; set; }

        /// <summary>
        /// 奖励数额
        /// </summary>
        public double Reward { get; set; }

        /// <summary>
        /// 限制条件
        /// </summary>
        public virtual List<ActivityCondition> Conditions { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的默认活动
        /// </summary>
        public Activity()
        {
        }

        /// <summary>
        /// 实例化一个新的默认活动
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="type">活动类型</param>
        /// <param name="minRestrictionValue">活动涉及指标的最小值</param>
        /// <param name="maxRestrictionValues">活动涉及指标的最大值</param>
        /// <param name="rewardType">奖励类型</param>
        /// <param name="rewardValueIsAbsolute">奖励数额类型</param>
        /// <param name="reward">奖励数额</param>
        /// <param name="conditions">限制条件</param>
        /// <param name="beginTime">开始公示时间</param>
        /// <param name="days">持续天数</param>
        /// <param name="autoDelete">过期自动删除</param>
        public Activity(string title, ActivityType type, double minRestrictionValue, double maxRestrictionValues, ActivityRewardType rewardType
            , bool rewardValueIsAbsolute, double reward, List<ActivityCondition> conditions, DateTime beginTime, int days
            , bool autoDelete)
            : base(beginTime, days, autoDelete)
        {
            this.Title = title;
            this.Type = type;
            this.MinRestrictionValue = minRestrictionValue;
            this.MaxRestrictionValues = maxRestrictionValues;
            this.RewardType = rewardType;
            this.RewardValueIsAbsolute = rewardValueIsAbsolute;
            this.Reward = reward;
            this.Conditions = conditions;
        }

        #endregion
    }
}

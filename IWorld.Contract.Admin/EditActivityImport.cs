﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于修改默认活动信息的数据集
    /// </summary>
    [DataContract]
    public class EditActivityImport
    {
        /// <summary>
        /// 默认活动的存储指针
        /// </summary>
        [DataMember]
        public int ActivityId { get; set; }

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
        /// 限制条件
        /// </summary>
        [DataMember]
        public List<EditConditionImport> Conditions { get; set; }

        /// <summary>
        /// 持续天数
        /// </summary>
        [DataMember]
        public int Days { get; set; }

        /// <summary>
        /// 暂停显示
        /// </summary>
        [DataMember]
        public bool Hide { get; set; }

        /// <summary>
        /// 过期自动删除
        /// </summary>
        [DataMember]
        public bool AutoDelete { get; set; }
    }
}

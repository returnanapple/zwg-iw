using System;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 默认活动参与记录信息
    /// </summary>
    [DataContract]
    public class ActivityParticipateRecordResult
    {
        /// <summary>
        /// 默认活动参与记录的存储指针
        /// </summary>
        [DataMember]
        public int ActivityParticipateRecordId { get; set; }

        /// <summary>
        /// 参与人的存储指针
        /// </summary>
        [DataMember]
        public int OwnerId { get; set; }

        /// <summary>
        /// 参与人
        /// </summary>
        [DataMember]
        public string OwnerName { get; set; }

        /// <summary>
        /// 参与的活动的存储指针
        /// </summary>
        [DataMember]
        public int ActivityId { get; set; }

        /// <summary>
        /// 参与的活动
        /// </summary>
        [DataMember]
        public string ActivityName { get; set; }

        /// <summary>
        /// 奖励类型
        /// </summary>
        [DataMember]
        public ActivityRewardType RewardType { get; set; }

        /// <summary>
        /// 实际奖励
        /// </summary>
        [DataMember]
        public double Reward { get; set; }

        /// <summary>
        /// 参与时间
        /// </summary>
        [DataMember]
        public DateTime ParticipatedTime { get; set; }

        /// <summary>
        /// 实例化一个新的默认活动参与记录信息
        /// </summary>
        /// <param name="record">默认活动参与记录信息的数据封装</param>
        public ActivityParticipateRecordResult(ActivityParticipateRecord record)
        {
            this.ActivityParticipateRecordId = record.Id;
            this.OwnerId = record.Owner.Id;
            this.OwnerName = record.Owner.Username;
            this.ActivityId = record.Activity.Id;
            this.ActivityName = record.Activity.Title;
            this.RewardType = record.Activity.RewardType;
            this.Reward = record.Activity.RewardValueIsAbsolute
                ? record.Activity.Reward : Math.Round(record.Activity.Reward * record.Amount, 2);
            this.ParticipatedTime = record.CreatedTime;
        }
    }
}

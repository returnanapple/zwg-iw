using System.Runtime.Serialization;

namespace IWorld.Model
{
    /// <summary>
    /// 活动类型
    /// </summary>
    [DataContract]
    public enum ActivityType
    {
        [EnumMember]
        注册返点 = 101,
        [EnumMember]
        充值返点 = 201,
        [EnumMember]
        下级用户充值返点 = 202,
        [EnumMember]
        下下级用户充值返点 = 203,
        [EnumMember]
        消费返点 = 301,
        [EnumMember]
        下级用户消费返点 = 302,
        [EnumMember]
        下下级用户消费返点 = 303,
        [EnumMember]
        亏损返点 = 401,
        [EnumMember]
        下级用户亏损返点 = 402,
        [EnumMember]
        下下级用户亏损返点 = 403,

        [EnumMember]
        当日累计消费奖励_非周末 = 501,
        [EnumMember]
        当日累计消费奖励_周末 = 502,
        [EnumMember]
        下级用户当日累计消费奖励 = 503,
        [EnumMember]
        下下级用户当日累计消费奖励 = 504,
        [EnumMember]
        当日累计充值奖励 = 601,
        [EnumMember]
        下级用户当日累计充值奖励 = 602,
        [EnumMember]
        下下级用户当日累计充值奖励 = 603,
        [EnumMember]
        当日累计亏损补贴 = 701,
        [EnumMember]
        下级用户当日累计亏损补贴 = 702,
        [EnumMember]
        下下级用户当日累计亏损补贴 = 703
    }
}

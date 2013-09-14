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
        消费返点 = 301,
        [EnumMember]
        下级用户消费返点 = 302
    }
}

using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 关于活动类型的筛选
    /// </summary>
    [DataContract]
    public enum ActivityTypeSelectType
    {
        [EnumMember]
        全部,
        [EnumMember]
        注册返点,
        [EnumMember]
        充值返点,
        [EnumMember]
        下级用户充值返点,
        [EnumMember]
        消费返点,
        [EnumMember]
        下级用户消费返点
    }
}

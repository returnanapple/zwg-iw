using System.Runtime.Serialization;

namespace IWorld.Model
{
    /// <summary>
    /// 活动参与条件的类型
    /// </summary>
    [DataContract]
    public enum ConditionType
    {
        [EnumMember]
        用户组等级 = 1,
        [EnumMember]
        消费量 = 2,
        [EnumMember]
        注册时间 = 3,
        [EnumMember]
        资金余额 = 4
    }
}

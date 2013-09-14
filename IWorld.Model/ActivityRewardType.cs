using System.Runtime.Serialization;

namespace IWorld.Model
{
    /// <summary>
    /// 默认活动的奖励类型
    /// </summary>
    [DataContract]
    public enum ActivityRewardType
    {
        [EnumMember]
        积分 = 0,
        [EnumMember]
        人民币 = 1
    }
}

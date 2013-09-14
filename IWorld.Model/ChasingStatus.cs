using System.Runtime.Serialization;

namespace IWorld.Model
{
    /// <summary>
    /// 追号记录的当前状态
    /// </summary>
    [DataContract]
    public enum ChasingStatus
    {
        [EnumMember]
        未开始 = 0,
        [EnumMember]
        追号中 = 1,
        [EnumMember]
        因为所追号码已经开出而终止 = 2,
        [EnumMember]
        因为中奖而终止 = 3,
        [EnumMember]
        用户中止追号 = 4,
        [EnumMember]
        追号结束 = 5
    }
}

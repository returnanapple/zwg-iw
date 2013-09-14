using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 关于追号状态的筛选
    /// </summary>
    [DataContract]
    public enum ChasingStatusSelectType
    {
        [EnumMember]
        全部,
        [EnumMember]
        未开始,
        [EnumMember]
        追号中,
        [EnumMember]
        因为所追号码已经开出而终止,
        [EnumMember]
        因为中奖而终止,
        [EnumMember]
        用户中止追号,
        [EnumMember]
        追号结束
    }
}

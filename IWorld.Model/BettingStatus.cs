using System.Runtime.Serialization;

namespace IWorld.Model
{
    /// <summary>
    /// 投注状态
    /// </summary>
    [DataContract]
    public enum BettingStatus
    {
        [EnumMember]
        用户撤单 = -2,
        [EnumMember]
        未中奖 = -1,
        [EnumMember]
        等待开奖 = 0,
        [EnumMember]
        即将开奖 = 1,
        [EnumMember]
        中奖 = 2
    }
}

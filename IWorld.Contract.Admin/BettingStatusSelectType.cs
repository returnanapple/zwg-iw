using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 关于投注状态的筛选
    /// </summary>
    [DataContract]
    public enum BettingStatusSelectType
    {
        [EnumMember]
        全部,
        [EnumMember]
        用户撤单,
        [EnumMember]
        未中奖,
        [EnumMember]
        等待开奖,
        [EnumMember]
        即将开奖 ,
        [EnumMember]
        中奖 
    }
}

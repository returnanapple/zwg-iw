using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 关于开奖来源的筛选
    /// </summary>
    [DataContract]
    public enum LotterySourcesSelectType
    {
        [EnumMember]
        全部,
        [EnumMember]
        系统采集,
        [EnumMember]
        手动
    }
}

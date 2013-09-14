using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 关于充值状态的筛选
    /// </summary>
    [DataContract]
    public enum RechargeStatusSelectType
    {
        [EnumMember]
        全部,
        [EnumMember]
        失败,
        [EnumMember]
        等待支付,
        [EnumMember]
        充值成功,
        [EnumMember]
        用户已经支付
    }
}

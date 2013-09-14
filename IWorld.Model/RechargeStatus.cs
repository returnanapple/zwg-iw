using System.Runtime.Serialization;

namespace IWorld.Model
{
    /// <summary>
    /// 充值状态
    /// </summary>
    [DataContract]
    public enum RechargeStatus
    {
        [EnumMember]
        失败 = -1,
        [EnumMember]
        等待支付 = 0,
        [EnumMember]
        充值成功 = 1,
        [EnumMember]
        用户已经支付 = 2
    }
}

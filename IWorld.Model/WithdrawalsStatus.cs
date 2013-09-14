using System.Runtime.Serialization;

namespace IWorld.Model
{
    /// <summary>
    /// 提款状态
    /// </summary>
    [DataContract]
    public enum WithdrawalsStatus
    {
        [EnumMember]
        失败 = -1,
        [EnumMember]
        处理中 = 0,
        [EnumMember]
        提现成功 = 1
    }
}

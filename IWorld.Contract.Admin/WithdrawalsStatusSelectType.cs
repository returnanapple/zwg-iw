using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 关于提现状态的筛选
    /// </summary>
    [DataContract]
    public enum WithdrawalsStatusSelectType
    {
        [EnumMember]
        全部,
        [EnumMember]
        失败,
        [EnumMember]
        处理中,
        [EnumMember]
        提现成功
    }
}

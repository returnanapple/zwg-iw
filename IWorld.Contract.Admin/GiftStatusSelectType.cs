using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 关于实体奖品赠送状态的筛选
    /// </summary>
    [DataContract]
    public enum GiftStatusSelectType
    {
        [EnumMember]
        全部,
        [EnumMember]
        未赠送,
        [EnumMember]
        已赠送
    }
}

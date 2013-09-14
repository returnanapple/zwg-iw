using System.Runtime.Serialization;

namespace IWorld.Model
{
    /// <summary>
    /// 实体奖品赠送记录的状态
    /// </summary>
    [DataContract]
    public enum GiftStatus
    {
        [EnumMember]
        未赠送 = 0,
        [EnumMember]
        已赠送 = 1
    }
}
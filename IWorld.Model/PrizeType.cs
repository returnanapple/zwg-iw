using System.Runtime.Serialization;

namespace IWorld.Model
{
    /// <summary>
    /// 奖品类型
    /// </summary>
    [DataContract]
    public enum PrizeType
    {
        [EnumMember]
        人民币 = 101,
        [EnumMember]
        实物 = 201,
        [EnumMember]
        积分 = 301
    }
}

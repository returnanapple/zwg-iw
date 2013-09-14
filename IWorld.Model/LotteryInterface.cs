using System.Runtime.Serialization;

namespace IWorld.Model
{
    /// <summary>
    /// 开奖接口
    /// </summary>
    [DataContract]
    public enum LotteryInterface
    {
        [EnumMember]
        任N直选 = 1,
        [EnumMember]
        任N组选 = 2,
        [EnumMember]
        任N不定位 = 3,
        [EnumMember]
        任N定位胆 = 4,
        [EnumMember]
        大小单双 = 5
    }
}

using System.Runtime.Serialization;

namespace IWorld.Model
{
    /// <summary>
    /// 开奖记录的来源
    /// </summary>
    [DataContract]
    public enum LotterySources
    {
        [EnumMember]
        系统采集 = 101,
        [EnumMember]
        手动 = 201
    }
}

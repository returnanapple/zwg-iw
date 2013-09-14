using System.Runtime.Serialization;

namespace IWorld.Model
{
    /// <summary>
    /// 货币单位
    /// </summary>
    [DataContract]
    public enum Currency
    {
        /// <summary>
        /// 人民币
        /// </summary>
        [EnumMember]
        元 = 101
    }
}

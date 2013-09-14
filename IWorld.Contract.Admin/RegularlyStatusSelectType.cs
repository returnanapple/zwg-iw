using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 关于定时活动的状态的筛选
    /// </summary>
    [DataContract]
    public enum RegularlyStatusSelectType
    {
        [EnumMember]
        全部,
        [EnumMember]
        未过期,
        [EnumMember]
        正常,
        [EnumMember]
        暂停,
        [EnumMember]
        已过期
    }
}

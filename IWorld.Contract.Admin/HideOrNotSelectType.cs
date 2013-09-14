using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 关于对象是否隐藏的筛选
    /// </summary>
    [DataContract]
    public enum HideOrNotSelectType
    {
        [EnumMember]
        全部,
        [EnumMember]
        显示,
        [EnumMember]
        隐藏,
    }
}

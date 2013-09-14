using System.Runtime.Serialization;

namespace IWorld.Model
{
    /// <summary>
    /// 客服类型
    /// </summary>
    [DataContract]
    public enum CustomerType
    {
        [EnumMember]
        在线客服 = 1,
        [EnumMember]
        财务客服 = 2
    }
}

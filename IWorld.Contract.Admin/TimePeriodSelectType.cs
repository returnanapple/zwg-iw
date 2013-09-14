using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 关于时间段的筛选
    /// </summary>
    [DataContract]
    public enum TimePeriodSelectType
    {
        [EnumMember]
        月,
        [EnumMember]
        日
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 时限状态
    /// </summary>
    [DataContract]
    public enum TimeTasksStatus
    {
        [EnumMember]
        未开始 = -1,
        [EnumMember]
        暂停 = 0,
        [EnumMember]
        正常 = 1,
        [EnumMember]
        已过期 = 2
    }
}

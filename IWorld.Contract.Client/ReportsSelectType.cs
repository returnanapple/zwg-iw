using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 数据报表的筛选类型
    /// </summary>
    [DataContract]
    public enum ReportsSelectType
    {
        [EnumMember]
        当日,
        [EnumMember]
        当月,
        [EnumMember]
        全部
    }
}

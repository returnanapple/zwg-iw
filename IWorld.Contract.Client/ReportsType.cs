using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 数据报表的类型
    /// </summary>
    [DataContract]
    public enum ReportsType
    {
        [EnumMember]
        个人,
        [EnumMember]
        团队
    }
}

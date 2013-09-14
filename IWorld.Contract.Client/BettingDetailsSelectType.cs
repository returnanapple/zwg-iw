using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 投注明细的筛选类型
    /// </summary>
    [DataContract]
    public enum BettingDetailsSelectType
    {
        [EnumMember]
        个人,
        [EnumMember]
        团队,
        [EnumMember]
        直属下级
    }
}

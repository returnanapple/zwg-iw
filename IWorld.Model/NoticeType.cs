using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Model
{
    /// <summary>
    /// 通知类型
    /// </summary>
    [DataContract]
    public enum NoticeType
    {
        [EnumMember]
        开奖提醒,
        [EnumMember]
        充值反馈,
        [EnumMember]
        提现反馈
    }
}

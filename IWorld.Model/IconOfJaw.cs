using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Model
{
    /// <summary>
    /// 大白鲨游戏的图标
    /// </summary>
    [DataContract]
    public enum IconOfJaw
    {
        [EnumMember]
        金色鲨鱼 = 101,
        [EnumMember]
        蓝色鲨鱼 = 102,

        [EnumMember]
        燕子 = 201,
        [EnumMember]
        鸽子 = 202,
        [EnumMember]
        孔雀 = 203,
        [EnumMember]
        老鹰 = 204,

        [EnumMember]
        狮子 = 301,
        [EnumMember]
        熊猫 = 302,
        [EnumMember]
        猴子 = 303,
        [EnumMember]
        兔子 = 304,

        [EnumMember]
        通杀 = 401,
        [EnumMember]
        通赔 = 402,

        [EnumMember]
        飞禽 = 501,
        [EnumMember]
        走兽 = 502
    }
}

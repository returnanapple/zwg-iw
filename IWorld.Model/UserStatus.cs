﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 用户状态
    /// </summary>
    [DataContract]
    public enum UserStatus
    {
        [EnumMember]
        禁止访问 = -1,
        [EnumMember]
        未激活 = 0,
        [EnumMember]
        正常 = 1
    }
}

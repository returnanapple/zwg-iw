using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Admin.Framework
{
    /// <summary>
    /// 接收信息的实例委托
    /// </summary>
    /// <param name="message">消息</param>
    public delegate void RecipientDelegate(IMessage message);
}

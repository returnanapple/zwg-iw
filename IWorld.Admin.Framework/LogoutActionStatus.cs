using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Admin.Framework
{
    /// <summary>
    /// 等处动作的执行状态
    /// </summary>
    public enum LogoutActionStatus
    {
        /// <summary>
        /// 等待确认
        /// </summary>
        WaitForConfirm = 0,
        /// <summary>
        /// 执行
        /// </summary>
        DoNow = 1,
        /// <summary>
        /// 执行完毕
        /// </summary>
        Done = 2
    }
}

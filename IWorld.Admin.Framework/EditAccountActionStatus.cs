using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Admin.Framework
{
    /// <summary>
    /// 修改账户信息的执行状态
    /// </summary>
    public enum EditAccountActionStatus
    {
        /// <summary>
        /// 显示错误信息
        /// </summary>
        ShowError = -1,
        /// <summary>
        /// 填写信息
        /// </summary>
        WriteInformation = 0,
        /// <summary>
        /// 执行修改
        /// </summary>
        EditNow = 1,
        /// <summary>
        /// 显示修改结果
        /// </summary>
        ShowResult = 2,
        /// <summary>
        /// 执完毕
        /// </summary>
        Done = 3
    }
}

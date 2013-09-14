using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Admin.Framework
{
    /// <summary>
    /// 定义修改编辑用户信息的参数
    /// </summary>
    public interface IEditAccountPackage
    {
        /// <summary>
        /// 原密码
        /// </summary>
        string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        string NewPassword { get; set; }

        /// <summary>
        /// 新密码确认
        /// </summary>
        string NewPassword_Confirm { get; set; }
    }
}

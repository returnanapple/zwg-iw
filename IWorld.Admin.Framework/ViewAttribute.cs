using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Admin.Framework
{
    /// <summary>
    /// 标记视图对象为可跳转界面
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewAttribute : Attribute
    {
        /// <summary>
        /// 界面名称
        /// </summary>
        public string PageName { get; set; }

        /// <summary>
        /// 一个布尔值 表示被标记对象是否为默认界面
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 标记视图对象为可跳转界面
        /// </summary>
        /// <param name="pageName">界面名称</param>
        /// <param name="isDefault">一个布尔值 表示被标记对象是否为默认界面</param>
        public ViewAttribute(string pageName = "登陆", bool isDefault = false)
        {
            this.PageName = pageName;
            this.IsDefault = isDefault;
        }
    }
}

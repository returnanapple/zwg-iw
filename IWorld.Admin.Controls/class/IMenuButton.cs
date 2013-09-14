using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace IWorld.Admin.Controls
{
    /// <summary>
    /// 定义导航按键
    /// </summary>
    public interface IMenuButton
    {
        /// <summary>
        /// 显示文本
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// 一个布尔值 表示按键是否允许执行跳转命令
        /// </summary>
        bool CanJump { get; set; }
    }
}

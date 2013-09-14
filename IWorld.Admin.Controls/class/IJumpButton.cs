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
    /// 定义跳转按键
    /// </summary>
    public interface IJumpButton
    {
        /// <summary>
        /// 显示文本
        /// </summary>
        string Text { get; set; }
    }
}

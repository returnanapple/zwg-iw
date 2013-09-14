using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace IWorld.Admin.Controls
{
    /// <summary>
    /// 侧边栏导航按键
    /// </summary>
    public partial class SideTool_MenuButton : UserControl, IMenuButton, IJumpButton
    {
        /// <summary>
        /// 实例化一个新的侧边栏导航按键
        /// </summary>
        public SideTool_MenuButton()
        {
            InitializeComponent();
        }

        #region 依赖属性

        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SideTool_MenuButton)
            , new PropertyMetadata("Menu Button", (d, e) =>
            {
                SideTool_MenuButton tool = (SideTool_MenuButton)d;
                tool.button_content.Content = e.NewValue;
            }));

        /// <summary>
        /// 被选中的按键的显示文本
        /// </summary>
        public string SelectedText
        {
            get { return (string)GetValue(SelectedTextProperty); }
            set { SetValue(SelectedTextProperty, value); }
        }

        public static readonly DependencyProperty SelectedTextProperty =
            DependencyProperty.Register("SelectedText", typeof(string), typeof(SideTool_MenuButton)
            , new PropertyMetadata("", (d, e) =>
            {
                SideTool_MenuButton tool = (SideTool_MenuButton)d;
                string key = tool.Text == e.NewValue.ToString() ? "selected" : "normal";
                tool.button_content.Style = (Style)tool.Resources[key];
            }));

        /// <summary>
        /// 一个布尔值 表示按键是否允许执行跳转命令
        /// </summary>
        public bool CanJump
        {
            get { return (bool)GetValue(CanJumpProperty); }
            set { SetValue(CanJumpProperty, value); }
        }

        public static readonly DependencyProperty CanJumpProperty =
            DependencyProperty.Register("CanJump", typeof(bool), typeof(SideTool_MenuButton), new PropertyMetadata(false));

        #endregion

        /// <summary>
        /// 触发按键
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        private void OnClick(object sender, EventArgs e)
        {
            if (Text == SelectedText && CanJump)
            {
                return;
            }

            if (Click != null)
            {
                Click(this, new EventArgs());
            }
        }

        /// <summary>
        /// 按键被点击时将触发的事件
        /// </summary>
        public event EventHandler Click;
    }
}

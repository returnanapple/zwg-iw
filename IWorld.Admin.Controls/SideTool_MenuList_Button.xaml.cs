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
    public partial class SideTool_MenuList_Button : UserControl, IJumpButton
    {
        public SideTool_MenuList_Button()
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
            DependencyProperty.Register("Text", typeof(string), typeof(SideTool_MenuList_Button)
            , new PropertyMetadata("Menu List Button", (d, e) =>
            {
                SideTool_MenuList_Button tool = (SideTool_MenuList_Button)d;
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
            DependencyProperty.Register("SelectedText", typeof(string), typeof(SideTool_MenuList_Button)
            , new PropertyMetadata("", (d, e) =>
            {
                SideTool_MenuList_Button tool = (SideTool_MenuList_Button)d;
                string key = tool.Text == e.NewValue.ToString() ? "selected" : "normal";
                tool.button_content.Style = (Style)tool.Resources[key];
            }));

        #endregion

        /// <summary>
        /// 触发按键
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        private void OnClick(object sender, EventArgs e)
        {
            if (Text == SelectedText)
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

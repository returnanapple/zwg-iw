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
using IWorld.Admin.Class;

namespace IWorld.Admin
{
    public partial class SubmitButton : UserControl
    {
        public SubmitButton()
        {
            InitializeComponent();
        }

        #region 依赖属性

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SubmitButton),
            new PropertyMetadata("", new PropertyChangedCallback(TextChanged)));

        static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SubmitButton sb = (SubmitButton)d;
            sb.text_content.Text = e.NewValue.ToString();
        }

        #endregion

        #region 鼠标事件

        private void ShowHoverStyle(object sender, MouseEventArgs e)
        {
            text_content.Style = (Style)this.Resources["hover"];
        }

        private void ShowNormalStyle(object sender, MouseEventArgs e)
        {
            text_content.Style = (Style)this.Resources["normal"];
        }

        private void Click(object sender, MouseButtonEventArgs e)
        {
            if (ClickEventHandler != null)
            {
                ClickEventHandler(this, new EventArgs());
            }
        }
        
        #endregion

        #region 事件

        /// <summary>
        /// 按键被点击的时候将触发的事件
        /// </summary>
        public event NDelegate ClickEventHandler;
        
        #endregion
    }
}

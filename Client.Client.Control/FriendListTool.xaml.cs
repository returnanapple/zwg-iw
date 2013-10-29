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

namespace Client.Client.Control
{
    public partial class FriendListTool : UserControl
    {
        bool sIsRunning = false;
        public FriendListTool()
        {
            InitializeComponent();
        }

        #region 打开/关闭动画

        void HideRow(object sender, EventArgs e)
        {
            r1.Height = new GridLength(0);
            r2.Height = new GridLength(0);
            root.Height = 31;
            sIsRunning = false;
        }

        void ShowRow()
        {
            r1.Height = new GridLength(25);
            r2.Height = new GridLength(30);
        }

        bool isShow = true;
        void ShowOrHideTool(object sender, MouseButtonEventArgs e)
        {
            if (sIsRunning) { return; }
            sIsRunning = true;
            if (!isShow) { ShowRow(); }
            string key = isShow ? "close" : "open";
            Storyboard s = (Storyboard)Resources[key];
            s.Begin();
            isShow = !isShow;
        }

        void HideTool(object sender, EventArgs e)
        {
            if (sIsRunning) { return; }
            sIsRunning = true;
            if (!isShow) { return; }
            Storyboard s = (Storyboard)Resources["close"];
            s.Begin();
            isShow = !isShow;
        }

        private void openRunned(object sender, EventArgs e)
        {
            sIsRunning = false;
        }

        #endregion

        #region 依赖属性

        public bool HaveUnreadMessage
        {
            get { return (bool)GetValue(HaveUnreadMessageProperty); }
            set { SetValue(HaveUnreadMessageProperty, value); }
        }

        public static readonly DependencyProperty HaveUnreadMessageProperty =
            DependencyProperty.Register("HaveUnreadMessage", typeof(bool), typeof(FriendListTool)
            , new PropertyMetadata(false, (d, e) =>
            {
                FriendListTool tool = (FriendListTool)d;
                Storyboard s = (Storyboard)tool.Resources["prompt"];
                if ((bool)e.NewValue == true)
                {
                    s.Begin();
                }
                else
                {
                    s.Stop();
                }
            }));

        #endregion
    }
}

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
using IWorld.Client.Class;
using IWorld.Client.BulletinService;

namespace IWorld.Client
{
    public partial class NoticeWindow : UserControl
    {
        public NoticeWindow()
        {
            InitializeComponent();
        }

        #region 快捷功能

        /// <summary>
        /// 顶部功能按键触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonXHover(object sender, EventArgs e)
        {
            ((Grid)sender).Background = new SolidColorBrush(Colors.Gray);
        }

        /// <summary>
        /// 顶部功能按键恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonXUnhover(object sender, EventArgs e)
        {
            ((Grid)sender).Background = null;
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseMainWindow(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion

        public event NDelegate ClickEventHandler;

        public event NDelegate CallMusicEventHandler;

        public NoticeResult Notice
        {
            get { return (NoticeResult)GetValue(NoticeProperty); }
            set { SetValue(NoticeProperty, value); }
        }

        public static readonly DependencyProperty NoticeProperty =
            DependencyProperty.Register("Notice", typeof(NoticeResult), typeof(NoticeWindow)
            , new PropertyMetadata(null, (d, e) =>
                {
                    NoticeWindow nw = (NoticeWindow)d;
                    nw.Visibility = Visibility.Visible;
                    nw.text_context.Text = ((NoticeResult)e.NewValue).Context;
                    if (nw.CallMusicEventHandler != null)
                    {
                        nw.CallMusicEventHandler(nw, new EventArgs());
                    }
                }));

        private void Click(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
            if (ClickEventHandler != null)
            {
                ClickEventHandler(this, new EventArgs());
            }
        }

        
    }
}

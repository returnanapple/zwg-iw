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
    public partial class UserOnlineStatusIcon : UserControl
    {
        public UserOnlineStatusIcon()
        {
            InitializeComponent();
        }

        #region 在线状态

        /// <summary>
        /// 一个布尔值 标识用户是否在线
        /// </summary>
        public bool Online
        {
            get { return (bool)GetValue(OnlineProperty); }
            set { SetValue(OnlineProperty, value); }
        }

        public static readonly DependencyProperty OnlineProperty =
            DependencyProperty.Register("Online", typeof(bool), typeof(UserOnlineStatusIcon)
            , new PropertyMetadata(false, (d, e) =>
            {
                UserOnlineStatusIcon tool = (UserOnlineStatusIcon)d;
                string key = (bool)e.NewValue == true ? "online" : "offline";
                tool.bg.Style = (Style)tool.Resources[key];
            }));

        #endregion
    }
}

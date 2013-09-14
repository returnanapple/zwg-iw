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
using IWorld.Client.UsersService;

namespace IWorld.Client
{
    public partial class UsersPage_FullWindow : ChildWindow
    {
        BasicUserInfoResult UserInfo { get; set; }
        public DoWork Work { get; set; }

        public UsersPage_FullWindow(BasicUserInfoResult userInfo)
        {
            InitializeComponent();
            this.UserInfo = userInfo;
            this.Work = DoWork.无;

            if (userInfo.UserId == App.UserInfo.UserId)
            {
                button_r.Visibility = System.Windows.Visibility.Collapsed;
                button_u.Visibility = System.Windows.Visibility.Collapsed;
                button_b.SetValue(Canvas.TopProperty, 25.0);
            }

            text_username.Text = userInfo.Username;
            text_group.Text = userInfo.Group;
            text_nrp.Text = userInfo.NormalReturnPoints + "%";
            text_urp.Text = userInfo.UncertainReturnPoints + "%";
            text_money.Text = userInfo.Money.ToString("0.00");
            text_consumption.Text = userInfo.Consumption.ToString("0.00");
            text_status.Text = userInfo.Status.ToString();
            text_time.Text = userInfo.LastLoginTime.ToLongDateString();
            text_ip.Text = userInfo.LastLoginIp;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Recharge(object sender, RoutedEventArgs e)
        {
            this.Work = DoWork.代充值;
            this.DialogResult = true;
        }

        private void UpPoints(object sender, RoutedEventArgs e)
        {
            this.Work = DoWork.升点;
            this.DialogResult = true;
        }

        public enum DoWork
        {
            无,
            代充值,
            升点
        }
    }
}


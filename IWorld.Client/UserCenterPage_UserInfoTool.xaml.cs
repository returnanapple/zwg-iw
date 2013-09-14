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

namespace IWorld.Client
{
    public partial class UserCenterPage_UserInfoTool : UserControl
    {
        public UserCenterPage_UserInfoTool()
        {
            InitializeComponent();

            text_Username.Text = App.UserInfo.Username;
            text_Email.Text = App.UserInfo.Email;
            text_NormalReturnPoints.Text = App.UserInfo.NormalReturnPoints + "%";
            text_UncertainReturnPoints.Text = App.UserInfo.UncertainReturnPoints + "%";
            text_Money.Text = App.UserInfo.Money.ToString("0.00");
            text_MoneyBeFrozen.Text = App.UserInfo.MoneyBeFrozen.ToString("0.00");
            text_Consumption.Text = App.UserInfo.Consumption.ToString("0.00");
            text_Integral.Text = App.UserInfo.Integral.ToString();
            text_Status.Text = App.UserInfo.Status.ToString();
            text_LastLoginTime.Text = App.UserInfo.LastLoginTime.ToLongDateString();
            text_LastLoginIp.Text = App.UserInfo.LastLoginIp;
            text_Card.Text = App.UserInfo.Card;
            text_Holder.Text = App.UserInfo.Holder;
            text_Bank.Text = App.UserInfo.Bank.ToString();

            text_Group.Text = App.UserInfo.Group.Name;
            text_Grade.Text = App.UserInfo.Group.Grade.ToString();
            text_Withdrawals.Text = App.UserInfo.Group.Withdrawals.ToString();
            text_MinimumWithdrawalAmount.Text = App.UserInfo.Group.MinimumWithdrawalAmount.ToString();
            text_MaximumWithdrawalAmount.Text = App.UserInfo.Group.MaximumWithdrawalAmount.ToString();
            text_MinimumRechargeAmount.Text = App.UserInfo.Group.MinimumRechargeAmount.ToString();
            text_MaximumRechargeAmount.Text = App.UserInfo.Group.MaximumRechargeAmount.ToString();
            text_WithdrawalsAtAnyTime.Text = App.UserInfo.Group.WithdrawalsAtAnyTime ? "是" : "否";
        }
    }
}

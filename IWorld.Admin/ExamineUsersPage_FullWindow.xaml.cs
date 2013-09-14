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
using IWorld.Admin.UserInfoService;

namespace IWorld.Admin
{
    public partial class ExamineUsersPage_FullWindow : ChildWindow
    {
        private UserInfoResult UserInfo { get; set; }

        private bool changed = false;

        public ExamineUsersPage_FullWindow(UserInfoResult userInfo)
        {
            InitializeComponent();
            this.UserInfo = userInfo;
            text_username.Text = userInfo.Username;
            text_group.Text = userInfo.GroupName;
            text_email.Text = userInfo.BindingEmail ? userInfo.Email : "（未绑定）";
            text_returnPoints.Text = string.Format("{0}/{1}", userInfo.NormalReturnPoints, userInfo.UncertainReturnPoints);
            text_money.Text = userInfo.Money.ToString();
            text_frozen.Text = userInfo.MoneyBeFrozen.ToString();
            text_consumption.Text = userInfo.Consumption.ToString();
            text_integral.Text = userInfo.Integral.ToString();
            text_subordinate.Text = string.Format("{0}/{1}", userInfo.Subordinate, userInfo.MaxOfSubordinate); ;
            text_lastLoginTime.Text = userInfo.LastLoginTime.ToLongDateString();
            text_lastLoginIp.Text = userInfo.LastLoginIp;
            text_status.Text = userInfo.Status.ToString();
            text_card.Text = userInfo.BindingCard ? userInfo.Card : "（未绑定）";
            text_holder.Text = userInfo.BindingCard ? userInfo.Name : "";
            text_bank.Text = userInfo.BindingCard ? userInfo.Bank.ToString() : "";
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetPassword(object sender, EventArgs e)
        {
            UserInfoServiceClient client = new UserInfoServiceClient();
            client.ResetPasswordCompleted += (_sender, _e) =>
                {
                    if (_e.Result.Success)
                    {
                        text_password.Text = _e.Result.NewPassword;
                    }
                };
            client.ResetPasswordAsync(this.UserInfo.UserId, App.Token);
        }

        /// <summary>
        /// 重置安全码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetSafeCode(object sender, EventArgs e)
        {
            UserInfoServiceClient client = new UserInfoServiceClient();
            client.ResetSafeCodeCompleted += (_sender, _e) =>
                {
                    if (_e.Result.Success)
                    {
                        text_safrCode.Text = _e.Result.NewSafeWord;
                    }
                };
            client.ResetSafeCodeAsync(this.UserInfo.UserId, App.Token);
        }

        /// <summary>
        /// 重置Email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetEmail(object sender, EventArgs e)
        {
            UserInfoServiceClient client = new UserInfoServiceClient();
            client.ResetEmailCompleted += (_sender, _e) =>
                {
                    if (_e.Result.Success)
                    {
                        this.changed = true;
                        text_email.Text = "（未绑定）";
                    }
                };
            client.ResetEmailAsync(this.UserInfo.UserId, App.Token);
        }

        /// <summary>
        /// 重置银行卡信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetBankCard(object sender, EventArgs e)
        {
            UserInfoServiceClient client = new UserInfoServiceClient();
            client.ResetBankCardCompleted += (_sender, _e) =>
                {
                    this.changed = true;
                    text_card.Text = "（未绑定）";
                    text_holder.Text = "";
                    text_bank.Text = "";
                };
            client.ResetBankCardAsync(this.UserInfo.UserId, App.Token);
        }

        /// <summary>
        /// 返回列表页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToListPage(object sender, EventArgs e)
        {
            this.DialogResult = this.changed;
        }
    }
}


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
using IWorld.Client.FundsService;

namespace IWorld.Client
{
    public partial class RechargeTool : ChildWindow
    {
        int userId = 0;
        public bool ShowError { get; set; }
        public RechargeResult Result { get; set; }

        public RechargeTool(int userId, string username)
        {
            InitializeComponent();
            ShowError = false;
            this.userId = userId;
            string t = userId == App.UserInfo.UserId ? "本人" : "下级用户";

            text_user.Text = string.Format("{0}（{1}）", username, t);
        }

        private void Recharge(object sender, RoutedEventArgs e)
        {
            double sum = Math.Round(Convert.ToDouble(input_sum.Text));
            if (sum > 0)
            {
                FundsServiceClient client = new FundsServiceClient();
                client.RechargeCompleted += ShowRechargeResult;
                client.RechargeAsync(userId, sum, App.Token);
            }
        }
        #region 提现
        void ShowRechargeResult(object sender, RechargeCompletedEventArgs e)
        {
            if (!e.Result.Success)
            {
                ShowError = true;
            }
            Result = e.Result;
            this.DialogResult = true;
        }
        #endregion

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


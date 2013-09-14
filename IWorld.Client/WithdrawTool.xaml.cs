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
    public partial class WithdrawTool : ChildWindow
    {
        public bool ShowError { get; set; }
        public string Error { get; set; }

        public WithdrawTool()
        {
            InitializeComponent();
        }

        private void Withdraw(object sender, RoutedEventArgs e)
        {
            double sum = Math.Round(Convert.ToDouble(input_sum.Text));
            FundsServiceClient client = new FundsServiceClient();
            client.WithdrawCompleted += ShowWithdrawRsult;
            client.WithdrawAsync(sum, input_safeCode.Password, App.Token);
        }
        #region 提现

        void ShowWithdrawRsult(object sender, WithdrawCompletedEventArgs e)
        {
            if (!e.Result.Success)
            {
                this.ShowError = true;
                this.Error = e.Result.Error;
            }
            this.DialogResult = true;
        }

        #endregion

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


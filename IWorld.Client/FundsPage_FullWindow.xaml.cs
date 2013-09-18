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
using IWorld.Client.SystemSettingService;

namespace IWorld.Client
{
    public partial class FundsPage_FullWindow : ChildWindow
    {
        RechargeDetailsResult RdResult { get; set; }
        BankAccountResult BankAccount { get; set; }

        public FundsPage_FullWindow(RechargeDetailsResult rdResult)
        {
            InitializeComponent();
            this.RdResult = rdResult;

            text_to.Text = rdResult.To;
            text_from.Text = rdResult.From;
            text_sum.Text = rdResult.Sum.ToString("0.00");
            text_time.Text = rdResult.Time.ToLongDateString();
            text_status.Text = rdResult.Status.ToString();
            text_code.Text = rdResult.Code;
            text_bank.Text = BankAccount.Bank.ToString();

            SystemSettingServiceClient client = new SystemSettingServiceClient();
            client.GetBankAccountCompleted += ShowBankAccount;
            client.GetBankAccountAsync();
        }

        void ShowBankAccount(object sender, GetBankAccountCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                BankAccount = e.Result;
                text_card.Text = e.Result.Card;
                text_holder.Text = e.Result.Holder;
                text_bank.Text = e.Result.Bank.ToString();
            }
            else
            {
                text_card.Text = string.Format("({0})", e.Result.Error);
                text_card.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void GetCard(object sender, RoutedEventArgs e)
        {
            if (BankAccount != null)
            {
                Clipboard.SetText(BankAccount.Card);
            }
        }

        private void GetHolder(object sender, RoutedEventArgs e)
        {
            if (BankAccount != null)
            {
                Clipboard.SetText(BankAccount.Holder);
            }
        }

        private void GetCode(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(RdResult.Code);
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


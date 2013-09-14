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
    public partial class RechargeResultTool : ChildWindow
    {
        RechargeResult Recharge { get; set; }

        public RechargeResultTool(RechargeResult recharge)
        {
            InitializeComponent();
            this.Recharge = recharge;

            text_card.Text = recharge.Card;
            text_holder.Text = recharge.Holder;
            text_bank.Text = recharge.Bank.ToString();
            text_code.Text = recharge.Code;
        }

        private void GetCard(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Recharge.Card);
        }

        private void GetHolder(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Recharge.Holder);
        }

        private void GetCode(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Recharge.Code);
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


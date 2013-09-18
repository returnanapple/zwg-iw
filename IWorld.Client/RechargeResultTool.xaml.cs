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

            text_card1.Text = recharge.Card1;
            text_holder1.Text = recharge.Holder1;
            text_bank1.Text = recharge.Bank1.ToString();
            text_code1.Text = recharge.Code;
            text_card2.Text = recharge.Card2;
            text_holder2.Text = recharge.Holder2;
            text_bank2.Text = recharge.Bank2.ToString();
            text_code2.Text = recharge.Code;
        }

        private void GetCard1(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Recharge.Card1);
        }

        private void GetHolder1(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Recharge.Holder1);
        }

        private void GetCard2(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Recharge.Card2);
        }

        private void GetHolder2(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Recharge.Holder2);
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


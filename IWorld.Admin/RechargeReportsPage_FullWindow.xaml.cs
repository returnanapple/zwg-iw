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
using IWorld.Admin.DataReportService;

namespace IWorld.Admin
{
    public partial class RechargeReportsPage_FullWindow : ChildWindow
    {
        public RechargeReportsPage_FullWindow(RechargeResult recharge)
        {
            InitializeComponent();

            text_owner.Text = recharge.Owner;
            text_payer.Text = recharge.Payer;
            text_sum.Text = recharge.Sum.ToString("0.00");
            text_code.Text = recharge.Code;
            text_card.Text = recharge.Card;
            text_holer.Text = recharge.Name;
            text_status.Text = recharge.Status.ToString();
            text_remark.Text = recharge.Remark;
        }

        private void BackToList(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


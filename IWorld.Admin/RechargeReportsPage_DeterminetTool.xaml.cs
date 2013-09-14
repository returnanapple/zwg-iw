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
    public partial class RechargeReportsPage_DeterminetTool : ChildWindow
    {
        public bool ShowError { get; set; }
        public string Error { get; set; }
        RechargeResult Recharge { get; set; }

        public RechargeReportsPage_DeterminetTool(RechargeResult recharge)
        {
            InitializeComponent();
            this.ShowError = false;
            this.Error = "";
            this.Recharge = recharge;
        }

        private void Determinet(object sender, EventArgs e)
        {
            DataReportServiceClient client = new DataReportServiceClient();
            client.DeterminetRechargeCompleted += ShowDeterminetResult;
            client.DeterminetRechargeAsync(this.Recharge.RechargeId, input_card.Text, input_holder.Text
                , Bank.中国工商银行, App.Token);
        }
        #region 确认
        void ShowDeterminetResult(object sender, DeterminetRechargeCompletedEventArgs e)
        {
            if (!e.Result.Success)
            {
                this.ShowError = true;
                this.Error = e.Result.Error;
            }
            this.DialogResult = true;
        }
        #endregion

        private void BackToList(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


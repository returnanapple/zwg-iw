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
    public partial class RechargeReportsPage_NegativeTool : ChildWindow
    {
        public bool ShowError { get; set; }
        public string Error { get; set; }
        RechargeResult Recharge { get; set; }

        public RechargeReportsPage_NegativeTool(RechargeResult recharge)
        {
            InitializeComponent();
            this.ShowError = false;
            this.Error = "";
            this.Recharge = recharge;
        }

        private void Negative(object sender, EventArgs e)
        {
            DataReportServiceClient client = new DataReportServiceClient();
            client.NegativeRechargeCompleted += ShowNegativeResult;
            client.NegativeRechargeAsync(this.Recharge.RechargeId, input_remark.Text, App.Token);
        }
        #region 确认
        void ShowNegativeResult(object sender, NegativeRechargeCompletedEventArgs e)
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


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
    public partial class TransferReportsPage_CreateTool : ChildWindow
    {
        public bool ShowError { get; set; }
        public string Error { get; set; }

        public TransferReportsPage_CreateTool()
        {
            InitializeComponent();
            this.ShowError = false;
            this.Error = "";
        }

        private void Create(object sender, EventArgs e)
        {
            DataReportServiceClient client = new DataReportServiceClient();
            client.AddTransferCompleted += ShowCreateResult;
            client.AddTransferAsync(Convert.ToDouble(input_sum.Text), input_remark.Text, App.Token);
        }
        #region 添加
        void ShowCreateResult(object sender, AddTransferCompletedEventArgs e)
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


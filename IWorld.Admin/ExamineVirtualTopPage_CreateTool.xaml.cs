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
using IWorld.Admin.LotteryTicketService;

namespace IWorld.Admin
{
    public partial class ExamineVirtualTopPage_CreateTool : ChildWindow
    {
        public bool ShowError { get; set; }

        public string Error { get; set; }

        public ExamineVirtualTopPage_CreateTool()
        {
            InitializeComponent();
        }

        private void Create(object sender, EventArgs e)
        {
            AddVirtualTopImport import = new AddVirtualTopImport
            {
                Ticket = input_ticket.Text,
                Sum = Convert.ToDouble(input_sum.Text)
            };
            LotteryTicketServiceClient client = new LotteryTicketServiceClient();
            client.AddVirtualTopCompleted += ShowCreateResult;
            client.AddVirtualTopAsync(import, App.Token);
        }
        #region 创建
        void ShowCreateResult(object sender, AddVirtualTopCompletedEventArgs e)
        {
            if (!e.Result.Success)
            {
                this.ShowError = true;
                this.Error = e.Result.Error;
            }
            this.DialogResult = true;
        }
        #endregion

        private void BackToListPage(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


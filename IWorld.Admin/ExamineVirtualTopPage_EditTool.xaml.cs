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
using IWorld.Admin.Class;
using IWorld.Admin.LotteryTicketService;

namespace IWorld.Admin
{
    public partial class ExamineVirtualTopPage_EditTool : ChildWindow
    {
        public bool ShowError { get; set; }

        public string Error { get; set; }

        VirtualTopResult VirtualTop { get; set; }

        public ExamineVirtualTopPage_EditTool(VirtualTopResult virtualTop)
        {
            InitializeComponent();
            this.VirtualTop = virtualTop;
            this.Error = "";
            this.ShowError = false;

            input_ticket.Text = virtualTop.Ticket;
            input_sum.Text = virtualTop.Sum.ToString();
        }

        private void Edit(object sender, EventArgs e)
        {
            EditVirtualTopImport import = new EditVirtualTopImport
            {
                VirtualTopId = this.VirtualTop.VirtualTopId,
                Ticket = input_ticket.Text,
                Sum = Convert.ToDouble(input_sum.Text)
            };
            LotteryTicketServiceClient client = new LotteryTicketServiceClient();
            client.EditVirtualTopCompleted += ShowEditResult;
            client.EditVirtualTopAsync(import, App.Token);
        }
        #region 修改
        void ShowEditResult(object sender, EditVirtualTopCompletedEventArgs e)
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


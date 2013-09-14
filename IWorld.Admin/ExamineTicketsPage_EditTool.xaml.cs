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
    public partial class ExamineTicketsPage_EditTool : ChildWindow
    {
        public bool ShowError { get; set; }

        public string Error { get; set; }

        TicketResult Ticket { get; set; }

        public ExamineTicketsPage_EditTool(TicketResult ticket)
        {
            InitializeComponent();
            this.Ticket = ticket;
            this.Error = "";
            this.ShowError = false;

            input_name.Text = ticket.Name;
            input_order.Text = ticket.Order.ToString();
        }

        private void Edit(object sender, EventArgs e)
        {
            EditTicketImport import = new EditTicketImport
            {
                TicketId = this.Ticket.TicketId,
                Name = input_name.Text,
                Order = Convert.ToInt32(input_order.Text)
            };
            LotteryTicketServiceClient client = new LotteryTicketServiceClient();
            client.EdotTicketCompleted += ShowEditResult;
            client.EdotTicketAsync(import, App.Token);
        }
        #region 修改
        void ShowEditResult(object sender, EdotTicketCompletedEventArgs e)
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


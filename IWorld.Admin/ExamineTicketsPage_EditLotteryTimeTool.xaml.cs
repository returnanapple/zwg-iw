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
    public partial class ExamineTicketsPage_EditLotteryTimeTool : ChildWindow
    {
        public bool ShowError { get; set; }

        public string Error { get; set; }

        TicketResult Ticket { get; set; }

        public ExamineTicketsPage_EditLotteryTimeTool(TicketResult ticket)
        {
            InitializeComponent();
            Ticket = ticket;
            body.RowDefinitions.Clear();
            body.Children.Clear();
            for (int i = 0; i < ticket.LotteryTimes.Count; i++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(30);
                body.RowDefinitions.Add(rd);

                TextBlock tb = new TextBlock();
                tb.Text = string.Format("第 {0} 期", ticket.LotteryTimes[i].Phases);
                tb.Margin = new Thickness(10, 0, 0, 0);
                tb.SetValue(Grid.RowProperty, i);
                body.Children.Add(tb);

                TextBox box = new TextBox();
                box.Name = string.Format("input_timeValue_{0}", i);
                box.Text = ticket.LotteryTimes[i].TimeValue;
                box.Width = 120;
                box.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                box.SetValue(Grid.RowProperty, i);
                body.Children.Add(box);
            }
        }

        private void BackToList(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }

        private void Edit(object sender, EventArgs e)
        {
            List<EditLotteryTimeImport> imports = new List<EditLotteryTimeImport>();
            for (int i = 0; i < Ticket.LotteryTimes.Count; i++)
            {
                string name = string.Format("input_timeValue_{0}", i);
                TextBox box = (TextBox)FindName(name);
                EditLotteryTimeImport import = new EditLotteryTimeImport
                {
                    Phases = Ticket.LotteryTimes[i].Phases,
                    TimeValue = box.Text
                };
                imports.Add(import);
            }
            LotteryTicketServiceClient client = new LotteryTicketServiceClient();
            client.EditLotteryTimeCompleted += ShowEditResult;
            client.EditLotteryTimeAsync(Ticket.TicketId, imports, App.Token);
        }
        #region 修改

        void ShowEditResult(object sender, EditLotteryTimeCompletedEventArgs e)
        {
            if (!e.Result.Success)
            {
                this.ShowError = true;
                this.Error = e.Result.Error;
            }
            this.DialogResult = true;
        }

        #endregion
    }
}


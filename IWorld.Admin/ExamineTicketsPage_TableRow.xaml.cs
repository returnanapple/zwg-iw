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
    public partial class ExamineTicketsPage_TableRow : UserControl, ITableToolRow
    {
        public TicketResult Ticket { get; set; }

        int _row = 0;

        public ExamineTicketsPage_TableRow(TicketResult ticket, int row)
        {
            InitializeComponent();
            this._row = row;
            this.Ticket = ticket;

            text_name.Text = ticket.Name;
            text_phases.Text = ticket.Phases;
            text_lottery.Text = ticket.Lottery;
            button_tags.Text = ticket.CountOfPlayTag.ToString();
            button_plays.Text = ticket.CountOfHowToPlay.ToString();
            text_order.Text = ticket.Order.ToString();
            button_hide.Text = ticket.Hide ? "显示" : "隐藏";
        }

        #region 事件

        public event NDelegate ViewTagsEventHandler;

        public event NDelegate ViewPlaysEventHandler;

        public event NDelegate RefreshEventHandler;

        public event TableToolBodyHoverDelegate HoverEventHandler;

        public event TableToolBodyHoverDelegate UnhoverEventHandler;

        #endregion

        private void ViewTags(object sender, MouseButtonEventArgs e)
        {
            if (ViewTagsEventHandler != null)
            {
                ViewTagsEventHandler(this, new EventArgs());
            }
        }

        private void ViewPlays(object sender, MouseButtonEventArgs e)
        {
            if (ViewPlaysEventHandler != null)
            {
                ViewPlaysEventHandler(this, new EventArgs());
            }
        }

        private void Hide(object sender, MouseButtonEventArgs e)
        {
            LotteryTicketServiceClient client = new LotteryTicketServiceClient();
            if (this.Ticket.Hide)
            {
                client.ShowTicketCompleted += (_sender, _e) =>
                    {
                        if (_e.Result.Success)
                        {
                            if (RefreshEventHandler != null)
                            {
                                RefreshEventHandler(this, new EventArgs());
                            }
                        }
                        else
                        {
                            ErrorPrompt ep = new ErrorPrompt(_e.Result.Error);
                            ep.Show();
                        }
                    };
                client.ShowTicketAsync(this.Ticket.TicketId, App.Token);
            }
            else
            {
                client.HideTicketCompleted += (_sender, _e) =>
                    {
                        if (_e.Result.Success)
                        {
                            if (RefreshEventHandler != null)
                            {
                                RefreshEventHandler(this, new EventArgs());
                            }
                        }
                        else
                        {
                            ErrorPrompt ep = new ErrorPrompt(_e.Result.Error);
                            ep.Show();
                        }
                    };
                client.HideTicketAsync(this.Ticket.TicketId, App.Token);
            }
        }

        private void Edit(object sender, MouseButtonEventArgs e)
        {
            ExamineTicketsPage_EditTool et = new ExamineTicketsPage_EditTool(this.Ticket);
            et.Closed += ShowEditResult;
            et.Show();
        }
        #region 修改
        void ShowEditResult(object sender, EventArgs e)
        {
            ExamineTicketsPage_EditTool et = (ExamineTicketsPage_EditTool)sender;
            if (et.DialogResult == true)
            {
                if (et.ShowError)
                {
                    ErrorPrompt ep = new ErrorPrompt(et.Error);
                    ep.Show();
                }
                else
                {
                    if (RefreshEventHandler != null)
                    {
                        RefreshEventHandler(this, new EventArgs());
                    }
                }
            }
        }
        #endregion

        private void EditLotteryTime(object sender, MouseButtonEventArgs e)
        {
            ExamineTicketsPage_EditLotteryTimeTool et = new ExamineTicketsPage_EditLotteryTimeTool(this.Ticket);
            et.Closed += ShowEditLotteryTimeResult;
            et.Show();
        }
        #region 修改开奖时间
        void ShowEditLotteryTimeResult(object sender, EventArgs e)
        {
            ExamineTicketsPage_EditLotteryTimeTool et = (ExamineTicketsPage_EditLotteryTimeTool)sender;
            if (et.DialogResult == true)
            {
                if (et.ShowError)
                {
                    ErrorPrompt ep = new ErrorPrompt(et.Error);
                    ep.Show();
                }
                else
                {
                    if (RefreshEventHandler != null)
                    {
                        RefreshEventHandler(this, new EventArgs());
                    }
                }
            }
        }
        #endregion

        public FrameworkElement GetElement()
        {
            return this;
        }

        public FrameworkElement GetChildWindow()
        {
            return null;
        }

        private void Hover(object sender, MouseEventArgs e)
        {
            if (HoverEventHandler != null)
            {
                HoverEventHandler(this, new TableToolBodyHoverEventArgs(this._row));
            }
        }

        private void Unhover(object sender, MouseEventArgs e)
        {
            if (UnhoverEventHandler != null)
            {
                UnhoverEventHandler(this, new TableToolBodyHoverEventArgs(this._row));
            }
        }
    }
}

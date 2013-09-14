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
    public partial class ExamineTagsPage_TableRow : UserControl, ITableToolRow
    {
        public PlayTagResult PlayTag { get; set; }

        int _row = 0;

        public ExamineTagsPage_TableRow(PlayTagResult tag, int row)
        {
            InitializeComponent();
            this.PlayTag = tag;
            this._row = row;

            text_name.Text = tag.Name;
            button_ticket.Text = tag.TicketName;
            button_plays.Text = tag.CountOfHowToPlay.ToString();
            text_order.Text = tag.Order.ToString();
            button_hide.Text = tag.Hide ? "显示" : "隐藏";
        }

        #region 事件

        public event NDelegate SelectForTicketEventHandler;

        public event NDelegate ViewPlaysEventHandler;

        public event NDelegate RefreshEventHandler;

        public event TableToolBodyHoverDelegate HoverEventHandler;

        public event TableToolBodyHoverDelegate UnhoverEventHandler;

        #endregion

        #region 鼠标事件

        private void SelectForTicket(object sender, MouseButtonEventArgs e)
        {
            if (SelectForTicketEventHandler != null)
            {
                SelectForTicketEventHandler(this, new EventArgs());
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
            if (this.PlayTag.Hide)
            {
                client.ShowPlayTagCompleted += (_sender, _e) =>
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
                client.ShowPlayTagAsync(this.PlayTag.PlayTagId, App.Token);
            }
            else
            {
                client.HidePlayTagCompleted += (_sender, _e) =>
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
                client.HidePlayTagAsync(this.PlayTag.PlayTagId, App.Token);
            }
        }

        private void Edit(object sender, MouseButtonEventArgs e)
        {
            ExamineTagsPage_EditTool et = new ExamineTagsPage_EditTool(this.PlayTag);
            et.Closed += ShowEditResult;
            et.Show();
        }
        #region 编辑
        void ShowEditResult(object sender, EventArgs e)
        {
            ExamineTagsPage_EditTool et = (ExamineTagsPage_EditTool)sender;
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

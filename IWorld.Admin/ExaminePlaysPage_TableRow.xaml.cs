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
    public partial class ExaminePlaysPage_TableRow : UserControl, ITableToolRow
    {
        public HowToPlayResult HowToPlay { get; set; }

        int _row = 0;

        public ExaminePlaysPage_TableRow(HowToPlayResult howToPlay, int row)
        {
            InitializeComponent();
            this.HowToPlay = howToPlay;
            this._row = row;

            text_name.Text = howToPlay.Name;
            button_ticket.Text = howToPlay.TicketName;
            button_tag.Text = howToPlay.TagName;
            text_cardinalNumber.Text = howToPlay.CardinalNumber.ToString();
            text_order.Text = howToPlay.Order.ToString();
            button_hide.Text = howToPlay.Hide ? "显示" : "隐藏";
        }

        #region 事件

        public event NDelegate SelectForTicketEventHandler;

        public event NDelegate SelectForTagEventHandler;

        public event NDelegate RefreshEventHandler;

        public event TableToolBodyHoverDelegate HoverEventHandler;

        public event TableToolBodyHoverDelegate UnhoverEventHandler;

        #endregion

        private void SelectForTicket(object sender, MouseButtonEventArgs e)
        {
            if (SelectForTicketEventHandler != null)
            {
                SelectForTicketEventHandler(this, null);
            }
        }

        private void SelectForTag(object sender, MouseButtonEventArgs e)
        {
            if (SelectForTagEventHandler != null)
            {
                SelectForTagEventHandler(this, null);
            }
        }

        void Edit(object sender, EventArgs e)
        {
            ExaminePlaysPage_EditTool et = new ExaminePlaysPage_EditTool(this.HowToPlay);
            et.Closed += ShowEditResult;
            et.Show();
        }
        #region 编辑
        void ShowEditResult(object sender, EventArgs e)
        {
            ExaminePlaysPage_EditTool et = (ExaminePlaysPage_EditTool)sender;
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

        void Hide(object sender, EventArgs e)
        {
            LotteryTicketServiceClient client = new LotteryTicketServiceClient();
            if (this.HowToPlay.Hide)
            {
                client.ShowHowToPlayCompleted += (_sender, _e) =>
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
                client.ShowHowToPlayAsync(this.HowToPlay.HowToPlayId, App.Token);
            }
            else
            {
                client.HideHowToPlayCompleted += (_sender, _e) =>
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
                client.HideHowToPlayAsync(this.HowToPlay.HowToPlayId, App.Token);
            }
        }

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

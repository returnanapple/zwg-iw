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
using IWorld.Admin.DataReportService;

namespace IWorld.Admin
{
    public partial class BettingReportsPage_TableRow : UserControl, ITableToolRow
    {
        public BettingResult Betting { get; set; }
        int _row = 0;

        public BettingReportsPage_TableRow(BettingResult betting, int row)
        {
            InitializeComponent();
            this.Betting = betting;
            this._row = row;

            button_owner.Text = betting.Owner;
            button_ticket.Text = betting.Ticket;
            button_tag.Text = betting.Tag;
            button_howToPlay.Text = betting.HowToPlay;
            text_pay.Text = betting.Pay.ToString("0.00");
            text_bonus.Text = betting.Bonus.ToString("0.00");
            button_cheat.Text = betting.Cheat ? "取消作弊" : "作弊";
            List<BettingStatus> showStatus = new List<BettingStatus> { BettingStatus.等待开奖, BettingStatus.即将开奖 };
            if (!showStatus.Contains(betting.Status))
            {
                button_cheat.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        #region 事件

        public event NDelegate SelectForOwnerEventHandler;

        public event NDelegate SelectForTicketEventHandler;

        public event NDelegate SelectForTagEventHandler;

        public event NDelegate SelectForHowToPlayEventHandler;

        public event TableToolBodyHoverDelegate HoverEventHandler;

        public event TableToolBodyHoverDelegate UnhoverEventHandler;

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

        private void SelectForOwner(object sender, MouseButtonEventArgs e)
        {
            if (SelectForOwnerEventHandler != null)
            {
                SelectForOwnerEventHandler(this, new EventArgs());
            }
        }

        private void SelectForTicket(object sender, MouseButtonEventArgs e)
        {
            if (SelectForTicketEventHandler != null)
            {
                SelectForTicketEventHandler(this, new EventArgs());
            }
        }

        private void SelectForTag(object sender, MouseButtonEventArgs e)
        {
            if (SelectForTagEventHandler != null)
            {
                SelectForTagEventHandler(this, new EventArgs());
            }
        }

        private void SelectForHowToPlay(object sender, MouseButtonEventArgs e)
        {
            if (SelectForHowToPlayEventHandler != null)
            {
                SelectForHowToPlayEventHandler(this, new EventArgs());
            }
        }

        private void ViewFull(object sender, MouseButtonEventArgs e)
        {
            BettingReportsPage_FullWindow fw = new BettingReportsPage_FullWindow(this.Betting);
            fw.Show();
        }

        private void SetIfCheat(object sender, MouseButtonEventArgs e)
        {
            DataReportServiceClient client = new DataReportServiceClient();
            client.SetCheatForBettingCompleted += ShowSetIfCheatResualt;
            client.SetCheatForBettingAsync(this.Betting.BettingId, !this.Betting.Cheat, App.Token);
        }

        void ShowSetIfCheatResualt(object sender, SetCheatForBettingCompletedEventArgs e)
        {
            if (!e.Result.Success)
            {
                ErrorPrompt ep = new ErrorPrompt(e.Result.Error);
                ep.Show();
                return;
            }
            Betting.Cheat = !Betting.Cheat;
            button_cheat.Text = Betting.Cheat ? "取消作弊" : "作弊";
            List<BettingStatus> showStatus = new List<BettingStatus> { BettingStatus.等待开奖, BettingStatus.即将开奖 };
            if (!showStatus.Contains(Betting.Status))
            {
                button_cheat.Visibility = System.Windows.Visibility.Collapsed;
            }
            ErrorPrompt _ep = new ErrorPrompt("操作成功");
            _ep.Show();
        }
    }
}

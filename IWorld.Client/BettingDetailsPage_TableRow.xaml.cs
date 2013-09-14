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
using IWorld.Client.Class;
using IWorld.Client.GamingService;

namespace IWorld.Client
{
    public partial class BettingDetailsPage_TableRow : UserControl, ITableToolRow
    {
        BettingDetailsResult Result { get; set; }
        int _row = 0;
        public BettingDetailsPage_TableRow(BettingDetailsResult result, int row)
        {
            InitializeComponent();
            this._row = row;
            this.Result = result;

            text_owner.Text = result.Owner;
            text_ticket.Text = result.Ticket;
            text_howToPlay.Text = result.HowToPlay;
            text_phases.Text = result.Phases;
            text_values.Text = result.Values;
            text_time.Text = result.Time.ToShortDateString();
            text_status.Text = result.Status.ToString();
            if (result.Status == BettingStatus.未中奖 || result.Status == BettingStatus.中奖)
            {
                text_profit.Text = (result.Bonus - result.Pay).ToString("0.00");
                if (result.Bonus - result.Pay < 0)
                {
                    text_profit.Foreground = new SolidColorBrush(Colors.Green);
                }
                else if (result.Bonus - result.Pay > 0)
                {
                    text_profit.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                text_profit.Text = "0";
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

        public event NDelegate RefreshEventHandler;

        public event TableToolBodyHoverDelegate HoverEventHandler;

        public event TableToolBodyHoverDelegate UnhoverEventHandler;

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

        private void ViewFull(object sender, MouseButtonEventArgs e)
        {
            BettingDetailsPage_FullWindow fw = new BettingDetailsPage_FullWindow(this.Result);
            fw.Closed += Remove;
            fw.Show();
        }
        #region 撤单

        void Remove(object sender, EventArgs e)
        {
            BettingDetailsPage_FullWindow fw = (BettingDetailsPage_FullWindow)sender;
            if (fw.DialogResult == true)
            {
                NormalPromt np = new NormalPromt(string.Format("你确定要撤除订单号为 {0} 的投注吗？", Result.BettingId));
                np.Closed += Remove_do;
                np.Show();
            }
        }

        void Remove_do(object sender, EventArgs e)
        {
            NormalPromt np = (NormalPromt)sender;
            if (np.DialogResult == true)
            {
                GamingServiceClient client = new GamingServiceClient();
                client.RemoveBettingCompleted += ShowRemoveResult;
                client.RemoveBettingAsync(this.Result.BettingId, App.Token);
            }

        }

        void ShowRemoveResult(object sender, RemoveBettingCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                if (RefreshEventHandler != null)
                {
                    RefreshEventHandler(this, new EventArgs());
                }
            }
            else
            {
                ErrorPromt ep = new ErrorPromt(e.Result.Error);
                ep.Show();
            }
        }

        #endregion
    }
}

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
    public partial class ExamineLotteryPage_TableRow : UserControl, ITableToolRow
    {
        public LotteryResult Lottery { get; set; }

        int _row = 0;

        public ExamineLotteryPage_TableRow(LotteryResult lottery, int row)
        {
            InitializeComponent();
            this.Lottery = lottery;
            this._row = row;

            button_ticket.Text = lottery.TicketName;
            text_issue.Text = lottery.Phases;
            text_value.Text = lottery.Seats;
            text_time.Text = string.Format("{0} {1}", lottery.Time.ToShortDateString(), lottery.Time.ToShortTimeString());
        }

        public FrameworkElement GetElement()
        {
            return this;
        }

        public FrameworkElement GetChildWindow()
        {
            return null;
        }

        private void SelectForTicket(object sender, MouseButtonEventArgs e)
        {
            if (SelectForTicketEventHandler != null)
            {
                SelectForTicketEventHandler(this, null);
            }
        }

        public event NDelegate SelectForTicketEventHandler;

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
    }
}

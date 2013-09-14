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
using IWorld.Admin.DataReportService;

namespace IWorld.Admin
{
    public partial class BettingReportsPage_FullWindow : ChildWindow
    {
        public BettingReportsPage_FullWindow(BettingResult betting)
        {
            InitializeComponent();
            text_Id.Text = betting.BettingId.ToString();
            text_Owner.Text = betting.Owner;
            text_Phases.Text = betting.Phases;
            text_Sum.Text = betting.Sum.ToString();
            text_Multiple.Text = betting.Multiple.ToString("0,00");
            text_Points.Text = betting.Points.ToString("0.00");
            text_Ticket.Text = betting.Ticket;
            text_Tag.Text = betting.Tag;
            text_HowToPlay.Text = betting.HowToPlay;
            text_Seats.Text = betting.Seats;
            text_Status.Text = betting.Status.ToString();
            text_Pay.Text = betting.Pay.ToString("0.00");
            text_Bonus.Text = betting.Bonus.ToString("0.00");
        }

        private void BackToList(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


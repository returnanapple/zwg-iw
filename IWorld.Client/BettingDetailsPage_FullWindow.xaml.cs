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
using IWorld.Client.GamingService;

namespace IWorld.Client
{
    public partial class BettingDetailsPage_FullWindow : ChildWindow
    {
        public BettingDetailsPage_FullWindow(BettingDetailsResult result)
        {
            InitializeComponent();

            text_values.Text = result.Values;
            text_ticket.Text = result.Ticket;
            text_playTag.Text = result.PlayTag;
            text_howToPlay.Text = result.HowToPlay;
            text_phases.Text = result.Phases;
            text_points.Text = result.RetutnPoints;
            text_sum.Text = result.Sum.ToString();
            text_multiple.Text = result.Multiple.ToString();
            text_id.Text = result.BettingId.ToString("000000"); ;
            text_owner.Text = result.Owner;
            text_time.Text = result.Time.ToLongDateString();
            text_price.Text = App.Websetting.UnitPrice.ToString("0.00"); ;
            text_money.Text = result.Pay.ToString("0.00");
            text_bonus.Text = result.Bonus.ToString("0.00");
            text_profit.Text = "0.00";
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

            if (result.Status != BettingStatus.等待开奖)
            {
                button_r.Visibility = System.Windows.Visibility.Collapsed;
                button_b.SetValue(Canvas.TopProperty, 20.0);
            }
            List<string> tName = new List<string> { "福彩3D", "排列三", "上海时时乐" };
            if (tName.Contains(result.Ticket))
            {
                ball_blue.Visibility = System.Windows.Visibility.Collapsed;
                ball_purple.Visibility = System.Windows.Visibility.Collapsed;
                text_status.SetValue(Canvas.LeftProperty, 240.0);
            }
            if (result.LotteryValues != "")
            {
                List<string> t = result.LotteryValues.Split(new char[] { ',' }).ToList();
                List<Ball> balls = new List<Ball> { ball_red, ball_yellow, ball_green, ball_blue, ball_purple };
                for (int i = 0; i < t.Count; i++)
                {
                    balls[i].Text = t[i];
                }
            }
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


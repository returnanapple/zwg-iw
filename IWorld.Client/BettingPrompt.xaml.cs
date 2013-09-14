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

namespace IWorld.Client
{
    public partial class BettingPrompt : ChildWindow
    {
        public BettingPrompt()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #region 依赖属性

        public string _Values
        {
            get { return (string)GetValue(_ValuesProperty); }
            set { SetValue(_ValuesProperty, value); }
        }

        public static readonly DependencyProperty _ValuesProperty =
            DependencyProperty.Register("_Values", typeof(string), typeof(BettingPrompt)
            , new PropertyMetadata("", (d, e) =>
                {
                    BettingPrompt bp = (BettingPrompt)d;
                    bp.text_values.Text = e.NewValue.ToString();
                }));



        public string _Ticket
        {
            get { return (string)GetValue(_TicketProperty); }
            set { SetValue(_TicketProperty, value); }
        }

        public static readonly DependencyProperty _TicketProperty =
            DependencyProperty.Register("_Ticket", typeof(string), typeof(BettingPrompt)
            , new PropertyMetadata("", (d, e) =>
                {
                    BettingPrompt bp = (BettingPrompt)d;
                    bp.text_ticket.Text = e.NewValue.ToString();
                }));



        public string _PlayTag
        {
            get { return (string)GetValue(_PlayTagProperty); }
            set { SetValue(_PlayTagProperty, value); }
        }

        public static readonly DependencyProperty _PlayTagProperty =
            DependencyProperty.Register("_PlayTag", typeof(string), typeof(BettingPrompt)
            , new PropertyMetadata("", (d, e) =>
                {
                    BettingPrompt bp = (BettingPrompt)d;
                    bp.text_playTag.Text = e.NewValue.ToString();
                }));



        public string _HowToPlay
        {
            get { return (string)GetValue(_HowToPlayProperty); }
            set { SetValue(_HowToPlayProperty, value); }
        }

        public static readonly DependencyProperty _HowToPlayProperty =
            DependencyProperty.Register("_HowToPlay", typeof(string), typeof(BettingPrompt)
            , new PropertyMetadata("", (d, e) =>
                {
                    BettingPrompt bp = (BettingPrompt)d;
                    bp.text_howToPlay.Text = e.NewValue.ToString();
                }));



        public string _Phases
        {
            get { return (string)GetValue(_PhasesProperty); }
            set { SetValue(_PhasesProperty, value); }
        }

        public static readonly DependencyProperty _PhasesProperty =
            DependencyProperty.Register("_Phases", typeof(string), typeof(BettingPrompt)
            , new PropertyMetadata("", (d, e) =>
                {
                    BettingPrompt bp = (BettingPrompt)d;
                    bp.text_phases.Text = e.NewValue.ToString();
                }));



        public string _Odds
        {
            get { return (string)GetValue(_OddsProperty); }
            set { SetValue(_OddsProperty, value); }
        }

        public static readonly DependencyProperty _OddsProperty =
            DependencyProperty.Register("_Odds", typeof(string), typeof(BettingPrompt)
            , new PropertyMetadata("", (d, e) =>
            {
                BettingPrompt bp = (BettingPrompt)d;
                bp.text_odds.Text = e.NewValue.ToString();
            }));



        public string _Points
        {
            get { return (string)GetValue(_PointsProperty); }
            set { SetValue(_PointsProperty, value); }
        }

        public static readonly DependencyProperty _PointsProperty =
            DependencyProperty.Register("_Points", typeof(string), typeof(BettingPrompt)
            , new PropertyMetadata("", (d, e) =>
            {
                BettingPrompt bp = (BettingPrompt)d;
                bp.text_points.Text = e.NewValue.ToString();
            }));



        public string _Price
        {
            get { return (string)GetValue(_PriceProperty); }
            set { SetValue(_PriceProperty, value); }
        }

        public static readonly DependencyProperty _PriceProperty =
            DependencyProperty.Register("_Price", typeof(string), typeof(BettingPrompt)
            , new PropertyMetadata("", (d, e) =>
            {
                BettingPrompt bp = (BettingPrompt)d;
                bp.text_price.Text = e.NewValue.ToString();
            }));



        public string _Sum
        {
            get { return (string)GetValue(_SumProperty); }
            set { SetValue(_SumProperty, value); }
        }

        public static readonly DependencyProperty _SumProperty =
            DependencyProperty.Register("_Sum", typeof(string), typeof(BettingPrompt)
            , new PropertyMetadata("", (d, e) =>
            {
                BettingPrompt bp = (BettingPrompt)d;
                bp.text_sum.Text = e.NewValue.ToString();
            }));



        public string _Multiple
        {
            get { return (string)GetValue(_MultipleProperty); }
            set { SetValue(_MultipleProperty, value); }
        }

        public static readonly DependencyProperty _MultipleProperty =
            DependencyProperty.Register("_Multiple", typeof(string), typeof(BettingPrompt)
            , new PropertyMetadata("", (d, e) =>
            {
                BettingPrompt bp = (BettingPrompt)d;
                bp.text_multiple.Text = e.NewValue.ToString();
            }));



        public string _Money
        {
            get { return (string)GetValue(_MoneyProperty); }
            set { SetValue(_MoneyProperty, value); }
        }

        public static readonly DependencyProperty _MoneyProperty =
            DependencyProperty.Register("_Money", typeof(string), typeof(BettingPrompt)
            , new PropertyMetadata("", (d, e) =>
            {
                BettingPrompt bp = (BettingPrompt)d;
                bp.text_money.Text = e.NewValue.ToString();
            }));

        

        #endregion
    }
}


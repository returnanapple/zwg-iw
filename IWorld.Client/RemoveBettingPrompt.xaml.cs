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
    public partial class RemoveBettingPrompt : ChildWindow
    {
        public RemoveBettingPrompt()
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



        public string _Id
        {
            get { return (string)GetValue(_IdProperty); }
            set { SetValue(_IdProperty, value); }
        }

        public static readonly DependencyProperty _IdProperty =
            DependencyProperty.Register("_Id", typeof(string), typeof(RemoveBettingPrompt)
            , new PropertyMetadata("", (d, e) =>
            {
                RemoveBettingPrompt bp = (RemoveBettingPrompt)d;
                bp.text_id.Text = e.NewValue.ToString();
            }));

        

        public string _Values
        {
            get { return (string)GetValue(_ValuesProperty); }
            set { SetValue(_ValuesProperty, value); }
        }

        public static readonly DependencyProperty _ValuesProperty =
            DependencyProperty.Register("_Values", typeof(string), typeof(RemoveBettingPrompt)
            , new PropertyMetadata("", (d, e) =>
            {
                RemoveBettingPrompt bp = (RemoveBettingPrompt)d;
                bp.text_values.Text = e.NewValue.ToString();
            }));



        public string _Ticket
        {
            get { return (string)GetValue(_TicketProperty); }
            set { SetValue(_TicketProperty, value); }
        }

        public static readonly DependencyProperty _TicketProperty =
            DependencyProperty.Register("_Ticket", typeof(string), typeof(RemoveBettingPrompt)
            , new PropertyMetadata("", (d, e) =>
            {
                RemoveBettingPrompt bp = (RemoveBettingPrompt)d;
                bp.text_ticket.Text = e.NewValue.ToString();
            }));



        public string _PlayTag
        {
            get { return (string)GetValue(_PlayTagProperty); }
            set { SetValue(_PlayTagProperty, value); }
        }

        public static readonly DependencyProperty _PlayTagProperty =
            DependencyProperty.Register("_PlayTag", typeof(string), typeof(RemoveBettingPrompt)
            , new PropertyMetadata("", (d, e) =>
            {
                RemoveBettingPrompt bp = (RemoveBettingPrompt)d;
                bp.text_playTag.Text = e.NewValue.ToString();
            }));



        public string _HowToPlay
        {
            get { return (string)GetValue(_HowToPlayProperty); }
            set { SetValue(_HowToPlayProperty, value); }
        }

        public static readonly DependencyProperty _HowToPlayProperty =
            DependencyProperty.Register("_HowToPlay", typeof(string), typeof(RemoveBettingPrompt)
            , new PropertyMetadata("", (d, e) =>
            {
                RemoveBettingPrompt bp = (RemoveBettingPrompt)d;
                bp.text_howToPlay.Text = e.NewValue.ToString();
            }));



        public string _Money
        {
            get { return (string)GetValue(_MoneyProperty); }
            set { SetValue(_MoneyProperty, value); }
        }

        public static readonly DependencyProperty _MoneyProperty =
            DependencyProperty.Register("_Money", typeof(string), typeof(RemoveBettingPrompt)
            , new PropertyMetadata("", (d, e) =>
            {
                RemoveBettingPrompt bp = (RemoveBettingPrompt)d;
                bp.text_money.Text = e.NewValue.ToString();
            }));



        #endregion
    }
}


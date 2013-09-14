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
using IWorld.Client.ActivityService;

namespace IWorld.Client
{
    public partial class ExchangesPage_ExchangeTool : ChildWindow
    {
        ExchangeActivitiesResult Exchange { get; set; }
        public bool ShowError { get; set; }
        public string Error { get; set; }

        public ExchangesPage_ExchangeTool(ExchangeActivitiesResult exchange)
        {
            InitializeComponent();
            this.Exchange = exchange;
            this.ShowError = false;
            this.Error = "";

            text_title.Text = exchange.Name;
        }

        private void Enter(object sender, RoutedEventArgs e)
        {
            ActivityServiceClient client = new ActivityServiceClient();
            client.ExchangeCompleted += ShowExchangeResult;
            client.ExchangeAsync(this.Exchange.ExchangeId, Convert.ToInt32(input_sum.Text), App.Token);
        }
        #region 兑换

        void ShowExchangeResult(object sender, ExchangeCompletedEventArgs e)
        {
            if (!e.Result.Success)
            {
                this.ShowError = true;
                this.Error = e.Result.Error;
            }
            this.DialogResult = true;
        }

        #endregion

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


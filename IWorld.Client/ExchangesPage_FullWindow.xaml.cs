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
    public partial class ExchangesPage_FullWindow : ChildWindow
    {
        public ExchangesPage_FullWindow(ExchangeActivitiesResult exchange)
        {
            InitializeComponent();

            text_name.Text = exchange.Name;
            text_UnitPrice.Text = exchange.UnitPrice.ToString();
            text_pName.Text = exchange.Prizes.First().Name;
            text_pPrice.Text = exchange.Prizes.First().Price.ToString();
        }

        private void Enter(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


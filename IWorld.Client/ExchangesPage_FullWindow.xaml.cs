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
            text_places.Text = exchange.Places.ToString();
            text_UnitPrice.Text = exchange.Places.ToString();
            text_EachPersonCanExchangeTheNumberOfTimes.Text = exchange.EachPersonCanExchangeTheNumberOfTimes.ToString();
            text_EachPersonCanExchangeTheTimesOfDays.Text = exchange.EachPersonCanExchangeTheTimesOfDays.ToString();
            text_EachPersonCanExchangeTheNumberOfDays.Text = exchange.EachPersonCanExchangeTheNumberOfDays.ToString();
            text_EachPersonCanExchangeTheTimesOfAll.Text = exchange.EachPersonCanExchangeTheTimesOfAll.ToString();
            text_EachPersonCanExchangeTheNumberOfAll.Text = exchange.EachPersonCanExchangeTheNumberOfAll.ToString();
            text_beginTime.Text = exchange.BeginTime.ToLongDateString();
            text_endTime.Text = exchange.EndTime.ToLongDateString();

            double tHeight = root.Height;
            double tTitleTop = (double)title_showPrize.GetValue(Canvas.TopProperty);
            double tToolTop = (double)tool_showPrize.GetValue(Canvas.TopProperty);
            double rowHeight = 30;
            if (exchange.Conditions.Count > 1)
            {
                double height = (exchange.Conditions.Count - 1) * rowHeight;
                root.Height = tHeight + height;
                title_showPrize.SetValue(Canvas.TopProperty, tTitleTop + height);
                tool_showPrize.SetValue(Canvas.TopProperty, tToolTop + height);
            }

            tool_showConditions.RowDefinitions.Clear();
            tool_showConditions.Children.Clear();

            int t = 0;
            exchange.Conditions.ForEach(x =>
                {
                    RowDefinition rd = new RowDefinition();
                    rd.Height = new GridLength(rowHeight);
                    tool_showConditions.RowDefinitions.Add(rd);

                    TextBlock tb1 = new TextBlock();
                    tb1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    tb1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb1.SetValue(Grid.ColumnProperty, 0);
                    tb1.SetValue(Grid.RowProperty, t);
                    tb1.Text = x.Type.ToString();
                    tool_showConditions.Children.Add(tb1);

                    TextBlock tb2 = new TextBlock();
                    tb2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    tb2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb2.SetValue(Grid.ColumnProperty, 1);
                    tb2.SetValue(Grid.RowProperty, t);
                    tb2.Text = x.Limit.ToString();
                    tool_showConditions.Children.Add(tb2);

                    TextBlock tb3 = new TextBlock();
                    tb3.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    tb3.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb3.SetValue(Grid.ColumnProperty, 2);
                    tb3.SetValue(Grid.RowProperty, t);
                    tb3.Text = x.Upper.ToString();
                    tool_showConditions.Children.Add(tb3);

                    t++;
                });

            t = 0;
            exchange.Prizes.ForEach(x =>
                {
                    RowDefinition rd = new RowDefinition();
                    rd.Height = new GridLength(rowHeight);
                    tool_showConditions.RowDefinitions.Add(rd);

                    TextBlock tb1 = new TextBlock();
                    tb1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    tb1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb1.SetValue(Grid.ColumnProperty, 0);
                    tb1.SetValue(Grid.RowProperty, t);
                    tb1.Text = x.Name;
                    tool_showPrize.Children.Add(tb1);

                    TextBlock tb2 = new TextBlock();
                    tb2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    tb2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb2.SetValue(Grid.ColumnProperty, 1);
                    tb2.SetValue(Grid.RowProperty, t);
                    tb2.Text = x.Price.ToString();
                    tool_showPrize.Children.Add(tb2);

                    t++;
                });
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


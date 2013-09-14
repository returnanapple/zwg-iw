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
using IWorld.Admin.ExchangeService;

namespace IWorld.Admin
{
    public partial class ExamineExchangesPage_FullWindow : ChildWindow
    {
        public ExamineExchangesPage_FullWindow(ExchangeResult exchange)
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
            text_days.Text = exchange.Days.ToString(); ;
            text_endTime.Text = exchange.EndTime.ToLongDateString();
            text_hide.Text = exchange.Hide ? "是" : "否";
            text_autoDelete.Text = exchange.AutoDelete ? "是" : "否";

            double rowHeight = 30;

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

            tool_showPrize.RowDefinitions.Clear();
            tool_showPrize.Children.Clear();

            t = 0;
            exchange.Prizes.ForEach(x =>
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(rowHeight);
                tool_showPrize.RowDefinitions.Add(rd);

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

            root.MouseWheel += VerticalScroll;
        }
        #region 滚屏

        /// <summary>
        /// 竖向滚屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void VerticalScroll(object sender, MouseWheelEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            UIHelper.VerticalScroll(fe, e);
        }

        #endregion

        private void BackToList(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


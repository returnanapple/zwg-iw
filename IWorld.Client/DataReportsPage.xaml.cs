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
using IWorld.Client.DataReportService;

namespace IWorld.Client
{
    public partial class DataReportsPage : UserControl
    {
        ReportsType type = ReportsType.个人;
        string beginTime = "";
        string endTime = "";
        int pageIndex = 1;

        public DataReportsPage()
        {
            InitializeComponent();
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("用户", 80));
            columns.Add(new TableToolColumnImport("余额", 80));
            columns.Add(new TableToolColumnImport("充值", 80));
            columns.Add(new TableToolColumnImport("提现", 80));
            columns.Add(new TableToolColumnImport("投注", 80));
            columns.Add(new TableToolColumnImport("中奖", 80));
            columns.Add(new TableToolColumnImport("返点", 80));
            columns.Add(new TableToolColumnImport("活动返还", 80));
            columns.Add(new TableToolColumnImport("盈亏", 80));
            List<ITableToolRow> rows = new List<Class.ITableToolRow>();

            DataReportServiceClient client = new DataReportServiceClient();
            client.GetReportsCompleted += (sender, e) =>
            {
                int t = 0;
                e.Result.Content.ForEach(x =>
                {
                    DataReportsPage_TableRow row = new DataReportsPage_TableRow(x, t);
                    rows.Add(row);
                    t++;
                });

                TableTool tt = new TableTool("", e.Result.PageIndex, e.Result.TotalOfPage, columns, rows);
                tt.NextPageEventHandler += GoToNextPage;
                tableBody.Children.Clear();
                tableBody.Children.Add(tt);
            };
            client.GetReportsAsync(beginTime, endTime, type, pageIndex, App.Token);
        }

        void GoToNextPage(object sender, NextPageEventArgs e)
        {
            pageIndex = e.To;
            InsertTable();
        }

        private void SelectForType(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            TextBlock tb = (TextBlock)cb.SelectedItem;
            type = (ReportsType)Enum.Parse(typeof(ReportsType), tb.Text, false);
            pageIndex = 1;
            InsertTable();
        }

        private void SelectForTime(object sender, RoutedEventArgs e)
        {
            beginTime = input_beginTime.Text;
            endTime = input_endTime.Text;
            pageIndex = 1;
            InsertTable();
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            input_type.SelectedIndex = 0;

            type = ReportsType.个人;
            beginTime = "";
            endTime = "";
            pageIndex = 1;
            InsertTable();
        }
    }
}

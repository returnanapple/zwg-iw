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
    public partial class ExchangesPage : UserControl
    {
        int pageIndex = 1;

        public ExchangesPage()
        {
            InitializeComponent();
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("标题", 240));
            columns.Add(new TableToolColumnImport("名额", 120));
            columns.Add(new TableToolColumnImport("兑换单价", 120));
            columns.Add(new TableToolColumnImport("开始时间", 120));
            columns.Add(new TableToolColumnImport("结束时间", 120));
            List<ITableToolRow> rows = new List<Class.ITableToolRow>();

            ActivityServiceClient client = new ActivityServiceClient();
            client.GetExchangeActivitiesCompleted += (sender, e) =>
            {
                int t = 0;
                e.Result.Content.ForEach(x =>
                {
                    ExchangesPage_TableRow row = new ExchangesPage_TableRow(x, t);
                    row.RefreshEventHandler += Refresh;
                    rows.Add(row);
                    t++;
                });

                TableTool tt = new TableTool("", e.Result.PageIndex, e.Result.TotalOfPage, columns, rows);
                tt.NextPageEventHandler += GoToNextPage;
                tableBody.Children.Clear();
                tableBody.Children.Add(tt);
            };
            client.GetExchangeActivitiesAsync(pageIndex, App.Token);
        }

        void Refresh(object sender, EventArgs e)
        {
            InsertTable();
        }

        void GoToNextPage(object sender, NextPageEventArgs e)
        {
            pageIndex = e.To;
            InsertTable();
        }
    }
}

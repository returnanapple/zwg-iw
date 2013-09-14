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
using IWorld.Client.GamingService;

namespace IWorld.Client
{
    public partial class BettingDetailsPage : UserControl
    {
        BettingDetailsSelectType selectType = BettingDetailsSelectType.个人;
        string beginTime = "";
        string endTime = "";
        int pageIndex = 1;

        public BettingDetailsPage()
        {
            InitializeComponent();
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("用户", 90));
            columns.Add(new TableToolColumnImport("彩种", 80));
            columns.Add(new TableToolColumnImport("玩法", 70));
            columns.Add(new TableToolColumnImport("期号", 90));
            columns.Add(new TableToolColumnImport("投注号码", 150));
            columns.Add(new TableToolColumnImport("状态", 70));
            columns.Add(new TableToolColumnImport("盈亏", 80));
            columns.Add(new TableToolColumnImport("投注时间", 90));
            List<ITableToolRow> rows = new List<Class.ITableToolRow>();

            GamingServiceClient client = new GamingServiceClient();
            client.GetBettingDetailsCompleted += (sender, e) =>
                {
                    int t = 0;
                    e.Result.Content.ForEach(x =>
                    {
                        BettingDetailsPage_TableRow row = new BettingDetailsPage_TableRow(x, t);
                        row.RefreshEventHandler += Refresh;
                        rows.Add(row);
                        t++;
                    });

                    TableTool tt = new TableTool("", e.Result.PageIndex, e.Result.TotalOfPage, columns, rows);
                    tt.NextPageEventHandler += GoToNextPage;
                    tableBody.Children.Clear();
                    tableBody.Children.Add(tt);
                };
            client.GetBettingDetailsAsync(selectType, beginTime, endTime, pageIndex, App.Token);
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

        private void SelectForTime(object sender, RoutedEventArgs e)
        {
            beginTime = input_beginTime.Text;
            endTime = input_endTime.Text;
            pageIndex = 1;
            InsertTable();
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            input_beginTime.Text = "";
            input_endTime.Text = "";

            selectType = BettingDetailsSelectType.个人;
            beginTime = "";
            endTime = "";
            pageIndex = 1;
            InsertTable();
        }

        private void SelectForSelectType(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            TextBlock tb = (TextBlock)cb.SelectedItem;
            selectType = (BettingDetailsSelectType)Enum.Parse(typeof(BettingDetailsSelectType), tb.Text, false);
            pageIndex = 1;
            InsertTable();
        }
    }
}

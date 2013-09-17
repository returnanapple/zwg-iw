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
using IWorld.Admin.DataReportService;

namespace IWorld.Admin
{
    public partial class BettingReportsPage : UserControl
    {
        string beginTime = "";
        string endTime = "";
        int ownerId = 0;
        int ticketId = 0;
        int tagId = 0;
        int howToPlatId = 0;
        BettingStatusSelectType status = BettingStatusSelectType.全部;
        int pageIndex = 1;
        string username = "";

        public BettingReportsPage()
        {
            InitializeComponent();
            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("投注人", 130));
            columns.Add(new TableToolColumnImport("彩票", 130));
            columns.Add(new TableToolColumnImport("玩法标签", 130));
            columns.Add(new TableToolColumnImport("玩法", 130));
            columns.Add(new TableToolColumnImport("投注金额", 130));
            columns.Add(new TableToolColumnImport("中奖金额", 130));
            columns.Add(new TableToolColumnImport("操作", 132));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            DataReportServiceClient client = new DataReportServiceClient();
            client.GetBettingListCompleted += (sender, e) =>
            {
                if (e.Result.Success)
                {
                    int t = 0;
                    e.Result.Content.ForEach(x =>
                    {
                        BettingReportsPage_TableRow row = new BettingReportsPage_TableRow(x, t);
                        row.SelectForOwnerEventHandler += SelectForOwner;
                        row.SelectForTicketEventHandler += SelectForTicket;
                        row.SelectForTagEventHandler += SelectForTag;
                        row.SelectForHowToPlayEventHandler += SelectForHowToPlay;
                        rows.Add(row);
                        t++;
                    });
                    TableTool tool = new TableTool("查看投注记录", e.Result.PageIndex, e.Result.TotalOfPage
                        , columns, rows);
                    tool.NextPageEventHandler += GoNextPage;
                    tableBody.Children.Clear();
                    tableBody.Children.Add(tool);
                }
            };
            client.GetBettingListAsync(beginTime, endTime, ownerId, username, ticketId, tagId, howToPlatId
                , status, pageIndex, App.Token);
        }

        void SelectForHowToPlay(object sender, EventArgs e)
        {
            BettingReportsPage_TableRow row = (BettingReportsPage_TableRow)sender;
            howToPlatId = row.Betting.HowToPlayId;
            pageIndex = 1;
            InsertTable();
        }

        void SelectForTag(object sender, EventArgs e)
        {
            BettingReportsPage_TableRow row = (BettingReportsPage_TableRow)sender;
            tagId = row.Betting.TagId;
            pageIndex = 1;
            InsertTable();
        }

        void SelectForTicket(object sender, EventArgs e)
        {
            BettingReportsPage_TableRow row = (BettingReportsPage_TableRow)sender;
            ticketId = row.Betting.TicketId;
            pageIndex = 1;
            InsertTable();
        }

        void SelectForOwner(object sender, EventArgs e)
        {
            BettingReportsPage_TableRow row = (BettingReportsPage_TableRow)sender;
            ownerId = row.Betting.OwnerId;
            pageIndex = 1;
            InsertTable();
        }

        private void Reset(object sender, EventArgs e)
        {
            input_beginTime.Text = "";
            input_endTime.Text = "";
            input_status_all.IsChecked = true;
            input_username.Text = "";

            username = "";
            beginTime = "";
            endTime = "";
            ownerId = 0;
            ticketId = 0;
            tagId = 0;
            howToPlatId = 0;
            status = BettingStatusSelectType.全部;
            pageIndex = 1;
            InsertTable();
        }

        void GoNextPage(object sender, NextPageEventArgs e)
        {
            pageIndex = e.To;
            InsertTable();
        }

        private void SelectForStatus(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            this.status = (BettingStatusSelectType)Enum.Parse(typeof(BettingStatusSelectType), (string)rb.Content, false);
            pageIndex = 1;
            InsertTable();
        }

        private void SelectForTime(object sender, EventArgs e)
        {
            beginTime = input_beginTime.Text;
            endTime = input_endTime.Text;
            pageIndex = 1;
            InsertTable();
        }

        private void SelectForUsername(object sender, EventArgs e)
        {
            username = input_username.Text;
            input_username.Text = "";
            pageIndex = 1;
            InsertTable();
        }

    }
}

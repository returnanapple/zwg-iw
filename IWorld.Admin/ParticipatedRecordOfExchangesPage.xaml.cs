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
    public partial class ParticipatedRecordOfExchangesPage : UserControl
    {
        int pageIndex = 1;
        int activityId = 0;
        int userId = 0;
        string beginTime = "";
        string endTime = "";

        public ParticipatedRecordOfExchangesPage()
        {
            InitializeComponent();
            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("用户", 150));
            columns.Add(new TableToolColumnImport("活动", 312));
            columns.Add(new TableToolColumnImport("兑换数量", 150));
            columns.Add(new TableToolColumnImport("总价值", 150));
            columns.Add(new TableToolColumnImport("时间", 150));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            ExchangeServiceClient client = new ExchangeServiceClient();
            client.GetParticipateRecordListCompleted += (sender, e) =>
                {
                    if (e.Result.Success)
                    {
                        int t = 0;
                        e.Result.Content.ForEach(x =>
                        {
                            ParticipatedRecordOfExchangesPage_TableRow row = new ParticipatedRecordOfExchangesPage_TableRow(x, t);
                            row.SelecteForOwnerEventHandler += SelecteForOwner;
                            row.SeleteForActivityEventHandler += SeleteForActivity;
                            rows.Add(row);
                            t++;
                        });
                        TableTool tool = new TableTool("查看兑换活动参与记录", e.Result.PageIndex, e.Result.TotalOfPage
                            , columns, rows);
                        tool.NextPageEventHandler += GoNextPage;
                        tableBody.Children.Clear();
                        tableBody.Children.Add(tool);
                    }
                };
            client.GetParticipateRecordListAsync(activityId, userId, beginTime, endTime, pageIndex, App.Token);
        }

        void SeleteForActivity(object sender, EventArgs e)
        {
            ParticipatedRecordOfActivitiesPage_TableRow row = (ParticipatedRecordOfActivitiesPage_TableRow)sender;
            activityId = row.Participate.ActivityId;
            pageIndex = 1;

            InsertTable();
        }

        void SelecteForOwner(object sender, EventArgs e)
        {
            ParticipatedRecordOfActivitiesPage_TableRow row = (ParticipatedRecordOfActivitiesPage_TableRow)sender;
            userId = row.Participate.OwnerId;
            pageIndex = 1;

            InsertTable();
        }

        void GoNextPage(object sender, NextPageEventArgs e)
        {
            pageIndex = e.To;
            InsertTable();
        }

        private void SelcetForTime(object sender, EventArgs e)
        {
            beginTime = input_beginTime.Text;
            endTime = input_endTime.Text;
            pageIndex = 1;
            InsertTable();
        }

        private void Reset(object sender, EventArgs e)
        {
            input_beginTime.Text = "";
            input_endTime.Text = "";

            activityId = 0;
            userId = 0;
            beginTime = "";
            endTime = "";
            pageIndex = 1;
            InsertTable();
        }
    }
}

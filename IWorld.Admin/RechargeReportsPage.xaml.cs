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
    public partial class RechargeReportsPage : UserControl
    {
        string beginTime = "";
        string endTime = "";
        RechargeStatusSelectType status = RechargeStatusSelectType.全部;
        int userId = 0;
        int pageIndex = 1;
        string username = "";

        public RechargeReportsPage()
        {
            InitializeComponent();
            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("用户", 180));
            columns.Add(new TableToolColumnImport("支付人", 180));
            columns.Add(new TableToolColumnImport("金额", 180));
            columns.Add(new TableToolColumnImport("状态", 180));
            columns.Add(new TableToolColumnImport("操作", 192));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            DataReportServiceClient client = new DataReportServiceClient();
            client.GetRechargeListCompleted += (sender, e) =>
                {
                    if (e.Result.Success)
                    {
                        int t = 0;
                        e.Result.Content.ForEach(x =>
                        {
                            RechargeReportsPage_TableRow row = new RechargeReportsPage_TableRow(x, t);
                            row.SelectForOwnerEventHandler += SelectForOwner;
                            row.RefreshEventHandler += Refresh;
                            rows.Add(row);
                            t++;
                        });
                        TableTool tool = new TableTool("查看充值记录", e.Result.PageIndex, e.Result.TotalOfPage
                            , columns, rows);
                        tool.NextPageEventHandler += GoNextPage;
                        tableBody.Children.Clear();
                        tableBody.Children.Add(tool);
                    }
                };
            client.GetRechargeListAsync(beginTime, endTime, status, userId, username, pageIndex, App.Token);
        }

        void SelectForOwner(object sender, EventArgs e)
        {
            RechargeReportsPage_TableRow row = (RechargeReportsPage_TableRow)sender;
            userId = row.Recharge.OwnerId;
            pageIndex = 1;
            InsertTable();
        }

        void Refresh(object sender, EventArgs e)
        {
            InsertTable();
        }

        private void Reset(object sender, EventArgs e)
        {
            input_beginTime.Text = "";
            input_endTime.Text = ""; 
            input_username.Text = "";
            input_status_all.IsChecked = true;

            username = "";
            beginTime = "";
            endTime = "";
            status = RechargeStatusSelectType.全部;
            userId = 0;
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
            this.status = (RechargeStatusSelectType)Enum.Parse(typeof(RechargeStatusSelectType), (string)rb.Content, false);
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

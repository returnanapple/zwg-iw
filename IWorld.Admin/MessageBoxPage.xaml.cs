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
    public partial class MessageBoxPage : UserControl
    {
        string beginTime = "";
        string endTime = "";
        RechargeStatusSelectType rStatus = RechargeStatusSelectType.用户已经支付;
        WithdrawalsStatusSelectType wStatus = WithdrawalsStatusSelectType.处理中;
        int userId = 0;
        int pageIndex = 1;
        string username = "";

        public MessageBoxPage()
        {
            InitializeComponent();
            InsertTable_R();
            InsertTable_W();
        }

        void InsertTable_R()
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
                        row.SelectForOwnerEventHandler += SelectForOwner_R;
                        row.RefreshEventHandler += Refresh_R;
                        rows.Add(row);
                        t++;
                    });
                    TableTool tool = new TableTool("等待处理充值记录", e.Result.PageIndex, e.Result.TotalOfPage
                        , columns, rows);
                    tool.NextPageEventHandler += GoNextPage_R;
                    rTable.Children.Clear();
                    rTable.Children.Add(tool);
                }
            };
            client.GetRechargeListAsync(beginTime, endTime, rStatus, userId, username, pageIndex, App.Token);
        }

        void SelectForOwner_R(object sender, EventArgs e)
        {
            //不存在该功能
        }

        void Refresh_R(object sender, EventArgs e)
        {
            InsertTable_R();
        }

        void GoNextPage_R(object sender, NextPageEventArgs e)
        {
            pageIndex = e.To;
            InsertTable_R();
        }



        void InsertTable_W()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("用户", 152));
            columns.Add(new TableToolColumnImport("金额", 152));
            columns.Add(new TableToolColumnImport("银行卡", 152));
            columns.Add(new TableToolColumnImport("开户人", 152));
            columns.Add(new TableToolColumnImport("状态", 152));
            columns.Add(new TableToolColumnImport("操作", 152));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            DataReportServiceClient client = new DataReportServiceClient();
            client.GetWithdrawalListCompleted += (sender, e) =>
            {
                if (e.Result.Success)
                {
                    int t = 0;
                    e.Result.Content.ForEach(x =>
                    {
                        WithdrawalReportsPage_TableRow row = new WithdrawalReportsPage_TableRow(x, t);
                        row.SelectForOwnerEventHandler += SelectForOwner_W;
                        row.RefreshEventHandler += Refresh_W;
                        rows.Add(row);
                        t++;
                    });
                    TableTool tool = new TableTool("待处理的提现记录", e.Result.PageIndex, e.Result.TotalOfPage
                        , columns, rows);
                    tool.NextPageEventHandler += GoNextPage_W;
                    wTable.Children.Clear();
                    wTable.Children.Add(tool);
                }
            };
            client.GetWithdrawalListAsync(beginTime, endTime, wStatus, userId, username, pageIndex, App.Token);
        }

        void SelectForOwner_W(object sender, EventArgs e)
        {
            //不存在该功能
        }

        void Refresh_W(object sender, EventArgs e)
        {
            InsertTable_W();
        }

        void GoNextPage_W(object sender, NextPageEventArgs e)
        {
            pageIndex = e.To;
            InsertTable_W();
        }
    }
}

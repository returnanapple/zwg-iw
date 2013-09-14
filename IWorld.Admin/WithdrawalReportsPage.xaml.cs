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
    public partial class WithdrawalReportsPage : UserControl
    {
        string beginTime = "";
        string endTime = "";
        WithdrawalsStatusSelectType status = WithdrawalsStatusSelectType.全部;
        int userId = 0;
        int pageIndex = 1;

        public WithdrawalReportsPage()
        {
            InitializeComponent();
            InsertTable();
        }

        void InsertTable()
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
                            row.SelectForOwnerEventHandler += SelectForOwner;
                            row.RefreshEventHandler += Refresh;
                            rows.Add(row);
                            t++;
                        });
                        TableTool tool = new TableTool("查看提现记录", e.Result.PageIndex, e.Result.TotalOfPage
                            , columns, rows);
                        tool.NextPageEventHandler += GoNextPage;
                        tableBody.Children.Clear();
                        tableBody.Children.Add(tool);
                    }
                };
            client.GetWithdrawalListAsync(beginTime, endTime, status, userId, pageIndex, App.Token);
        }

        void SelectForOwner(object sender, EventArgs e)
        {
            WithdrawalReportsPage_TableRow row = (WithdrawalReportsPage_TableRow)sender;
            userId = row.Withdrawal.OwnerId;
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
            input_status_all.IsChecked = true;

            beginTime = "";
            endTime = "";
            status = WithdrawalsStatusSelectType.全部;
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
            this.status = (WithdrawalsStatusSelectType)Enum.Parse(typeof(WithdrawalsStatusSelectType), (string)rb.Content, false);
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
    }
}

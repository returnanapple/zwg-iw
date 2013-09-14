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
    public partial class TransferReportsPage : UserControl
    {
        string beginTime = "";
        string endTime = "";
        int userId = 0;
        int pageIndex = 1;

        public TransferReportsPage()
        {
            InitializeComponent();
            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("用户", 200));
            columns.Add(new TableToolColumnImport("金额", 200));
            columns.Add(new TableToolColumnImport("备注", 512));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            DataReportServiceClient client = new DataReportServiceClient();
            client.GetTransferListCompleted += (sender, e) =>
            {
                if (e.Result.Success)
                {
                    int t = 0;
                    e.Result.Content.ForEach(x =>
                    {
                        TransferReportsPage_TableRow row = new TransferReportsPage_TableRow(x, t);
                        row.SelectForOwnerEventHandler += SelectForOwner;
                        rows.Add(row);
                        t++;
                    });
                    TableTool tool = new TableTool("查看支取记录", e.Result.PageIndex, e.Result.TotalOfPage
                        , columns, rows);
                    tool.NextPageEventHandler += GoNextPage;
                    tableBody.Children.Clear();
                    tableBody.Children.Add(tool);
                }
            };
            client.GetTransferListAsync(beginTime, endTime, userId, pageIndex, App.Token);
        }

        void SelectForOwner(object sender, EventArgs e)
        {
            TransferReportsPage_TableRow row = (TransferReportsPage_TableRow)sender;
            userId = row.Transfer.OwnerId;
            pageIndex = 1;
            InsertTable();
        }

        private void Reset(object sender, EventArgs e)
        {
            input_beginTime.Text = "";
            input_endTime.Text = "";

            beginTime = "";
            endTime = "";
            userId = 0;
            pageIndex = 1;
            InsertTable();
        }

        void GoNextPage(object sender, NextPageEventArgs e)
        {
            pageIndex = e.To;
            InsertTable();
        }

        private void SelectForTime(object sender, EventArgs e)
        {
            beginTime = input_beginTime.Text;
            endTime = input_endTime.Text;
            pageIndex = 1;
            InsertTable();
        }

        private void Create(object sender, EventArgs e)
        {
            TransferReportsPage_CreateTool ct = new TransferReportsPage_CreateTool();
            ct.Closed += ShowCreateResult;
            ct.Show();
        }
        #region 创建
        void ShowCreateResult(object sender, EventArgs e)
        {
            TransferReportsPage_CreateTool ct = (TransferReportsPage_CreateTool)sender;
            if (ct.DialogResult == true)
            {
                if (ct.ShowError == true)
                {
                    ErrorPrompt ep = new ErrorPrompt(ct.Error);
                    ep.Show();
                }
                else
                {
                    InsertTable();
                }
            }
        }
        #endregion
    }
}

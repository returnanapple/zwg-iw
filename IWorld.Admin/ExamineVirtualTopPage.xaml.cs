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
using IWorld.Admin.LotteryTicketService;

namespace IWorld.Admin
{
    public partial class ExamineVirtualTopPage : UserControl
    {
        #region 私有变量

        int ticketId = 0;

        int pageIndex = 1;

        #endregion

        public ExamineVirtualTopPage()
        {
            InitializeComponent();
            InsertTable();
        }

        private void Create(object sender, EventArgs e)
        {
            ExamineVirtualTopPage_CreateTool ct = new ExamineVirtualTopPage_CreateTool();
            ct.Closed += ShowCreateResult;
            ct.Show();
        }
        #region 创建
        void ShowCreateResult(object sender, EventArgs e)
        {
            ExamineVirtualTopPage_CreateTool ct = (ExamineVirtualTopPage_CreateTool)sender;
            if (ct.DialogResult == true)
            {
                if (ct.ShowError)
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

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("彩票", 304));
            columns.Add(new TableToolColumnImport("金额", 304));
            columns.Add(new TableToolColumnImport("操作", 304));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            LotteryTicketServiceClient client = new LotteryTicketServiceClient();
            client.GetVirtualTopListCompleted += (sender, e) =>
            {
                if (e.Result.Success)
                {
                    int t = 0;
                    e.Result.Content.ForEach(x =>
                    {
                        ExamineVirtualTopPage_TableRow row = new ExamineVirtualTopPage_TableRow(x, t);
                        row.SelectForTicketEventHandler += SelectForTicket;
                        row.RefreshEventHandler += Refresh;
                        rows.Add(row);
                        t++;
                    });
                    TableTool tool = new TableTool("查看虚拟排行信息", e.Result.PageIndex, e.Result.TotalOfPage
                        , columns, rows);
                    tool.NextPageEventHandler += GoNextPage;
                    tableBody.Children.Clear();
                    tableBody.Children.Add(tool);
                }
            };
            client.GetVirtualTopListAsync(ticketId, pageIndex, App.Token);
        }

        void SelectForTicket(object sender, EventArgs e)
        {
            ExamineVirtualTopPage_TableRow row = (ExamineVirtualTopPage_TableRow)sender;
            ticketId = row.VirtualTop.TicketId;
            pageIndex = 1;

            InsertTable();
        }

        void Refresh(object sender, EventArgs e)
        {
            InsertTable();
        }

        void GoNextPage(object sender, NextPageEventArgs e)
        {
            pageIndex = e.To;
            InsertTable();
        }

        private void Reset(object sender, EventArgs e)
        {
            ticketId = 0;
            pageIndex = 1;

            InsertTable();
        }
    }
}

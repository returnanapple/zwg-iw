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
    public partial class ExamineLotteryPage : UserControl
    {
        #region 私有变量

        int ticketId = 0;

        int pageIndex = 1;

        #endregion

        public ExamineLotteryPage()
        {
            InitializeComponent();
            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("彩票", 228));
            columns.Add(new TableToolColumnImport("期号", 228));
            columns.Add(new TableToolColumnImport("开奖号码", 228));
            columns.Add(new TableToolColumnImport("开奖时间", 228));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            LotteryTicketServiceClient client = new LotteryTicketServiceClient();
            client.GetLotteryListCompleted += (sender, e) =>
                {
                    if (e.Result.Success)
                    {
                        int t = 0;
                        e.Result.Content.ForEach(x =>
                        {
                            ExamineLotteryPage_TableRow row = new ExamineLotteryPage_TableRow(x, t);
                            row.SelectForTicketEventHandler += SelectForTicket;
                            rows.Add(row);
                            t++;
                        });
                        TableTool tool = new TableTool("查看开奖记录", e.Result.PageIndex, e.Result.TotalOfPage
                            , columns, rows);
                        tool.NextPageEventHandler += GoNextPage;
                        tableBody.Children.Clear();
                        tableBody.Children.Add(tool);
                    }
                };
            client.GetLotteryListAsync(ticketId, LotterySourcesSelectType.全部, pageIndex, App.Token);
        }

        void SelectForTicket(object sender, EventArgs e)
        {
            ExamineLotteryPage_TableRow row = (ExamineLotteryPage_TableRow)sender;
            ticketId = row.Lottery.TicketId;
            pageIndex = 1;

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

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
using IWorld.Client.FundsService;

namespace IWorld.Client
{
    public partial class FundsPage : UserControl
    {
        bool SeeW { get; set; }
        int pageIndex = 1;

        public FundsPage(bool seeW = false)
        {
            InitializeComponent();
            SeeW = seeW;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            InSertTable();
        }

        void InSertTable()
        {
            if (SeeW)
            {
                List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
                columns.Add(new TableToolColumnImport("订单号", 120));
                columns.Add(new TableToolColumnImport("提现金额", 200));
                columns.Add(new TableToolColumnImport("提现时间", 200));
                columns.Add(new TableToolColumnImport("处理状态", 200));
                List<ITableToolRow> rows = new List<Class.ITableToolRow>();

                FundsServiceClient client = new FundsServiceClient();
                client.GetWithdrawDetailsCompleted += (sender, e) =>
                    {
                        int t = 0;
                        e.Result.Content.ForEach(x =>
                        {
                            FundsPage_TableRow_w row = new FundsPage_TableRow_w(x, t);
                            rows.Add(row);
                            t++;
                        });

                        TableTool tt = new TableTool("", e.Result.PageIndex, e.Result.TotalOfPage, columns, rows);
                        tt.NextPageEventHandler += GoToNextPage;
                        tableBody.Children.Clear();
                        tableBody.Children.Add(tt);
                    };
                client.GetWithdrawDetailsAsync(pageIndex, App.Token);
            }
            else
            {
                List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
                columns.Add(new TableToolColumnImport("订单号", 120));
                columns.Add(new TableToolColumnImport("目标用户", 120));
                columns.Add(new TableToolColumnImport("支付人", 120));
                columns.Add(new TableToolColumnImport("充值金额", 120));
                columns.Add(new TableToolColumnImport("充值时间", 120));
                columns.Add(new TableToolColumnImport("状态", 120));
                List<ITableToolRow> rows = new List<Class.ITableToolRow>();

                FundsServiceClient client = new FundsServiceClient();
                client.GetRechargeDetailsCompleted += (sender, e) =>
                    {
                        int t = 0;
                        e.Result.Content.ForEach(x =>
                            {
                                FundsPage_TableRow row = new FundsPage_TableRow(x, t);
                                rows.Add(row);
                                t++;
                            });

                        TableTool tt = new TableTool("", e.Result.PageIndex, e.Result.TotalOfPage, columns, rows);
                        tt.NextPageEventHandler += GoToNextPage;
                        tableBody.Children.Clear();
                        tableBody.Children.Add(tt);
                    };
                client.GetRechargeDetailsAsync(pageIndex, App.Token);
            }
        }

        void GoToNextPage(object sender, NextPageEventArgs e)
        {
            pageIndex = e.To;
            InSertTable();
        }

        public event NDelegate OpenRToolEventHacnler;
        public event NDelegate OpenWToolEventHacnler;

        private void OpenRTool(object sender, RoutedEventArgs e)
        {
            if (OpenRToolEventHacnler != null)
            {
                OpenRToolEventHacnler(this, new EventArgs());
            }
        }

        private void OpenWTool(object sender, RoutedEventArgs e)
        {
            if (OpenWToolEventHacnler != null)
            {
                OpenWToolEventHacnler(this, new EventArgs());
            }
        }

        private void SeeRList(object sender, RoutedEventArgs e)
        {
            SeeW = false;
            pageIndex = 1;
            InSertTable();
        }

        private void SeeWList(object sender, RoutedEventArgs e)
        {
            SeeW = true;
            pageIndex = 1;
            InSertTable();
        }
    }
}

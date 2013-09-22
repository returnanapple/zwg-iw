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
    public partial class SiteReportPage : UserControl
    {
        int pageIndex = 1;
        string beginTime = "";
        string endTime = "";
        TimePeriodSelectType timePeriod = TimePeriodSelectType.月;

        public SiteReportPage()
        {
            InitializeComponent();
            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("日期", 88));
            columns.Add(new TableToolColumnImport("登陆", 60));
            columns.Add(new TableToolColumnImport("注册", 60));
            columns.Add(new TableToolColumnImport("投注", 88));
            columns.Add(new TableToolColumnImport("奖金", 88));
            columns.Add(new TableToolColumnImport("活动返还", 88));
            columns.Add(new TableToolColumnImport("盈亏", 88));
            columns.Add(new TableToolColumnImport("充值", 88));
            columns.Add(new TableToolColumnImport("提现", 88));
            columns.Add(new TableToolColumnImport("支取", 88));
            columns.Add(new TableToolColumnImport("返点", 88));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            DataReportServiceClient client = new DataReportServiceClient();
            client.GetSiteDataListCompleted += (sender, e) =>
                {
                    if (e.Result.Success)
                    {
                        int t = 0;
                        e.Result.Content.ForEach(x =>
                        {
                            SiteReportPage_TableRow row = new SiteReportPage_TableRow(x, t);
                            rows.Add(row);
                            t++;
                        });
                        TableTool tool = new TableTool("查看系统报表", e.Result.PageIndex, e.Result.TotalOfPage
                            , columns, rows);
                        tool.NextPageEventHandler += GoNextPage;
                        tableBody.Children.Clear();
                        tableBody.Children.Add(tool);
                    }
                };
            client.GetSiteDataListAsync(beginTime, endTime, timePeriod, pageIndex, App.Token);
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

            beginTime = "";
            endTime = "";
            timePeriod = TimePeriodSelectType.月;
            pageIndex = 1;
            InsertTable();
        }

        private void SelectForTimePeriod(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Name == "input_timePeriod_month")
            {
                timePeriod = TimePeriodSelectType.月;
            }
            else
            {
                timePeriod = TimePeriodSelectType.日;
            }
            pageIndex = 1;
            InsertTable();
        }
    }
}

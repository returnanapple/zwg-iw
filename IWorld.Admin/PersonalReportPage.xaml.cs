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
    public partial class PersonalReportPage : UserControl
    {
        int userId = 0;
        int pageIndex = 1;
        string beginTime = "";
        string endTime = "";
        TimePeriodSelectType timePeriod = TimePeriodSelectType.月;
        string username = "";

        public PersonalReportPage(int userId = 0)
        {
            InitializeComponent();
            this.userId = userId;
            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("用户", 88));
            columns.Add(new TableToolColumnImport("层级", 60));
            columns.Add(new TableToolColumnImport("日期", 88));
            columns.Add(new TableToolColumnImport("下级", 60));
            columns.Add(new TableToolColumnImport("帐户余额", 88));
            columns.Add(new TableToolColumnImport("投注", 88));
            columns.Add(new TableToolColumnImport("奖金", 88));
            columns.Add(new TableToolColumnImport("返还", 88));
            columns.Add(new TableToolColumnImport("盈亏", 88));
            columns.Add(new TableToolColumnImport("充值", 88));
            columns.Add(new TableToolColumnImport("提现", 88));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            DataReportServiceClient client = new DataReportServiceClient();
            client.GetPersonalDataListCompleted += (sender, e) =>
                {
                    if (e.Result.Success)
                    {
                        int t = 0;
                        e.Result.Content.ForEach(x =>
                        {
                            PersonalReportPage_TableRow row = new PersonalReportPage_TableRow(x, t);
                            row.SelectForOwnerEventHandler += SelectForOwner;
                            rows.Add(row);
                            t++;
                        });
                        TableTool tool = new TableTool("查看个人报表", e.Result.PageIndex, e.Result.TotalOfPage
                            , columns, rows);
                        tool.NextPageEventHandler += GoNextPage;
                        tableBody.Children.Clear();
                        tableBody.Children.Add(tool);
                    }
                };
            client.GetPersonalDataListAsync(beginTime, endTime, timePeriod, userId, username, pageIndex, App.Token);
        }

        void SelectForOwner(object sender, EventArgs e)
        {
            PersonalReportPage_TableRow row = (PersonalReportPage_TableRow)sender;
            userId = row.Data.OwnerId;
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
            input_timePeriod_month.IsChecked = true;
            input_username.Text = "";

            username = "";
            userId = 0;
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

        private void SelectForUsername(object sender, EventArgs e)
        {
            username = input_username.Text;
            input_username.Text = "";
            pageIndex = 1;
            InsertTable();
        }
    }
}

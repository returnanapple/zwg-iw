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
using IWorld.Admin.ManagerService;

namespace IWorld.Admin
{
    public partial class ManagerLandingRecordsPage : UserControl
    {
        int pageIndex = 1;
        int userId = 0;
        string beginTime = "";
        string endTime = "";

        public ManagerLandingRecordsPage()
        {
            InitializeComponent();
            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("用户名", 304));
            columns.Add(new TableToolColumnImport("登录时间", 304));
            columns.Add(new TableToolColumnImport("登录IP", 304));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            ManagerServiceClient client = new ManagerServiceClient();
            client.GetLandingRecordListCompleted += (sender, e) =>
            {
                if (e.Result.Success)
                {
                    int t = 0;
                    e.Result.Content.ForEach(x =>
                    {
                        ManagerLandingRecordsPage_TableRow row = new ManagerLandingRecordsPage_TableRow(x, t);
                        rows.Add(row);
                        t++;
                    });
                    TableTool tool = new TableTool("查看管理员登陆信息", e.Result.PageIndex, e.Result.TotalOfPage
                        , columns, rows);
                    tool.NextPageEventHandler += GoNextPage;
                    tableBody.Children.Clear();
                    tableBody.Children.Add(tool);
                }
            };
            client.GetLandingRecordListAsync(userId, beginTime, endTime, pageIndex, App.Token);
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

            userId = 0;
            beginTime = "";
            endTime = "";
            pageIndex = 1;
            InsertTable();
        }
    }
}

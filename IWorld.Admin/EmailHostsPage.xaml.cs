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
using IWorld.Admin.SystemSettingService;

namespace IWorld.Admin
{
    public partial class EmailHostsPage : UserControl
    {
        int pageIndex = 1;

        public EmailHostsPage()
        {
            InitializeComponent();
            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("索引字", 180));
            columns.Add(new TableToolColumnImport("地址", 180));
            columns.Add(new TableToolColumnImport("端口", 180));
            columns.Add(new TableToolColumnImport("备注", 192));
            columns.Add(new TableToolColumnImport("操作", 180));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            SystemSettingServiceClient client = new SystemSettingServiceClient();
            client.GetEmailClientListCompleted += (sender, e) =>
                {
                    if (e.Result.Success)
                    {
                        int t = 0;
                        e.Result.Content.ForEach(x =>
                        {
                            EmailHostsPage_TableRow row = new EmailHostsPage_TableRow(x, t);
                            row.RefreshEventHandler += Refresh;
                            rows.Add(row);
                            t++;
                        });
                        TableTool tool = new TableTool("查看邮箱服务器", e.Result.PageIndex, e.Result.TotalOfPage
                            , columns, rows);
                        tool.NextPageEventHandler += GoNextPage;
                        tableBody.Children.Clear();
                        tableBody.Children.Add(tool);
                    }
                };
            client.GetEmailClientListAsync(pageIndex, App.Token);
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

        private void Create(object sender, EventArgs e)
        {
            EmailHostsPage_CreateTool ct = new EmailHostsPage_CreateTool();
            ct.Closed += ShowCreateResult;
            ct.Show();
        }
        #region 创建
        void ShowCreateResult(object sender, EventArgs e)
        {
            EmailHostsPage_CreateTool ct = (EmailHostsPage_CreateTool)sender;
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
    }
}

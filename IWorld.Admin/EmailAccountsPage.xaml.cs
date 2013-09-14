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
    public partial class EmailAccountsPage : UserControl
    {
        int pageIndex = 1;

        public EmailAccountsPage()
        {
            InitializeComponent();
            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("索引字", 152));
            columns.Add(new TableToolColumnImport("帐号", 152));
            columns.Add(new TableToolColumnImport("密码", 152));
            columns.Add(new TableToolColumnImport("备注", 184));
            columns.Add(new TableToolColumnImport("服务器", 120));
            columns.Add(new TableToolColumnImport("操作", 152));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            SystemSettingServiceClient client = new SystemSettingServiceClient();
            client.GetEmailAccountListCompleted += (sender, e) =>
                {
                    if (e.Result.Success)
                    {
                        int t = 0;
                        e.Result.Content.ForEach(x =>
                        {
                            EmailAccountsPage_TableRow row = new EmailAccountsPage_TableRow(x, t);
                            row.RefreshEventHandler += Refresh;
                            rows.Add(row);
                            t++;
                        });
                        TableTool tool = new TableTool("查看系统邮箱", e.Result.PageIndex, e.Result.TotalOfPage
                            , columns, rows);
                        tool.NextPageEventHandler += GoNextPage;
                        tableBody.Children.Clear();
                        tableBody.Children.Add(tool);
                    }
                };
            client.GetEmailAccountListAsync(pageIndex, App.Token);
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
            EmailAccountsPage_CreateTool ct = new EmailAccountsPage_CreateTool();
            ct.Closed += ShowCreateResult;
            ct.Show();
        }
        #region 创建
        void ShowCreateResult(object sender, EventArgs e)
        {
            EmailAccountsPage_CreateTool ct = (EmailAccountsPage_CreateTool)sender;
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

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
    public partial class BankAccountsPage : UserControl
    {
        int pageIndex = 1;

        public BankAccountsPage()
        {
            InitializeComponent();
            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("索引字", 100));
            columns.Add(new TableToolColumnImport("卡号", 160));
            columns.Add(new TableToolColumnImport("开户人", 100));
            columns.Add(new TableToolColumnImport("银行", 100));
            columns.Add(new TableToolColumnImport("备注", 212));
            columns.Add(new TableToolColumnImport("排列", 100));
            columns.Add(new TableToolColumnImport("操作", 140));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            SystemSettingServiceClient client = new SystemSettingServiceClient();
            client.GetBankAccountListCompleted += (sender, e) =>
            {
                if (e.Result.Success)
                {
                    int t = 0;
                    e.Result.Content.ForEach(x =>
                    {
                        BankAccountsPage_TableRow row = new BankAccountsPage_TableRow(x, t);
                        row.RefreshEventHandler += Refresh;
                        rows.Add(row);
                        t++;
                    });
                    TableTool tool = new TableTool("查看银行账户（系统）", e.Result.PageIndex, e.Result.TotalOfPage
                        , columns, rows);
                    tool.NextPageEventHandler += GoNextPage;
                    tableBody.Children.Clear();
                    tableBody.Children.Add(tool);
                }
            };
            client.GetBankAccountListAsync(pageIndex, App.Token);
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
            BankAccountsPage_CreateTool ct = new BankAccountsPage_CreateTool();
            ct.Closed += ShowCreateResult;
            ct.Show();
        }
        #region 创建
        void ShowCreateResult(object sender, EventArgs e)
        {
            BankAccountsPage_CreateTool ct = (BankAccountsPage_CreateTool)sender;
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

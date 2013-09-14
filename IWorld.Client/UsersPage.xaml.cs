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
using IWorld.Client.UsersService;

namespace IWorld.Client
{
    public partial class UsersPage : UserControl
    {
        string keyword = "";
        bool onlyImmediate = false;
        int pageIndex = 1;

        public UsersPage()
        {
            InitializeComponent();
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("用户名", 120));
            columns.Add(new TableToolColumnImport("用户组", 120));
            columns.Add(new TableToolColumnImport("返点", 120));
            columns.Add(new TableToolColumnImport("帐户余额", 120));
            columns.Add(new TableToolColumnImport("消费量", 120));
            columns.Add(new TableToolColumnImport("用户状态", 120));
            List<ITableToolRow> rows = new List<Class.ITableToolRow>();

            UsersServiceClient client = new UsersServiceClient();
            client.GetUsersCompleted += (sender, e) =>
            {
                int t = 0;
                e.Result.Content.ForEach(x =>
                {
                    UsersPage_TableRow row = new UsersPage_TableRow(x, t);
                    row.RefreshEventHandler += Refresh;
                    rows.Add(row);
                    t++;
                });

                TableTool tt = new TableTool("", e.Result.PageIndex, e.Result.TotalOfPage, columns, rows);
                tt.NextPageEventHandler += GoToNextPage;
                tableBody.Children.Clear();
                tableBody.Children.Add(tt);
            };
            client.GetUsersAsync(keyword, onlyImmediate, pageIndex, App.Token);
        }

        void Refresh(object sender, EventArgs e)
        {
            InsertTable();
        }

        void GoToNextPage(object sender, NextPageEventArgs e)
        {
            pageIndex = e.To;
            InsertTable();
        }

        private void SelectForKeyword(object sender, RoutedEventArgs e)
        {
            keyword = input_keyword.Text;
            pageIndex = 1;
            InsertTable();
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            input_keyword.Text = "";
            input_onlyImmediate.SelectedIndex = 0;

            keyword = "";
            onlyImmediate = false;
            pageIndex = 1;
            InsertTable();
        }

        private void SelectByIfOM(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            onlyImmediate = cb.SelectedIndex == 1;
            pageIndex = 1;
            InsertTable();
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            UsersPage_CreateTool ct = new UsersPage_CreateTool();
            ct.Closed += ShowCreateResult;
            ct.Show();
        }
        #region 创建
        void ShowCreateResult(object sender, EventArgs e)
        {
            UsersPage_CreateTool ct = (UsersPage_CreateTool)sender;
            if (ct.DialogResult == true)
            {
                if (ct.ShowError)
                {
                    ErrorPromt ep = new ErrorPromt(ct.Result.Error);
                    ep.Show();
                }
                else
                {
                    UsersPage_CreateResultTool crt = new UsersPage_CreateResultTool(ct.Result);
                    crt.Closed += Refresh;
                    crt.Show();
                }
            }
        }
        #endregion
    }
}

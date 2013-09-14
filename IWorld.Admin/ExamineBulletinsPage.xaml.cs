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
using IWorld.Admin.BulletinService;

namespace IWorld.Admin
{
    public partial class ExamineBulletinsPage : UserControl
    {
        #region 私有变量

        int pageIndex = 1;

        string keyword = "";

        RegularlyStatusSelectType selectType = RegularlyStatusSelectType.全部;

        #endregion

        public ExamineBulletinsPage()
        {
            InitializeComponent();
            InsertTable();
        }

        #region 时间

        public event NDelegate ViewCreatePageEventHandler;

        #endregion

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("标题", 272));
            columns.Add(new TableToolColumnImport("开始时间", 160));
            columns.Add(new TableToolColumnImport("结束时间", 160));
            columns.Add(new TableToolColumnImport("过期自动删除", 160));
            columns.Add(new TableToolColumnImport("操作", 160));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            BulletinServiceClient client = new BulletinServiceClient();
            client.GetBulletinListCompleted += (sender, e) =>
                {
                    if (e.Result.Success)
                    {
                        int t = 0;
                        e.Result.Content.ForEach(x =>
                        {
                            ExamineBulletinsPage_TableRow row = new ExamineBulletinsPage_TableRow(x, t);
                            row.RefreshEventHandler += Refresh;
                            rows.Add(row);
                            t++;
                        });
                        TableTool tool = new TableTool("查看公告信息", e.Result.PageIndex, e.Result.TotalOfPage
                            , columns, rows);
                        tool.NextPageEventHandler += GoNextPage;
                        tableBody.Children.Clear();
                        tableBody.Children.Add(tool);
                    }
                };
            client.GetBulletinListAsync(keyword, selectType, pageIndex, App.Token);
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

        private void SelectForIsS(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            this.selectType = (RegularlyStatusSelectType)Enum
                .Parse(typeof(RegularlyStatusSelectType), (string)button.Content, false);
            pageIndex = 1;
            InsertTable();
        }

        private void SelectForKeyword(object sender, EventArgs e)
        {
            keyword = input_keyword.Text;
            pageIndex = 1;
            InsertTable();
        }

        private void Reset(object sender, EventArgs e)
        {
            input_keyword.Text = "";
            input_selecetType_all.IsChecked = true;

            keyword = "";
            pageIndex = 1;
            selectType = RegularlyStatusSelectType.全部;

            InsertTable();
        }

        private void ViewCreatePage(object sender, EventArgs e)
        {
            if (ViewCreatePageEventHandler != null)
            {
                ViewCreatePageEventHandler(this, new EventArgs());
            }
        }
    }
}

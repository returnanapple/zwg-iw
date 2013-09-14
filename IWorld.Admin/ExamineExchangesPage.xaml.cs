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
using IWorld.Admin.ExchangeService;

namespace IWorld.Admin
{
    public partial class ExamineExchangesPage : UserControl
    {
        #region 私有变量

        int pageIndex = 1;

        string keyword = "";

        RegularlyStatusSelectType status = RegularlyStatusSelectType.全部;

        #endregion

        public ExamineExchangesPage()
        {
            InitializeComponent();

            List<TextBlock> _status = new List<TextBlock>();
            Enum.GetNames(typeof(RegularlyStatusSelectType)).ToList().ForEach(x =>
            {
                TextBlock tb = new TextBlock { Text = x };
                _status.Add(tb);
            });
            input_status.ItemsSource = _status;
            input_status.SelectedItem = input_status.Items.FirstOrDefault(x => ((TextBlock)x).Text == "全部");

            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("标题", 192));
            columns.Add(new TableToolColumnImport("名额", 120));
            columns.Add(new TableToolColumnImport("兑换单价", 120));
            columns.Add(new TableToolColumnImport("开始时间", 120));
            columns.Add(new TableToolColumnImport("结束时间", 120));
            columns.Add(new TableToolColumnImport("暂停", 120));
            columns.Add(new TableToolColumnImport("操作", 120));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            ExchangeServiceClient client = new ExchangeServiceClient();
            client.GetExchangeListCompleted += (sender, e) =>
                {
                    if (e.Result.Success)
                    {
                        int t = 0;
                        e.Result.Content.ForEach(x =>
                        {
                            ExamineExchangesPage_TableRow row = new ExamineExchangesPage_TableRow(x, t);
                            row.RefreshEventHandler += Refresh;
                            rows.Add(row);
                            t++;
                        });
                        TableTool tool = new TableTool("查看兑换活动信息", e.Result.PageIndex, e.Result.TotalOfPage
                            , columns, rows);
                        tool.NextPageEventHandler += GoNextPage;
                        tableBody.Children.Clear();
                        tableBody.Children.Add(tool);
                    }
                };
            client.GetExchangeListAsync(keyword, status, pageIndex, App.Token);
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

        private void SelectForKeyword(object sender, EventArgs e)
        {
            keyword = input_keyword.Text;
            pageIndex = 1;

            InsertTable();
        }

        private void Reset(object sender, EventArgs e)
        {
            input_keyword.Text = "";
            input_status.SelectedItem = input_status.Items.FirstOrDefault(x => ((TextBlock)x).Text == "全部");

            keyword = "";
            pageIndex = 1;
            status = RegularlyStatusSelectType.全部;

            InsertTable();
        }

        private void SelectForStatus(object sender, SelectionChangedEventArgs e)
        {
            status = (RegularlyStatusSelectType)Enum.Parse(typeof(RegularlyStatusSelectType)
                , ((TextBlock)input_status.SelectedItem).Text, false);
            pageIndex = 1;

            InsertTable();
        }

        private void Create(object sender, EventArgs e)
        {
            ExamineExchangesPage_CreateTool ct = new ExamineExchangesPage_CreateTool();
            ct.Closed += ShowCreateResult;
            ct.Show();
        }
        #region 创建
        void ShowCreateResult(object sender, EventArgs e)
        {
            ExamineExchangesPage_CreateTool ct = (ExamineExchangesPage_CreateTool)sender;
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

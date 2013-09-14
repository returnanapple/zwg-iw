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
using IWorld.Admin.ActivityService;

namespace IWorld.Admin
{
    public partial class ExamineActivitiesPage : UserControl
    {
        #region 私有变量

        int pageIndex = 1;

        string keyword = "";

        ActivityTypeSelectType type = ActivityTypeSelectType.全部;

        RegularlyStatusSelectType status = RegularlyStatusSelectType.全部;

        #endregion

        public ExamineActivitiesPage()
        {
            InitializeComponent();

            List<TextBlock> _type = new List<TextBlock>();
            Enum.GetNames(typeof(ActivityTypeSelectType)).ToList().ForEach(x =>
                {
                    TextBlock tb = new TextBlock { Text = x };
                    _type.Add(tb);
                });
            input_type.ItemsSource = _type;
            input_type.SelectedItem = input_type.Items.FirstOrDefault(x => ((TextBlock)x).Text == "全部");
            
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
            columns.Add(new TableToolColumnImport("类型", 120));
            columns.Add(new TableToolColumnImport("奖励", 120));
            columns.Add(new TableToolColumnImport("开始时间", 120));
            columns.Add(new TableToolColumnImport("结束时间", 120));
            columns.Add(new TableToolColumnImport("暂停", 120));
            columns.Add(new TableToolColumnImport("操作", 120));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            ActivityServiceClient client = new ActivityServiceClient();
            client.GetActivityListCompleted += (sender, e) =>
                {
                    if (e.Result.Success)
                    {
                        int t = 0;
                        e.Result.Content.ForEach(x =>
                        {
                            ExamineActivitiesPage_TableRow row = new ExamineActivitiesPage_TableRow(x, t);
                            row.RefreshEventHandler += Refresh;
                            rows.Add(row);
                            t++;
                        });
                        TableTool tool = new TableTool("查看默认活动信息", e.Result.PageIndex, e.Result.TotalOfPage
                            , columns, rows);
                        tool.NextPageEventHandler += GoNextPage;
                        tableBody.Children.Clear();
                        tableBody.Children.Add(tool);
                    }
                };
            client.GetActivityListAsync(keyword, type,status, pageIndex, App.Token);
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
            input_type.SelectedItem = input_type.Items.FirstOrDefault(x => ((TextBlock)x).Text == "全部");
            input_status.SelectedItem = input_status.Items.FirstOrDefault(x => ((TextBlock)x).Text == "全部");

            keyword = "";
            pageIndex = 1;
            type = ActivityTypeSelectType.全部;
            status = RegularlyStatusSelectType.全部;

            InsertTable();
        }

        private void SelectForType(object sender, SelectionChangedEventArgs e)
        {
            type = (ActivityTypeSelectType)Enum.Parse(typeof(ActivityTypeSelectType)
                , ((TextBlock)input_type.SelectedItem).Text, false);
            pageIndex = 1;

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
            ExamineActivitiesPage_CreateTool ct = new ExamineActivitiesPage_CreateTool();
            ct.Closed += ct_Closed;
            ct.Show();
        }
        #region 创建
        void ct_Closed(object sender, EventArgs e)
        {
            ExamineActivitiesPage_CreateTool ct = (ExamineActivitiesPage_CreateTool)sender;
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

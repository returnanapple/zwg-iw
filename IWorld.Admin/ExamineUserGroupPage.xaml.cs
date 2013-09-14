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
using IWorld.Admin.UserInfoService;

namespace IWorld.Admin
{
    public partial class ExamineUserGroupPage : UserControl
    {
        #region 私有变量

        int pageIndex = 1;

        string keyword = "";

        #endregion

        public ExamineUserGroupPage()
        {
            InitializeComponent();
            InsertTable();
        }

        #region 事件

        public event NDelegate SeleceUserByGroupEventHandler;

        #endregion

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("名称", 180));
            columns.Add(new TableToolColumnImport("等级", 180));
            columns.Add(new TableToolColumnImport("消费率下限", 180));
            columns.Add(new TableToolColumnImport("消费率上限", 180));
            columns.Add(new TableToolColumnImport("操作", 192));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            UserInfoServiceClient client = new UserInfoServiceClient();
            client.GetGroupListCompleted += (sender, e) =>
                {
                    if (e.Result.Success)
                    {
                        int t = 0;
                        e.Result.Content.ForEach(x =>
                        {
                            ExamineUserGroupPage_TableRow row = new ExamineUserGroupPage_TableRow(x, t);
                            row.SeleceUserByGroupEventHandler += SeleceUserByGroup;
                            row.RefreshEventHandler += Refresh;
                            rows.Add(row);
                            t++;
                        });
                        TableTool tool = new TableTool("查看用户信息", e.Result.PageIndex, e.Result.TotalOfPage
                            , columns, rows);
                        tool.NextPageEventHandler += GoNextPage;
                        tableBody.Children.Clear();
                        tableBody.Children.Add(tool);
                    }
                };
            client.GetGroupListAsync(keyword, pageIndex, App.Token);
        }

        void SeleceUserByGroup(object sender, EventArgs e)
        {
            if (SeleceUserByGroupEventHandler != null)
            {
                SeleceUserByGroupEventHandler(sender, e);
            }
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

        void SelectForKeyword(object sender, EventArgs e)
        {
            keyword = input_keyword.Text;
            InsertTable();
        }

        void Reset(object sender, EventArgs e)
        {
            input_keyword.Text = "";

            keyword = "";
            pageIndex = 1;
            InsertTable();
        }

        void CreateNewGroup(object sender, EventArgs e)
        {
            ExamineUserGroupPage_CreateTool ct = new ExamineUserGroupPage_CreateTool();
            ct.Closed += ShowCreateNewGroupResult;
            ct.Show();
        }
        #region 创建用户组
        void ShowCreateNewGroupResult(object sender, EventArgs e)
        {
            ExamineUserGroupPage_CreateTool ct = (ExamineUserGroupPage_CreateTool)sender;
            if (ct.DialogResult == true)
            {
                if (ct.ShowError)
                {
                    ErrorPrompt ep = new ErrorPrompt(ct.Error);
                    ep.Show();
                }
                else
                {
                    keyword = "";
                    pageIndex = 1;
                    InsertTable();
                }
            }
        }
        #endregion
    }
}

﻿using System;
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
    public partial class ExamineMangersPage : UserControl
    {
        int pageIndex = 1;
        string keyword = "";
        int groupId = 0;

        public ExamineMangersPage()
        {
            InitializeComponent();
            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("用户名", 180));
            columns.Add(new TableToolColumnImport("用户组", 180));
            columns.Add(new TableToolColumnImport("最后登录", 180));
            columns.Add(new TableToolColumnImport("最后登录IP", 180));
            columns.Add(new TableToolColumnImport("操作", 192));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            ManagerServiceClient client = new ManagerServiceClient();
            client.GetListCompleted += (sender, e) =>
            {
                if (e.Result.Success)
                {
                    int t = 0;
                    e.Result.Content.ForEach(x =>
                    {
                        ExamineMangersPage_TableRow row = new ExamineMangersPage_TableRow(x, t);
                        row.RefreshEventHandler += Refresh;
                        row.SelectByGroupEventHandler += SelectByGroup;
                        rows.Add(row);
                        t++;
                    });
                    TableTool tool = new TableTool("查看管理员信息", e.Result.PageIndex, e.Result.TotalOfPage
                        , columns, rows);
                    tool.NextPageEventHandler += GoNextPage;
                    tableBody.Children.Clear();
                    tableBody.Children.Add(tool);
                }
            };
            client.GetListAsync(keyword, groupId, pageIndex, App.Token);
        }

        void SelectByGroup(object sender, EventArgs e)
        {
            ExamineMangersPage_TableRow row = (ExamineMangersPage_TableRow)sender;
            groupId = row.UserInfo.GroupId;
            pageIndex = 1;

            InsertTable();
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

            keyword = "";
            pageIndex = 1;
            groupId = 0;

            InsertTable();
        }

        private void Create(object sender, EventArgs e)
        {
            ExamineMangersPage_CreateTool ct = new ExamineMangersPage_CreateTool();
            ct.Closed += ShowCreateResult;
            ct.Show();
        }
        #region 创建
        void ShowCreateResult(object sender, EventArgs e)
        {
            ExamineMangersPage_CreateTool ct = (ExamineMangersPage_CreateTool)sender;
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
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
    public partial class ExamineUsersPage : UserControl
    {
        #region 私有变量

        int pageIndex = 1;

        int groupId = 0;

        int teamFor = 0;

        string keyword = "";

        #endregion

        public ExamineUsersPage(int groupId = 0)
        {
            InitializeComponent();
            this.groupId = groupId;
            InsertTable();
        }

        #region 事件

        public event NDelegate ViewLandingReportEventHander;

        public event NDelegate ViewReportEventHandler;

        public event NDelegate GoToCreateNewUserEventHandler;

        #endregion

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("用户名", 100));
            columns.Add(new TableToolColumnImport("用户组", 100));
            columns.Add(new TableToolColumnImport("账户余额", 100));
            columns.Add(new TableToolColumnImport("被冻结", 100));
            columns.Add(new TableToolColumnImport("消费量", 100));
            columns.Add(new TableToolColumnImport("上次登录", 132));
            columns.Add(new TableToolColumnImport("操作", 280));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            UserInfoServiceClient client = new UserInfoServiceClient();
            client.GetListCompleted += (sender, e) =>
                {
                    if (e.Result.Success)
                    {
                        int t = 0;
                        e.Result.Content.ForEach(x =>
                            {
                                ExamineUsersPage_TableBody row = new ExamineUsersPage_TableBody(x, t);
                                row.SelectByGroupEventHandler += SelectByGroup;
                                row.ViewTeamInfoEventHandler += ViewTeamInfo;
                                row.ViewReportEventHandler += ViewReport;
                                row.ViewLandingReportEventHandler += ViewLandingReport;
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
            client.GetListAsync(keyword, groupId, teamFor, pageIndex, App.Token);
        }

        void ViewLandingReport(object sender, EventArgs e)
        {
            if (ViewLandingReportEventHander != null)
            {
                ViewLandingReportEventHander(sender, e);
            }
        }

        void ViewReport(object sender, EventArgs e)
        {
            if (ViewReportEventHandler != null)
            {
                ViewReportEventHandler(sender, e);
            }
        }

        void ViewTeamInfo(object sender, EventArgs e)
        {
            ExamineUsersPage_TableBody row = (ExamineUsersPage_TableBody)sender;
            teamFor = row.UserInfo.UserId;
            pageIndex = 1;
            InsertTable();
        }

        void SelectByGroup(object sender, EventArgs e)
        {
            ExamineUsersPage_TableBody row = (ExamineUsersPage_TableBody)sender;
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
            groupId = 0;
            teamFor = 0;
            pageIndex = 1;
            InsertTable();
        }

        private void GoToCreateNewUser(object sender, EventArgs e)
        {
            if (GoToCreateNewUserEventHandler != null)
            {
                GoToCreateNewUserEventHandler(this, new EventArgs());
            }
        }
    }
}

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
    public partial class ExamineUsersPage_TableBody : UserControl, ITableToolRow
    {
        #region 公开属性

        public UserInfoResult UserInfo { get; set; }

        #endregion

        int _row = 0;

        public ExamineUsersPage_TableBody(UserInfoResult userInfo, int row)
        {
            InitializeComponent();
            this.UserInfo = userInfo;
            this._row = row;
            text_username.Text = userInfo.Username;
            button_group.Text = userInfo.GroupName;
            text_money.Text = userInfo.Money.ToString();
            text_frozen.Text = userInfo.MoneyBeFrozen.ToString();
            text_consumption.Text = userInfo.Consumption.ToString();
            text_lastLoginTime.Text = userInfo.LastLoginTime.ToShortDateString();
            this.SetValue(Grid.RowProperty, row);
        }

        #region 方法

        private void SelectByGroup(object sender, MouseButtonEventArgs e)
        {
            if (SelectByGroupEventHandler != null)
            {
                SelectByGroupEventHandler(this, new EventArgs());
            }
        }

        private void ViewFull(object sender, MouseButtonEventArgs e)
        {
            ExamineUsersPage_FullWindow fw = new ExamineUsersPage_FullWindow(this.UserInfo);
            fw.Closed += (_sender, _e) =>
                {
                    ExamineUsersPage_FullWindow _fw = (ExamineUsersPage_FullWindow)_sender;
                    if (_fw.DialogResult == true)
                    {
                        if (RefreshEventHandler != null)
                        {
                            RefreshEventHandler(this, new EventArgs());
                        }
                    }
                };
            fw.Show();
        }

        private void ViewTeamInfo(object sender, MouseButtonEventArgs e)
        {
            if (ViewTeamInfoEventHandler != null)
            {
                ViewTeamInfoEventHandler(this, new EventArgs());
            }
        }

        private void ViewReport(object sender, MouseButtonEventArgs e)
        {
            if (ViewReportEventHandler != null)
            {
                ViewReportEventHandler(this, new EventArgs());
            }
        }

        private void ViewLandingReport(object sender, MouseButtonEventArgs e)
        {
            if (ViewLandingReportEventHandler != null)
            {
                ViewLandingReportEventHandler(this, new EventArgs());
            }
        }

        private void DeleteUser(object sender, MouseButtonEventArgs e)
        {
            ExamineUsersPage_DeleteUserPrompt prompt = new ExamineUsersPage_DeleteUserPrompt(this.UserInfo.Username);
            prompt.Closed += DeleteUser_Do;
            prompt.Show();
        }
        #region 删除用户
        void DeleteUser_Do(object sender, EventArgs e)
        {
            ExamineUsersPage_DeleteUserPrompt prompt = (ExamineUsersPage_DeleteUserPrompt)sender;
            if (prompt.DialogResult == true)
            {
                UserInfoServiceClient client = new UserInfoServiceClient();
                client.RemoveUserCompleted += ShowDeleteUserResult;
                client.RemoveUserAsync(this.UserInfo.UserId, App.Token);
            }
        }

        void ShowDeleteUserResult(object sender, RemoveUserCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                if (RefreshEventHandler != null)
                {
                    RefreshEventHandler(this, new EventArgs());
                }
            }
            else
            {
                ErrorPrompt prompt = new ErrorPrompt(e.Result.Error);
                prompt.Show();
            }
        }
        #endregion

        private void Hover(object sender, MouseEventArgs e)
        {
            if (HoverEventHandler != null)
            {
                HoverEventHandler(this, new TableToolBodyHoverEventArgs(this._row));
            }
        }

        private void Unhover(object sender, MouseEventArgs e)
        {
            if (UnhoverEventHandler != null)
            {
                UnhoverEventHandler(this, new TableToolBodyHoverEventArgs(this._row));
            }
        }

        #endregion

        #region 事件

        public event NDelegate SelectByGroupEventHandler;

        public event NDelegate ViewTeamInfoEventHandler;

        public event NDelegate ViewReportEventHandler;

        public event NDelegate ViewLandingReportEventHandler;

        public event NDelegate RefreshEventHandler;

        public event TableToolBodyHoverDelegate HoverEventHandler;

        public event TableToolBodyHoverDelegate UnhoverEventHandler;

        #endregion

        public FrameworkElement GetElement()
        {
            return this;
        }

        public FrameworkElement GetChildWindow()
        {
            return null;
        }
    }
}

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
    public partial class ExamineUserGroupPage_TableRow : UserControl, ITableToolRow
    {
        public UserGroupResult Group { get; set; }

        int _row = 0;

        public ExamineUserGroupPage_TableRow(UserGroupResult group, int row)
        {
            InitializeComponent();
            this.Group = group;
            this._row = row;
            button_name.Text = group.Name;
            text_grade.Text = group.Grade.ToString();
            text_limit.Text = group.LimitOfConsumption.ToString();
            text_upper.Text = group.UpperOfConsumption.ToString();
        }

        #region 事件

        public event NDelegate SeleceUserByGroupEventHandler;

        public event TableToolBodyHoverDelegate HoverEventHandler;

        public event TableToolBodyHoverDelegate UnhoverEventHandler;

        public event NDelegate RefreshEventHandler;

        #endregion

        #region 鼠标事件

        private void SeleceUserByGroup(object sender, MouseButtonEventArgs e)
        {
            if (SeleceUserByGroupEventHandler != null)
            {
                SeleceUserByGroupEventHandler(this, new EventArgs());
            }
        }

        private void ViewFull(object sender, MouseButtonEventArgs e)
        {
            ExamineUserGroupPage_FullWindow fw = new ExamineUserGroupPage_FullWindow(this.Group);
            fw.Show();
        }

        private void EditUserGroup(object sender, MouseButtonEventArgs e)
        {
            ExamineUserGroupPage_EditTool et = new ExamineUserGroupPage_EditTool(this.Group);
            et.Closed += ShowEditResult;
            et.Show();
        }
        #region 修改用户组

        void ShowEditResult(object sender, EventArgs e)
        {
            ExamineUserGroupPage_EditTool et = (ExamineUserGroupPage_EditTool)sender;
            if (et.DialogResult == true)
            {
                if (et.ShowError)
                {
                    ErrorPrompt ep = new ErrorPrompt(et.Error);
                    ep.Show();
                }
                else
                {
                    if (RefreshEventHandler != null)
                    {
                        RefreshEventHandler(this, new EventArgs());
                    }
                }
            }
        }

        #endregion

        private void DeleteUserGroup(object sender, MouseButtonEventArgs e)
        {
            string messgage = string.Format("警告：该操作将删除用户组：{0}", this.Group.Name);
            NormalPrompt np = new NormalPrompt(messgage);
            np.Closed += DeleteUserGroup_do;
            np.Show();
        }
        #region 删除用户组

        void DeleteUserGroup_do(object sender, EventArgs e)
        {
            NormalPrompt np = (NormalPrompt)sender;
            if (np.DialogResult == true)
            {
                UserInfoServiceClient client = new UserInfoServiceClient();
                client.RemoveGroupCompleted += ShowDeleteUserGroupResult;
                client.RemoveGroupAsync(this.Group.GroupId, App.Token);
            }
        }

        void ShowDeleteUserGroupResult(object sender, RemoveGroupCompletedEventArgs e)
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
                ErrorPrompt er = new ErrorPrompt(e.Result.Error);
                er.Show();
            }
        }

        #endregion

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

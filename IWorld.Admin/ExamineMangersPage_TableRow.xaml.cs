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
using IWorld.Admin.ManagerService;

namespace IWorld.Admin
{
    public partial class ExamineMangersPage_TableRow : UserControl, ITableToolRow
    {
        public ManagerInfoResult UserInfo { get; set; }
        int _row = 0;

        public ExamineMangersPage_TableRow(ManagerInfoResult userInfo, int row)
        {
            InitializeComponent();
            this.UserInfo = userInfo;
            this._row = row;

            text_username.Text = userInfo.Username;
            button_group.Text = userInfo.Group;
            text_lastLoginTime.Text = userInfo.LastLoginTime.ToLongDateString();
            text_lastLoginIp.Text = userInfo.LastLoginIp;
            if (userInfo.Group == "系统管理员")
            {
                button_delete.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        #region 事件

        public event NDelegate SelectByGroupEventHandler;

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

        private void SelectByGroup(object sender, MouseButtonEventArgs e)
        {
            if (SelectByGroupEventHandler != null)
            {
                SelectByGroupEventHandler(this, new EventArgs());
            }
        }

        private void Delete(object sender, MouseButtonEventArgs e)
        {
            string message = string.Format("警告：该操作将删除管理员 {0}", this.UserInfo.Username);
            NormalPrompt np = new NormalPrompt(message);
            np.Closed += Delete_do;
            np.Show();
        }
        #region 删除

        void Delete_do(object sender, EventArgs e)
        {
            NormalPrompt np = (NormalPrompt)sender;
            if (np.DialogResult == true)
            {
                ManagerServiceClient client = new ManagerServiceClient();
                client.RemoveCompleted += ShowDeleteResult;
                client.RemoveAsync(this.UserInfo.UserId, App.Token);
            }
        }

        void ShowDeleteResult(object sender, RemoveCompletedEventArgs e)
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
                ErrorPrompt ep = new ErrorPrompt(e.Result.Error);
                ep.Show();
            }
        }

        #endregion
    }
}

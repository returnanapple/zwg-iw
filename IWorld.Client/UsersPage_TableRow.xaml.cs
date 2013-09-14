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
    public partial class UsersPage_TableRow : UserControl, ITableToolRow
    {
        BasicUserInfoResult UserInfo { get; set; }
        int _row = 0;
        public UsersPage_TableRow(BasicUserInfoResult userInfo, int row)
        {
            InitializeComponent();
            this.UserInfo = userInfo;
            this._row = row;

            text_username.Text = userInfo.Username;
            text_group.Text = userInfo.Group;
            text_returnPoints.Text = string.Format("{0}% / {1}%", userInfo.NormalReturnPoints, userInfo.UncertainReturnPoints);
            text_money.Text = userInfo.Money.ToString("0.00");
            text_consumption.Text = userInfo.Consumption.ToString("0.00");
            text_status.Text = userInfo.Status.ToString();
        }

        public FrameworkElement GetElement()
        {
            return this;
        }

        public FrameworkElement GetChildWindow()
        {
            return null;
        }

        public event NDelegate RefreshEventHandler;

        public event TableToolBodyHoverDelegate HoverEventHandler;

        public event TableToolBodyHoverDelegate UnhoverEventHandler;

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

        private void ViewFull(object sender, MouseButtonEventArgs e)
        {
            UsersPage_FullWindow fw = new UsersPage_FullWindow(this.UserInfo);
            fw.Closed += DoWork;
            fw.Show();
        }
        #region 操作

        void DoWork(object sender, EventArgs e)
        {
            UsersPage_FullWindow fw = (UsersPage_FullWindow)sender;
            if (fw.DialogResult == true)
            {
                switch (fw.Work)
                {
                    case UsersPage_FullWindow.DoWork.代充值:
                        RechargeTool rt = new RechargeTool(this.UserInfo.UserId, this.UserInfo.Username);
                        rt.Closed += ShowRechargeResult;
                        rt.Show();
                        break;
                    case UsersPage_FullWindow.DoWork.升点:
                        UsersPageUppointsTool ut = new UsersPageUppointsTool(this.UserInfo);
                        ut.Closed += ShowUpPointsResulr;
                        ut.Show();
                        break;
                }
            }
        }
        #region 充值
        void ShowRechargeResult(object sender, EventArgs e)
        {
            RechargeTool rt = (RechargeTool)sender;
            if (rt.DialogResult == true)
            {
                if (rt.ShowError)
                {
                    ErrorPromt ep = new ErrorPromt(rt.Result.Error);
                    ep.Show();
                }
                else
                {
                    RechargeResultTool rrt = new RechargeResultTool(rt.Result);
                    rrt.Closed += Refresh;
                    rrt.Show();
                }
            }
        }
        #endregion
        #region 升点
        void ShowUpPointsResulr(object sender, EventArgs e)
        {
            UsersPageUppointsTool ut = (UsersPageUppointsTool)sender;
            if (ut.ShowError)
            {
                ErrorPromt ep = new ErrorPromt(ut.Error);
                ep.Show();
            }
            else
            {
                ErrorPromt ep = new ErrorPromt("操作成功");
                ep.Closed += Refresh;
                ep.Show();
            }
        }
        #endregion

        void Refresh(object sender, EventArgs e)
        {
            if (RefreshEventHandler != null)
            {
                RefreshEventHandler(this, new EventArgs());
            }
        }

        #endregion
    }
}

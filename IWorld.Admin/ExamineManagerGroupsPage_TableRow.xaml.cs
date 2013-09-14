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
    public partial class ExamineManagerGroupsPage_TableRow : UserControl, ITableToolRow
    {
        ManagerGroupResult Group { get; set; }
        int _row = 0;

        public ExamineManagerGroupsPage_TableRow(ManagerGroupResult group, int row)
        {
            InitializeComponent();
            this.Group = group;
            this._row = row;

            text_name.Text = group.Name;
            text_grade.Text = group.Grade.ToString();
            if (group.Grade == 255)
            {
                button_delete.Visibility = System.Windows.Visibility.Collapsed;
                button_edit.Visibility = System.Windows.Visibility.Collapsed;
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

        private void Edit(object sender, MouseButtonEventArgs e)
        {
            ExamineManagerGroupsPage_EditTool et = new ExamineManagerGroupsPage_EditTool(this.Group);
            et.Closed+=ShowEditResult;
            et.Show();
        }
        #region 编辑

        void ShowEditResult(object sender, EventArgs e)
        {
            ExamineManagerGroupsPage_EditTool et = (ExamineManagerGroupsPage_EditTool)sender;
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

        private void Delete(object sender, MouseButtonEventArgs e)
        {
            string message = string.Format("警告：该操作将删除管理员用户组 {0}，请注意删除前必须清空改组成员", this.Group.Name);
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
                client.RemoveGroupCompleted += ShowDeleteResult;
                client.RemoveGroupAsync(this.Group.GroupId, App.Token);
            }
        }

        void ShowDeleteResult(object sender, RemoveGroupCompletedEventArgs e)
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

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
    public partial class ExamineActivitiesPage_TableRow : UserControl, ITableToolRow
    {
        ActivityResult Activity { get; set; }
        int _row = 0;

        public ExamineActivitiesPage_TableRow(ActivityResult activity, int row)
        {
            InitializeComponent();
            this.Activity = activity;
            this._row = row;

            text_title.Text = activity.Title;
            text_type.Text = activity.Type.ToString();
            text_reward.Text = activity.RewardType.ToString();
            text_beginTime.Text = activity.BeginTime.ToShortDateString();
            text_endTime.Text = activity.EndTime.ToShortDateString();
            text_hide.Text = activity.Hide ? "是" : "否";
        }

        #region 事件

        public event NDelegate RefreshEventHandler;

        public event TableToolBodyHoverDelegate HoverEventHandler;

        public event TableToolBodyHoverDelegate UnhoverEventHandler;

        #endregion

        private void ViewFull(object sender, MouseButtonEventArgs e)
        {
            ExamineActivitiesPage_FullWindow fw = new ExamineActivitiesPage_FullWindow(this.Activity);
            fw.Show();
        }

        private void Edit(object sender, MouseButtonEventArgs e)
        {
            if (this.Activity.BeginTime < DateTime.Now)
            {
                ExamineActivitiesPage_EditTool_Basic et = new ExamineActivitiesPage_EditTool_Basic(this.Activity);
                et.Closed += ShowEditResult_Basic;
                et.Show();
            }
            else
            {
                ExamineActivitiesPage_EditTool et = new ExamineActivitiesPage_EditTool(this.Activity);
                et.Closed += ShowEditResult;
                et.Show();
            }
        }
        #region 编辑

        void ShowEditResult_Basic(object sender, EventArgs e)
        {
            ExamineActivitiesPage_EditTool_Basic et = (ExamineActivitiesPage_EditTool_Basic)sender;
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

        void ShowEditResult(object sender, EventArgs e)
        {
            ExamineActivitiesPage_EditTool et = (ExamineActivitiesPage_EditTool)sender;
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
            string message = string.Format("警告：该操作将删除活动 {0}", this.Activity.Title);
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
                ActivityServiceClient client = new ActivityServiceClient();
                client.RemoveActivityCompleted += ShowDeleteResult;
                client.RemoveActivityAsync(this.Activity.ActivityId, App.Token);
            }
        }

        void ShowDeleteResult(object sender, RemoveActivityCompletedEventArgs e)
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
    }
}

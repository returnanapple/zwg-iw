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
using IWorld.Admin.ActivityService;

namespace IWorld.Admin
{
    public partial class ExamineActivitiesPage_EditTool_Basic : ChildWindow
    {
        public bool ShowError { get; set; }
        public string Error { get; set; }
        ActivityResult Activity { get; set; }

        public ExamineActivitiesPage_EditTool_Basic(ActivityResult activity)
        {
            InitializeComponent();
            this.ShowError = false;
            this.Error = "";
            this.Activity = activity;

            input_days.Text = activity.Days.ToString();
            input_autoDelete.SelectedIndex = activity.AutoDelete ? 0 : 1;
            input_hide.SelectedIndex = activity.AutoDelete ? 0 : 1;
        }

        private void Edit(object sender, EventArgs e)
        {
            EditActivityImport_Basic import = new EditActivityImport_Basic
            {
                ActivityId = this.Activity.ActivityId,
                Days = Convert.ToInt32(input_days.Text),
                AutoDelete = ((TextBlock)input_autoDelete.SelectedItem).Text == "是",
                Hide = ((TextBlock)input_hide.SelectedItem).Text == "是"

            };
            ActivityServiceClient client = new ActivityServiceClient();
            client.EditActivity_BasicCompleted += ShowEditResult;
            client.EditActivity_BasicAsync(import, App.Token);
        }
        #region 编辑
        void ShowEditResult(object sender, EditActivity_BasicCompletedEventArgs e)
        {
            if (!e.Result.Success)
            {
                this.ShowError = true;
                this.Error = e.Result.Error;
            }
            this.DialogResult = true;
        }
        #endregion

        private void BackToList(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


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
    public partial class ExamineActivitiesPage_FullWindow : ChildWindow
    {
        public ExamineActivitiesPage_FullWindow(ActivityResult activity)
        {
            InitializeComponent();

            text_title.Text = activity.Title;
            text_type.Text = activity.Type.ToString();
            text_minRestrictionValue.Text = activity.MinRestrictionValue.ToString("0.00");
            text_maxRestrictionValues.Text = activity.MaxRestrictionValues.ToString("0.00");
            text_rewardType.Text = activity.RewardType.ToString();
            text_rewardValueIsAbsolute.Text = activity.RewardValueIsAbsolute ? "绝对值" : "百分比";
            text_reward.Text = activity.Reward.ToString("0.00");
            text_beginTime.Text = activity.BeginTime.ToLongDateString();
            text_days.Text = activity.Days.ToString(); ;
            text_endTime.Text = activity.EndTime.ToLongDateString();
            text_hide.Text = activity.Hide ? "是" : "否";
            text_autoDelete.Text = activity.AutoDelete ? "是" : "否";

            double tHeight = root.Height;
            double rowHeight = 30;
            if (activity.Conditions.Count > 1)
            {
                root.Height = tHeight + (activity.Conditions.Count - 1) * rowHeight;
            }

            tool_showConditions.RowDefinitions.Clear();
            tool_showConditions.Children.Clear();

            int t = 0;
            activity.Conditions.ForEach(x =>
                {
                    RowDefinition rd = new RowDefinition();
                    rd.Height = new GridLength(rowHeight);
                    tool_showConditions.RowDefinitions.Add(rd);

                    TextBlock tb1 = new TextBlock();
                    tb1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    tb1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb1.SetValue(Grid.ColumnProperty, 0);
                    tb1.SetValue(Grid.RowProperty, t);
                    tb1.Text = x.Type.ToString();
                    tool_showConditions.Children.Add(tb1);

                    TextBlock tb2 = new TextBlock();
                    tb2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    tb2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb2.SetValue(Grid.ColumnProperty, 1);
                    tb2.SetValue(Grid.RowProperty, t);
                    tb2.Text = x.Limit.ToString();
                    tool_showConditions.Children.Add(tb2);

                    TextBlock tb3 = new TextBlock();
                    tb3.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    tb3.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb3.SetValue(Grid.ColumnProperty, 2);
                    tb3.SetValue(Grid.RowProperty, t);
                    tb3.Text = x.Upper.ToString();
                    tool_showConditions.Children.Add(tb3);

                    t++;
                });

            root.MouseWheel += VerticalScroll;
        }
        #region 滚屏

        /// <summary>
        /// 竖向滚屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void VerticalScroll(object sender, MouseWheelEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            UIHelper.VerticalScroll(fe, e);
        }

        #endregion

        private void BackToList(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


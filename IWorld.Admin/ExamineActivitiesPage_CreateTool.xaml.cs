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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IWorld.Admin.Class;
using IWorld.Admin.ActivityService;

namespace IWorld.Admin
{
    public partial class ExamineActivitiesPage_CreateTool : ChildWindow
    {
        Dictionary<string, EditConditionImport> Conditions { get; set; }
        double allHeight = 0;
        double addToolTop = 0;
        public bool ShowError { get; set; }
        public string Error { get; set; }

        public ExamineActivitiesPage_CreateTool()
        {
            InitializeComponent();
            this.ShowError = false;
            this.Error = "";
            Conditions = new Dictionary<string, EditConditionImport>();

            allHeight = root.Height;
            addToolTop = (double)tool_AddConditions.GetValue(Canvas.TopProperty);

            List<TextBlock> _type = new List<TextBlock>();
            Enum.GetNames(typeof(ActivityType)).ToList().ForEach(x =>
                {
                    TextBlock tb = new TextBlock { Text = x };
                    _type.Add(tb);
                });
            input_type.ItemsSource = _type;
            input_type.SelectedIndex = 0;

            List<TextBlock> _rewardType = new List<TextBlock>();
            Enum.GetNames(typeof(ActivityRewardType)).ToList().ForEach(x =>
                {
                    TextBlock tb = new TextBlock { Text = x };
                    _rewardType.Add(tb);
                });
            input_rewardType.ItemsSource = _rewardType;
            input_rewardType.SelectedIndex = 0;

            List<TextBlock> _conditionType = new List<TextBlock>();
            Enum.GetNames(typeof(ConditionType)).ToList().ForEach(x =>
                {
                    TextBlock tb = new TextBlock { Text = x };
                    _conditionType.Add(tb);
                });
            input_conditionType.ItemsSource = _conditionType;
            input_conditionType.SelectedIndex = 0;

            root.MouseWheel += VerticalScroll;
            ShowConditions();
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

        void ShowConditions()
        {
            double rowHeighr = 30;
            root.Height = allHeight + (Conditions.Count - 1) * rowHeighr;
            tool_AddConditions.SetValue(Canvas.TopProperty, addToolTop + (Conditions.Count - 1) * rowHeighr);

            tool_showConditions.RowDefinitions.Clear();
            tool_showConditions.Children.Clear();

            int t = 0;
            Conditions.Keys.ToList().ForEach(key =>
                {
                    RowDefinition rd = new RowDefinition();
                    rd.Height = new GridLength(rowHeighr);
                    tool_showConditions.RowDefinitions.Add(rd);

                    TextBlock tb1 = new TextBlock();
                    tb1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    tb1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb1.SetValue(Grid.ColumnProperty, 0);
                    tb1.SetValue(Grid.RowProperty, t);
                    tb1.Text = Conditions[key].Type.ToString();
                    tool_showConditions.Children.Add(tb1);

                    TextBlock tb2 = new TextBlock();
                    tb2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    tb2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb2.SetValue(Grid.ColumnProperty, 1);
                    tb2.SetValue(Grid.RowProperty, t);
                    tb2.Text = Conditions[key].Limit.ToString();
                    tool_showConditions.Children.Add(tb2);

                    TextBlock tb3 = new TextBlock();
                    tb3.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    tb3.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb3.SetValue(Grid.ColumnProperty, 2);
                    tb3.SetValue(Grid.RowProperty, t);
                    tb3.Text = Conditions[key].Upper.ToString();
                    tool_showConditions.Children.Add(tb3);

                    Image img = new Image();
                    img.Name = key;
                    img.Width = 20;
                    img.Height = 20;
                    img.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    img.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    img.SetValue(Grid.ColumnProperty, 3);
                    img.SetValue(Grid.RowProperty, t);
                    img.Source = new BitmapImage(new Uri("img/cross_small.png", UriKind.Relative));
                    img.MouseLeftButtonDown += RemoveCondition;
                    img.Cursor = Cursors.Hand;
                    tool_showConditions.Children.Add(img);

                    t++;
                });
        }

        private void AddCondition(object sender, EventArgs e)
        {
            EditConditionImport import = new EditConditionImport
            {
                Type = (ConditionType)Enum.Parse(typeof(ConditionType)
                    , ((TextBlock)input_conditionType.SelectedItem).Text, false),
                Limit = Convert.ToDouble(input_conditionLimit.Text),
                Upper = Convert.ToDouble(input_conditionUpper.Text)
            };
            string t = Guid.NewGuid().ToString("N");
            Conditions.Add(t, import);

            input_conditionType.SelectedIndex = 0;
            input_conditionLimit.Text = "";
            input_conditionUpper.Text = "";

            ShowConditions();
        }

        private void RemoveCondition(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            Conditions.Remove(fe.Name);

            ShowConditions();
        }

        private void Create(object sender, EventArgs e)
        {
            AddActivityImport import = new AddActivityImport
            {
                Title = input_title.Text,
                Type = (ActivityType)Enum.Parse(typeof(ActivityType)
                    , ((TextBlock)input_type.SelectedItem).Text, false),
                MinRestrictionValue = Convert.ToDouble(input_minRestrictionValue.Text),
                MaxRestrictionValues = Convert.ToDouble(input_maxRestrictionValues.Text),
                RewardType = (ActivityRewardType)Enum.Parse(typeof(ActivityRewardType)
                    , ((TextBlock)input_rewardType.SelectedItem).Text, false),
                RewardValueIsAbsolute = ((TextBlock)input_rewardValueIsAbsolute.SelectedItem).Text == "绝对值",
                Reward = Convert.ToDouble(input_reward.Text),
                Conditions = this.Conditions.Values.ToList(),
                BeginTime = input_beginTime.Text,
                Days = Convert.ToInt32(input_days.Text),
                AutoDelete = ((TextBlock)input_autoDelete.SelectedItem).Text == "是"
            };
            ActivityServiceClient client = new ActivityServiceClient();
            client.AddActivityCompleted += ShowCreateResult;
            client.AddActivityAsync(import, App.Token);
        }
        #region 创建
        void ShowCreateResult(object sender, AddActivityCompletedEventArgs e)
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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IWorld.Admin.Class;
using IWorld.Admin.ExchangeService;

namespace IWorld.Admin
{
    public partial class ExamineExchangesPage_CreateTool : ChildWindow
    {
        Dictionary<string, EditConditionImport> Conditions { get; set; }
        Dictionary<string, EditPrizeImport> Prizes { get; set; }
        double allHeight = 0;
        double conditionAddToolTop = 0;
        double prizeShowToolTop = 0;
        double prizeAddToolTop = 0;
        public bool ShowError { get; set; }
        public string Error { get; set; }

        public ExamineExchangesPage_CreateTool()
        {
            InitializeComponent();
            this.ShowError = false;
            this.Error = "";
            Conditions = new Dictionary<string, EditConditionImport>();
            Prizes = new Dictionary<string, EditPrizeImport>();

            allHeight = root.Height;
            conditionAddToolTop = (double)tool_AddConditions.GetValue(Canvas.TopProperty);
            prizeShowToolTop = (double)tool_showPrize.GetValue(Canvas.TopProperty);
            prizeAddToolTop = (double)tool_AddPrize.GetValue(Canvas.TopProperty);

            List<TextBlock> _conditionType = new List<TextBlock>();
            Enum.GetNames(typeof(ConditionType)).ToList().ForEach(x =>
                {
                    TextBlock tb = new TextBlock { Text = x };
                    _conditionType.Add(tb);
                });
            input_conditionType.ItemsSource = _conditionType;
            input_conditionType.SelectedIndex = 0;

            List<TextBlock> _prizeType = new List<TextBlock>();
            Enum.GetNames(typeof(PrizeType)).ToList().ForEach(x =>
            {
                TextBlock tb = new TextBlock { Text = x };
                _prizeType.Add(tb);
            });
            input_prizeType.ItemsSource = _prizeType;
            input_prizeType.SelectedIndex = 0;

            root.MouseWheel += VerticalScroll;
            ShowConditionsAndPrizes();
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

        void ShowConditionsAndPrizes()
        {
            double rowHeighr = 30;
            double conditionShowToolHeight = (Conditions.Count - 1) * rowHeighr;
            double prizeShowToolHeight = (Prizes.Count - 1) * rowHeighr;

            root.Height = allHeight + conditionShowToolHeight + prizeShowToolHeight;
            tool_AddConditions.SetValue(Canvas.TopProperty, conditionAddToolTop + conditionShowToolHeight);
            tool_showPrize.SetValue(Canvas.TopProperty, prizeShowToolTop + conditionShowToolHeight);
            tool_AddPrize.SetValue(Canvas.TopProperty, prizeAddToolTop + conditionShowToolHeight + prizeShowToolHeight);

            tool_showConditions.RowDefinitions.Clear();
            tool_showConditions.Children.Clear();

            int t = 0;
            #region 显示已添加的限制条件
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
            #endregion

            tool_showPrize.RowDefinitions.Clear();
            tool_showPrize.Children.Clear();

            t = 0;
            #region 显示已添加的奖品
            Prizes.Keys.ToList().ForEach(key =>
                {
                    RowDefinition rd = new RowDefinition();
                    rd.Height = new GridLength(rowHeighr);
                    tool_showConditions.RowDefinitions.Add(rd);

                    TextBlock tb1 = new TextBlock();
                    tb1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    tb1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb1.SetValue(Grid.ColumnProperty, 0);
                    tb1.SetValue(Grid.RowProperty, t);
                    tb1.Text = Prizes[key].Name;
                    tool_showPrize.Children.Add(tb1);

                    TextBlock tb2 = new TextBlock();
                    tb2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    tb2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb2.SetValue(Grid.ColumnProperty, 1);
                    tb2.SetValue(Grid.RowProperty, t);
                    tb2.Text = Prizes[key].Price.ToString();
                    tool_showPrize.Children.Add(tb2);

                    Image img = new Image();
                    img.Name = key;
                    img.Width = 20;
                    img.Height = 20;
                    img.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    img.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    img.SetValue(Grid.ColumnProperty, 2);
                    img.SetValue(Grid.RowProperty, t);
                    img.Source = new BitmapImage(new Uri("img/cross_small.png", UriKind.Relative));
                    img.MouseLeftButtonDown += RemovePrize;
                    img.Cursor = Cursors.Hand;
                    tool_showPrize.Children.Add(img);

                    t++;
                });
            #endregion
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

            ShowConditionsAndPrizes();
        }

        private void RemoveCondition(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            Conditions.Remove(fe.Name);

            ShowConditionsAndPrizes();
        }

        private void AddPrize(object sender, EventArgs e)
        {
            EditPrizeImport import = new EditPrizeImport
            {
                Name = input_prizeName.Text,
                Description = input_prizeDescription.Text,
                Sum = Convert.ToInt32(input_prizeSum.Text),
                Type = (PrizeType)Enum.Parse(typeof(PrizeType)
                    , ((TextBlock)input_prizeType.SelectedItem).Text, false),
                Price = Convert.ToDouble(input_prizePrice.Text),
                Remark = input_prizeRemark.Text
            };
            string t = Guid.NewGuid().ToString("N");
            Prizes.Add(t, import);

            input_prizeName.Text = "";
            input_prizeDescription.Text = "";
            input_prizeSum.Text = "";
            input_prizeType.SelectedIndex = 0;
            input_prizePrice.Text = "";
            input_prizeRemark.Text = "";

            ShowConditionsAndPrizes();
        }

        private void RemovePrize(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            Prizes.Remove(fe.Name);

            ShowConditionsAndPrizes();
        }

        private void Create(object sender, EventArgs e)
        {
            AddExchangeImport import = new AddExchangeImport
            {
                Name = input_name.Text,
                Places = Convert.ToInt32(input_places.Text),
                UnitPrice = Convert.ToDouble(input_unitPrice.Text),
                EachPersonCanExchangeTheNumberOfTimes = Convert.ToInt32(input_eachPersonCanExchangeTheNumberOfTimes.Text),
                EachPersonCanExchangeTheTimesOfDays = Convert.ToInt32(input_eachPersonCanExchangeTheTimesOfDays.Text),
                EachPersonCanExchangeTheNumberOfDays = Convert.ToInt32(input_eachPersonCanExchangeTheNumberOfDays.Text),
                EachPersonCanExchangeTheTimesOfAll = Convert.ToInt32(input_eachPersonCanExchangeTheTimesOfAll.Text),
                EachPersonCanExchangeTheNumberOfAll = Convert.ToInt32(input_eachPersonCanExchangeTheNumberOfAll.Text),
                Conditions = this.Conditions.Values.ToList(),
                Prizes = this.Prizes.Values.ToList(),
                BeginTime = input_beginTime.Text,
                Days = Convert.ToInt32(input_days.Text),
                AutoDelete = ((TextBlock)input_autoDelete.SelectedItem).Text == "是"
            };
            ExchangeServiceClient client = new ExchangeServiceClient();
            client.AddExchangeCompleted += ShowCreateResult;
            client.AddExchangeAsync(import, App.Token);
        }
        #region 创建
        void ShowCreateResult(object sender, AddExchangeCompletedEventArgs e)
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


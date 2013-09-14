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
using IWorld.Admin.DataReportService;
using IWorld.Admin.SystemSettingService;
using Visifire.Charts;

namespace IWorld.Admin
{
    /// <summary>
    /// 首页
    /// </summary>
    public partial class IndexPage : UserControl
    {
        /// <summary>
        /// 采集程序运行状态
        /// </summary>
        bool collectionRunning = true;

        /// <summary>
        /// 实例化一个新的首页
        /// </summary>
        public IndexPage()
        {
            InitializeComponent();
            InitializeMouseEvent();
            InsertDate();
        }

        #region 鼠标事件

        /// <summary>
        /// 初始化鼠标事件
        /// </summary>
        void InitializeMouseEvent()
        {
            #region 鼠标悬停
            List<FrameworkElement> buttons = new List<FrameworkElement>();
            buttons.Add(button_editManagerAccount);
            buttons.Add(button_clearUsers);
            buttons.Add(button_closeOrOpenCollection);
            buttons.Add(button_dataReport);
            buttons.Add(button_setting);
            buttons.Add(button_logout);

            buttons.ForEach(button =>
                {
                    button.MouseEnter += UIHelper.NButtonTrigger;
                    button.MouseLeave += UIHelper.NButtonReply;
                    button.MouseLeftButtonDown += UIHelper.NButtonReply;
                });
            #endregion

            button_editManagerAccount.MouseLeftButtonDown += ShowEditManagerAccountTool;
            button_clearUsers.MouseLeftButtonDown += ClearUsers;
            button_closeOrOpenCollection.MouseLeftButtonDown += CloseOrOpenCollection;
            button_dataReport.MouseLeftButtonDown += GoToDataReportPage;
            button_setting.MouseLeftButtonDown += GoToSystemSettingPage;
            button_logout.MouseLeftButtonDown += Logout;
        }

        #region 动作

        /// <summary>
        /// 显示管理员信息编辑工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ShowEditManagerAccountTool(object sender, MouseButtonEventArgs e)
        {
            IndexPage_EditManagerAccountWindow eaw = new IndexPage_EditManagerAccountWindow();
            eaw.Closed += EditManagerAccount_Do;
            eaw.Show();
        }
        #region 执行动作

        /// <summary>
        /// 编辑管理员信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void EditManagerAccount_Do(object sender, EventArgs e)
        {
            IndexPage_EditManagerAccountWindow cw = (IndexPage_EditManagerAccountWindow)sender;
            if (cw.DialogResult == true)
            {
                App.ShowCover();
                ManagerServiceClient client = new ManagerServiceClient();
                client.ResetPassageCompleted += ShowEditManagerAccountResult;
                client.ResetPassageAsync(0, cw.OldPassword, cw.NewPassword, App.Token);
            }
        }

        /// <summary>
        /// 反馈编辑管理员信息的结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ShowEditManagerAccountResult(object sender, ResetPassageCompletedEventArgs e)
        {
            ChildWindow cw = new ChildWindow();
            cw.Width = 240;
            cw.Height = 160;
            cw.Opacity = 0.96;
            TextBlock tb = new TextBlock();
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            if (e.Result.Success)
            {
                cw.Title = "操作成功";
                tb.Text = "密码修改成功，请重新登陆";
                cw.Closed += BackToLoginAgain;
            }
            else
            {
                cw.Title = "操作失败";
                tb.Text = e.Result.Error;
            }

            App.HideCover();
            cw.Content = tb;
            cw.Show();
        }

        /// <summary>
        /// 重新登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BackToLoginAgain(object sender, EventArgs e)
        {
            App.GoToLoginPage();
        }

        #endregion

        /// <summary>
        /// 强制下线前台已经登录的用户并重置缓存池
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ClearUsers(object sender, MouseButtonEventArgs e)
        {
            IndexPage_ClearUserPrompt t = new IndexPage_ClearUserPrompt();
            t.Closed += ClearUsers_Do;
            t.Show();
        }
        #region 执行动作

        /// <summary>
        /// 执行强制下线前台已经登录的用户并重置缓存池动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ClearUsers_Do(object sender, EventArgs e)
        {
            ChildWindow cw = (ChildWindow)sender;
            if (cw.DialogResult == true)
            {
                App.ShowCover();

                SystemSettingServiceClient client = new SystemSettingServiceClient();
                client.ClearCachePondCompleted += ShowClearUsersResult;
                client.ClearCachePondAsync(App.Token);
            }
        }

        /// <summary>
        /// 反馈强制下线前台已经登录的用户并重置缓存池的结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ShowClearUsersResult(object sender, ClearCachePondCompletedEventArgs e)
        {
            ChildWindow cw = new ChildWindow();
            cw.Width = 240;
            cw.Height = 160;
            cw.Opacity = 0.96;
            TextBlock tb = new TextBlock();
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            if (e.Result.Success)
            {
                cw.Title = "操作成功";
                tb.Text = "前台用户清理完成\n缓存池重置完成\n请检查数据是否正确";
            }
            else
            {
                cw.Title = "操作失败";
                tb.Text = "缓存系统连接失败，请稍后再试";
            }

            App.HideCover();
            cw.Content = tb;
            cw.Show();
        }

        #endregion

        /// <summary>
        /// 打开或关闭数据采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CloseOrOpenCollection(object sender, MouseButtonEventArgs e)
        {
            App.ShowCover();
            SystemSettingServiceClient client = new SystemSettingServiceClient();
            client.CloseOrOpenCollectionCompleted += WriteCloseOrOpenCollectionResult;
            client.CloseOrOpenCollectionAsync(this.collectionRunning, App.Token);
        }
        #region 操作结果

        /// <summary>
        /// 反馈打开或关闭数据采集操作结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WriteCloseOrOpenCollectionResult(object sender, CloseOrOpenCollectionCompletedEventArgs e)
        {
            string message = "";

            if (e.Result.Success)
            {
                this.collectionRunning = !this.collectionRunning;
                button_closeOrOpenCollection.Text = this.collectionRunning ? "关闭" : "打开";
                message = this.collectionRunning ? "数据采集系统已经打开" : "数据采集系统已经关闭";
            }
            else
            {
                message = "数据采集系统连接失败，请稍后再试";
            }

            App.HideCover();
            ErrorPrompt ep = new ErrorPrompt(message);
            ep.Show();
        }

        #endregion

        /// <summary>
        /// 跳转到数据报表页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GoToDataReportPage(object sender, MouseButtonEventArgs e)
        {
            if (GoToDataReportPageEventHandler != null)
            {
                GoToDataReportPageEventHandler(this, new EventArgs());
            }
        }

        /// <summary>
        /// 跳转到系统设置页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GoToSystemSettingPage(object sender, MouseButtonEventArgs e)
        {
            if (GoToSystemSettingPageEventHandler != null)
            {
                GoToSystemSettingPageEventHandler(this, new EventArgs());
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Logout(object sender, MouseButtonEventArgs e)
        {
            MainPage mainPage = (MainPage)App.Current.RootVisual;
            mainPage.ShowCover();
            try
            {
                ManagerServiceClient client = new ManagerServiceClient();
                client.LogoutCompleted += (_sender, _e) =>
                {
                    mainPage.GoToLoginPage();
                };
                client.LogoutAsync(App.Token);
            }
            catch (Exception)
            {
                mainPage.GoToLoginPage();
            }
        }

        #endregion

        #region 事件

        /// <summary>
        /// 查看数据报表按键被触发的时候将触发的事件
        /// </summary>
        public event NDelegate GoToDataReportPageEventHandler;

        /// <summary>
        /// 修改系统设置按键被触发的时候将触发的事件
        /// </summary>
        public event NDelegate GoToSystemSettingPageEventHandler;

        #endregion

        #endregion

        #region 插入数据

        /// <summary>
        /// 插入页面数据
        /// </summary>
        private void InsertDate()
        {
            SystemSettingServiceClient sClient = new SystemSettingServiceClient();
            sClient.GetCollectionStatusResultCompleted += WriteCollectionStatus;
            sClient.GetCollectionStatusResultAsync(App.Token);

            DataReportServiceClient dClient = new DataReportServiceClient();
            dClient.GetComprehensiveInformationCompleted += WriteTheDataToPage;
            dClient.GetComprehensiveInformationAsync(App.Token);
        }

        /// <summary>
        /// 写入数据采集程序的运行状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WriteCollectionStatus(object sender, GetCollectionStatusResultCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                this.collectionRunning = e.Result.Running;
                button_closeOrOpenCollection.Text = e.Result.Running ? "关闭" : "打开";
            }
            else
            {
                button_closeOrOpenCollection.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 将数据写入页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WriteTheDataToPage(object sender, GetComprehensiveInformationCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                #region 投注额/奖金/活动返还/盈亏

                text_amountOfBetsAtDay.Text = e.Result.AmountOfBetsAtDay.ToString();
                text_amountOfBetsAtMonth.Text = e.Result.AmountOfBetsAtMonth.ToString();
                text_amountOfBetsAtAll.Text = e.Result.AmountOfBetsAtAll.ToString();

                text_bonusAtDay.Text = e.Result.BonusAtDay.ToString();
                text_bonusAtMonth.Text = e.Result.BonusAtMonth.ToString();
                text_bonusAtAll.Text = e.Result.BonusAtAll.ToString();

                text_expendituresAtDay.Text = e.Result.ExpendituresAtDay.ToString();
                text_expendituresAtMonth.Text = e.Result.ExpendituresAtMonth.ToString();
                text_expendituresAtAll.Text = e.Result.ExpendituresAtAll.ToString();

                text_gainsAndLossesAtDay.Text = e.Result.GainsAndLossesAtDay.ToString();
                text_gainsAndLossesAtMonth.Text = e.Result.GainsAndLossesAtMonth.ToString();
                text_gainsAndLossesAtAll.Text = e.Result.GainsAndLossesAtAll.ToString();

                if (e.Result.GainsAndLossesAtDay < 0)
                {
                    text_gainsAndLossesAtDay.Foreground = new SolidColorBrush(Colors.Green);
                }
                if (e.Result.GainsAndLossesAtMonth < 0)
                {
                    text_gainsAndLossesAtMonth.Foreground = new SolidColorBrush(Colors.Green);
                }
                if (e.Result.GainsAndLossesAtAll < 0)
                {
                    text_gainsAndLossesAtAll.Foreground = new SolidColorBrush(Colors.Green);
                }

                #endregion

                #region 充值/提现/支取/现金流

                text_rechargeAtDay.Text = e.Result.RechargeAtDay.ToString();
                text_rechargeAtMonth.Text = e.Result.RechargeAtMonth.ToString();
                text_rechargeAtAll.Text = e.Result.RechargeAtAll.ToString();

                text_withdrawalAtDay.Text = e.Result.WithdrawalAtDay.ToString();
                text_withdrawalAtMonth.Text = e.Result.WithdrawalAtMonth.ToString();
                text_withdrawalAtAll.Text = e.Result.WithdrawalAtAll.ToString();

                text_transferAtDay.Text = e.Result.TransferAtDay.ToString();
                text_transferAtMonth.Text = e.Result.TransferAtMonth.ToString();
                text_transferAtAll.Text = e.Result.TransferAtAll.ToString();

                text_cashAtDay.Text = e.Result.CashAtDay.ToString();
                text_cashAtMonth.Text = e.Result.CashAtMonth.ToString();
                text_cashAtAll.Text = e.Result.CashAtAll.ToString();

                if (e.Result.CashAtDay < 0)
                {
                    text_cashAtDay.Foreground = new SolidColorBrush(Colors.Green);
                }
                if (e.Result.CashAtMonth < 0)
                {
                    text_cashAtMonth.Foreground = new SolidColorBrush(Colors.Green);
                }
                if (e.Result.CashAtAll < 0)
                {
                    text_cashAtAll.Foreground = new SolidColorBrush(Colors.Green);
                }

                #endregion

                InsertDateReport(e.Result.CountOfUserLoginAtLast2Week);
            }
        }

        #region 插入数据报表

        /// <summary>
        /// 插入数据报表
        /// </summary>
        /// <param name="countOfUserLoginAtLast2Week">数据集</param>
        private void InsertDateReport(List<int> countOfUserLoginAtLast2Week)
        {
            ReportFigure rf = ReportFigure.CreateChart();
            rf.Width = 912;
            rf.Height = 500;
            rf.SetValue(Grid.RowProperty, 1);
            rf.View3D = true;

            DataSeries dataSeries = new DataSeries();
            dataSeries.Name = "";
            dataSeries.RenderAs = RenderAs.StackedArea;

            for (int i = 0; i < countOfUserLoginAtLast2Week.Count; i++)
            {
                DataPoint dp = new DataPoint();
                DateTime time = DateTime.Now.AddDays(-countOfUserLoginAtLast2Week.Count - 1 + i);
                dp.AxisXLabel = string.Format("{0}月{1}日", time.Month, time.Day);
                dp.XValue = i;
                dp.YValue = countOfUserLoginAtLast2Week[i];

                dataSeries.DataPoints.Add(dp);
            }

            rf.Series.Add(dataSeries);
            frame_report.Children.Add(rf);
        }

        #endregion

        #endregion
    }
}

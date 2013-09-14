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

namespace IWorld.Admin
{
    public partial class SetUpPage : UserControl
    {
        #region 功能键

        #region 相关引用

        /// <summary>
        /// 当前激活的顶栏功能键
        /// </summary>
        SetUpPageTopButtonS ActivationTopButton { get; set; }

        /// <summary>
        /// 当前激活的侧边栏功能键
        /// </summary>
        SetUpPageSideButtons ActivationSideButton { get; set; }

        /// <summary>
        /// 顶栏功能按键组
        /// </summary>
        List<FrameworkElement> TopButtons { get; set; }

        /// <summary>
        /// 侧边栏功能按键组
        /// </summary>
        List<FrameworkElement> SideButtons { get; set; }

        #endregion

        /// <summary>
        /// 初始化功能按键组
        /// </summary>
        void InitializeFunctionKey()
        {
            #region 初始化引用列表

            this.ActivationTopButton = SetUpPageTopButtonS.topButton_GoToIndex;
            this.ActivationSideButton = SetUpPageSideButtons.none;

            this.TopButtons = new List<FrameworkElement>();
            this.TopButtons.Add(topButton_GoToIndex);
            this.TopButtons.Add(topButton_manageUser);
            this.TopButtons.Add(topButton_manageTicket);
            this.TopButtons.Add(topButton_manageBulletin);
            this.TopButtons.Add(topButton_manageActivity);
            this.TopButtons.Add(topButton_dataReport);
            this.TopButtons.Add(topButton_setting);
            this.TopButtons.Add(topButton_manager);

            this.SideButtons = new List<FrameworkElement>();
            this.SideButtons.Add(sideButton_examineUsers);
            this.SideButtons.Add(sideButton_addNewUser);
            this.SideButtons.Add(sideButton_examineUserGroups);
            this.SideButtons.Add(sideButton_landingRecords);

            this.SideButtons.Add(sideButton_examineTickets);
            this.SideButtons.Add(sideButton_examineTags);
            this.SideButtons.Add(sideButton_examinePatterns);
            this.SideButtons.Add(sideButton_examineLottery);
            this.SideButtons.Add(sideButton_examineVirtualTop);

            this.SideButtons.Add(sideButton_examineBulletins);
            this.SideButtons.Add(sideButton_addNewBulletin);

            this.SideButtons.Add(sideButton_examineActivities);
            this.SideButtons.Add(sideButton_participatedRecordOfActivities);
            this.SideButtons.Add(sideButton_examineExchanges);
            this.SideButtons.Add(sideButton_participatedRecordOfExchanges);

            this.SideButtons.Add(sideButton_siteReports);
            this.SideButtons.Add(sideButton_personalReports);
            this.SideButtons.Add(sideButton_bettingReports);
            this.SideButtons.Add(sideButton_rechargeReports);
            this.SideButtons.Add(sideButton_withdrawalReports);
            this.SideButtons.Add(sideButton_transferReports);

            this.SideButtons.Add(sideButton_webiteSetting);
            this.SideButtons.Add(sideButton_bankAccounts);
            this.SideButtons.Add(sideButton_emailAccounts);
            this.SideButtons.Add(sideButton_emailHosts);

            this.SideButtons.Add(sideButton_examineMangers);
            this.SideButtons.Add(sideButton_examineManagerGroups);
            this.SideButtons.Add(sideButton_managerLandingRecords);
            this.SideButtons.Add(sideButton_managedRecords);

            #endregion

            #region 初始化功能键鼠标事件

            //顶栏
            foreach (var i in this.TopButtons)
            {
                i.MouseEnter += TopButtonHover;
                i.MouseLeave += TopButtonToNormal;
                i.MouseLeftButtonDown += TopButtonClick;
            }

            //标签栏
            foreach (var i in this.SideButtons)
            {
                i.MouseEnter += SideButtonHover;
                i.MouseLeave += SideButtonToNormal;
                i.MouseLeftButtonDown += SideButtonClick;
            }

            #endregion
        }

        #region 功能键反馈

        #region 顶栏

        /// <summary>
        /// 顶栏功能键激活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TopButtonHover(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            SetUpPageTopButtonS button = (SetUpPageTopButtonS)Enum.Parse(typeof(SetUpPageTopButtonS), label.Name, true);
            if (button != this.ActivationTopButton)
            {
                label.Style = (Style)this.Resources["top_hover"];
            }
        }

        /// <summary>
        /// 顶栏功能键还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TopButtonToNormal(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            SetUpPageTopButtonS button = (SetUpPageTopButtonS)Enum.Parse(typeof(SetUpPageTopButtonS), label.Name, true);
            if (button != this.ActivationTopButton)
            {
                label.Style = (Style)this.Resources["top_normal"];
            }
        }

        /// <summary>
        /// 顶栏功能键触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TopButtonClick(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            SetUpPageTopButtonS button = (SetUpPageTopButtonS)Enum.Parse(typeof(SetUpPageTopButtonS), fe.Name, true);
            if (button != this.ActivationTopButton)
            {
                SelectTopButton(fe, button);

                switch (button)
                {
                    case SetUpPageTopButtonS.topButton_GoToIndex:
                        SelectSideButton(null, SetUpPageSideButtons.none);
                        GoToIndex();
                        break;
                    default:
                        int value = (int)button * 100 + 1;
                        SetUpPageSideButtons _button = (SetUpPageSideButtons)value;
                        FrameworkElement sLabel = this.SideButtons.FirstOrDefault(x => x.Name == _button.ToString());
                        SelectSideButton(sLabel, _button);

                        DoWork(_button);
                        break;
                }
            }
        }

        #endregion

        #region 侧边栏

        /// <summary>
        /// 侧边栏功能键激活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SideButtonHover(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            SetUpPageSideButtons button = (SetUpPageSideButtons)Enum.Parse(typeof(SetUpPageSideButtons), label.Name, true);
            if (button != this.ActivationSideButton)
            {
                ((Grid)label.Parent).Style = (Style)this.Resources["bg_hover"];
            }
        }

        /// <summary>
        /// 侧边栏功能键还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SideButtonToNormal(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            SetUpPageSideButtons button = (SetUpPageSideButtons)Enum.Parse(typeof(SetUpPageSideButtons), label.Name, true);
            if (button != this.ActivationSideButton)
            {
                ((Grid)label.Parent).Style = null;
            }
        }

        /// <summary>
        /// 侧边栏功能键触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SideButtonClick(object sender, MouseButtonEventArgs e)
        {

            Label label = (Label)sender;
            SetUpPageSideButtons button = (SetUpPageSideButtons)Enum.Parse(typeof(SetUpPageSideButtons), label.Name, true);
            if (button != this.ActivationSideButton)
            {
                SelectSideButton(label, button);

                int value = (int)button / 100;
                SetUpPageTopButtonS _button = (SetUpPageTopButtonS)value;
                FrameworkElement tLabel = this.TopButtons.FirstOrDefault(x => x.Name == _button.ToString());
                SelectTopButton(tLabel, _button);

                DoWork(button);
            }
        }

        #endregion

        #region 选中格式切换

        /// <summary>
        /// 选中顶部功能键（样式）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="button"></param>
        void SelectTopButton(FrameworkElement sender, SetUpPageTopButtonS button)
        {
            FrameworkElement fe = this.TopButtons.FirstOrDefault(x => x.Name == this.ActivationTopButton.ToString());
            fe.Style = (Style)this.Resources["top_normal"];
            fe.Cursor = Cursors.Hand;
            FrameworkElement bg = ((Grid)fe.Parent).Children
                .FirstOrDefault() as FrameworkElement;
            bg.Style = (Style)this.Resources["topBg_normal"];

            sender.Style = (Style)this.Resources["top_seletcted"];
            sender.Cursor = Cursors.Arrow;
            Rectangle bg2 = ((Grid)sender.Parent).Children
                .FirstOrDefault() as Rectangle;
            bg2.Style = (Style)this.Resources["topBg_selected"];

            this.ActivationTopButton = button;
        }

        /// <summary>
        /// 选中侧边栏功能键（样式）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="button"></param>
        void SelectSideButton(FrameworkElement sender, SetUpPageSideButtons button)
        {
            if (this.ActivationSideButton != SetUpPageSideButtons.none)
            {
                FrameworkElement fe = this.SideButtons.FirstOrDefault(x => x.Name == this.ActivationSideButton.ToString());
                fe.Style = (Style)this.Resources["side_normal"];
                ((FrameworkElement)fe.Parent).Style = null;
                fe.Cursor = Cursors.Hand;
            }

            if (button != SetUpPageSideButtons.none)
            {
                ((FrameworkElement)sender.Parent).Style = (Style)this.Resources["bg_selected"];
                sender.Style = (Style)this.Resources["side_selected"];
                sender.Cursor = Cursors.Arrow;
            }

            this.ActivationSideButton = button;
        }

        #endregion

        #endregion

        #endregion

        #region 快捷功能

        /// <summary>
        /// 初始化快捷功能
        /// </summary>
        void InitializeFastKey()
        {
            button_backToReception.MouseEnter += UIHelper.NButtonTrigger;
            button_backToReception.MouseLeave += UIHelper.NButtonReply;
            button_backToReception.MouseLeftButtonDown += UIHelper.OpenReceptionIndex;

            button_logout.MouseEnter += UIHelper.NButtonTrigger;
            button_logout.MouseLeave += UIHelper.NButtonReply;
            button_logout.MouseLeftButtonDown += Logout;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Logout(object sender, MouseEventArgs e)
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

        /// <summary>
        /// 返回首页
        /// </summary>
        void GoToIndex()
        {
            body.Children.Clear();
            IndexPage index = new IndexPage();
            index.GoToDataReportPageEventHandler += (sender, e) =>
                {
                    SelectTopButton(topButton_dataReport, SetUpPageTopButtonS.topButton_dataReport);
                    SelectSideButton(sideButton_siteReports, SetUpPageSideButtons.sideButton_siteReports);
                    GoToSiteReportPage();
                };
            index.GoToSystemSettingPageEventHandler += (sender, e) =>
                {
                    SelectTopButton(topButton_setting, SetUpPageTopButtonS.topButton_setting);
                    SelectSideButton(sideButton_webiteSetting, SetUpPageSideButtons.sideButton_webiteSetting);
                    GoToWebSiteSettingPage();
                };
            body.Children.Add(index);
        }

        #endregion

        public SetUpPage()
        {
            InitializeComponent();
            InitializeInterface();
            InitializeFunctionKey();
            InitializeFastKey();
            InitializeMouseEvent();
            Heartbeat();

            text_username.Content = App.User.Username;
            text_lastLoginDate.Content = string.Format("最后登入时间：{0}/{1}/{2}"
                , App.User.LastLoginTime.Year.ToString("0000")
                , App.User.LastLoginTime.Month.ToString("00")
                , App.User.LastLoginTime.Day.ToString("00"));

            GoToIndex();
        }

        #region 鼠标事件

        void InitializeMouseEvent()
        {
            button_closePromt.MouseEnter += UIHelper.MoveButtonBody;
            button_closePromt.MouseLeave += UIHelper.ReplyButonBody;
            button_closePromt.MouseLeftButtonDown += HidePromt;

            button_openMessageBox.MouseEnter += UIHelper.NButtonTrigger;
            button_openMessageBox.MouseLeave += UIHelper.NButtonReply;
            button_openMessageBox.MouseLeftButtonDown += OpenMessageBox;
        }

        /// <summary>
        /// 关闭信息提示栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HidePromt(object sender, MouseButtonEventArgs e)
        {
            Storyboard storyboard = (Storyboard)this.Resources["promt_close"];
            storyboard.Completed += (_sender, _e) =>
                {
                    frame_promt.Visibility = System.Windows.Visibility.Collapsed;
                };
            storyboard.Begin();
        }

        /// <summary>
        /// 打开消息盒子
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OpenMessageBox(object sender, MouseButtonEventArgs e)
        {
            Storyboard storyboard = (Storyboard)this.Resources["promt_open"];
            storyboard.Begin();
        }

        #endregion

        #region 伺配中心

        /// <summary>
        /// 功能键对应功能伺配
        /// </summary>
        /// <param name="button">被触发的功能键</param>
        void DoWork(SetUpPageSideButtons button)
        {
            switch (button)
            {
                #region 用户

                case SetUpPageSideButtons.sideButton_examineUsers:
                    GoToExamineUsersPage();
                    break;
                case SetUpPageSideButtons.sideButton_addNewUser:
                    GoToCreateNewUserPage();
                    break;
                case SetUpPageSideButtons.sideButton_examineUserGroups:
                    GoToExamineUserGroupPage();
                    break;
                case SetUpPageSideButtons.sideButton_landingRecords:
                    GoToLandingRecordsPage();
                    break;

                #endregion
                #region 彩票管理
                case SetUpPageSideButtons.sideButton_examineTickets:
                    GoToExamineTicketsPage();
                    break;
                case SetUpPageSideButtons.sideButton_examineTags:
                    GoToExamineTagsPage();
                    break;
                case SetUpPageSideButtons.sideButton_examinePatterns:
                    GoToExaminePlaysPage();
                    break;
                case SetUpPageSideButtons.sideButton_examineLottery:
                    GoToExamineLotteryPage();
                    break;
                case SetUpPageSideButtons.sideButton_examineVirtualTop:
                    GoToExamineVirtualTopPage();
                    break;
                #endregion
                #region 公告管理
                case SetUpPageSideButtons.sideButton_examineBulletins:
                    GoToExamineBulletinsPage();
                    break;
                case SetUpPageSideButtons.sideButton_addNewBulletin:
                    GoToCreateNewBulletinPage();
                    break;
                #endregion
                #region 活动管理
                case SetUpPageSideButtons.sideButton_examineActivities:
                    GoToExamineActivitiesPage();
                    break;
                case SetUpPageSideButtons.sideButton_participatedRecordOfActivities:
                    GoToParticipatedRecordOfActivitiesPage();
                    break;
                case SetUpPageSideButtons.sideButton_examineExchanges:
                    GoToExamineExchangesPage();
                    break;
                case SetUpPageSideButtons.sideButton_participatedRecordOfExchanges:
                    GoToParticipatedRecordOfExchangesPage();
                    break;
                #endregion
                #region 数据统计

                case SetUpPageSideButtons.sideButton_siteReports:
                    GoToSiteReportPage();
                    break;
                case SetUpPageSideButtons.sideButton_personalReports:
                    GoToPersonalReportPage();
                    break;
                case SetUpPageSideButtons.sideButton_bettingReports:
                    GoToBettingReportsPage();
                    break;
                case SetUpPageSideButtons.sideButton_rechargeReports:
                    GoToRechargeReportsPage();
                    break;
                case SetUpPageSideButtons.sideButton_withdrawalReports:
                    GoToWithdrawalReportsPage();
                    break;
                case SetUpPageSideButtons.sideButton_transferReports:
                    GoToTransferReportsPage();
                    break;

                #endregion
                #region 系统设置

                case SetUpPageSideButtons.sideButton_webiteSetting:
                    GoToWebSiteSettingPage();
                    break;
                case SetUpPageSideButtons.sideButton_bankAccounts:
                    GoToBankAccountsPage();
                    break;
                case SetUpPageSideButtons.sideButton_emailAccounts:
                    GoToEmailAccountsPage();
                    break;
                case SetUpPageSideButtons.sideButton_emailHosts:
                    GoToEmailHostsPage();
                    break;

                #endregion
                #region 管理员组
                case SetUpPageSideButtons.sideButton_examineMangers:
                    GoToExamineMangersPage();
                    break;
                case SetUpPageSideButtons.sideButton_examineManagerGroups:
                    GoToExamineManagerGroupsPage();
                    break;
                case SetUpPageSideButtons.sideButton_managerLandingRecords:
                    GoToManagerLandingRecordsPage();
                    break;
                case SetUpPageSideButtons.sideButton_managedRecords:
                    GoToManagedRecordsPage();
                    break;
                #endregion
            }
        }

        #endregion

        #region 主体功能

        #region 用户

        void GoToExamineUsersPage(int groupId = 0)
        {
            ExamineUsersPage tPage = new ExamineUsersPage(groupId);
            tPage.GoToCreateNewUserEventHandler += (sender, e) =>
                {
                    SelectTopButton(topButton_manageUser, SetUpPageTopButtonS.topButton_manageUser);
                    SelectSideButton(sideButton_addNewUser, SetUpPageSideButtons.sideButton_addNewUser);
                    GoToCreateNewUserPage();
                };
            tPage.ViewLandingReportEventHander += (sender, e) =>
                {
                    SelectTopButton(topButton_manageUser, SetUpPageTopButtonS.topButton_manageUser);
                    SelectSideButton(sideButton_landingRecords, SetUpPageSideButtons.sideButton_landingRecords);
                    ExamineUsersPage_TableBody row = (ExamineUsersPage_TableBody)sender;
                    GoToLandingRecordsPage(row.UserInfo.UserId);
                };
            tPage.ViewReportEventHandler += (sender, e) =>
                {
                    SelectTopButton(topButton_dataReport, SetUpPageTopButtonS.topButton_dataReport);
                    SelectSideButton(sideButton_personalReports, SetUpPageSideButtons.sideButton_personalReports);
                    ExamineUsersPage_TableBody row = (ExamineUsersPage_TableBody)sender;
                    GoToPersonalReportPage(row.UserInfo.UserId);
                };
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToCreateNewUserPage()
        {
            CreateNewUserPage page = new CreateNewUserPage();
            page.BankToListPageEventHandler += (sender, e) =>
            {
                SelectTopButton(topButton_manageUser, SetUpPageTopButtonS.topButton_manageUser);
                SelectSideButton(sideButton_examineUsers, SetUpPageSideButtons.sideButton_examineUsers);
                GoToExamineUsersPage();
            };
            body.Children.Clear();
            body.Children.Add(page);
        }

        void GoToExamineUserGroupPage()
        {
            ExamineUserGroupPage tPage = new ExamineUserGroupPage();
            tPage.SeleceUserByGroupEventHandler += (sender, e) =>
                {
                    SelectTopButton(topButton_manageUser, SetUpPageTopButtonS.topButton_manageUser);
                    SelectSideButton(sideButton_examineUsers, SetUpPageSideButtons.sideButton_examineUsers);
                    ExamineUserGroupPage_TableRow tr = (ExamineUserGroupPage_TableRow)sender;
                    GoToExamineUsersPage(tr.Group.GroupId);
                };
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToLandingRecordsPage(int userId = 0)
        {
            LandingRecordsPage tPage = new LandingRecordsPage(userId);
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        #endregion

        #region 彩票管理

        void GoToExamineTicketsPage()
        {
            ExamineTicketsPage tPage = new ExamineTicketsPage();
            tPage.ViewTagsEventHandler += (sender, e) =>
                {
                    SelectTopButton(topButton_manageTicket, SetUpPageTopButtonS.topButton_manageTicket);
                    SelectSideButton(sideButton_examineTags, SetUpPageSideButtons.sideButton_examineTags);
                    ExamineTicketsPage_TableRow row = (ExamineTicketsPage_TableRow)sender;
                    GoToExamineTagsPage(row.Ticket.TicketId);
                };
            tPage.ViewPlaysEventHandler += (sender, e) =>
                {
                    SelectTopButton(topButton_manageTicket, SetUpPageTopButtonS.topButton_manageTicket);
                    SelectSideButton(sideButton_examinePatterns, SetUpPageSideButtons.sideButton_examinePatterns);
                    ExamineTicketsPage_TableRow row = (ExamineTicketsPage_TableRow)sender;
                    GoToExaminePlaysPage(row.Ticket.TicketId);
                };
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToExamineTagsPage(int ticketId = 0)
        {
            ExamineTagsPage tPage = new ExamineTagsPage(ticketId);
            tPage.ViewPlaysEventHandler += (sender, e) =>
                {
                    SelectTopButton(topButton_manageTicket, SetUpPageTopButtonS.topButton_manageTicket);
                    SelectSideButton(sideButton_examinePatterns, SetUpPageSideButtons.sideButton_examinePatterns);
                    ExamineTagsPage_TableRow row = (ExamineTagsPage_TableRow)sender;
                    GoToExaminePlaysPage(row.PlayTag.TicketId, row.PlayTag.PlayTagId);
                };
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToExaminePlaysPage(int ticketId = 0, int tagId = 0)
        {
            ExaminePlaysPage tPage = new ExaminePlaysPage(ticketId, tagId);
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToExamineLotteryPage()
        {
            ExamineLotteryPage tPage = new ExamineLotteryPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToExamineVirtualTopPage()
        {
            ExamineVirtualTopPage tPage = new ExamineVirtualTopPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        #endregion

        #region 公告管理

        void GoToExamineBulletinsPage()
        {
            ExamineBulletinsPage tPage = new ExamineBulletinsPage();
            tPage.ViewCreatePageEventHandler += (sender, e) =>
                {
                    SelectTopButton(topButton_manageBulletin, SetUpPageTopButtonS.topButton_manageBulletin);
                    SelectSideButton(sideButton_addNewBulletin, SetUpPageSideButtons.sideButton_addNewBulletin);
                    GoToCreateNewBulletinPage();
                };
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToCreateNewBulletinPage()
        {
            CreateNewBulletinPage tPage = new CreateNewBulletinPage();
            tPage.ViewListPageEventHandler += (sender, e) =>
                {
                    SelectTopButton(topButton_manageBulletin, SetUpPageTopButtonS.topButton_manageBulletin);
                    SelectSideButton(sideButton_examineBulletins, SetUpPageSideButtons.sideButton_examineBulletins);
                    GoToExamineBulletinsPage();
                };
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        #endregion

        #region 活动管理

        void GoToExamineActivitiesPage()
        {
            ExamineActivitiesPage tPage = new ExamineActivitiesPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToParticipatedRecordOfActivitiesPage()
        {
            ParticipatedRecordOfActivitiesPage tPage = new ParticipatedRecordOfActivitiesPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToExamineExchangesPage()
        {
            ExamineExchangesPage tPage = new ExamineExchangesPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToParticipatedRecordOfExchangesPage()
        {
            ParticipatedRecordOfExchangesPage tPage = new ParticipatedRecordOfExchangesPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        #endregion

        #region 数据报表

        void GoToSiteReportPage()
        {
            SiteReportPage tPage = new SiteReportPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToPersonalReportPage(int userId = 0)
        {
            PersonalReportPage tPage = new PersonalReportPage(userId);
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToBettingReportsPage()
        {
            BettingReportsPage tPage = new BettingReportsPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToRechargeReportsPage()
        {
            RechargeReportsPage tPage = new RechargeReportsPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToWithdrawalReportsPage()
        {
            WithdrawalReportsPage tPage = new WithdrawalReportsPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToTransferReportsPage()
        {
            TransferReportsPage tPage = new TransferReportsPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        #endregion

        #region 系统设置

        void GoToWebSiteSettingPage()
        {
            WebSiteSettingPage tPage = new WebSiteSettingPage();
            tPage.BackToIndexEventHandler += (sender, e) =>
                {
                    SelectTopButton(topButton_GoToIndex, SetUpPageTopButtonS.topButton_GoToIndex);
                    SelectSideButton(null, SetUpPageSideButtons.none);
                    GoToIndex();
                };
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToBankAccountsPage()
        {
            BankAccountsPage tPage = new BankAccountsPage();
            body.Children.Clear();
            body.Children.Add(tPage);

        }

        void GoToEmailAccountsPage()
        {
            EmailAccountsPage tPage = new EmailAccountsPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToEmailHostsPage()
        {
            EmailHostsPage tPage = new EmailHostsPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        #endregion

        #region 管理员组

        void GoToExamineMangersPage()
        {
            ExamineMangersPage tPage = new ExamineMangersPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToExamineManagerGroupsPage()
        {
            ExamineManagerGroupsPage tPage = new ExamineManagerGroupsPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToManagerLandingRecordsPage()
        {
            ManagerLandingRecordsPage tPage = new ManagerLandingRecordsPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        void GoToManagedRecordsPage()
        {
            //该功能移除
        }

        #endregion

        #endregion

        #region 后台功能

        /// <summary>
        /// 构造界面
        /// </summary>
        void InitializeInterface()
        {
            if (!App.User.CanViewUsers)
            {
                topKey_manageUser.Visibility = System.Windows.Visibility.Collapsed;
                module_users.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (!App.User.CanViewTickets)
            {
                topKey_manageTicket.Visibility = System.Windows.Visibility.Collapsed;
                module_lotteryTickets.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (!App.User.CanViewActivities)
            {
                topKey_manageActivity.Visibility = System.Windows.Visibility.Collapsed;
                module_activities.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (!App.User.CanViewDataReports)
            {
                topKey_dataReport.Visibility = System.Windows.Visibility.Collapsed;
                module_dataReport.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (!App.User.CanSettingSite)
            {
                topKey_setting.Visibility = System.Windows.Visibility.Collapsed;
                module_setting.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (!App.User.CanViewAndEditManagers)
            {
                topKey_manager.Visibility = System.Windows.Visibility.Collapsed;
                module_manager.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        void OpenPromt(int countOfMessage)
        {
            messageCall.Stop();
            messageCall.Play();
            text_countOfNewMessage.Text = countOfMessage.ToString();
            frame_promt.Visibility = System.Windows.Visibility.Visible;
            Storyboard storyboard = (Storyboard)this.Resources["promt_open"];
            storyboard.Begin();
        }

        void HidePromt()
        {
            frame_promt.Visibility = System.Windows.Visibility.Collapsed;
        }

        void GoToMessageBoxPage(object sender, MouseButtonEventArgs e)
        {
            HidePromt();

            MessageBoxPage tPage = new MessageBoxPage();
            body.Children.Clear();
            body.Children.Add(tPage);
        }

        /// <summary>
        /// 心跳
        /// </summary>
        void Heartbeat()
        {
            Storyboard sb = new Storyboard();
            sb.Duration = new Duration(new TimeSpan(0, 0, 15));
            sb.Completed += Heartbeat_do;
            sb.Begin();
        }
        #region 心跳
        void Heartbeat_do(object sender, EventArgs e)
        {
            if (App.User.CanViewDataReports)
            {
                DataReportServiceClient client = new DataReportServiceClient();
                client.GetUntreatedRecharCountCompleted += ManageGetUntreatedRecharCountResult;
                client.GetUntreatedRecharCountAsync(App.Token);
            }
            else
            {
                ManagerServiceClient client = new ManagerServiceClient();
                client.HeartbeatCompleted += ShowHeartbeatResult;
                client.HeartbeatAsync(App.Token);
            }

            Storyboard sb = (Storyboard)sender;
            sb.Begin();
        }

        void ManageGetUntreatedRecharCountResult(object sender, GetUntreatedRecharCountCompletedEventArgs e)
        {
            string key = "mbCount";
            if (e.Result.Success)
            {
                if (App.Cache.Any(x => x.Key == key))
                {
                    App.Cache.Remove(key);
                }
                App.Cache.Add(key, e.Result.Count);

                DataReportServiceClient client = (DataReportServiceClient)sender;
                client.GetUntreatedWithdrawalCountCompleted += ManageGetUntreatedWithdrawalCountResult;
                client.GetUntreatedWithdrawalCountAsync(App.Token);
            }
            else
            {
                App.GoToLoginPage();
            }
        }

        void ManageGetUntreatedWithdrawalCountResult(object sender, GetUntreatedWithdrawalCountCompletedEventArgs e)
        {
            string key = "mbCount";
            if (e.Result.Success)
            {
                if (App.Cache.Any(x => x.Key == key))
                {
                    int count = (int)App.Cache[key];
                    if (count + e.Result.Count > 0)
                    {
                        OpenPromt(count + e.Result.Count);
                    }

                    App.Cache.Remove(key);
                }
            }
            else
            {
                App.GoToLoginPage();
            }
        }

        void ShowHeartbeatResult(object sender, HeartbeatCompletedEventArgs e)
        {
            if (!e.Result.Success)
            {
                App.GoToLoginPage();
            }
        }
        #endregion

        #endregion
    }
}

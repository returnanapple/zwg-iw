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
    public partial class LoginPage : UserControl
    {
        public LoginPage()
        {
            InitializeComponent();
            InitializeMouseEvent();
            InitializeKeyboardEvent();
        }

        #region 鼠标事件

        /// <summary>
        /// 初始化鼠标事件
        /// </summary>
        void InitializeMouseEvent()
        {
            #region 鼠标悬停反馈

            button_login.MouseEnter += ChangeToSelectedStyle;
            button_login.MouseLeave += ChangeToNormalStyle;
            button_backToReception.MouseEnter += UIHelper.NButtonTrigger;
            button_backToReception.MouseLeave += UIHelper.NButtonReply;
            button_closePromt.MouseEnter += UIHelper.MoveButtonBody;
            button_closePromt.MouseLeave += UIHelper.ReplyButonBody;

            #endregion

            button_backToReception.MouseLeftButtonDown += UIHelper.OpenReceptionIndex;
            button_login.MouseLeftButtonDown += GoLoginFromMouse;
            button_closePromt.MouseLeftButtonDown += HidePrompt;
        }

        #region 鼠标悬停反馈

        /// <summary>
        /// 转变为选中状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChangeToSelectedStyle(object sender, MouseEventArgs e)
        {
            text_login.Style = (Style)this.Resources["color_selected"];
        }

        /// <summary>
        /// 恢复正常状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChangeToNormalStyle(object sender, MouseEventArgs e)
        {
            text_login.Style = (Style)this.Resources["color_normal"];
        }

        #endregion

        /// <summary>
        /// 关闭信息提示栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HidePrompt(object sender, MouseButtonEventArgs e)
        {
            Storyboard storyboard = (Storyboard)this.Resources["promt_close"];
            storyboard.Completed += (_sender, _e) =>
                 {
                     form_top.Visibility = System.Windows.Visibility.Collapsed;
                 };
            storyboard.Begin();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GoLoginFromMouse(object sender, MouseButtonEventArgs e)
        {
            Login();
        }

        #endregion

        #region 键盘事件

        /// <summary>
        /// 初始化键盘事件
        /// </summary>
        void InitializeKeyboardEvent()
        {
            input_username.KeyDown += SwitchFocusToPasswordInput;
            input_password.KeyDown += GoLoginFromKeyboard;
        }

        /// <summary>
        /// 将焦点切换到密码输入框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SwitchFocusToPasswordInput(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                #region 验证输入的信息
                string username = TextHelper.EliminateSpaces(input_username.Text);
                try
                {
                    TextHelper.Check(username, TextHelper.Key.Nickname);
                }
                catch (Exception error)
                {
                    ShowPromtTool(string.Format("用户名输入不合法！{0}", error.Message));
                    return;
                }
                #endregion
                input_password.Focus();
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GoLoginFromKeyboard(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                #region 验证输入的信息
                string password = TextHelper.EliminateSpaces(input_password.Password);
                try
                {
                    TextHelper.Check(password, TextHelper.Key.Password);
                }
                catch (Exception error)
                {
                    ShowPromtTool(string.Format("密码输入不合法！{0}", error.Message));
                    return;
                }
                #endregion
                Login();
            }
        }


        #endregion

        #region 信息提示栏

        /// <summary>
        /// 显示提示栏
        /// （并清空用户名输入框和密码输入框）
        /// </summary>
        /// <param name="message">所要显示的信息</param>
        void ShowPromtTool(string message = "Server connection fails...")
        {
            App.HideCover();
            input_username.Text = "";
            input_password.Password = "";
            promt_message.Content = string.Format("Error：{0}", message);
            if (form_top.Visibility == System.Windows.Visibility.Collapsed)
            {
                form_top.Visibility = System.Windows.Visibility.Visible;
                Storyboard storyboard = (Storyboard)this.Resources["promt_open"];
                storyboard.Completed += (sender, e) =>
                    {
                        input_username.Focus();
                    };
                storyboard.Begin();
            }
        }

        #endregion

        #region 用户登录

        /// <summary>
        /// 用户登录
        /// </summary>
        void Login()
        {
            MainPage mainPage = (MainPage)App.Current.RootVisual;
            mainPage.ShowCover();
            string username = TextHelper.EliminateSpaces(input_username.Text);
            string password = TextHelper.EliminateSpaces(input_password.Password);
            #region 验证输入的信息
            try
            {
                TextHelper.Check(username, TextHelper.Key.Nickname);
                TextHelper.Check(password, TextHelper.Key.Password);
            }
            catch (Exception)
            {
                ShowPromtTool("用户名/密码输入不合法，请重新输入");
                return;
            }
            #endregion
            try
            {
                ManagerServiceClient client = new ManagerServiceClient();
                client.LoginCompleted += (sender, e) =>
                    {
                        if (e.Result.Success)
                        {
                            App.Token = e.Result.Token;
                            App.User = new UserInfoCaChe
                            {
                                Username = e.Result.Username,
                                LastLoginTime = e.Result.LastLoginTime,
                                GroupName = e.Result.Group.Name,
                                GroupGrade = e.Result.Group.Grade,
                                CanViewUsers = e.Result.Group.CanViewUsers,
                                CanEditUsers = e.Result.Group.CanEditUsers,
                                CanViewTickets = e.Result.Group.CanViewTickets,
                                CanEditTickets = e.Result.Group.CanEditTickets,
                                CanViewActivities = e.Result.Group.CanViewActivities,
                                CanEditActivities = e.Result.Group.CanEditActivities,
                                CanSettingSite = e.Result.Group.CanSettingSite,
                                CanViewDataReports = e.Result.Group.CanViewDataReports,
                                CanViewAndAddFundsReports = e.Result.Group.CanViewAndAddFundsReports,
                                CanViewAndEditMessageBox = e.Result.Group.CanViewAndEditMessageBox,
                                CanViewAndEditManagers = e.Result.Group.CanViewAndEditManagers
                            };
                            mainPage.GoToSetUpPage();
                        }
                        else
                        {
                            ShowPromtTool(e.Result.Error);
                        }
                    };
                client.LoginAsync(username, password);
            }
            catch (Exception e)
            {
                mainPage.HideCover();
                ShowPromtTool(e.Message);
            }
        }

        #endregion
    }
}

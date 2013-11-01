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
using IWorld.Client.UsersService;
using IWorld.Client.SystemSettingService;
using IWorld.Client.GamingService;
using IWorld.Client.BulletinService;

namespace IWorld.Client
{
    public partial class LoginPage : UserControl
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        #region 快捷功能

        /// <summary>
        /// 顶部功能按键触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonXHover(object sender, EventArgs e)
        {
            ((Grid)sender).Background = new SolidColorBrush(Colors.Gray);
        }

        /// <summary>
        /// 顶部功能按键恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonXUnhover(object sender, EventArgs e)
        {
            ((Grid)sender).Background = null;
        }

        /// <summary>
        /// 最小化窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeMainWindow(object sender, EventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseMainWindow(object sender, EventArgs e)
        {
            App.Current.MainWindow.Close();
        }

        #endregion

        #region 遮蔽层

        void ShowCover()
        {
            //coverFloor.Visibility = System.Windows.Visibility.Visible;
        }

        void HideCover()
        {
            //coverFloor.Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion

        private void Login(object sender, EventArgs e)
        {
            //ShowCover();
            BeginAnimation();
            string username = input_username.Text;
            string password = input_password.Password;

            UsersServiceClient client = new UsersServiceClient();
            client.LoginCompleted += ManageLoginResult;
            client.LoginAsync(username, password);
        }
        #region 登陆
        void ManageLoginResult(object sender, LoginCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                App.Token = e.Result.Token;

                UsersServiceClient client = (UsersServiceClient)sender;
                client.GetUserInfoCompleted += ManageGetUserInfoResult;
                client.GetUserInfoAsync(App.Token);
            }
            else
            {
                input_username.Text = "";
                input_password.Password = "";

                HideCover();
                StopAnimation();

                ErrorPromt ep = new ErrorPromt(e.Result.Error);
                ep.Show();
            }
        }

        void ManageGetUserInfoResult(object sender, GetUserInfoCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                App.UserInfo = e.Result;

                SystemSettingServiceClient client = new SystemSettingServiceClient();
                client.GetWebSettingCompleted += ManageGetWebSettingResult;
                client.GetWebSettingAsync();
            }
            else
            {
                HideCover();
                StopAnimation();
                ErrorPromt ep = new ErrorPromt(e.Result.Error);
                ep.Show();
            }
        }

        void ManageGetWebSettingResult(object sender, GetWebSettingCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                App.Websetting = e.Result;

                GamingServiceClient client = new GamingServiceClient();
                client.GetHowToPlaysCompleted += ManageGetHowToPlaysResult;
                client.GetHowToPlaysAsync();
            }
            else
            {
                HideCover();

                StopAnimation();
                ErrorPromt ep = new ErrorPromt(e.Result.Error);
                ep.Show();
            }
        }

        void ManageGetHowToPlaysResult(object sender, GetHowToPlaysCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                App.Ticktes = e.Result.Content;
                App.TicketsRefreshTime = DateTime.Now;
                BulletinServiceClient client = new BulletinServiceClient();
                client.GetBulletinsCompleted += ManageGetBulletinsResult;
                client.GetBulletinsAsync();
            }
            else
            {
                HideCover();

                StopAnimation();
                ErrorPromt ep = new ErrorPromt(e.Result.Error);
                ep.Show();
            }
        }

        void ManageGetBulletinsResult(object sender, GetBulletinsCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                App.Bulletins = e.Result.Content;
                SetIn();
            }
            else
            {
                HideCover();

                StopAnimation();
                ErrorPromt ep = new ErrorPromt(e.Result.Error);
                ep.Show();
            }
        }

        /// <summary>
        /// 进入主要操作界面
        /// </summary>
        void SetIn()
        {
            HideCover();
            switch (App.UserInfo.Status)
            {
                case UserStatus.禁止访问:
                    ErrorPromt ep = new ErrorPromt("黑名单，你懂的");
                    ep.Show();
                    break;
                case UserStatus.未激活:
                    App.GoToBindingPage();
                    break;
                case UserStatus.正常:
                    App.GoToOperatePage();
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 开始动态效果
        /// </summary>
        private void BeginAnimation()
        {
            Storyboard sb = (Storyboard)this.Resources["s_Rotating"];
            sb.Begin();
        }

        /// <summary>
        /// 开始动态效果
        /// </summary>
        private void StopAnimation()
        {
            Storyboard sb = (Storyboard)this.Resources["s_Rotating"];
            sb.Stop();
        }

        private void input_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login(null, null);
            }
        }

        private void login_Comb_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                ComboBoxItem cbi = (ComboBoxItem)(sender as ComboBox).SelectedItem;
                string selectedText = cbi.Content.ToString();
                login_Comb.SelectedIndex = -1;
                if (!string.IsNullOrEmpty(selectedText))
                {
                    input_username.Text = selectedText;
                }
            }
            catch (Exception)
            {
                input_username.Text = "";
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Current.IsRunningOutOfBrowser && App.HadSetSize)
            {
                App.Current.MainWindow.Width = 241;
                App.Current.MainWindow.Height = 541;
                App.Current.MainWindow.Top = App.TopOfInitial;
                App.Current.MainWindow.Left = App.LeftOfInitia;
            }
        }
    }
}

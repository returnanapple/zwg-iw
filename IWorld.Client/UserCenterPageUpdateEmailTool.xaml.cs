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
using IWorld.Client.Class;
using IWorld.Client.UsersService;

namespace IWorld.Client
{
    public partial class UserCenterPageUpdateEmailTool : UserControl
    {
        public UserCenterPageUpdateEmailTool()
        {
            InitializeComponent();
            input_email.Text = App.UserInfo.Email;
            if (App.UserInfo.BindingEmail)
            {
                input_email.IsEnabled = false;
                button_update.IsEnabled = false;
            }
        }

        public event NDelegate BackEventHandler;

        private void Update(object sender, RoutedEventArgs e)
        {

            UsersServiceClient client = new UsersServiceClient();
            client.EditEmailCompleted += ShowUpdateResult;
            client.EditEmailAsync(input_email.Text, App.Token);
        }
        #region 更新
        void ShowUpdateResult(object sender, EditEmailCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                ErrorPromt ep = new ErrorPromt("修改安全邮箱成功，请重新登陆");
                ep.Closed += BankToLoginPage;
                ep.Show();
            }
            else
            {
                ErrorPromt ep = new ErrorPromt(e.Result.Error);
                ep.Show();

                input_email.Text = "";
            }
        }
        void BankToLoginPage(object sender, EventArgs e)
        {
            App.GoToLoginPage();

            if (App.Current.IsRunningOutOfBrowser)
            {
                App.Current.MainWindow.Width = 1024;
                App.Current.MainWindow.Height = 768;
                App.Current.MainWindow.Top = App.TopOfInitial;
                App.Current.MainWindow.Left = App.LeftOfInitia;
            }
        }

        #endregion

        private void Cancel(object sender, RoutedEventArgs e)
        {
            if (BackEventHandler != null)
            {
                BackEventHandler(this, new EventArgs());
            }
        }
    }
}

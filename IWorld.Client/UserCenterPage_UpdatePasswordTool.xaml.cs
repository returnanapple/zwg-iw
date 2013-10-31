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
    public partial class UserCenterPage_UpdatePasswordTool : UserControl
    {
        public UserCenterPage_UpdatePasswordTool()
        {
            InitializeComponent();
        }

        public event NDelegate BackEventHandler;

        private void Update(object sender, RoutedEventArgs e)
        {
            if (input_newPassword.Password != input_newPassword_a.Password)
            {
                ErrorPromt ep = new ErrorPromt("两次输入的新密码不一致");
                ep.Show();

                input_oldPassword.Password = "";
                input_newPassword.Password = "";
                input_newPassword_a.Password = "";
            }

            UsersServiceClient client = new UsersServiceClient();
            client.EditPasswordCompleted += ShowUpdateResult;
            client.EditPasswordAsync(input_oldPassword.Password, input_newPassword.Password, App.Token);
        }
        #region 更新

        void ShowUpdateResult(object sender, EditPasswordCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                ErrorPromt ep = new ErrorPromt("修改密码成功，请重新登陆");
                ep.Closed += BankToLoginPage;
                ep.Show();
            }
            else
            {
                ErrorPromt ep = new ErrorPromt(e.Result.Error);
                ep.Show();

                input_oldPassword.Password = "";
                input_newPassword.Password = "";
                input_newPassword_a.Password = "";
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

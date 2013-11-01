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

namespace IWorld.Client
{
    public partial class BindingPage : UserControl
    {
        public BindingPage()
        {
            InitializeComponent();

        }

        private void Enter(object sender, RoutedEventArgs e)
        {
            if (inpu_newPassword.Password != input_newPassword_a.Password)
            {
                ErrorPromt ep = new ErrorPromt("两次输入的新密码不一致");
                ep.Show();
            }
            else if (input_safeCode.Password != input_safeCode_a.Password)
            {
                ErrorPromt ep = new ErrorPromt("两次输入的安全密码不一致");
                ep.Show();
            }
            else
            {
                UserInfoBindingImport import = new UserInfoBindingImport
                {
                    OldPassword = input_oldPassword.Password,
                    NewPassword = inpu_newPassword.Password,
                    SafeCode = input_safeCode.Password,
                    Email = input_email.Text,
                    Card = input_card.Text,
                    Holder = input_holder.Text,
                    Bank = (Bank)(Enum.Parse(typeof(Bank), ((TextBlock)input_bank.SelectedItem).Text, false))
                };
                UsersServiceClient client = new UsersServiceClient();
                client.BindingCompleted += ShowBindingResult;
                client.BindingAsync(import, App.Token);
            }
        }
        #region 绑定
        void ShowBindingResult(object sender, BindingCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                ErrorPromt ep = new ErrorPromt("绑定成功，请重新登陆");
                ep.Closed += ep_Closed;
                ep.Show();
            }
            else
            {
                ErrorPromt ep = new ErrorPromt(e.Result.Error);
                ep.Show();
            }
        }

        void ep_Closed(object sender, EventArgs e)
        {
            App.GoToLoginPage();
        }
        #endregion

        private void Reset(object sender, RoutedEventArgs e)
        {
            input_oldPassword.Password = "";
            inpu_newPassword.Password = "";
            input_newPassword_a.Password = "";
            input_safeCode.Password = "";
            input_safeCode_a.Password = "";
            input_email.Text = "";
            input_card.Text = "";
            input_holder.Text = "";
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            App.GoToLoginPage();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Current.IsRunningOutOfBrowser)
            {
                App.Current.MainWindow.Width = 420;
                App.Current.MainWindow.Height = 400;
                App.Current.MainWindow.Top = App.TopOfInitial + 100;
                App.Current.MainWindow.Left = App.LeftOfInitia + 50;
            }
        }
    }
}

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
    public partial class UsersPage_CreateResultTool : ChildWindow
    {
        CreateUserResult Result { get; set; }

        public UsersPage_CreateResultTool(CreateUserResult result)
        {
            InitializeComponent();
            this.Result = result;

            text_username.Text = result.Username;
            text_password.Text = result.Password;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(string.Format("用户名：{0} | 密码：{1}", this.Result.Username, this.Result.Password));
        }
    }
}


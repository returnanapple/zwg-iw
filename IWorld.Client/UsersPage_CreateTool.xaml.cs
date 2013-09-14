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
    public partial class UsersPage_CreateTool : ChildWindow
    {
        public bool ShowError { get; set; }
        public CreateUserResult Result { get; set; }

        public UsersPage_CreateTool()
        {
            InitializeComponent();
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            CreateUserImport import = new CreateUserImport
            {
                Username = input_username.Text,
                NormalReturnPoints = (Math.Round(Convert.ToDouble(input_nrp.Text))),
                UncertainReturnPoints = (Math.Round(Convert.ToDouble(input_urp.Text))),
                Quota = 0
            };

            UsersServiceClient client = new UsersServiceClient();
            client.CreateUserCompleted += ShowCreateResult;
            client.CreateUserAsync(import, App.Token);
        }
        #region 创建

        void ShowCreateResult(object sender, CreateUserCompletedEventArgs e)
        {
            if (!e.Result.Success)
            {
                ShowError = true;
            }
            Result = e.Result;
            this.DialogResult = true;
        }

        #endregion

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


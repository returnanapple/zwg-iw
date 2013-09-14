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
using IWorld.Admin.SystemSettingService;

namespace IWorld.Admin
{
    public partial class EmailAccountsPage_CreateTool : ChildWindow
    {
        public bool ShowError { get; set; }
        public string Error { get; set; }

        public EmailAccountsPage_CreateTool()
        {
            InitializeComponent();
            this.ShowError = false;
            this.Error = "";
        }

        private void Create(object sender, EventArgs e)
        {
            AddEmailAccountImport import = new AddEmailAccountImport
            {
                Key = input_key.Text,
                Account = input_account.Text,
                Password = input_password.Text,
                Remark = input_remark.Text,
                ClientKey = input_clientKey.Text
            };
            SystemSettingServiceClient client = new SystemSettingServiceClient();
            client.AddEmailAccountCompleted += ShowCreateResult;
            client.AddEmailAccountAsync(import, App.Token);
        }
        #region 创建
        void ShowCreateResult(object sender, AddEmailAccountCompletedEventArgs e)
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


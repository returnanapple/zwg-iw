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
    public partial class EmailAccountsPage_EditTool : ChildWindow
    {
        public bool ShowError { get; set; }
        public string Error { get; set; }
        EmailAccountResult EmailAccount { get; set; }

        public EmailAccountsPage_EditTool(EmailAccountResult emailAccount)
        {
            InitializeComponent();
            this.ShowError = false;
            this.Error = "";
            this.EmailAccount = emailAccount;

            input_key.Text = emailAccount.Key;
            input_account.Text = emailAccount.Account;
            input_password.Text = emailAccount.Password;
            input_remark.Text = emailAccount.Remark;
            input_clientKey.Text = emailAccount.Client.Key;
        }

        private void Edit(object sender, EventArgs e)
        {
            EditEmailAccountImport import = new EditEmailAccountImport
            {
                EmailAccountId = this.EmailAccount.EmailAccountId,
                Key = input_key.Text,
                Account = input_account.Text,
                Password = input_password.Text,
                Remark = input_remark.Text,
                ClientKey = input_clientKey.Text
            };
            SystemSettingServiceClient client = new SystemSettingServiceClient();
            client.EditEmailAccountCompleted += ShowEditResult;
            client.EditEmailAccountAsync(import, App.Token);
        }
        #region 编辑
        void ShowEditResult(object sender, EditEmailAccountCompletedEventArgs e)
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


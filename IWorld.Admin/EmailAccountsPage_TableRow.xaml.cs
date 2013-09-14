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
using IWorld.Admin.SystemSettingService;

namespace IWorld.Admin
{
    public partial class EmailAccountsPage_TableRow : UserControl, ITableToolRow
    {
        EmailAccountResult EmailAccount { get; set; }
        int _row = 0;

        public EmailAccountsPage_TableRow(EmailAccountResult emailAccount, int row)
        {
            InitializeComponent();
            this.EmailAccount = emailAccount;
            this._row = row;

            text_key.Text = emailAccount.Key;
            text_account.Text = emailAccount.Account;
            text_password.Text = emailAccount.Password;
            text_remark.Text = emailAccount.Remark;
            text_client.Text = emailAccount.Client.Key;
            if (emailAccount.IsDefault)
            {
                button_setDefault.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        public FrameworkElement GetElement()
        {
            return this;
        }

        public FrameworkElement GetChildWindow()
        {
            return null;
        }

        public event NDelegate RefreshEventHandler;

        public event TableToolBodyHoverDelegate HoverEventHandler;

        public event TableToolBodyHoverDelegate UnhoverEventHandler;

        private void Hover(object sender, MouseEventArgs e)
        {
            if (HoverEventHandler != null)
            {
                HoverEventHandler(this, new TableToolBodyHoverEventArgs(this._row));
            }
        }

        private void Unhover(object sender, MouseEventArgs e)
        {
            if (UnhoverEventHandler != null)
            {
                UnhoverEventHandler(this, new TableToolBodyHoverEventArgs(this._row));
            }
        }

        private void Edit(object sender, MouseButtonEventArgs e)
        {
            EmailAccountsPage_EditTool et = new EmailAccountsPage_EditTool(this.EmailAccount);
            et.Closed += ShowEditResult;
            et.Show();
        }
        #region 编辑
        void ShowEditResult(object sender, EventArgs e)
        {
            EmailAccountsPage_EditTool et = (EmailAccountsPage_EditTool)sender;
            if (et.DialogResult == true)
            {
                if (et.ShowError)
                {
                    ErrorPrompt ep = new ErrorPrompt(et.Error);
                    ep.Show();
                }
                else
                {
                    if (RefreshEventHandler != null)
                    {
                        RefreshEventHandler(this, new EventArgs());
                    }
                }
            }
        }
        #endregion

        private void Delete(object sender, MouseButtonEventArgs e)
        {
            NormalPrompt np = new NormalPrompt("警告：该操作将删除该邮箱账户！");
            np.Closed += Delete_do;
            np.Show();
        }
        #region 删除

        void Delete_do(object sender, EventArgs e)
        {
            NormalPrompt np = (NormalPrompt)sender;
            if (np.DialogResult == true)
            {
                SystemSettingServiceClient client = new SystemSettingServiceClient();
                client.RemoveEmailAccountCompleted += ShowDeleteResult;
                client.RemoveEmailAccountAsync(this.EmailAccount.EmailAccountId, App.Token);
            }
        }

        void ShowDeleteResult(object sender, RemoveEmailAccountCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                if (RefreshEventHandler != null)
                {
                    RefreshEventHandler(this, new EventArgs());
                }
            }
            else
            {
                ErrorPrompt ep = new ErrorPrompt(e.Result.Error);
                ep.Show();
            }
        }

        #endregion

        private void SetDefault(object sender, MouseButtonEventArgs e)
        {
            SystemSettingServiceClient client = new SystemSettingServiceClient();
            client.SetDefaultEmailAccountCompleted += ShowSetDefaultResult;
            client.SetDefaultEmailAccountAsync(this.EmailAccount.EmailAccountId, App.Token);
        }
        #region 设置默认
        void ShowSetDefaultResult(object sender, SetDefaultEmailAccountCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                if (RefreshEventHandler != null)
                {
                    RefreshEventHandler(this, new EventArgs());
                }
            }
            else
            {
                ErrorPrompt ep = new ErrorPrompt(e.Result.Error);
                ep.Show();
            }
        }
        #endregion
    }
}

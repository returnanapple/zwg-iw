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
    public partial class EmailHostsPage_TableRow : UserControl, ITableToolRow
    {
        EmailClientResult EmailClient { get; set; }
        int _row = 0;

        public EmailHostsPage_TableRow(EmailClientResult emailClient, int row)
        {
            InitializeComponent();
            this.EmailClient = emailClient;
            this._row = row;

            text_key.Text = emailClient.Key;
            text_host.Text = emailClient.Host;
            text_port.Text = emailClient.Port.ToString();
            text_remark.Text = emailClient.Remark;
            if (emailClient.IsDefault)
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
            EmailHostsPage_EditTool et = new EmailHostsPage_EditTool(this.EmailClient);
            et.Closed += ShowEditResult;
            et.Show();
        }
        #region 编辑
        void ShowEditResult(object sender, EventArgs e)
        {
            EmailHostsPage_EditTool et = (EmailHostsPage_EditTool)sender;
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
            NormalPrompt np = new NormalPrompt("警告：该操作将删除该邮箱服务器！");
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
                client.RemoveEmailClientCompleted += ShowDeleteResult;
                client.RemoveEmailClientAsync(this.EmailClient.EmailClientId, App.Token);
            }
        }

        void ShowDeleteResult(object sender, RemoveEmailClientCompletedEventArgs e)
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
            client.SetDefaultEmailClientCompleted += ShowSetDefaultResult;
            client.SetDefaultEmailClientAsync(this.EmailClient.EmailClientId, App.Token);
        }
        #region 设置默认
        void ShowSetDefaultResult(object sender, SetDefaultEmailClientCompletedEventArgs e)
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

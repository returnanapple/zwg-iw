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
    public partial class EmailHostsPage_EditTool : ChildWindow
    {
        public bool ShowError { get; set; }
        public string Error { get; set; }
        EmailClientResult EmailClient { get; set; }

        public EmailHostsPage_EditTool(EmailClientResult emailClient)
        {
            InitializeComponent();
            this.ShowError = false;
            this.Error = "";
            this.EmailClient = emailClient;

            input_key.Text = emailClient.Key;
            input_host.Text = emailClient.Host;
            input_port.Text = emailClient.Port.ToString();
            input_remark.Text = emailClient.Remark;
        }

        private void Edit(object sender, EventArgs e)
        {
            EditEmailClientImport import = new EditEmailClientImport
            {
                EmailClientId = this.EmailClient.EmailClientId,
                Key = input_key.Text,
                Host = input_host.Text,
                Port = Convert.ToInt32(input_port.Text),
                Remark = input_remark.Text
            };
            SystemSettingServiceClient client = new SystemSettingServiceClient();
            client.EditEmailClientCompleted += ShowEditResult;
            client.EditEmailClientAsync(import, App.Token);
        }
        #region 编辑
        void ShowEditResult(object sender, EditEmailClientCompletedEventArgs e)
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


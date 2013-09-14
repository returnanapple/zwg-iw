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
    public partial class EmailHostsPage_CreateTool : ChildWindow
    {
        public bool ShowError { get; set; }
        public string Error { get; set; }

        public EmailHostsPage_CreateTool()
        {
            InitializeComponent();
            this.ShowError = false;
            this.Error = "";
        }

        private void Create(object sender, EventArgs e)
        {
            AddEmailClientImport import = new AddEmailClientImport
            {
                Key = input_key.Text,
                Host = input_host.Text,
                Port = Convert.ToInt32(input_port.Text),
                Remark = input_remark.Text
            };
            SystemSettingServiceClient client = new SystemSettingServiceClient();
            client.AddEmailClientCompleted += ShowCreateResult;
            client.AddEmailClientAsync(import, App.Token);
        }
        #region 创建
        void ShowCreateResult(object sender, AddEmailClientCompletedEventArgs e)
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


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
using IWorld.Admin.ManagerService;

namespace IWorld.Admin
{
    public partial class ExamineMangersPage_CreateTool : ChildWindow
    {
        public bool ShowError { get; set; }
        public string Error { get; set; }

        public ExamineMangersPage_CreateTool()
        {
            InitializeComponent();
        }

        private void Create(object sender, EventArgs e)
        {
            ManagerServiceClient client = new ManagerServiceClient();
            client.AddCompleted += ShowCreateResult;
            client.AddAsync(input_username.Text, input_password.Text, input_group.Text, App.Token);
        }
        #region 创建

        void ShowCreateResult(object sender, AddCompletedEventArgs e)
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


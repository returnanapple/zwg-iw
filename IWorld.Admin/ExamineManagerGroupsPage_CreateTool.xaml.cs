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
    public partial class ExamineManagerGroupsPage_CreateTool : ChildWindow
    {
        public bool ShowError { get; set; }
        public string Error { get; set; }

        public ExamineManagerGroupsPage_CreateTool()
        {
            InitializeComponent();
            this.ShowError = false;
            this.Error = "";
        }

        private void Create(object sender, EventArgs e)
        {
            AddManagerGroupImport impoet = new AddManagerGroupImport
            {
                Name = input_Name.Text,
                Grade = Convert.ToInt32(input_Grade.Text),
                CanViewUsers = input_CanViewUsers.SelectedIndex == 0,
                CanEditUsers = input_CanEditUsers.SelectedIndex == 0,
                CanViewTickets = input_CanViewTickets.SelectedIndex == 0,
                CanEditTickets = input_CanEditTickets.SelectedIndex == 0,
                CanViewActivities = input_CanViewActivities.SelectedIndex == 0,
                CanEditActivities = input_CanEditActivities.SelectedIndex == 0,
                CanSettingSite = input_CanSettingSite.SelectedIndex == 0,
                CanViewDataReports = input_CanViewDataReports.SelectedIndex == 0,
                CanViewAndAddFundsReports = input_CanViewAndAddFundsReports.SelectedIndex == 0,
                CanViewAndEditMessageBox = input_CanViewAndEditMessageBox.SelectedIndex == 0,
                CanViewAndEditManagers = input_CanViewAndEditManagers.SelectedIndex == 0
            };
            ManagerServiceClient client = new ManagerServiceClient();
            client.AddGroupCompleted += ShowCreateResult;
            client.AddGroupAsync(impoet, App.Token);
        }
        #region 创建

        void ShowCreateResult(object sender, AddGroupCompletedEventArgs e)
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


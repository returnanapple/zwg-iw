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
    public partial class ExamineManagerGroupsPage_EditTool : ChildWindow
    {
        public bool ShowError { get; set; }
        public string Error { get; set; }
        ManagerGroupResult Group { get; set; }

        public ExamineManagerGroupsPage_EditTool(ManagerGroupResult group)
        {
            InitializeComponent();
            this.ShowError = false;
            this.Error = "";
            this.Group = group;

            input_Name.Text = group.Name;
            input_Grade.Text = group.Grade.ToString();
            input_CanViewUsers.SelectedIndex = group.CanViewUsers ? 0 : 1;
            input_CanEditUsers.SelectedIndex = group.CanEditUsers ? 0 : 1;
            input_CanViewTickets.SelectedIndex = group.CanViewTickets ? 0 : 1;
            input_CanEditTickets.SelectedIndex = group.CanEditTickets ? 0 : 1;
            input_CanViewActivities.SelectedIndex = group.CanViewActivities ? 0 : 1;
            input_CanEditActivities.SelectedIndex = group.CanEditActivities ? 0 : 1;
            input_CanSettingSite.SelectedIndex = group.CanSettingSite ? 0 : 1;
            input_CanViewDataReports.SelectedIndex = group.CanViewDataReports ? 0 : 1;
            input_CanViewAndAddFundsReports.SelectedIndex = group.CanViewAndAddFundsReports ? 0 : 1;
            input_CanViewAndEditMessageBox.SelectedIndex = group.CanViewAndEditMessageBox ? 0 : 1;
        }

        private void Edit(object sender, EventArgs e)
        {
            EditManagerGroupImport impoet = new EditManagerGroupImport
            {
                GroupId = this.Group.GroupId,
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
                CanViewAndEditMessageBox = input_CanViewAndEditMessageBox.SelectedIndex == 0
            };
            ManagerServiceClient client = new ManagerServiceClient();
            client.EditGroupCompleted += ShowEditResult;
            client.EditGroupAsync(impoet, App.Token);
        }
        #region 编辑

        void ShowEditResult(object sender, EditGroupCompletedEventArgs e)
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


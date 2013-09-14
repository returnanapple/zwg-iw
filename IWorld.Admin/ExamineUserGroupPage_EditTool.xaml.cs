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
using IWorld.Admin.UserInfoService;

namespace IWorld.Admin
{
    public partial class ExamineUserGroupPage_EditTool : ChildWindow
    {
        UserGroupResult Group { get; set; }

        public bool ShowError { get; set; }

        public string Error { get; set; }

        private bool _wthdrawalsAtAnyTime = false;

        public ExamineUserGroupPage_EditTool(UserGroupResult group)
        {
            InitializeComponent();
            this.Group = group;
            this.ShowError = false;
            this.Error = "";

            input_Name.Text = group.Name;
            input_Grade.Text = group.Grade.ToString();
            input_LimitOfConsumption.Text = group.LimitOfConsumption.ToString();
            input_UpperOfConsumption.Text = group.UpperOfConsumption.ToString();
            input_Withdrawals.Text = group.Withdrawals.ToString();
            input_MinimumWithdrawalAmount.Text = group.MinimumWithdrawalAmount.ToString();
            input_MaximumWithdrawalAmount.Text = group.MaximumWithdrawalAmount.ToString();
            input_MinimumRechargeAmount.Text = group.MinimumRechargeAmount.ToString();
            input_MaximumRechargeAmount.Text = group.MaximumRechargeAmount.ToString();
            input_MaxOfSubordinate.Text = group.MaxOfSubordinate.ToString();

            if (group.WithdrawalsAtAnyTime == true)
            {
                this._wthdrawalsAtAnyTime = true;
                input_WithdrawalsAtAnyTime_true.IsChecked = true;
            }
            else
            {
                this._wthdrawalsAtAnyTime = false;
                input_WithdrawalsAtAnyTime_false.IsChecked = true;
            }
        }

        private void Edit(object sender, EventArgs e)
        {
            EditUserGroupImport import = new EditUserGroupImport
            {
                Name = input_Name.Text,
                GroupId = this.Group.GroupId,
                Grade = Convert.ToInt32(input_Grade.Text),
                ColorOfName = Group.ColorOfName,
                LimitOfConsumption = Convert.ToInt32(input_LimitOfConsumption.Text),
                UpperOfConsumption = Convert.ToInt32(input_UpperOfConsumption.Text),
                Withdrawals = Convert.ToInt32(input_Withdrawals.Text),
                MinimumWithdrawalAmount = Convert.ToInt32(input_MinimumWithdrawalAmount.Text),
                MaximumWithdrawalAmount = Convert.ToInt32(input_MaximumWithdrawalAmount.Text),
                MinimumRechargeAmount = Convert.ToInt32(input_MinimumRechargeAmount.Text),
                MaximumRechargeAmount = Convert.ToInt32(input_MaximumRechargeAmount.Text),
                WithdrawalsAtAnyTime = this._wthdrawalsAtAnyTime,
                MaxOfSubordinate = Convert.ToInt32(input_MaxOfSubordinate.Text)
            };
            UserInfoServiceClient client = new UserInfoServiceClient();
            client.EditGroupCompleted += ShowEditResult;
            client.EditGroupAsync(import, App.Token);
        }
        #region 编辑用户组
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

        private void BackToListPage(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }

        private void SelectWithdrawalsAtAnyTime(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Name == "input_WithdrawalsAtAnyTime_true")
            {
                this._wthdrawalsAtAnyTime = true;
            }
            else
            {
                this._wthdrawalsAtAnyTime = false;
            }
        }
    }
}


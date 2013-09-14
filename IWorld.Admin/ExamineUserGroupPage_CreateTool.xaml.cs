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
    public partial class ExamineUserGroupPage_CreateTool : ChildWindow
    {
        public bool ShowError { get; set; }

        public string Error { get; set; }

        private bool _wthdrawalsAtAnyTime = false;

        public ExamineUserGroupPage_CreateTool()
        {
            InitializeComponent();
            this.ShowError = false;
            this.Error = "";
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

        private void Create(object sender, EventArgs e)
        {
            AddUserGroupImport import = new AddUserGroupImport
            {
                Name = input_Name.Text,
                Grade = Convert.ToInt32(input_Grade.Text),
                ColorOfName = "",
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
            client.AddGroupCompleted += ShowCreateResult;
            client.AddGroupAsync(import, App.Token);
        }
        #region 创建用户组
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

        private void BackToListPage(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


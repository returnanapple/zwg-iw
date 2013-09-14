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
using IWorld.Admin.UserInfoService;

namespace IWorld.Admin
{
    public partial class ExamineUserGroupPage_FullWindow : ChildWindow
    {
        public ExamineUserGroupPage_FullWindow(UserGroupResult group)
        {
            InitializeComponent();
            text_Name.Text = group.Name;
            text_Grade.Text = group.Grade.ToString();
            text_LimitOfConsumption.Text = group.LimitOfConsumption.ToString();
            text_UpperOfConsumption.Text = group.UpperOfConsumption.ToString();
            text_Withdrawals.Text = group.Withdrawals.ToString();
            text_MinimumWithdrawalAmount.Text = group.MinimumWithdrawalAmount.ToString();
            text_MaximumWithdrawalAmount.Text = group.MaximumWithdrawalAmount.ToString();
            text_MinimumRechargeAmount.Text = group.MinimumRechargeAmount.ToString();
            text_MaximumRechargeAmount.Text = group.MaximumRechargeAmount.ToString();
            text_WithdrawalsAtAnyTime.Text = group.WithdrawalsAtAnyTime.ToString();
            text_MaxOfSubordinate.Text = group.MaxOfSubordinate.ToString();
        }

        private void SubmitButton_ClickEventHandler(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }
    }
}


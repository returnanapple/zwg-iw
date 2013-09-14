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
    public partial class BankAccountsPage_EditTool : ChildWindow
    {
        public bool ShowError { get; set; }
        public string Error { get; set; }
        BankAccountResult BankAccount { get; set; }

        public BankAccountsPage_EditTool(BankAccountResult bankAccount)
        {
            InitializeComponent();
            this.ShowError = false;
            this.Error = "";
            this.BankAccount = bankAccount;

            input_key.Text = bankAccount.Key;
            input_card.Text = bankAccount.Card;
            input_holder.Text = bankAccount.Name;
            input_remark.Text = bankAccount.Remark;
            input_order.Text = bankAccount.Order.ToString();
        }

        private void Edit(object sender, EventArgs e)
        {
            EditBankAccountImport import = new EditBankAccountImport
            {
                BankAccountId = this.BankAccount.BankAccountId,
                Key = input_key.Text,
                Bank = Bank.中国工商银行,
                Card = input_card.Text,
                Name = input_holder.Text,
                Remark = input_remark.Text,
                Order = Convert.ToInt32(input_order.Text)
            };
            SystemSettingServiceClient client = new SystemSettingServiceClient();
            client.EditBankAccountCompleted += ShowEditResult;
            client.EditBankAccountAsync(import, App.Token);
        }
        #region 编辑
        void ShowEditResult(object sender, EditBankAccountCompletedEventArgs e)
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


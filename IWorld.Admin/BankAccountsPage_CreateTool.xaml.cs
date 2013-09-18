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
    public partial class BankAccountsPage_CreateTool : ChildWindow
    {
        public bool ShowError { get; set; }
        public string Error { get; set; }

        public BankAccountsPage_CreateTool()
        {
            InitializeComponent();
            this.ShowError = false;
            this.Error = "";
        }

        private void Create(object sender, EventArgs e)
        {
            Bank _bank;
            Enum.TryParse<Bank>(input_bank.Text, out _bank);
            AddBankAccountImport import = new AddBankAccountImport
            {
                Key = input_key.Text,
                Bank = _bank,
                Card = input_card.Text,
                Name = input_holder.Text,
                Remark = input_remark.Text,
                Order = Convert.ToInt32(input_order.Text)
            };
            SystemSettingServiceClient client = new SystemSettingServiceClient();
            client.AddBankAccountCompleted += ShowCreateResult;
            client.AddBankAccountAsync(import, App.Token);
        }
        #region 创建
        void ShowCreateResult(object sender, AddBankAccountCompletedEventArgs e)
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


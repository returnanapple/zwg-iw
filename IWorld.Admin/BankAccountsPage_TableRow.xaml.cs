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
using IWorld.Admin.SystemSettingService;

namespace IWorld.Admin
{
    public partial class BankAccountsPage_TableRow : UserControl, ITableToolRow
    {
        BankAccountResult BankAccount { get; set; }
        int _row = 0;

        public BankAccountsPage_TableRow(BankAccountResult bankAccount, int row)
        {
            InitializeComponent();
            this.BankAccount = bankAccount;
            this._row = row;

            text_key.Text = bankAccount.Key;
            text_card.Text = bankAccount.Card;
            text_holder.Text = bankAccount.Name;
            text_bank.Text = bankAccount.Bank.ToString();
            text_remark.Text = bankAccount.Remark;
            text_order.Text = bankAccount.Order.ToString();
            if (bankAccount.IsDefault)
            {
                button_setDefault.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        public FrameworkElement GetElement()
        {
            return this;
        }

        public FrameworkElement GetChildWindow()
        {
            return null;
        }

        public event NDelegate RefreshEventHandler;

        public event TableToolBodyHoverDelegate HoverEventHandler;

        public event TableToolBodyHoverDelegate UnhoverEventHandler;

        private void Hover(object sender, MouseEventArgs e)
        {
            if (HoverEventHandler != null)
            {
                HoverEventHandler(this, new TableToolBodyHoverEventArgs(this._row));
            }
        }

        private void Unhover(object sender, MouseEventArgs e)
        {
            if (UnhoverEventHandler != null)
            {
                UnhoverEventHandler(this, new TableToolBodyHoverEventArgs(this._row));
            }
        }

        private void Edit(object sender, MouseButtonEventArgs e)
        {
            BankAccountsPage_EditTool et = new BankAccountsPage_EditTool(this.BankAccount);
            et.Closed += ShowEditResult;
            et.Show();
        }
        #region 编辑
        void ShowEditResult(object sender, EventArgs e)
        {
            BankAccountsPage_EditTool et = (BankAccountsPage_EditTool)sender;
            if (et.DialogResult == true)
            {
                if (et.ShowError)
                {
                    ErrorPrompt ep = new ErrorPrompt(et.Error);
                    ep.Show();
                }
                else
                {
                    if (RefreshEventHandler != null)
                    {
                        RefreshEventHandler(this, new EventArgs());
                    }
                }
            }
        }
        #endregion

        private void Delete(object sender, MouseButtonEventArgs e)
        {
            NormalPrompt np = new NormalPrompt("警告：该操作将删除该银行账户！");
            np.Closed += Delete_do;
            np.Show();
        }
        #region 删除

        void Delete_do(object sender, EventArgs e)
        {
            NormalPrompt np = (NormalPrompt)sender;
            if (np.DialogResult == true)
            {
                SystemSettingServiceClient client = new SystemSettingServiceClient();
                client.RemoveBankAccountCompleted += ShowDeleteResult;
                client.RemoveBankAccountAsync(this.BankAccount.BankAccountId, App.Token);
            }
        }

        void ShowDeleteResult(object sender, RemoveBankAccountCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                if (RefreshEventHandler != null)
                {
                    RefreshEventHandler(this, new EventArgs());
                }
            }
            else
            {
                ErrorPrompt ep = new ErrorPrompt(e.Result.Error);
                ep.Show();
            }
        }

        #endregion

        private void SetDefault(object sender, MouseButtonEventArgs e)
        {
            SystemSettingServiceClient client = new SystemSettingServiceClient();
            client.SetDefaultBankAccountCompleted += ShowSetDefaultResult;
            client.SetDefaultBankAccountAsync(this.BankAccount.BankAccountId, App.Token);
        }
        #region 设置默认
        void ShowSetDefaultResult(object sender, SetDefaultBankAccountCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                if (RefreshEventHandler != null)
                {
                    RefreshEventHandler(this, new EventArgs());
                }
            }
            else
            {
                ErrorPrompt ep = new ErrorPrompt(e.Result.Error);
                ep.Show();
            }
        }
        #endregion
    }
}

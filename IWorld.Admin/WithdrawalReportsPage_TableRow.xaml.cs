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
using IWorld.Admin.DataReportService;

namespace IWorld.Admin
{
    public partial class WithdrawalReportsPage_TableRow : UserControl, ITableToolRow
    {
        public WithdrawalResult Withdrawal { get; set; }
        int _row = 0;

        public WithdrawalReportsPage_TableRow(WithdrawalResult withdrawal, int row)
        {
            InitializeComponent();
            this.Withdrawal = withdrawal;
            this._row = row;

            button_owner.Text = withdrawal.Owner;
            text_sum.Text = withdrawal.Sum.ToString("0.00");
            text_card.Text = withdrawal.Card;
            text_holder.Text = withdrawal.Name;
            text_status.Text = withdrawal.Status.ToString();
            if (withdrawal.Status != WithdrawalsStatus.处理中)
            {
                button_determinet.Visibility = System.Windows.Visibility.Collapsed;
                button_negative.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        #region 事件

        public event NDelegate RefreshEventHandler;

        public event NDelegate SelectForOwnerEventHandler;

        public event TableToolBodyHoverDelegate HoverEventHandler;

        public event TableToolBodyHoverDelegate UnhoverEventHandler;

        #endregion

        public FrameworkElement GetElement()
        {
            return this;
        }

        public FrameworkElement GetChildWindow()
        {
            return null;
        }

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

        private void CopyUsername(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(Withdrawal.Owner);
        }

        private void CopyCard(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(Withdrawal.Card);
        }

        private void CopyHolder(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(Withdrawal.Name);
        }

        private void CopySum(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(Withdrawal.Sum.ToString());
        }

        private void Determinet(object sender, MouseButtonEventArgs e)
        {
            NormalPrompt np = new NormalPrompt("你确定要支付该提现请求吗？");
            np.Closed += Determinet_do;
            np.Show();
        }
        #region 确认

        void Determinet_do(object sender, EventArgs e)
        {
            NormalPrompt np = (NormalPrompt)sender;
            if (np.DialogResult == true)
            {
                DataReportServiceClient client = new DataReportServiceClient();
                client.DeterminetWithdrawalCompleted += ShowDeterminetResult;
                client.DeterminetWithdrawalAsync(this.Withdrawal.WithdrawalId, App.Token);
            }
        }

        void ShowDeterminetResult(object sender, DeterminetWithdrawalCompletedEventArgs e)
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

        private void Negative(object sender, MouseButtonEventArgs e)
        {
            WithdrawalReportsPage_NegativeTool nt = new WithdrawalReportsPage_NegativeTool(this.Withdrawal);
            nt.Closed += ShowNegativeResult;
            nt.Show();
        }
        #region 否决
        void ShowNegativeResult(object sender, EventArgs e)
        {
            WithdrawalReportsPage_NegativeTool nt = (WithdrawalReportsPage_NegativeTool)sender;
            if (nt.DialogResult == true)
            {
                if (nt.ShowError)
                {
                    ErrorPrompt ep = new ErrorPrompt(nt.Error);
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
    }
}

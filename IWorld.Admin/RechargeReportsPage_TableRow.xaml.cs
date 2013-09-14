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
    public partial class RechargeReportsPage_TableRow : UserControl, ITableToolRow
    {
        public RechargeResult Recharge { get; set; }
        int _row = 0;

        public RechargeReportsPage_TableRow(RechargeResult recharge, int row)
        {
            InitializeComponent();
            this.Recharge = recharge;
            this._row = row;

            button_owner.Text = recharge.Owner;
            text_payer.Text = recharge.Payer;
            text_sum.Text = recharge.Sum.ToString("0.00");
            text_status.Text = recharge.Status.ToString();
            if (recharge.Status == RechargeStatus.充值成功 || recharge.Status == RechargeStatus.失败)
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

        private void SelectForOwner(object sender, MouseButtonEventArgs e)
        {
            if (SelectForOwnerEventHandler != null)
            {
                SelectForOwnerEventHandler(this, new EventArgs());
            }
        }

        private void ViewFull(object sender, MouseButtonEventArgs e)
        {
            RechargeReportsPage_FullWindow fw = new RechargeReportsPage_FullWindow(this.Recharge);
            fw.Show();
        }

        private void Determinet(object sender, MouseButtonEventArgs e)
        {
            RechargeReportsPage_DeterminetTool dt = new RechargeReportsPage_DeterminetTool(this.Recharge);
            dt.Closed += ShowDeterminetResult;
            dt.Show();
        }
        #region 确认
        void ShowDeterminetResult(object sender, EventArgs e)
        {
            RechargeReportsPage_DeterminetTool dt = (RechargeReportsPage_DeterminetTool)sender;
            if (dt.DialogResult == true)
            {
                if (dt.ShowError)
                {
                    ErrorPrompt ep = new ErrorPrompt(dt.Error);
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

        private void Negative(object sender, MouseButtonEventArgs e)
        {
            RechargeReportsPage_NegativeTool nt = new RechargeReportsPage_NegativeTool(this.Recharge);
            nt.Closed += ShowNegativeResult;
            nt.Show();
        }
        #region 否决
        void ShowNegativeResult(object sender, EventArgs e)
        {
            RechargeReportsPage_NegativeTool nt = (RechargeReportsPage_NegativeTool)sender;
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

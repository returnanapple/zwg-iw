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
using IWorld.Client.Class;
using IWorld.Client.FundsService;

namespace IWorld.Client
{
    public partial class FundsPage_TableRow : UserControl, ITableToolRow
    {
        RechargeDetailsResult RdResult { get; set; }
        int _row = 0;
        public FundsPage_TableRow(RechargeDetailsResult rdResult, int row)
        {
            InitializeComponent();
            this._row = row;
            this.RdResult = rdResult;

            text_id.Text = rdResult.RechargeId.ToString("000000");
            text_to.Text = rdResult.To;
            text_from.Text = rdResult.From;
            text_sum.Text = rdResult.Sum.ToString("0.00");
            text_time.Text = rdResult.Time.ToShortDateString();
            text_status.Text = rdResult.Status.ToString();
        }

        public FrameworkElement GetElement()
        {
            return this;
        }

        public FrameworkElement GetChildWindow()
        {
            return null;
        }

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

        private void ViewFull(object sender, MouseButtonEventArgs e)
        {
            FundsPage_FullWindow fw = new FundsPage_FullWindow(this.RdResult);
            fw.Show();
        }
    }
}

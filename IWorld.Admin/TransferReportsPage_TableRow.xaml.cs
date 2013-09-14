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
    public partial class TransferReportsPage_TableRow : UserControl, ITableToolRow
    {
        public TransferResult Transfer { get; set; }
        int _row = 0;

        public TransferReportsPage_TableRow(TransferResult transfer, int row)
        {
            InitializeComponent();
            this.Transfer = transfer;
            this._row = row;

            button_owner.Text = transfer.Owner;
            text_sum.Text = transfer.Sum.ToString("0.00");
            text_remark.Text = transfer.Remark;
        }

        #region 事件

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
    }
}

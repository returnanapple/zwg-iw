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
    public partial class SiteReportPage_TableRow : UserControl, ITableToolRow
    {
        int _row = 0;

        public SiteReportPage_TableRow(SiteDataResult data, int row)
        {
            InitializeComponent();
            this._row = row;

            text_Date.Text = data.Date;
            text_CountOfSetIn.Text = data.CountOfSetIn.ToString();
            text_CountOfSetUp.Text = data.CountOfSetUp.ToString();
            text_AmountOfBets.Text = data.AmountOfBets.ToString();
            text_Bonus.Text = data.Bonus.ToString();
            text_Expenditures.Text = data.Expenditures.ToString();
            text_GainsAndLosses.Text = data.GainsAndLosses.ToString();
            text_Recharge.Text = data.Recharge.ToString();
            text_Withdrawal.Text = data.Withdrawal.ToString();
            text_Transfer.Text = data.Transfer.ToString();
            text_Rebate.Text = data.ReturnPoints.ToString();
        }

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

        public event TableToolBodyHoverDelegate HoverEventHandler;

        public event TableToolBodyHoverDelegate UnhoverEventHandler;
    }
}

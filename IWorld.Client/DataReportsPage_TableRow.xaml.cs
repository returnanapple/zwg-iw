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
using IWorld.Client.DataReportService;

namespace IWorld.Client
{
    public partial class DataReportsPage_TableRow : UserControl, ITableToolRow
    {
        int _row = 0;
        public DataReportsPage_TableRow(DataReportsResult result, int row)
        {
            InitializeComponent();
            this._row = row;

            text_user.Text = result.User;
            text_returnPoints.Text = result.ReturnPoints.ToString();
            text_bet.Text = result.Bet.ToString();
            text_bonus.Text = result.Bonus.ToString();
            text_profit.Text = result.Profit.ToString();
            text_recharge.Text = result.Recharge.ToString();
            text_withdrawal.Text = result.Withdrawal.ToString();
            text_expenditures.Text = result.Expenditures.ToString();
            if (result.Profit > 0)
            {
                text_profit.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (result.Profit < 0)
            {
                text_profit.Foreground = new SolidColorBrush(Colors.Green);
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
    }
}

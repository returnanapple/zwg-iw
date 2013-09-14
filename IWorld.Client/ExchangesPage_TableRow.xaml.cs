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
using IWorld.Client.ActivityService;

namespace IWorld.Client
{
    public partial class ExchangesPage_TableRow : UserControl, ITableToolRow
    {
        ExchangeActivitiesResult Exchange { get; set; }
        int _row = 0;

        public ExchangesPage_TableRow(ExchangeActivitiesResult exchange, int row)
        {
            InitializeComponent();
            this.Exchange = exchange;
            this._row = row;

            text_name.Text = exchange.Name;
            text_places.Text = exchange.Places.ToString();
            text_unitPrice.Text = exchange.UnitPrice.ToString("0.00");
            text_beginTime.Text = exchange.BeginTime.ToShortDateString();
            text_endTime.Text = exchange.EndTime.ToShortDateString();
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

        private void ViewFull(object sender, MouseButtonEventArgs e)
        {
            ExchangesPage_FullWindow fw = new ExchangesPage_FullWindow(this.Exchange);
            fw.Closed += Exchange_do;
            fw.Show();
        }
        #region 兑换

        void Exchange_do(object sender, EventArgs e)
        {
            ExchangesPage_FullWindow fw = (ExchangesPage_FullWindow)sender;
            if (fw.DialogResult == true)
            {
                ExchangesPage_ExchangeTool et = new ExchangesPage_ExchangeTool(this.Exchange);
                et.Closed += ShowExchangeResult;
                et.Show();
            }
        }

        void ShowExchangeResult(object sender, EventArgs e)
        {
            ExchangesPage_ExchangeTool et = (ExchangesPage_ExchangeTool)sender;
            if (et.DialogResult == true)
            {
                if (et.ShowError)
                {
                    ErrorPromt ep = new ErrorPromt(et.Error);
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

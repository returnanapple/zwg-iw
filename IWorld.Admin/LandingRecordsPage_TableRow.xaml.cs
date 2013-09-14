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
using IWorld.Admin.UserInfoService;

namespace IWorld.Admin
{
    public partial class LandingRecordsPage_TableRow : UserControl, ITableToolRow
    {
        int _row = 0;

        public LandingRecordsPage_TableRow(LandingRecordResult record, int row)
        {
            InitializeComponent();
            this._row = row;
            text_username.Text = record.Username;
            text_time.Text = string.Format("{0} {1}"
                , record.LandingTime.ToShortDateString(), record.LandingTime.ToShortTimeString());
            text_ip.Text = record.Ip;
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

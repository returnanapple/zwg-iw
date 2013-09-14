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
using IWorld.Admin.LotteryTicketService;

namespace IWorld.Admin
{
    public partial class ExamineVirtualTopPage_TableRow : UserControl, ITableToolRow
    {
        public VirtualTopResult VirtualTop { get; set; }

        int _row = 0;

        public ExamineVirtualTopPage_TableRow(VirtualTopResult virtualTop, int row)
        {
            InitializeComponent();
            this.VirtualTop = virtualTop;
            this._row = row;

            text_ticket.Text = virtualTop.Ticket;
            text_sum.Text = virtualTop.Sum.ToString();
        }

        #region 事件

        public event NDelegate SelectForTicketEventHandler;

        public event NDelegate RefreshEventHandler;

        public event TableToolBodyHoverDelegate HoverEventHandler;

        public event TableToolBodyHoverDelegate UnhoverEventHandler;
        
        #endregion

        private void Edit(object sender, MouseButtonEventArgs e)
        {
            ExamineVirtualTopPage_EditTool et = new ExamineVirtualTopPage_EditTool(this.VirtualTop);
            et.Closed += ShowEditResult;
            et.Show();
        }
        #region 编辑
        void ShowEditResult(object sender, EventArgs e)
        {
            ExamineVirtualTopPage_EditTool et = (ExamineVirtualTopPage_EditTool)sender;
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
            LotteryTicketServiceClient client = new LotteryTicketServiceClient();
            client.RemoveVirtualTopCompleted += ShowDeleteResult;
            client.RemoveVirtualTopAsync(this.VirtualTop.VirtualTopId, App.Token);
        }
        #region 删除
        void ShowDeleteResult(object sender, RemoveVirtualTopCompletedEventArgs e)
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

        private void SelectForTickt(object sender, MouseButtonEventArgs e)
        {
            if (SelectForTicketEventHandler != null)
            {
                SelectForTicketEventHandler(this, new EventArgs());
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

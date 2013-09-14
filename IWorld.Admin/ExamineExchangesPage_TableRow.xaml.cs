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
using IWorld.Admin.ExchangeService;

namespace IWorld.Admin
{
    public partial class ExamineExchangesPage_TableRow : UserControl, ITableToolRow
    {
        ExchangeResult Exchange { get; set; }
        int _row = 0;

        public ExamineExchangesPage_TableRow(ExchangeResult exchange, int row)
        {
            InitializeComponent();
            this.Exchange = exchange;
            this._row = row;

            text_name.Text = exchange.Name;
            text_places.Text = exchange.Places.ToString();
            text_unitPrice.Text = exchange.UnitPrice.ToString("0.00");
            text_beginTime.Text = exchange.BeginTime.ToShortDateString();
            text_endTime.Text = exchange.EndTime.ToShortDateString();
            text_hide.Text = exchange.Hide ? "是" : "否";
        }

        #region 事件

        public event NDelegate RefreshEventHandler;

        public event TableToolBodyHoverDelegate HoverEventHandler;

        public event TableToolBodyHoverDelegate UnhoverEventHandler;

        #endregion

        private void ViewFull(object sender, MouseButtonEventArgs e)
        {
            ExamineExchangesPage_FullWindow fw = new ExamineExchangesPage_FullWindow(this.Exchange);
            fw.Show();
        }

        private void Edit(object sender, MouseButtonEventArgs e)
        {
            if (this.Exchange.BeginTime < DateTime.Now)
            {
                ExamineExchangesPage_EditTool_Basic et = new ExamineExchangesPage_EditTool_Basic(this.Exchange);
                et.Closed += ShowEditResult_Basic;
                et.Show();
            }
            else
            {
                ExamineExchangesPage_EditTool et = new ExamineExchangesPage_EditTool(this.Exchange);
                et.Closed += ShowEditResult;
                et.Show();
            }
        }
        #region 编辑

        void ShowEditResult_Basic(object sender, EventArgs e)
        {
            ExamineExchangesPage_EditTool_Basic et = (ExamineExchangesPage_EditTool_Basic)sender;
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

        void ShowEditResult(object sender, EventArgs e)
        {
            ExamineExchangesPage_EditTool et = (ExamineExchangesPage_EditTool)sender;
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
            string message = string.Format("警告：该操作将删除活动 {0}", this.Exchange.Name);
            NormalPrompt np = new NormalPrompt(message);
            np.Closed += Delete_do;
            np.Show();
        }
        #region 删除

        void Delete_do(object sender, EventArgs e)
        {
            NormalPrompt np = (NormalPrompt)sender;
            if (np.DialogResult == true)
            {
                ExchangeServiceClient client = new ExchangeServiceClient();
                client.RemoveExchangeCompleted += ShowDeleteResult;
                client.RemoveExchangeAsync(this.Exchange.ExchangeId, App.Token);
            }
        }

        void ShowDeleteResult(object sender, RemoveExchangeCompletedEventArgs e)
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

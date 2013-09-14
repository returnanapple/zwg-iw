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
using IWorld.Admin.ExchangeService;

namespace IWorld.Admin
{
    public partial class ExamineExchangesPage_EditTool_Basic : ChildWindow
    {
        public bool ShowError { get; set; }
        public string Error { get; set; }
        ExchangeResult Exchange { get; set; }

        public ExamineExchangesPage_EditTool_Basic(ExchangeResult exchange)
        {
            InitializeComponent();
            this.ShowError = false;
            this.Error = "";
            this.Exchange = exchange;

            input_days.Text = exchange.Days.ToString();
            input_autoDelete.SelectedIndex = exchange.AutoDelete ? 0 : 1;
            input_hide.SelectedIndex = exchange.AutoDelete ? 0 : 1;
        }

        private void Edit(object sender, EventArgs e)
        {
            EditExchangeImport_Basic import = new EditExchangeImport_Basic
            {
                ExchangeId = this.Exchange.ExchangeId,
                Days = Convert.ToInt32(input_days.Text),
                AutoDelete = ((TextBlock)input_autoDelete.SelectedItem).Text == "是",
                Hide = ((TextBlock)input_hide.SelectedItem).Text == "是"

            };
            ExchangeServiceClient client = new ExchangeServiceClient();
            client.EditExchange_BasicCompleted += ShowEditResult;
            client.EditExchange_BasicAsync(import, App.Token);
        }
        #region 编辑
        void ShowEditResult(object sender, EditExchange_BasicCompletedEventArgs e)
        {
            if (!e.Result.Success)
            {
                this.ShowError = true;
                this.Error = e.Result.Error;
            }
            this.DialogResult = true;
        }
        #endregion

        private void BackToList(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


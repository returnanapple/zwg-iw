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
using IWorld.Admin.LotteryTicketService;

namespace IWorld.Admin
{
    public partial class ExaminePlaysPage_EditTool : ChildWindow
    {
        public bool ShowError { get; set; }

        public string Error { get; set; }

        HowToPlayResult HowToPlay { get; set; }

        public ExaminePlaysPage_EditTool(HowToPlayResult howToPlay)
        {
            InitializeComponent();
            this.HowToPlay = howToPlay;
            this.Error = "";
            this.ShowError = false;

            input_name.Text = howToPlay.Name;
            input_description.Text = howToPlay.Description;
            input_rule.Text = howToPlay.Rule;
            input_odds.Text = howToPlay.Odds.ToString();
            input_order.Text = howToPlay.Order.ToString();
        }

        private void Edit(object sender, EventArgs e)
        {
            EditHowToPlayImport import = new EditHowToPlayImport
            {
                HowToPlayId = this.HowToPlay.HowToPlayId,
                Name = input_name.Text,
                Description = input_description.Text,
                Rule = input_rule.Text,
                Odds = Convert.ToDouble(input_odds.Text),
                Order = Convert.ToInt32(input_order.Text)
            };
            LotteryTicketServiceClient client = new LotteryTicketServiceClient();
            client.EditHowToPlayCompleted += ShowEditResult;
            client.EditHowToPlayAsync(import, App.Token);
        }
        #region 修改
        void ShowEditResult(object sender, EditHowToPlayCompletedEventArgs e)
        {
            if (!e.Result.Success)
            {
                this.ShowError = true;
                this.Error = e.Result.Error;
            }
            this.DialogResult = true;
        }
        #endregion

        private void BackToListPage(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


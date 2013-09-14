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
    public partial class ExamineTagsPage_EditTool : ChildWindow
    {
        public bool ShowError { get; set; }

        public string Error { get; set; }

        PlayTagResult PlayTag { get; set; }

        public ExamineTagsPage_EditTool(PlayTagResult tag)
        {
            InitializeComponent();
            this.Tag = tag;
            this.Error = "";
            this.ShowError = false;

            input_name.Text = tag.Name;
            input_order.Text = tag.Order.ToString();
        }

        private void Edit(object sender, EventArgs e)
        {
            EditPlayTagImport import = new EditPlayTagImport
            {
                PlayTagId = this.PlayTag.PlayTagId,
                Name = input_name.Text,
                Order = Convert.ToInt32(input_order.Text)
            };
            LotteryTicketServiceClient client = new LotteryTicketServiceClient();
            client.EditPlayTagCompleted += ShowEditResult;
            client.EditPlayTagAsync(import, App.Token);
        }
        #region 修改
        void ShowEditResult(object sender, EditPlayTagCompletedEventArgs e)
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


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
using IWorld.Admin.BulletinService;

namespace IWorld.Admin
{
    public partial class ExamineBulletinsPage_EditTool : ChildWindow
    {
        public bool ShowError { get; set; }

        public string Error { get; set; }

        BulletinResult Bulletin { get; set; }

        bool hide = false;

        bool autoDelete = false;

        public ExamineBulletinsPage_EditTool(BulletinResult bulletin)
        {
            InitializeComponent();
            this.Bulletin = bulletin;
            this.Error = "";
            this.ShowError = false;

            input_title.Text = bulletin.Title;
            input_content.Text = bulletin.Context;
            input_days.Text = bulletin.Days.ToString();
            if (bulletin.Hide)
            {
                hide = true;
                input_hide_true.IsChecked = true;
            }
            if (bulletin.AutoDelete)
            {
                autoDelete = true;
                input_autoDelete_true.IsChecked = true;
            }
        }

        private void Edit(object sender, EventArgs e)
        {
            EditBulletinImport import = new EditBulletinImport
            {
                BulletinId = this.Bulletin.BulletinId,
                Title = input_title.Text,
                Context = input_content.Text,
                Days = Convert.ToInt32(input_days.Text),
                Hide = hide,
                AutoDelete = autoDelete
            };
            BulletinServiceClient client = new BulletinServiceClient();
            client.EditBulletinCompleted += ShowEditResult;
            client.EditBulletinAsync(import, App.Token);
        }
        #region 编辑
        void ShowEditResult(object sender, EditBulletinCompletedEventArgs e)
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

        private void SelectHide(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Name == "input_hide_true")
            {
                hide = true;
            }
            else
            {
                hide = false;
            }
        }

        private void SelectAutoDelete(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Name == "input_autoDelete_true")
            {
                autoDelete = true;
            }
            else
            {
                autoDelete = false;
            }
        }
    }
}


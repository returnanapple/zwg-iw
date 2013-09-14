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
using IWorld.Admin.BulletinService;

namespace IWorld.Admin
{
    public partial class CreateNewBulletinPage : UserControl
    {
        public CreateNewBulletinPage()
        {
            InitializeComponent();
        }

        #region 事件

        public event NDelegate ViewListPageEventHandler;

        #endregion

        private void Create(object sender, EventArgs e)
        {
            AddBulletinImport import = new AddBulletinImport
            {
                Title = input_title.Text,
                Context = input_context.Text,
                BeginTime = input_beginTime.Text,
                Days = Convert.ToInt32(input_days.Text),
                AutoDelete = input_autoDelete_true.IsChecked == true
            };
            BulletinServiceClient client = new BulletinServiceClient();
            client.AddBulletinCompleted += ShowCreateResult;
            client.AddBulletinAsync(import, App.Token);
        }
        #region 创建

        void ShowCreateResult(object sender, AddBulletinCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                if (ViewListPageEventHandler != null)
                {
                    ViewListPageEventHandler(this, new EventArgs());
                }
            }
            else
            {
                ErrorPrompt ep = new ErrorPrompt(e.Result.Error);
                ep.Show();
            }
        }

        #endregion

        private void BackToListPage(object sender, EventArgs e)
        {
            if (ViewListPageEventHandler != null)
            {
                ViewListPageEventHandler(this, new EventArgs());
            }
        }
    }
}

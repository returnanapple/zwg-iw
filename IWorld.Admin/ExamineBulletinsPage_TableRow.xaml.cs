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
    public partial class ExamineBulletinsPage_TableRow : UserControl, ITableToolRow
    {
        BulletinResult Bulletin { get; set; }

        int _row = 0;

        public ExamineBulletinsPage_TableRow(BulletinResult bulletin, int row)
        {
            InitializeComponent();
            this.Bulletin = bulletin;
            this._row = row;

            text_title.Text = TextHelper.Interception(bulletin.Title, 16);
            text_beginTime.Text = bulletin.BeginTime.ToLongDateString();
            text_endTime.Text = bulletin.EndTime.ToLongDateString();
            text_autoDelete.Text = bulletin.AutoDelete ? "是" : "否";
        }

        private void Edit(object sender, MouseButtonEventArgs e)
        {
            ExamineBulletinsPage_EditTool et = new ExamineBulletinsPage_EditTool(Bulletin);
            et.Closed += ShowEditResult;
            et.Show();
        }
        #region 编辑
        void ShowEditResult(object sender, EventArgs e)
        {
            ExamineBulletinsPage_EditTool et = (ExamineBulletinsPage_EditTool)sender;
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
            NormalPrompt np = new NormalPrompt("警告：这个操作将删除公告  " + this.Bulletin.Title);
            np.Closed += Delete_do;
            np.Show();
        }
        #region 删除

        void Delete_do(object sender, EventArgs e)
        {
            NormalPrompt np = (NormalPrompt)sender;
            if (np.DialogResult == true)
            {
                BulletinServiceClient client = new BulletinServiceClient();
                client.RemoveBulletinCompleted += ShowDeleteResult;
                client.RemoveBulletinAsync(this.Bulletin.BulletinId, App.Token);
            }
        }

        void ShowDeleteResult(object sender, RemoveBulletinCompletedEventArgs e)
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
    }
}

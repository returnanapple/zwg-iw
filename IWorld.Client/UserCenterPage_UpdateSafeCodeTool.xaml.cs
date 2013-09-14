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
using IWorld.Client.UsersService;

namespace IWorld.Client
{
    public partial class UserCenterPage_UpdateSafeCodeTool : UserControl
    {
        public UserCenterPage_UpdateSafeCodeTool()
        {
            InitializeComponent();
        }

        public event NDelegate BackEventHandler;

        private void Update(object sender, RoutedEventArgs e)
        {
            if (input_newSafeCode.Password != input_newSafeCode_a.Password)
            {
                ErrorPromt ep = new ErrorPromt("两次输入的新安全密码不一致");
                ep.Show();

                input_oldSafeCode.Password = "";
                input_newSafeCode.Password = "";
                input_newSafeCode_a.Password = "";
            }

            UsersServiceClient client = new UsersServiceClient();
            client.EditSafeWordCompleted += ShowUpdateResult;
            client.EditSafeWordAsync(input_oldSafeCode.Password, input_newSafeCode.Password, App.Token);
        }
        #region 更新

        void ShowUpdateResult(object sender, EditSafeWordCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                ErrorPromt ep = new ErrorPromt("修改密码安全成功");
                ep.Closed += Bank;
                ep.Show();
            }
            else
            {
                ErrorPromt ep = new ErrorPromt(e.Result.Error);
                ep.Show();

                input_oldSafeCode.Password = "";
                input_newSafeCode.Password = "";
                input_newSafeCode_a.Password = "";
            }
        }

        void Bank(object sender, EventArgs e)
        {
            if (BackEventHandler != null)
            {
                BackEventHandler(this, new EventArgs());
            }
        }

        #endregion

        private void Cancel(object sender, RoutedEventArgs e)
        {
            if (BackEventHandler != null)
            {
                BackEventHandler(this, new EventArgs());
            }
        }
    }
}

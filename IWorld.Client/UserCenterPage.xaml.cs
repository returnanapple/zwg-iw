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

namespace IWorld.Client
{
    public partial class UserCenterPage : UserControl
    {
        public UserCenterPage()
        {
            InitializeComponent();
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            InsertUserInfoTool(null, null);
        }

        private void InsertUserInfoTool(object sender, EventArgs e)
        {
            UserCenterPage_UserInfoTool tool = new UserCenterPage_UserInfoTool();
            body.Children.Clear();
            body.Children.Add(tool);
        }

        private void InsertUpdatePasswordTool(object sender, EventArgs e)
        {
            UserCenterPage_UpdatePasswordTool tool = new UserCenterPage_UpdatePasswordTool();
            tool.BackEventHandler += InsertUserInfoTool;
            body.Children.Clear();
            body.Children.Add(tool);
        }

        private void InsertUpdateSafeCodeTool(object sender, EventArgs e)
        {
            UserCenterPage_UpdateSafeCodeTool tool = new UserCenterPage_UpdateSafeCodeTool();
            tool.BackEventHandler += InsertUserInfoTool;
            body.Children.Clear();
            body.Children.Add(tool);
        }

        private void InsertUpdateEmailTool(object sender, EventArgs e)
        {
            UserCenterPageUpdateEmailTool tool = new UserCenterPageUpdateEmailTool();
            tool.BackEventHandler += InsertUserInfoTool;
            body.Children.Clear();
            body.Children.Add(tool);
        }

        private void InsertUpdateBankCardTool(object sender, EventArgs e)
        {
            UserCenterPage_UpdateBankCardTool tool = new UserCenterPage_UpdateBankCardTool();
            tool.BackEventHandler += InsertUserInfoTool;
            body.Children.Clear();
            body.Children.Add(tool);
        }
    }
}

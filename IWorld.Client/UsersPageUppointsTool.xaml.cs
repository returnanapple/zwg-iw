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
    public partial class UsersPageUppointsTool : ChildWindow
    {
        BasicUserInfoResult UserInfo { get; set; }
        public bool ShowError { get; set; }
        public string Error { get; set; }

        public UsersPageUppointsTool(BasicUserInfoResult userInfo)
        {
            InitializeComponent();
            this.UserInfo = userInfo;
            this.ShowError = false;
            this.Error = "";

            input_nrp.Text = userInfo.NormalReturnPoints.ToString();
            input_urp.Text = userInfo.UncertainReturnPoints.ToString();
        }

        private void UpPoints(object sender, RoutedEventArgs e)
        {
            double nrp = Math.Round(Convert.ToDouble(input_nrp.Text), 1);
            double urp = Math.Round(Convert.ToDouble(input_urp.Text), 1);

            UsersServiceClient client = new UsersServiceClient();
            client.UpgradePornCompleted += ShowUpPointsResult;
            client.UpgradePornAsync(this.UserInfo.UserId, nrp, urp, App.Token);
        }
        #region 升点
        void ShowUpPointsResult(object sender, UpgradePornCompletedEventArgs e)
        {
            if (!e.Result.Success)
            {
                this.ShowError = true;
                this.Error = e.Result.Error;
            }
            this.DialogResult = true;
        }
        #endregion

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


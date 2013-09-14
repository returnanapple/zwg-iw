﻿using System;
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
    public partial class UserCenterPage_UpdateBankCardTool : UserControl
    {
        public UserCenterPage_UpdateBankCardTool()
        {
            InitializeComponent();
            input_card.Text = App.UserInfo.Card;
            input_holder.Text = App.UserInfo.Holder;
            if (App.UserInfo.BindingCard)
            {
                input_card.IsEnabled = false;
                input_holder.IsEnabled = false;
                button_update.IsEnabled = false;
            }
        }

        public event NDelegate BackEventHandler;

        private void Update(object sender, RoutedEventArgs e)
        {
            UsersServiceClient client = new UsersServiceClient();
            client.EditBankCompleted += ShowUpdateResult;
            client.EditBankAsync(input_card.Text, input_holder.Text, Bank.中国工商银行, App.Token);
        }
        #region 更新
        void ShowUpdateResult(object sender, EditBankCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                ErrorPromt ep = new ErrorPromt("修改银行卡绑定信息成功，请重新登陆");
                ep.Closed += BankToLoginPage;
                ep.Show();
            }
            else
            {
                ErrorPromt ep = new ErrorPromt(e.Result.Error);
                ep.Show();

                input_card.Text = "";
                input_holder.Text = "";
            }
        }
        void BankToLoginPage(object sender, EventArgs e)
        {
            App.GoToLoginPage();
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
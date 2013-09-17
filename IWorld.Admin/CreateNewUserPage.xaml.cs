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
using IWorld.Admin.UserInfoService;

namespace IWorld.Admin
{
    public partial class CreateNewUserPage : UserControl
    {
        public CreateNewUserPage()
        {
            InitializeComponent();
        }

        #region 事件

        public event NDelegate BankToListPageEventHandler;

        #endregion

        private void CreateNewUser(object sender, EventArgs e)
        {
            CheckUsername();
            CheckPassword();
            CheckNtp();
            ChekUtp();
            CheckMos();

            if (usernameOK && passwordOK && ntpOK && utpOK && mosOK)
            {
                string username = input_username.Text;
                string password = input_password.Text;
                double ntp = Convert.ToDouble(input_nrp.Text);
                double utp = Convert.ToDouble(input_urp.Text);
                int mos = Convert.ToInt32(input_mos.Text);

                UserInfoServiceClient client = new UserInfoServiceClient();
                client.AddUserCompleted += (_sender, _e) =>
                    {
                        if (_e.Result.Success)
                        {
                            if (BankToListPageEventHandler != null)
                            {
                                BankToListPageEventHandler(this, new EventArgs());
                            }
                        }
                        else
                        {
                            ErrorPrompt ep = new ErrorPrompt(_e.Result.Error);
                            ep.Show();
                        }
                    };
                AddUserImport import = new AddUserImport
                {
                    Username = username,
                    Password = password,
                    NormalReturnPoints = ntp,
                    UncertainReturnPoints = utp,
                    MaxOfSubordinate = mos,
                    IsAgents = true
                };
                client.AddUserAsync(import, App.Token);
            }
        }
        #region 检查输入

        bool usernameOK = false;
        bool passwordOK = false;
        bool ntpOK = false;
        bool utpOK = false;
        bool mosOK = false;

        void CheckUsername()
        {
            string username = input_username.Text;
            try
            {
                TextHelper.Check(username, TextHelper.Key.Nickname);
                usernameOK = true;
                check_username.Text = "";
            }
            catch (Exception ex)
            {
                usernameOK = false;
                check_username.Text = ex.Message;
            }
        }
        void CheckPassword()
        {
            string password = input_password.Text;
            try
            {
                TextHelper.Check(password, TextHelper.Key.Password);
                passwordOK = true;
                check_password.Text = "";
            }
            catch (Exception ex)
            {
                passwordOK = false;
                check_password.Text = ex.Message;
            }
        }
        void CheckNtp()
        {
            double ntp = Convert.ToDouble(input_nrp.Text);
            try
            {
                if (ntp < 0)
                {
                    throw new Exception("普通返点不应小于0");
                }
                if (ntp > 13)
                {
                    throw new Exception("普通返点不应大于13");
                }
                ntpOK = true;
                check_nrp.Text = "";
            }
            catch (Exception ex)
            {
                ntpOK = false;
                check_nrp.Text = ex.Message;
            }
        }
        void ChekUtp()
        {
            double utp = Convert.ToDouble(input_urp.Text);
            try
            {
                if (utp < 0)
                {
                    throw new Exception("不定位返点不应小于0");
                }
                if (utp > 13)
                {
                    throw new Exception("不定位返点不应大于13");
                }
                utpOK = true;
                check_urp.Text = "";
            }
            catch (Exception ex)
            {
                utpOK = false;
                check_urp.Text = ex.Message;
            }
        }
        void CheckMos()
        {
            int mos = Convert.ToInt32(input_mos.Text);
            try
            {
                if (mos < 0)
                {
                    throw new Exception("直属下属不能小于0");
                }
                mosOK = true;
                check_mos.Text = "";
            }
            catch (Exception ex)
            {
                mosOK = false;
                check_mos.Text = ex.Message;
            }
        }

        #endregion

        private void BankToListPage(object sender, EventArgs e)
        {
            if (BankToListPageEventHandler != null)
            {
                BankToListPageEventHandler(this, new EventArgs());
            }
        }

        #region 焦点事件

        private void input_username_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckUsername();
        }

        private void input_password_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckPassword();
        }

        private void input_nrp_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckNtp();
        }

        private void input_urp_LostFocus(object sender, RoutedEventArgs e)
        {
            ChekUtp();
        }

        private void input_mos_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckMos();
        }

        #endregion
    }
}

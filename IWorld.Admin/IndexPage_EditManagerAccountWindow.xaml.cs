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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IWorld.Admin.Class;

namespace IWorld.Admin
{
    /// <summary>
    /// 修改管理员密码弹窗
    /// </summary>
    public partial class IndexPage_EditManagerAccountWindow : ChildWindow
    {
        private bool oldPasswordOK = false;
        private bool newPasswordOK = false;
        private bool newPasswordAgainOK = false;

        /// <summary>
        /// 原密码
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// 实例化一个新的修改管理员密码弹窗
        /// </summary>
        public IndexPage_EditManagerAccountWindow()
        {
            InitializeComponent();

            this.OldPassword = "";
            this.NewPassword = "";

            #region 失去焦点

            input_oldPassword.LostFocus += (sender, e) =>
                {
                    CheckOldPassword();
                };
            input_newPassword.LostFocus += (sender, e) =>
                {
                    CheckNewPassword();
                };
            input_newPasswordAgain.LostFocus += (sender, e) =>
                {
                    CheckNewPasswordAgain();
                };

            #endregion
            #region 回车键

            input_oldPassword.KeyDown += (sender, e) =>
                {
                    if (e.Key == Key.Enter)
                    {
                        input_newPassword.Focus();
                    }
                };
            input_newPassword.KeyDown += (sender, e) =>
                {
                    if (e.Key == Key.Enter)
                    {
                        input_newPasswordAgain.Focus();
                    }
                };
            input_newPasswordAgain.KeyDown += (sender, e) =>
                {
                    if (e.Key == Key.Enter)
                    {
                        OKButton_Click(null, null);
                    }
                };
            
            #endregion
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            CheckOldPassword();
            CheckNewPassword();
            CheckNewPasswordAgain();

            if (oldPasswordOK && newPasswordOK && newPasswordAgainOK)
            {
                OldPassword = input_oldPassword.Password;
                NewPassword = input_newPassword.Password;
                this.DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        #region 检查输入合法性

        private void CheckOldPassword()
        {
            try
            {
                TextHelper.Check(input_oldPassword.Password, TextHelper.Key.Password);

                check_oldPassword.Visibility = System.Windows.Visibility.Collapsed;
                check_oldPassword_img.Source = new BitmapImage(new Uri("img/tick_circle.png", UriKind.Relative));
                check_oldPassword_img.Visibility = System.Windows.Visibility.Visible;
                this.oldPasswordOK = true;
            }
            catch (Exception ex)
            {
                check_oldPassword.Text = ex.Message;
                check_oldPassword.Visibility = System.Windows.Visibility.Visible;
                check_oldPassword_img.Source = new BitmapImage(new Uri("img/cross_circle.png", UriKind.Relative));
                check_oldPassword_img.Visibility = System.Windows.Visibility.Visible;
                this.oldPasswordOK = false;
            }
        }

        private void CheckNewPassword()
        {
            try
            {
                TextHelper.Check(input_newPassword.Password, TextHelper.Key.Password);

                check_newPassword.Visibility = System.Windows.Visibility.Collapsed;
                check_newPassword_img.Source = new BitmapImage(new Uri("img/tick_circle.png", UriKind.Relative));
                check_newPassword_img.Visibility = System.Windows.Visibility.Visible;
                this.newPasswordOK = true;
            }
            catch (Exception ex)
            {
                check_newPassword.Text = ex.Message;
                check_newPassword.Visibility = System.Windows.Visibility.Visible;
                check_newPassword_img.Source = new BitmapImage(new Uri("img/cross_circle.png", UriKind.Relative));
                check_newPassword_img.Visibility = System.Windows.Visibility.Visible;
                this.newPasswordOK = false;
            }
        }

        private void CheckNewPasswordAgain()
        {
            if (input_newPassword.Password == input_newPasswordAgain.Password)
            {
                check_newPasswordAgain.Visibility = System.Windows.Visibility.Collapsed;
                check_newPasswordAgain_img.Source = new BitmapImage(new Uri("img/tick_circle.png", UriKind.Relative));
                check_newPasswordAgain_img.Visibility = System.Windows.Visibility.Visible;
                this.newPasswordAgainOK = true;
            }
            else
            {
                check_newPasswordAgain.Text = "两次输入的新密码不一致";
                check_newPasswordAgain.Visibility = System.Windows.Visibility.Visible;
                check_newPasswordAgain_img.Source = new BitmapImage(new Uri("img/cross_circle.png", UriKind.Relative));
                check_newPasswordAgain_img.Visibility = System.Windows.Visibility.Visible;
                this.newPasswordAgainOK = false;
            }
        }

        #endregion
    }
}


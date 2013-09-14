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

namespace IWorld.Admin.Controls
{
    public partial class SideTool : UserControl
    {
        MenuSelectedInfo selectedInfo = new MenuSelectedInfo();

        public SideTool()
        {
            InitializeComponent();
            root.DataContext = selectedInfo;
        }

        #region 依赖属性

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register("Username", typeof(string), typeof(SideTool)
            , new PropertyMetadata("", (d, e) =>
            {
                SideTool tool = (SideTool)d;
                tool.selectedInfo.Username = e.NewValue.ToString();
            }));

        /// <summary>
        /// 用户组
        /// </summary>
        public string Group
        {
            get { return (string)GetValue(GroupProperty); }
            set { SetValue(GroupProperty, value); }
        }

        public static readonly DependencyProperty GroupProperty =
            DependencyProperty.Register("Group", typeof(string), typeof(SideTool)
            , new PropertyMetadata("", (d, e) =>
            {
                SideTool tool = (SideTool)d;
                tool.selectedInfo.Group = e.NewValue.ToString();
            }));

        /// <summary>
        /// 导航选择文本
        /// </summary>
        public string SelectedText_Menu
        {
            get { return (string)GetValue(SelectedText_MenuProperty); }
            set { SetValue(SelectedText_MenuProperty, value); }
        }

        public static readonly DependencyProperty SelectedText_MenuProperty =
            DependencyProperty.Register("SelectedText_Menu", typeof(string), typeof(SideTool)
            , new PropertyMetadata("", (d, e) =>
            {
                SideTool tool = (SideTool)d;
                tool.selectedInfo.SelectedText_Menu = e.NewValue.ToString();
                tool.selectedInfo.OpenMenuText = e.NewValue.ToString();
            }));

        /// <summary>
        /// 页面选择文本
        /// </summary>
        public string SelectedText_Page
        {
            get { return (string)GetValue(SelectedText_PageProperty); }
            set { SetValue(SelectedText_PageProperty, value); }
        }

        public static readonly DependencyProperty SelectedText_PageProperty =
            DependencyProperty.Register("SelectedText_Page", typeof(string), typeof(SideTool)
            , new PropertyMetadata("", (d, e) =>
            {
                SideTool tool = (SideTool)d;
                tool.selectedInfo.SelectedText_Page = e.NewValue.ToString();
            }));

        /// <summary>
        /// 跳转命令
        /// </summary>
        public ICommand JumpCommand
        {
            get { return (ICommand)GetValue(JumpCommandProperty); }
            set { SetValue(JumpCommandProperty, value); }
        }

        public static readonly DependencyProperty JumpCommandProperty =
            DependencyProperty.Register("JumpCommand", typeof(ICommand), typeof(SideTool), new PropertyMetadata(null));

        /// <summary>
        /// 编辑账户命令
        /// </summary>
        public ICommand EditAccountCommand
        {
            get { return (ICommand)GetValue(EditAccountCommandProperty); }
            set { SetValue(EditAccountCommandProperty, value); }
        }

        public static readonly DependencyProperty EditAccountCommandProperty =
            DependencyProperty.Register("EditAccountCommand", typeof(ICommand), typeof(SideTool), new PropertyMetadata(null));

        /// <summary>
        /// 退出命令
        /// </summary>
        public ICommand LogoutCommand
        {
            get { return (ICommand)GetValue(LogoutCommandProperty); }
            set { SetValue(LogoutCommandProperty, value); }
        }

        public static readonly DependencyProperty LogoutCommandProperty =
            DependencyProperty.Register("LogoutCommand", typeof(ICommand), typeof(SideTool), new PropertyMetadata(null));
            
        #endregion

        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        private void OnJump(object sender, EventArgs e)
        {
            IJumpButton button = (IJumpButton)sender;
            if (JumpCommand == null)
            {
                return;
            }
            if (JumpCommand.CanExecute(button.Text))
            {
                JumpCommand.Execute(button.Text);
            }
        }

        /// <summary>
        /// 选择主导航
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        private void OnSelect(object sender, EventArgs e)
        {
            IMenuButton button = (IMenuButton)sender;
            selectedInfo.OpenMenuText = button.Text;

            if (!button.CanJump)
            {
                return;
            }
            if (JumpCommand == null)
            {
                return;
            }
            if (JumpCommand.CanExecute(button.Text))
            {
                JumpCommand.Execute(button.Text);
            }
        }

        /// <summary>
        /// 编辑账户
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        private void OnEditAccount(object sender, RoutedEventArgs e)
        {
            if (EditAccountCommand == null)
            {
                return;
            }
            if (EditAccountCommand.CanExecute(null))
            {
                EditAccountCommand.Execute(null);
            }
        }

        /// <summary>
        /// 安全退出
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        private void OnLogout(object sender, RoutedEventArgs e)
        {
            if (LogoutCommand == null)
            {
                return;
            }
            if (LogoutCommand.CanExecute(null))
            {
                LogoutCommand.Execute(null);
            }
        }
    }
}

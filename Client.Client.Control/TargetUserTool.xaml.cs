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

namespace Client.Client.Control
{
    public partial class TargetUserTool : UserControl
    {
        public TargetUserTool()
        {
            InitializeComponent();
        }

        #region 依赖属性

        #region 当前关注的用户的用户名

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(TargetUserTool)
            , new PropertyMetadata(false, (d, e) =>
            {
                TargetUserTool tool = (TargetUserTool)d;
                if ((bool)e.NewValue == true)
                {
                    tool.bg.Visibility = Visibility.Visible;
                    tool.LayoutRoot.Margin = new Thickness(0, 0, -1, 0);
                }
                else
                {
                    tool.bg.Visibility = Visibility.Collapsed;
                    tool.LayoutRoot.Margin = new Thickness(0);
                }
            }));

        #endregion

        #region 用户名

        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register("Username", typeof(string), typeof(TargetUserTool)
            , new PropertyMetadata("", (d, e) =>
            {
                TargetUserTool tool = (TargetUserTool)d;
                tool.text_username.Text = e.NewValue.ToString();
            }));

        #endregion

        #region 命令

        public ICommand RemoveTalkingUserCommand
        {
            get { return (ICommand)GetValue(RemoveTalkingUserCommandProperty); }
            set { SetValue(RemoveTalkingUserCommandProperty, value); }
        }

        public static readonly DependencyProperty RemoveTalkingUserCommandProperty =
            DependencyProperty.Register("RemoveTalkingUserCommand", typeof(ICommand), typeof(TargetUserTool)
            , new PropertyMetadata(null));



        public ICommand ChooseTalkingUserCommand
        {
            get { return (ICommand)GetValue(ChooseTalkingUserCommandProperty); }
            set { SetValue(ChooseTalkingUserCommandProperty, value); }
        }

        public static readonly DependencyProperty ChooseTalkingUserCommandProperty =
            DependencyProperty.Register("ChooseTalkingUserCommand", typeof(ICommand), typeof(TargetUserTool)
            , new PropertyMetadata(null));



        #endregion

        #endregion

        private void OnRemove(object sender, MouseButtonEventArgs e)
        {
            if (RemoveTalkingUserCommand == null) { return; }
            if (!RemoveTalkingUserCommand.CanExecute(Username)) { return; }
            RemoveTalkingUserCommand.Execute(Username);
        }

        private void OnChoose(object sender, MouseButtonEventArgs e)
        {
            if (ChooseTalkingUserCommand == null) { return; }
            if (!ChooseTalkingUserCommand.CanExecute(Username)) { return; }
            ChooseTalkingUserCommand.Execute(Username);
        }

    }
}

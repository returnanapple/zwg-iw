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

namespace Client.Client.Control
{
    public partial class FriendListButton : UserControl
    {
        public FriendListButton()
        {
            InitializeComponent();
        }

        #region 依赖属性

        #region 用户状态

        public UserShowStatus UserStatus
        {
            get { return (UserShowStatus)GetValue(UserStatusProperty); }
            set { SetValue(UserStatusProperty, value); }
        }

        public static readonly DependencyProperty UserStatusProperty =
            DependencyProperty.Register("UserStatus", typeof(UserShowStatus), typeof(FriendListButton)
            , new PropertyMetadata(UserShowStatus.客服, (d, e) =>
            {
                FriendListButton tool = (FriendListButton)d;
                string path = "img/images/talkIconUser.png";
                UserShowStatus newStatus = (UserShowStatus)e.NewValue;
                if (newStatus == UserShowStatus.在线)
                {
                    path = "img/images/talkIconUser_on.png";
                }
                else if (newStatus == UserShowStatus.离线)
                {
                    path = "img/images/talkIconUser_off.png";
                }
                BitmapImage bi = new BitmapImage(new Uri(path, UriKind.Relative));
                tool.img.Source = bi;
            }));

        #endregion

        #region 新信息条数

        public int CountOfNewMessage
        {
            get { return (int)GetValue(CountOfNewMessageProperty); }
            set { SetValue(CountOfNewMessageProperty, value); }
        }

        public static readonly DependencyProperty CountOfNewMessageProperty =
            DependencyProperty.Register("CountOfNewMessage", typeof(int), typeof(FriendListButton)
            , new PropertyMetadata(0, (d, e) =>
            {
                FriendListButton tool = (FriendListButton)d;
                Storyboard s = (Storyboard)tool.Resources["s"];
                if ((int)e.NewValue > 0)
                {
                    s.Stop();
                    s.Begin();
                }
                else
                {
                    s.Stop();
                }
            }));

        #endregion

        #region 触发命令和命令参数

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(FriendListButton), new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(FriendListButton), new PropertyMetadata(null));

        #endregion

        #endregion

        private void OnHorver(object sender, MouseEventArgs e)
        {
            _text.Foreground = new SolidColorBrush(Colors.Green);
            bg.Opacity = 1;
        }

        private void OnUnhover(object sender, MouseEventArgs e)
        {
            _text.Foreground = new SolidColorBrush(Colors.Blue);
            bg.Opacity = 0;
        }

        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            if (Command == null) { return; }
            if (!Command.CanExecute(CommandParameter)) { return; }
            Command.Execute(CommandParameter);
        }
    }
}

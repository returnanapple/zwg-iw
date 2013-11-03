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
    public partial class IconAndPicButton : UserControl
    {
        public IconAndPicButton()
        {
            InitializeComponent();
        }

        #region 鼠标事件

        private void OnHover(object sender, MouseEventArgs e)
        {
            //_bg.Visibility = System.Windows.Visibility.Visible;
            //_text.Foreground = new SolidColorBrush(Colors.Green);
            IsSelected = true;
        }

        private void OnUnhover(object sender, MouseEventArgs e)
        {
            //_bg.Visibility = System.Windows.Visibility.Collapsed;
            //_text.Foreground = (SolidColorBrush)Resources["normalColor"];
            IsSelected = false;
        }

        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            if (Command == null) { return; }
            if (!Command.CanExecute(BottonTypeIs)) { return; }
            Command.Execute(BottonTypeIs);
        }

        #endregion

        #region 依赖属性

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(IconAndPicButton), new PropertyMetadata(null));

        public BottonType BottonTypeIs
        {
            get { return (BottonType)GetValue(BottonTypeIsProperty); }
            set { SetValue(BottonTypeIsProperty, value); }
        }

        public static readonly DependencyProperty BottonTypeIsProperty =
            DependencyProperty.Register("BottonTypeIs", typeof(BottonType), typeof(IconAndPicButton)
            , new PropertyMetadata(BottonType.Icon, (d, e) =>
            {
                IconAndPicButton tool = (IconAndPicButton)d;
                BottonType _type = (BottonType)e.NewValue;
                if (_type == BottonType.Icon)
                {
                    tool._img.Source = new BitmapImage(new Uri("img/images/talkIco1.png", UriKind.Relative));
                    tool._text.Text = "表情";
                }
                else if (_type == BottonType.Pic)
                {
                    tool._img.Source = new BitmapImage(new Uri("img/images/talkIco2.png", UriKind.Relative));
                    tool._text.Text = "图片";
                }
                else if (_type == BottonType.Screenshot)
                {
                    tool._img.Source = new BitmapImage(new Uri("img/images/talkIco3.png", UriKind.Relative));
                    tool._text.Text = "截图";
                }
            }));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(IconAndPicButton)
            , new PropertyMetadata(false, new PropertyChangedCallback(IsSelectedChanged)));

        public static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IconAndPicButton key = (IconAndPicButton)d;
            if ((bool)e.NewValue)
                key.LayoutRoot.Style = (Style)key.Resources["MGrid"];
            else
                key.LayoutRoot.Style = null;
        }
        #endregion

        #region 内置枚举

        public enum BottonType
        {
            Pic,
            Icon,
            Screenshot
        }

        #endregion
    }
}

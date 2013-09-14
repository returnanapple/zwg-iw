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

namespace IWorld.Client
{
    public partial class HowToPlayButton : UserControl
    {
        public HowToPlayButton()
        {
            InitializeComponent();
        }

        public event NDelegate ClickEventHandler;

        private void Click(object sender, MouseButtonEventArgs e)
        {
            if (ClickEventHandler != null)
            {
                ClickEventHandler(this, new EventArgs());
            }
        }

        #region 依赖属性

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(HowToPlayButton)
            , new PropertyMetadata(false, new PropertyChangedCallback(IsSelectedChanged)));

        public static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HowToPlayButton button = (HowToPlayButton)d;
            button.root.Style = (bool)e.NewValue == true ? (Style)button.Resources["selected"] : null;
            button.b1.Visibility = (bool)e.NewValue == true ? Visibility.Visible : Visibility.Collapsed;
            button.b2.Visibility = (bool)e.NewValue == true ? Visibility.Visible : Visibility.Collapsed;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(HowToPlayButton)
            , new PropertyMetadata("", new PropertyChangedCallback(TextChanged)));

        public static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HowToPlayButton button = (HowToPlayButton)d;
            button.text_content.Text = e.NewValue.ToString();
        }

        #endregion
    }
}

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
    public partial class NumGroupButton : UserControl
    {
        public NumGroupButton()
        {
            InitializeComponent();
        }

        public event NDelegate ClickEventHandler;

        private void Click(object sender, MouseButtonEventArgs e)
        {
            this.IsSelected = !this.IsSelected;
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
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(NumGroupButton)
            , new PropertyMetadata(false, new PropertyChangedCallback(IsSelectedChanged)));

        public static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NumGroupButton)
            , new PropertyMetadata("", new PropertyChangedCallback(TextChanged)));

        public static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumGroupButton button = (NumGroupButton)d;
            button.text_content.Text = e.NewValue.ToString();
        }

        public string SeatName
        {
            get { return (string)GetValue(SeatNameProperty); }
            set { SetValue(SeatNameProperty, value); }
        }

        public static readonly DependencyProperty SeatNameProperty =
            DependencyProperty.Register("SeatName", typeof(string), typeof(NumGroupButton), new PropertyMetadata(""));

        #endregion
    }
}

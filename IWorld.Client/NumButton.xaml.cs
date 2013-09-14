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

namespace IWorld.Client
{
    public partial class NumButton : UserControl
    {
        public NumButton()
        {
            InitializeComponent();
        }

        #region 依赖属性

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(NumButton)
            , new PropertyMetadata(false, new PropertyChangedCallback(IsSelectedChanged)));

        public static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumButton button = (NumButton)d;
            button.bg.Style = (bool)e.NewValue == true ? (Style)button.Resources["selected"]
                : (Style)button.Resources["normal"];
            button.text_content.Foreground = (bool)e.NewValue == false
                ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
        }

        public int Num
        {
            get { return (int)GetValue(NumProperty); }
            set { SetValue(NumProperty, value); }
        }

        public static readonly DependencyProperty NumProperty =
            DependencyProperty.Register("Num", typeof(int), typeof(NumButton)
            , new PropertyMetadata(0, new PropertyChangedCallback(NumChanged)));

        public static void NumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumButton button = (NumButton)d;
            button.text_content.Text = e.NewValue.ToString();
        }

        public string SeatName
        {
            get { return (string)GetValue(SeatNameProperty); }
            set { SetValue(SeatNameProperty, value); }
        }

        public static readonly DependencyProperty SeatNameProperty =
            DependencyProperty.Register("SeatName", typeof(string), typeof(NumButton), new PropertyMetadata(""));

        #endregion

        private void Click(object sender, MouseButtonEventArgs e)
        {
            this.IsSelected = !this.IsSelected;
        }
    }
}

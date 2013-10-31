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
    public partial class ContactListControl : UserControl
    {
         public class People
        {
            public string SName { get; set; }
        }
        List<People> ListPeople = new List<People>();
        public ContactListControl()
        {
            InitializeComponent();
            People p1 = new People();
            p1.SName = "admin";
            People p2 = new People();
            p2.SName = "test";
            ListPeople.Add(p1);
            ListPeople.Add(p2);
            LisP.DataContext = ListPeople;
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(ContactListControl)
            , new PropertyMetadata(false, new PropertyChangedCallback(IsSelectedChanged)));

        public static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ContactListControl key = (ContactListControl)d;
            key.page1.Style =(Style)key.Resources["normal"];
            key.page2.Style = (Style)key.Resources["normal"];
            key.page3.Style = (Style)key.Resources["normal"];
        }
        private void page1_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            IsSelected = !IsSelected;
            page2.Style = null;
            page3.Style = null;
            txt1.Foreground = new SolidColorBrush(Colors.Black);
            txt2.Foreground = new SolidColorBrush(Colors.White);
            txt3.Foreground = new SolidColorBrush(Colors.White);
        }

        private void page2_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            IsSelected = !IsSelected;
            page1.Style = null;
            page3.Style = null;

            txt1.Foreground = new SolidColorBrush(Colors.White);
            txt2.Foreground = new SolidColorBrush(Colors.Black);
            txt3.Foreground = new SolidColorBrush(Colors.White);
        }

        private void page3_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            IsSelected = !IsSelected;
            page2.Style = null;
            page1.Style = null;

            txt1.Foreground = new SolidColorBrush(Colors.White);
            txt2.Foreground = new SolidColorBrush(Colors.White);
            txt3.Foreground = new SolidColorBrush(Colors.Black);
        }
    }
}

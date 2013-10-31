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
    public partial class ContactControl : UserControl
    {
        public class People
        {
            public string SName { get; set; }
        }
        List<People> ListPeople = new List<People>();

        public ContactControl()
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
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(ContactControl)
            , new PropertyMetadata(false, new PropertyChangedCallback(IsSelectedChanged)));

        public static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ContactControl key = (ContactControl)d;
            key.grid1.Style = (Style)key.Resources["MGrid"];
            key.grid2.Style = (Style)key.Resources["MGrid"];
            key.grid3.Style = (Style)key.Resources["MGrid"];
        }
        private void Grid_MouseEnter_1(object sender, MouseEventArgs e)
        {
            IsSelected = !IsSelected;
            grid2.Style = null;
            grid3.Style = null;
        }

        private void Grid_MouseLeave_1(object sender, MouseEventArgs e)
        {
            grid1.Style = null;
        }

        private void grid2_MouseEnter_1(object sender, MouseEventArgs e)
        {
            IsSelected = !IsSelected;
            grid1.Style = null;
            grid3.Style = null;
        }

        private void grid2_MouseLeave_1(object sender, MouseEventArgs e)
        {
            grid2.Style = null;
        }

        private void grid3_MouseEnter_1(object sender, MouseEventArgs e)
        {
            IsSelected = !IsSelected;
            grid2.Style = null;
            grid1.Style = null;
        }

        private void grid3_MouseLeave_1(object sender, MouseEventArgs e)
        {
            grid3.Style = null;
        }
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IWorld.Client.Class;

namespace IWorld.Client
{
    public partial class FunctionKey : UserControl
    {
        public FunctionKey()
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

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(FunctionKey)
            , new PropertyMetadata("", (d, e) =>
                {
                    FunctionKey t = (FunctionKey)d;
                    t.keyText.Text = e.NewValue.ToString();
                }));
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(FunctionKey)
            , new PropertyMetadata(false, (d, e) =>
                {
                    FunctionKey t = (FunctionKey)d;
                    if ((bool)e.NewValue == true)
                    {
                        t.bg.Opacity = 1;
                    }
                    else
                    {
                        t.bg.Opacity = 0;
                    }
                }));
        public string ImgSource
        {
            get { return (string)GetValue(ImgSourceProperty); }
            set { SetValue(ImgSourceProperty, value); }
        }

        public static readonly DependencyProperty ImgSourceProperty =
            DependencyProperty.Register("ImgSource", typeof(string), typeof(FunctionKey)
            , new PropertyMetadata("", (d, e) =>
                {
                    FunctionKey t = (FunctionKey)d;
                    t.img.Source = new BitmapImage(new Uri(e.NewValue.ToString(), UriKind.Relative));
                }));
    }
}

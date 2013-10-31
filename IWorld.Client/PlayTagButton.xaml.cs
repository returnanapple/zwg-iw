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
    public partial class PlayTagButton : UserControl
    {
        public PlayTagButton()
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
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(PlayTagButton)
            , new PropertyMetadata(false, new PropertyChangedCallback(IsSelectedChanged)));

        public static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //PlayTagButton ptTutton = (PlayTagButton)d;
            //ptTutton.bg.Style = (bool)e.NewValue == true ? (Style)ptTutton.Resources["selected"] 
            //    : (Style)ptTutton.Resources["normal"];
            //ptTutton.text_content.Foreground = (bool)e.NewValue == false
            //    ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
            PlayTagButton ptTutton = (PlayTagButton)d;
            ptTutton.bg.Style = (bool)e.NewValue == true ? (Style)ptTutton.Resources["selected"]
                : (Style)ptTutton.Resources["normal"];
            //ptTutton.text_content.Foreground = (bool)e.NewValue == false
            //    ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Yellow);
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(PlayTagButton)
            , new PropertyMetadata("", new PropertyChangedCallback(TextChanged)));

        public static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlayTagButton ptTutton = (PlayTagButton)d;
            ptTutton.text_content.Text = e.NewValue.ToString();
        }

        #endregion
    }
}

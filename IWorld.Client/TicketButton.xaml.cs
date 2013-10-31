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
    public partial class TicketButton : UserControl
    {
        public TicketButton()
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

        #region 文字

        public string Text
        {
            get { return (string)GetValue(FTextProperty); }
            set { SetValue(FTextProperty, value); }
        }

        public static readonly DependencyProperty FTextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TicketButton)
            , new PropertyMetadata("Button", new PropertyChangedCallback(FTextChanged)));

        public static void FTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TicketButton fKey = (TicketButton)d;
            fKey.text_content.Text = e.NewValue.ToString();
        }

        #endregion

        #region 选中

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(TicketButton)
            , new PropertyMetadata(false, new PropertyChangedCallback(IsSelectedChanged)));

        public static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TicketButton key = (TicketButton)d;
            key.bg.Style = (bool)e.NewValue == false ? null : (Style)key.Resources["normal"];
        }

        #endregion

        #endregion
    }
}

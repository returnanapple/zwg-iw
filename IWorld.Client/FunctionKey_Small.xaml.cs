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
    public partial class FunctionKey_Small : UserControl
    {
        public FunctionKey_Small()
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

        #region 正文

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(FunctionKey_Small)
            , new PropertyMetadata("", (d, e) =>
                {
                    FunctionKey_Small fk = (FunctionKey_Small)d;
                    fk.tb.Text = e.NewValue.ToString();
                }));

        #endregion

        #region 图片链接

        public string ImgSource
        {
            get { return (string)GetValue(ImgSourceProperty); }
            set { SetValue(ImgSourceProperty, value); }
        }

        public static readonly DependencyProperty ImgSourceProperty =
            DependencyProperty.Register("ImgSource", typeof(string), typeof(FunctionKey_Small)
            , new PropertyMetadata("", (d, e) =>
                {
                    FunctionKey_Small fk = (FunctionKey_Small)d;
                    fk.img.Source = new BitmapImage(new Uri(e.NewValue.ToString(), UriKind.Relative));
                }));

        #endregion
    }
}

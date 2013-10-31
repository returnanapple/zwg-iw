

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
    public partial class Ball : UserControl
    {
        public Dictionary<BallColor, string> Contrast { get; set; }

        public Ball()
        {
            InitializeComponent();
            Contrast = new Dictionary<BallColor, string>();
            //Contrast.Add(BallColor.Red, "img/红球.png");
            //Contrast.Add(BallColor.Yellow, "img/黄球.png");
            //Contrast.Add(BallColor.Green, "img/绿球.png");
            //Contrast.Add(BallColor.Blue, "img/蓝球.png");
            //Contrast.Add(BallColor.Purple, "img/紫球.png");
            Contrast.Add(BallColor.Red, "img/images/ball.png");
            Contrast.Add(BallColor.Yellow, "img/images/ball.png");
            Contrast.Add(BallColor.Green, "img/images/ball.png");
            Contrast.Add(BallColor.Blue, "img/images/ball.png");
            Contrast.Add(BallColor.Purple, "img/images/ball.png");
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Ball)
            , new PropertyMetadata("", (d, e) =>
            {
                Ball t = (Ball)d;
                t.keyText.Text = e.NewValue.ToString();
            }));

        public BallColor Color
        {
            get { return (BallColor)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(BallColor), typeof(Ball)
            , new PropertyMetadata(BallColor.Red, (d, e) =>
            {
                Ball t = (Ball)d;
                BallColor color = (BallColor)e.NewValue;
                t.bg.ImageSource = new BitmapImage(new Uri(t.Contrast[color], UriKind.Relative));
            }));
    }
}

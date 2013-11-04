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
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace IWorld.Shark.Control
{
    public partial class Icon : UserControl
    {
        public Icon()
        {
            InitializeComponent();
        }

        #region IsAt
        public bool IsAt
        {
            get { return (bool)GetValue(IsAtProperty); }
            set { SetValue(IsAtProperty, value); }
        }
        public static readonly DependencyProperty IsAtProperty =
            DependencyProperty.Register("IsAt", typeof(bool), typeof(Icon), new PropertyMetadata(false, (d, e) =>
            {
                Icon tempd = (Icon)d;
                bool tempe = (bool)e.NewValue;
                DropShadowEffect dse = (DropShadowEffect)tempd.Resources["IsAtEffect"];
                if (tempe)
                {
                    tempd.Image.Effect = dse;
                }
                else
                {
                    tempd.Image.Effect = null;
                }
            }));
        #endregion

        #region ImagePath
        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }
        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath", typeof(string), typeof(Icon), new PropertyMetadata(""));
        #endregion

    }
}

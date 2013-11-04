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

namespace IWorld.Shark.Control
{
    public partial class NoBorderTextBox : TextBox
    {
        public NoBorderTextBox()
        {
            InitializeComponent();
        }
        public override void OnApplyTemplate()
        {
            foreach (string s in "FocusVisualElement,MouseOverBorder,DisabledVisualElement".Split(','))
            {
                var bdr = GetTemplateChild(s) as Border;
                if (bdr != null)
                {
                    bdr.BorderThickness = new Thickness(0);
                    bdr.Background = new SolidColorBrush(Colors.Transparent);
                }
            }
            base.OnApplyTemplate();
        }
    }
}

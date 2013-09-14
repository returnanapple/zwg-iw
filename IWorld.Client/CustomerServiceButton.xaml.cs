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
    public partial class CustomerServiceButton : UserControl
    {
        public CustomerServiceButton()
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
    }
}

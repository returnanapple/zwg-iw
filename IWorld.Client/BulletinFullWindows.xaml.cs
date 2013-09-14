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
using IWorld.Client.BulletinService;

namespace IWorld.Client
{
    public partial class BulletinFullWindows : ChildWindow
    {
        public BulletinFullWindows(BulletinResult bulletin)
        {
            InitializeComponent();
            text_title.Text = bulletin.Title;
            text_context.Text = bulletin.Context;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


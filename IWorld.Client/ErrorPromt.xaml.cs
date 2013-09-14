﻿using System;
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
    public partial class ErrorPromt : ChildWindow
    {
        public ErrorPromt(string message)
        {
            InitializeComponent();
            text_content.Text = message;
        }

        private void Enter(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

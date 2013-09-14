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

namespace IWorld.Admin
{
    public partial class ErrorPrompt : ChildWindow
    {
        public ErrorPrompt(string error)
        {
            InitializeComponent();
            text_content.Text = error;
        }

        private void SubmitButton_ClickEventHandler(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }
    }
}


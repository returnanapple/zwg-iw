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
    public partial class ExamineUsersPage_DeleteUserPrompt : ChildWindow
    {
        public ExamineUsersPage_DeleteUserPrompt(string username)
        {
            InitializeComponent();
            text_username.Text = username;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


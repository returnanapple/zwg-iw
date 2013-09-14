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

namespace IWorld.Admin.Controls
{
    public partial class ContentBox : ContentControl
    {
        public ContentBox()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 当前选择的
        /// </summary>
        public string TagText
        {
            get { return (string)GetValue(TagTextProperty); }
            set { SetValue(TagTextProperty, value); }
        }

        public static readonly DependencyProperty TagTextProperty =
            DependencyProperty.Register("TagText", typeof(string), typeof(ContentBox), new PropertyMetadata("列表"));

        private void ContentBox_TagButton_Click(object sender, EventArgs e)
        {
            NormalPrompt np = new NormalPrompt();
            np.Show();
        }
    }
}

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

namespace Client.Client.Control
{
    public partial class TalkClient : UserControl
    {
        public TalkClient()
        {
            InitializeComponent();
            TalkClientViewModel vm = new TalkClientViewModel();
            this.DataContext = vm;
            Myself = this;
        }
        public static TalkClient Myself { get; set; }
    }
}

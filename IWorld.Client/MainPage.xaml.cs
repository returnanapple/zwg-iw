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

namespace IWorld.Client
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            if (!App.Current.IsRunningOutOfBrowser)
            {
                root.Children.Clear();
                root.Children.Add(new DownloadPage());
            }
        }

        /// <summary>
        /// 窗体拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragMove(object sender, EventArgs e)
        {
            if (App.Current.IsRunningOutOfBrowser)
            {
                App.Current.MainWindow.DragMove();
            }
        }

        private void SetFirstValues(object sender, RoutedEventArgs e)
        {
            if (App.Current.IsRunningOutOfBrowser)
            {
                App.LeftOfInitia = App.Current.MainWindow.Left;
                App.TopOfInitial = App.Current.MainWindow.Top;
                App.HadSetSize = true;
            }
        }
    }
}

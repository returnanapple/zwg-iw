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
    public partial class DownloadPage : UserControl
    {
        public DownloadPage()
        {
            InitializeComponent();
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            fe.Margin = new Thickness(2, 2, -2, -2);
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            fe.Margin = new Thickness(0, 0, 0, 0);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            fe.Margin = new Thickness(0, 0, 0, 0);

            if (App.Current.InstallState == InstallState.Installed)
            {
                ErrorPromt ep = new ErrorPromt("您已经下载并安装该应用，请使用桌面快捷方式打开");
                ep.Show();
            }
            else
            {
                App.Current.Install();
            }
        }
    }
}

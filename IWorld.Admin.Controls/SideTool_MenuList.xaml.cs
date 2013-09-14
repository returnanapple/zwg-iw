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
    /// <summary>
    /// 侧边栏小型导航面板
    /// </summary>
    public partial class SideTool_MenuList : Grid
    {
        /// <summary>
        /// 实例化一个新的侧边栏小型导航面板
        /// </summary>
        public SideTool_MenuList()
        {
            InitializeComponent();
        }

        #region 依赖属性

        /// <summary>
        /// 面板所要容纳的按键个数
        /// </summary>
        public int CountOfButton
        {
            get { return (int)GetValue(CountOfButtonProperty); }
            set { SetValue(CountOfButtonProperty, value); }
        }

        public static readonly DependencyProperty CountOfButtonProperty =
            DependencyProperty.Register("CountOfButton", typeof(int), typeof(SideTool_MenuList)
            , new PropertyMetadata(0, (d, e) =>
            {
                SideTool_MenuList tool = (SideTool_MenuList)d;
                int _count = (int)e.NewValue;
                double height = 30.0 * _count;
                tool.root.Height = height;
                tool.da_open.To = height;
            }));

        /// <summary>
        /// 关联文本
        /// </summary>
        public string RelevanceText
        {
            get { return (string)GetValue(RelevanceTextProperty); }
            set { SetValue(RelevanceTextProperty, value); }
        }

        public static readonly DependencyProperty RelevanceTextProperty =
            DependencyProperty.Register("RelevanceText", typeof(string), typeof(SideTool_MenuList), new PropertyMetadata(""));

        /// <summary>
        /// 被选中的按键的显示文本
        /// </summary>
        public string SelectedText
        {
            get { return (string)GetValue(SelectedTextProperty); }
            set { SetValue(SelectedTextProperty, value); }
        }

        public static readonly DependencyProperty SelectedTextProperty =
            DependencyProperty.Register("SelectedText", typeof(string), typeof(SideTool_MenuList)
            , new PropertyMetadata("", (d, e) =>
            {
                SideTool_MenuList tool = (SideTool_MenuList)d;
                if (tool.RelevanceText == e.NewValue.ToString())
                {
                    OpenNow = e.NewValue.ToString();
                    tool.root.Visibility = Visibility.Visible;
                    Storyboard s = (Storyboard)tool.Resources["s_open"];
                    s.Begin();
                }
                else
                {
                    Storyboard s = (Storyboard)tool.Resources["s_close"];
                    s.Begin();
                }
            }));

        #endregion

        private void Hide(object sender, EventArgs e)
        {
            root.Visibility = System.Windows.Visibility.Collapsed;
        }

        static string OpenNow = "";

        private void CloseTheOpen(object sender, RoutedEventArgs e)
        {
            if (RelevanceText == OpenNow
                && RelevanceText != SelectedText)
            {
                root.Visibility = System.Windows.Visibility.Visible;
                Storyboard s = (Storyboard)Resources["s_close"];
                s.Begin();
            }
        }
    }
}

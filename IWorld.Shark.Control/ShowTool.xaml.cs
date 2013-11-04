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

namespace IWorld.Shark.Control
{
    public partial class ShowTool : UserControl
    {
        public ShowTool()
        {
            InitializeComponent();
        }

        #region 依赖属性
        /// <summary>
        /// 标题
        /// </summary>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(ShowTool), new PropertyMetadata(""));
        /// <summary>
        /// 得分and资产
        /// </summary>
        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }
        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register("Count", typeof(int), typeof(ShowTool), new PropertyMetadata(0));


        #endregion
    }
}

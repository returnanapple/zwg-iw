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
    public partial class MyButton : UserControl
    {
        public MyButton()
        {
            InitializeComponent();
        }

        #region 依赖属性
        /// <summary>
        /// 是否已经下注
        /// </summary>
        public bool Beted
        {
            get { return (bool)GetValue(BetedProperty); }
            set { SetValue(BetedProperty, value); }
        }
        public static readonly DependencyProperty BetedProperty =
            DependencyProperty.Register("Beted", typeof(bool), typeof(MyButton), new PropertyMetadata(false, (d, e) =>
            {
                MyButton tempd = (MyButton)d;
                bool tempe = (bool)e.NewValue;
                if (tempe)
                {
                    tempd.TextBlockX.Text = "取消";
                }
                else
                {
                    tempd.TextBlockX.Text = "下注";
                }
            }));

        /// <summary>
        /// 命令
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(MyButton), new PropertyMetadata(null));
        #endregion

        #region 鼠标事件
        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseEnterFun(object sender, MouseEventArgs e)
        {
            BorderX.Background = this.Resources["On"] as ImageBrush;
        }
        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeaveFun(object sender, MouseEventArgs e)
        {
            BorderX.Background = this.Resources["Default"] as ImageBrush;
        }
        /// <summary>
        /// 左键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeftButtonDownFun(object sender, MouseButtonEventArgs e)
        {
            BorderX.Background = this.Resources["Down"] as ImageBrush;
            Command.Execute(null);
        }
        /// <summary>
        /// 左键弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeftButtonUpFun(object sender, MouseButtonEventArgs e)
        {
            BorderX.Background = this.Resources["On"] as ImageBrush;
        }
        #endregion
    }
}

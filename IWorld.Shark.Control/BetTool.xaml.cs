using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace IWorld.Shark.Control
{
    public partial class BetTool : UserControl
    {
        public BetTool()
        {
            InitializeComponent();
        }
        #region 私有字段
        bool focus = false;
        #endregion

        #region 依赖属性
        /// <summary>
        /// 标签
        /// </summary>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(BetTool), new PropertyMetadata(""));
        /// <summary>
        /// 赔率
        /// </summary>
        public int Odds
        {
            get { return (int)GetValue(OddsProperty); }
            set { SetValue(OddsProperty, value); }
        }
        public static readonly DependencyProperty OddsProperty =
            DependencyProperty.Register("Odds", typeof(int), typeof(BetTool), new PropertyMetadata(0));
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }
        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath", typeof(string), typeof(BetTool), new PropertyMetadata(""));
        /// <summary>
        /// 下注数量(需要双向绑定)
        /// </summary>
        public int BetSum
        {
            get { return (int)GetValue(BetSumProperty); }
            set { SetValue(BetSumProperty, value); }
        }
        public static readonly DependencyProperty BetSumProperty =
            DependencyProperty.Register("BetSum", typeof(int), typeof(BetTool), new PropertyMetadata(0, (d, e) =>
            {
                BetTool tempd = (BetTool)d;
                int tempe = (int)e.NewValue;
                 if (tempe == 0)
                {
                    tempd.TextBoxX.Text = "";
                }
                else
                {
                    tempd.TextBoxX.Text = tempe.ToString();
                }
            }));
        #endregion

        #region 限制文本框只能输入数字处理程序
        /// <summary>
        /// 键盘按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void KeyDownFun(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// 文本改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextChangedFun(object sender, TextChangedEventArgs e)
        {
            if (TextBoxX.Text == "" || TextBoxX.Text == "0")
            {
                this.BetSum = 0;
            }
            else
            {
                this.BetSum = Convert.ToInt32(TextBoxX.Text);
            }
        }
        #endregion

        #region 焦点事件
        /// <summary>
        /// 获得焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GotFocusFun(object sender, RoutedEventArgs e)
        {
            DropShadowEffect dse = (DropShadowEffect)this.Resources["selectEffect"];
            this.bd.Effect = dse;
            this.focus = true;
        }
        /// <summary>
        /// 失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LostFocusFun(object sender, RoutedEventArgs e)
        {
            this.bd.Effect = null;
            this.focus = false;
        }
        #endregion

        #region 鼠标事件
        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseEnterFun(object sender, MouseEventArgs e)
        {
            if (!this.focus)
            {
                DropShadowEffect dse = (DropShadowEffect)this.Resources["selectEffect"];
                this.bd.Effect = dse;
            }
        }
        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeaveFun(object sender, MouseEventArgs e)
        {
            if (!this.focus)
            {
                this.bd.Effect = null;
            }
        }
        /// <summary>
        /// 左键点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeftButtonDownFun(object sender, MouseButtonEventArgs e)
        {
            if (TextBoxX.IsEnabled == true)
            {
                TextBoxX.Focus();
            }
            else
            {
                TipChildWindow x = new TipChildWindow();
                x.OKButton.Click += (_d, _e) => { this.bd.Effect = null; };
                x.Show();
            }
        }
        #endregion




    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace IWorld.Client.Class
{
    /// <summary>
    /// 通用的按键的鼠标事件反馈的触发器
    /// </summary>
    public class NButtonFeedbackTrigger : TriggerBase<FrameworkElement>
    {
        #region 动作

        /// <summary>
        /// 原始的按键状态
        /// </summary>
        private Cursor baseCursor;

        /// <summary>
        /// 偏移量
        /// </summary>
        private double _offset = 1;

        /// <summary>
        /// 按键偏移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Offset(object sender, MouseEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            fe.Margin = new Thickness(fe.Margin.Left + _offset
                , fe.Margin.Top + _offset
                , fe.Margin.Right - _offset
                , fe.Margin.Bottom - _offset);
        }

        /// <summary>
        /// 按键回复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Reply(object sender, MouseEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            fe.Margin = new Thickness(fe.Margin.Left - _offset
                , fe.Margin.Top - _offset
                , fe.Margin.Right + _offset
                , fe.Margin.Bottom + _offset);
        }

        /// <summary>
        /// 按键回复（点击的时候）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ReplyInClick(object sender, MouseButtonEventArgs e)
        {
            if (this.ReplyWhenClick)
            {
                Reply(sender, e);
            }
        }

        #endregion

        #region 依赖属性

        public double NBTOffset
        {
            get { return (double)GetValue(NBTOffsetProperty); }
            set { SetValue(NBTOffsetProperty, value); }
        }

        public static readonly DependencyProperty NBTOffsetProperty =
            DependencyProperty.Register("NBTOffset", typeof(double), typeof(NButtonFeedbackTrigger)
            , new PropertyMetadata(1.0, NBTOffsetChanged));

        public static void NBTOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NButtonFeedbackTrigger nbTrigger = (NButtonFeedbackTrigger)d;
            nbTrigger._offset = (double)e.NewValue;
        }

        public bool ReplyWhenClick
        {
            get { return (bool)GetValue(ReplyWhenClickProperty); }
            set { SetValue(ReplyWhenClickProperty, value); }
        }

        public static readonly DependencyProperty ReplyWhenClickProperty =
            DependencyProperty.Register("ReplyWhenClick", typeof(bool), typeof(NButtonFeedbackTrigger), new PropertyMetadata(false));

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();
            this.baseCursor = this.AssociatedObject.Cursor;
            this.AssociatedObject.Cursor = Cursors.Hand;
            this.AssociatedObject.MouseEnter += Offset;
            this.AssociatedObject.MouseLeave += Reply;
            this.AssociatedObject.MouseLeftButtonDown += ReplyInClick;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Cursor = baseCursor;
            this.AssociatedObject.MouseEnter -= Offset;
            this.AssociatedObject.MouseLeave -= Reply;
            this.AssociatedObject.MouseLeftButtonDown -= ReplyInClick;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace IWorld.Admin.Class
{
    public class SiteMapToolButtonFeedbackTrigger : TriggerBase<TextBlock>
    {
        #region 动作

        /// <summary>
        /// 原始的按键状态
        /// </summary>
        private Cursor baseCursor;

        /// <summary>
        /// 原始的前景色
        /// </summary>
        private Brush baseForeground;

        /// <summary>
        /// 按键偏移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Offset(object sender, MouseEventArgs e)
        {
            this.AssociatedObject.Foreground = new SolidColorBrush(Color.FromArgb(100, 65, 130, 159));
            this.AssociatedObject.TextDecorations = System.Windows.TextDecorations.Underline;
        }

        /// <summary>
        /// 按键回复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Reply(object sender, MouseEventArgs e)
        {
            this.AssociatedObject.Foreground = baseForeground;
            this.AssociatedObject.TextDecorations = null;
        }

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();
            this.baseCursor = this.AssociatedObject.Cursor;
            this.baseForeground = this.AssociatedObject.Foreground;
            this.AssociatedObject.Cursor = Cursors.Hand;
            this.AssociatedObject.MouseEnter += Offset;
            this.AssociatedObject.MouseLeave += Reply;
            this.AssociatedObject.MouseLeftButtonDown += Reply;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Cursor = baseCursor;
            this.AssociatedObject.MouseEnter -= Offset;
            this.AssociatedObject.MouseLeave -= Reply;
            this.AssociatedObject.MouseLeftButtonDown -= Reply;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace IWorld.Admin.Class
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
        /// 按键偏移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Offset(object sender, MouseEventArgs e)
        {
            UIHelper.Offset(this.AssociatedObject, 1);
        }

        /// <summary>
        /// 按键回复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Reply(object sender, MouseEventArgs e)
        {
            UIHelper.Offset(this.AssociatedObject, -1);
        }

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();
            this.baseCursor = this.AssociatedObject.Cursor;
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

using System;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace IWorld.Admin.Controls
{
    /// <summary>
    /// 用于保证信息实时性的触发器（仅限于 PasswordBox 控件）
    /// </summary>
    public class KeepPasswordRealTime : TriggerBase<PasswordBox>
    {
        #region 保护方法

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PasswordChanged += Update;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PasswordChanged -= Update;
        }

        #endregion

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        void Update(object sender, EventArgs e)
        {
            BindingExpression expression = ((PasswordBox)sender).GetBindingExpression(PasswordBox.PasswordProperty);
            expression.UpdateSource();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace IWorld.Admin.Controls
{
    /// <summary>
    /// 清空信息在获得焦点的时候（仅限于 PasswordBox 控件）
    /// </summary>
    public class ClearPasswordOnFocus : TriggerBase<PasswordBox>
    {
        #region 保护方法

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.GotFocus += Clear;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.GotFocus -= Clear;
        }

        #endregion

        /// <summary>
        /// 清空信息
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        void Clear(object sender, EventArgs e)
        {
            ((PasswordBox)sender).Password = "";
        }
    }
}

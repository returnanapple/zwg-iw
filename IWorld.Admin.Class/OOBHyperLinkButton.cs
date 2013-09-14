using System;
using System.Windows.Controls;

namespace IWorld.Admin.Class
{
    /// <summary>
    /// 虚拟化链接按键
    /// </summary>
    public class OOBHyperLinkButton : HyperlinkButton
    {
        //私有化构造方法 以禁止实例
        void DoClick()
        {
            base.OnClick();
        }

        /// <summary>
        /// 打开新的网页
        /// </summary>
        /// <param name="uri">目标链接</param>
        /// <param name="targetName">目标对象</param>
        public static void OpenWebPage(Uri uri, string targetName = "_blank")
        {
            OOBHyperLinkButton btn = new OOBHyperLinkButton();
            btn.NavigateUri = uri;
            btn.TargetName = targetName;
            btn.DoClick();
        }
    }
}

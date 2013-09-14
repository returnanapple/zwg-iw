using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IWorld.Admin.Class
{
    /// <summary>
    /// UI对象相关的帮助者对象
    /// </summary>
    public class UIHelper
    {
        #region 像素偏移

        /// <summary>
        /// 通用用户组件位移
        /// </summary>
        /// <param name="sender">目标组件</param>
        /// <param name="value">偏移量</param>
        public static void Offset(object sender, double value = 2)
        {
            Offset(sender, value, value);
        }

        /// <summary>
        /// 通用用户组件位移
        /// </summary>
        /// <param name="sender">目标组件</param>
        /// <param name="valueOfLeft">水平偏移量</param>
        /// <param name="valueOfTop">竖直偏移量</param>
        public static void Offset(object sender, double valueOfLeft, double valueOfTop)
        {
            if (sender is FrameworkElement)
            {
                FrameworkElement fe = (FrameworkElement)sender;
                fe.Margin = new Thickness(fe.Margin.Left + valueOfLeft
                    , fe.Margin.Top + valueOfTop
                    , fe.Margin.Right - valueOfLeft
                    , fe.Margin.Bottom - valueOfTop);
            }
            else
            {
                throw new Exception("该成员并非通用用户组件");
            }
        }

        #endregion

        #region 默认系统动作

        /// <summary>
        /// 通用的按键被触发的动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void NButtonTrigger(object sender, MouseEventArgs e)
        {
            UIHelper.Offset(sender, 1);
        }

        /// <summary>
        /// 通用的按键回复正常状况的动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void NButtonReply(object sender, MouseEventArgs e)
        {
            UIHelper.Offset(sender, -1);
        }

        /// <summary>
        /// 按键主体偏移
        /// （用于按键部分触发时候的动态反馈）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void MoveButtonBody(object sender, MouseEventArgs e)
        {
            var body = ((FrameworkElement)sender).Parent;
            UIHelper.Offset(body, 1);
        }

        /// <summary>
        /// 按键主体回复
        /// （用于按键部分触发时候的动态反馈）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ReplyButonBody(object sender, MouseEventArgs e)
        {
            var body = ((FrameworkElement)sender).Parent;
            UIHelper.Offset(body, -1);
        }

        /// <summary>
        /// 竖向滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void VerticalScroll(object sender, MouseWheelEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            ScrollViewer viewer = fe.Parent as ScrollViewer;
            if (viewer == null)
                return;
            double num = Math.Abs((int)(e.Delta / 2));
            double offset = 0.0;
            if (e.Delta > 0)
            {
                offset = Math.Max((double)0.0, (double)(viewer.VerticalOffset - num));
            }
            else
            {
                offset = Math.Min(viewer.ScrollableHeight, viewer.VerticalOffset + num);
            }
            if (offset != viewer.VerticalOffset)
            {
                viewer.ScrollToVerticalOffset(offset);
                e.Handled = true;
            }
        }

        /// <summary>
        /// 打开前台首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OpenReceptionIndex(object sender, MouseButtonEventArgs e)
        {
            OOBHyperLinkButton.OpenWebPage(new Uri("/Index.html", UriKind.Relative));
        }

        #endregion
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using IWorld.Admin.Class;

namespace IWorld.Admin
{
    /// <summary>
    /// 主界面
    /// </summary>
    public partial class MainPage : UserControl
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的主界面
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            root.MouseWheel += VerticalScroll;
            Storyboard s = (Storyboard)this.Resources["s_loading"];
            s.Begin();
            GoToLoginPage();
        }

        #endregion

        #region 滚屏

        /// <summary>
        /// 竖向滚屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void VerticalScroll(object sender, MouseWheelEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            UIHelper.VerticalScroll(fe, e);
        }

        #endregion

        #region 屏蔽层

        /// <summary>
        /// 显示屏蔽层
        /// </summary>
        public void ShowCover()
        {
            coverFloor.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// 隐藏屏蔽层
        /// </summary>
        public void HideCover()
        {
            coverFloor.Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion

        #region 跳转

        /// <summary>
        /// 跳转到操作页
        /// </summary>
        public void GoToSetUpPage()
        {
            this.HideCover();
            body.Children.Clear();
            body.Children.Add(new SetUpPage());
        }

        /// <summary>
        /// 跳转到登陆页
        /// </summary>
        public void GoToLoginPage()
        {
            this.HideCover();
            body.Children.Clear();
            body.Children.Add(new LoginPage());
        }

        #endregion

        #region 子窗口

        /// <summary>
        /// 关键字 - 子窗口 对照
        /// </summary>
        private Dictionary<string, FrameworkElement> childWindows = new Dictionary<string, FrameworkElement>();

        /// <summary>
        /// 插入新的子窗口
        /// </summary>
        /// <param name="_childWindow">子窗口</param>
        /// <returns>返回对照关键字</returns>
        public string InsertChildWindow(FrameworkElement _childWindow)
        {
            string key = Guid.NewGuid().ToString("N");
            childWindows.Add(key, _childWindow);
            childWindow.Children.Add(_childWindow);

            return key;
        }

        /// <summary>
        /// 移除子窗口
        /// </summary>
        /// <param name="key">关键字</param>
        public void RemoveChildWindow(string key)
        {
            if (childWindows.Keys.Any(x => x == key))
            {
                childWindow.Children.Remove(childWindows[key]);
                childWindows.Remove(key);
            }
        }

        /// <summary>
        /// 移除子窗口
        /// </summary>
        /// <param name="keys">关键字（集合）</param>
        public void RemoveChildWindow(List<string> keys)
        {
            keys.ForEach(key =>
                {
                    RemoveChildWindow(key);
                });
        }

        /// <summary>
        /// 清空子窗口
        /// </summary>
        public void ClearChildWindows()
        {
            childWindow.Children.Clear();
            childWindows.Clear();
        }

        #endregion
    }
}

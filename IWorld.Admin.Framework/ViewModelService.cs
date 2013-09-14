using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using IWorld.Admin.Framework.ManagerService;

namespace IWorld.Admin.Framework
{
    /// <summary>
    /// 视图模型服务
    /// </summary>
    public sealed class ViewModelService
    {
        #region 静态属性

        static ViewModelService _current = new ViewModelService();

        /// <summary>
        /// 获取应用程序的当前 ViewModelService 对象
        /// </summary>
        public static ViewModelService Current
        {
            get
            {
                return _current;
            }
        }

        #endregion

        #region 私有字段

        /// <summary>
        /// 界面名称对照
        /// </summary>
        Dictionary<string, Page> _contrast = new Dictionary<string, Page>();

        /// <summary>
        /// 界面创建者的集合
        /// </summary>
        Dictionary<Page, ControlCreater> _pageCreaters = new Dictionary<Page, ControlCreater>();

        /// <summary>
        /// 默认界面的标识
        /// </summary>
        Page defaultPage = Page.LoginPage;

        #endregion

        #region 公开属性

        /// <summary>
        /// 获取当前应用程序的主界面
        /// </summary>
        public IMainPage Root { get; set; }

        #endregion

        #region 实例方法

        /// <summary>
        /// 初始化视图模型服务
        /// </summary>
        /// <param name="root">当前应用程序的主界面</param>
        public void Initialize(IMainPage root)
        {
            this.Root = root;
            this.Root.RegisterViews();
            JumpToDefaultPage();
        }

        /// <summary>
        /// 根据界面名称获取对应界面标识
        /// </summary>
        /// <param name="pageName">界面名称</param>
        /// <returns>返回跟指定的界面名称所对应的界面标识
        /// （如果对照表中不存在对应的界面标识则返回默认页）</returns>
        public Page GetPageByName(string pageName)
        {
            bool hadName = _contrast.Any(x => x.Key == pageName);
            if (!hadName)
            {
                return defaultPage;
            }

            return _contrast[pageName];
        }

        #region 界面跳转

        /// <summary>
        /// 加载默认界面
        /// </summary>
        public void JumpToDefaultPage()
        {
            JumpTo(this.defaultPage);
        }

        /// <summary>
        /// 界面跳转
        /// </summary>
        /// <param name="page">界面标识</param>
        public void JumpTo(Page page)
        {
            bool haveCreater = _pageCreaters.Any(x => x.Key == page);
            if (!haveCreater)
            {
                throw new Exception("指定的界面并没有在系统中注册");
            }
            Messager.Default.ClearTemporarilyRegisters();
            UserControl userControl = _pageCreaters[page]();
            Root.DataContext = GetViewModel(page);
            Root.Show(userControl);
        }
        #region 获取ViewModel实例

        /// <summary>
        /// 获取ViewModel实例
        /// </summary>
        /// <param name="page">界面标识</param>
        /// <returns>返回ViewModel实例</returns>
        object GetViewModel(Page page)
        {
            string viewModelName = new Regex("Page").Replace(page.ToString(), "ViewModel");
            Assembly assembly = Assembly.GetExecutingAssembly();
            return assembly.CreateInstance(string.Format("IWorld.Admin.Framework.{0}", viewModelName));
        }

        #endregion

        #endregion

        #region 可跳转界面注册

        /// <summary>
        /// 注册界面名称对照
        /// </summary>
        /// <param name="page">界面标识</param>
        /// <param name="pageName">界面名称</param>
        public void RegisterContrast(Page page, string pageName)
        {
            bool hadName = _contrast.Any(x => x.Key == pageName);
            if (!hadName)
            {
                _contrast.Add(pageName, page);
            }
        }

        /// <summary>
        /// 注册可跳转界面
        /// </summary>
        /// <param name="page">界面标识</param>
        /// <param name="creater">对应的创建者委托</param>
        public void RegisterPage(Page page, ControlCreater creater)
        {
            bool haveCreater = _pageCreaters.Any(x => x.Key == page);
            if (!haveCreater)
            {
                _pageCreaters.Add(page, creater);
            }
        }

        /// <summary>
        /// 设置默认界面
        /// </summary>
        /// <param name="page">界面标识</param>
        public void SetDefaultPage(Page page)
        {
            this.defaultPage = page;
        }

        #endregion

        #endregion

        #region 内嵌委托

        /// <summary>
        /// UI元素的创建者委托
        /// </summary>
        /// <returns>返回界面实例</returns>
        public delegate UserControl ControlCreater();

        /// <summary>
        /// 然床元素的创建者委托
        /// </summary>
        /// <returns>返回子窗口实例</returns>
        public delegate ChildWindow WindowCreater();

        #endregion
    }
}

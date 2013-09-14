using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IWorld.Admin.Class;
using IWorld.Admin.SystemSettingService;

namespace IWorld.Admin
{
    public partial class App : Application
    {
        #region 用户个人信息缓存区

        /// <summary>
        /// 私匙
        /// </summary>
        public static string Token { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public static UserInfoCaChe User { get; set; }

        /// <summary>
        /// 站点设置
        /// </summary>
        public static WebSettingResult WebSetting { get; set; }

        /// <summary>
        /// 信息缓存
        /// </summary>
        public static Dictionary<string, object> Cache
        {
            get { return cache; }
            set { cache = value; }
        }
        #region 信息缓存

        private static Dictionary<string, object> cache = new Dictionary<string, object>();

        #endregion

        #endregion

        #region 动作

        /// <summary>
        /// 显示遮蔽层
        /// </summary>
        public static void ShowCover()
        {
            ((MainPage)App.Current.RootVisual).ShowCover();
        }

        /// <summary>
        /// 隐藏遮蔽层
        /// </summary>
        public static void HideCover()
        {
            ((MainPage)App.Current.RootVisual).HideCover();
        }

        /// <summary>
        /// 跳转到操作页
        /// </summary>
        public static void GoToSetUpPage()
        {
            ((MainPage)App.Current.RootVisual).GoToSetUpPage();
        }

        /// <summary>
        /// 跳转到登陆页
        /// </summary>
        public static void GoToLoginPage()
        {
            ((MainPage)App.Current.RootVisual).GoToLoginPage();
        }

        #endregion

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new MainPage();
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // 如果应用程序是在调试器外运行的，则使用浏览器的
            // 异常机制报告该异常。在 IE 上，将在状态栏中用一个 
            // 黄色警报图标来显示该异常，而 Firefox 则会显示一个脚本错误。
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // 注意: 这使应用程序可以在已引发异常但尚未处理该异常的情况下
                // 继续运行。 
                // 对于生产应用程序，此错误处理应替换为向网站报告错误
                // 并停止应用程序。
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}

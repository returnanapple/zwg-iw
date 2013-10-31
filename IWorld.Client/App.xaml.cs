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
using IWorld.Client.UsersService;
using IWorld.Client.SystemSettingService;
using IWorld.Client.GamingService;
using IWorld.Client.BulletinService;

namespace IWorld.Client
{
    public partial class App : Application
    {
        #region 参数

        public static double LeftOfInitia = 0;
        public static double TopOfInitial = 0;

        /// <summary>
        /// 身份标识
        /// </summary>
        public static string Token { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public static UserInfoResult UserInfo { get; set; }

        /// <summary>
        /// 站点设置
        /// </summary>
        public static WebSettingResult Websetting { get; set; }

        /// <summary>
        /// 彩票信息和设置
        /// </summary>
        public static List<LotteryTicketResult> Ticktes { get; set; }

        /// <summary>
        /// 彩票信息刷新时间
        /// </summary>
        public static DateTime TicketsRefreshTime { get; set; }

        /// <summary>
        /// 公告列表
        /// </summary>
        public static List<BulletinResult> Bulletins { get; set; }

        #endregion

        #region 方法

        /// <summary>
        /// 跳转到登陆页
        /// </summary>
        public static void GoToLoginPage()
        {
            MainPage mp = (MainPage)App.Current.RootVisual;
            mp.root.Children.Clear();
            mp.root.Children.Add(new LoginPage());

            if (App.Current.IsRunningOutOfBrowser)
            {
                App.Current.MainWindow.Width = 241;
                App.Current.MainWindow.Height = 541;
                App.Current.MainWindow.Top = App.TopOfInitial;
                //App.Current.MainWindow.Left = App.LeftOfInitia;
            }
        }

        /// <summary>
        /// 跳转到操作页
        /// </summary>
        public static void GoToOperatePage()
        {
            MainPage mp = (MainPage)App.Current.RootVisual;
            mp.root.Children.Clear();
            mp.root.Children.Add(new OperatePage());

            if (App.Current.IsRunningOutOfBrowser)
            {
                App.Current.MainWindow.Width = 1024;
                App.Current.MainWindow.Height = 768;
                App.Current.MainWindow.Top = 0;
                App.Current.MainWindow.Left = 100;
            }
        }

        /// <summary>
        /// 跳转到用户信息绑定页
        /// </summary>
        public static void GoToBindingPage()
        {
            MainPage mp = (MainPage)App.Current.RootVisual;
            mp.root.Children.Clear();
            mp.root.Children.Add(new BindingPage());

            if (App.Current.IsRunningOutOfBrowser)
            {
                App.Current.MainWindow.Width = 420;
                App.Current.MainWindow.Height = 400;
                App.Current.MainWindow.Top = App.TopOfInitial + 100;
                App.Current.MainWindow.Left = App.LeftOfInitia + 50;
            }
        }

        #endregion

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;
            Ticktes = new List<LotteryTicketResult>();
            Bulletins = new List<BulletinResult>();

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new MainPage();
            if (App.Current.IsRunningOutOfBrowser)
            {
                App.Current.MainWindow.Width = 241;
                App.Current.MainWindow.Height = 541;
                App.Current.MainWindow.Top = App.TopOfInitial + 100;
                ////App.Current.MainWindow.Left = App.LeftOfInitia;
            }
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

using System;
using System.Reflection;
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
using IWorld.Admin.Framework;

namespace IWorld.Admin
{
    public partial class MainPage : UserControl, IMainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        #region 系统方法

        /// <summary>
        /// 显示目标子界面
        /// </summary>
        /// <param name="userControl">所要显示的子界面</param>
        public void Show(UserControl userControl)
        {
            body.Children.Clear();
            body.Children.Add(userControl);
        }

        /// <summary>
        /// 注册可跳转界面
        /// </summary>
        public void RegisterViews()
        {
            //获取目标程序集
            Assembly assembly = Assembly.GetExecutingAssembly();

            #region 注册可跳转界面

            assembly.GetTypes()
                .Where(x => x.GetCustomAttributes(true).Any(t => t is ViewAttribute))
                .ToList().ForEach(x =>
                    {
                        bool isPage = Enum.GetNames(typeof(Page)).Contains(x.Name);
                        if (!isPage)
                        {
                            return;
                        }

                        Page page = (Page)Enum.Parse(typeof(Page), x.Name, false);
                        ViewModelService.Current.RegisterPage(page
                            , new ViewModelService.ControlCreater(() =>
                                {
                                    return assembly.CreateInstance(x.FullName) as UserControl;
                                }));
                        var attribute = x.GetCustomAttributes(true).First(t => t is ViewAttribute) as ViewAttribute;
                        if (attribute.IsDefault)
                        {
                            ViewModelService.Current.SetDefaultPage(page);
                        }
                        ViewModelService.Current.RegisterContrast(page, attribute.PageName);
                    });

            #endregion

            //注册自定义控件
            new IWorld.Admin.Controls.RI().Register();
        }

        #endregion
    }
}

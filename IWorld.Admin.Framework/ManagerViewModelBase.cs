using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.IO.IsolatedStorage;
using IWorld.Admin.Framework.ManagerService;

namespace IWorld.Admin.Framework
{
    /// <summary>
    /// 操作视图的基类
    /// </summary>
    public abstract class ManagerViewModelBase : ViewModelBase
    {
        #region 私有变量

        string username = "";
        bool readedUsername = false;
        string group = "";
        bool readedGroup = false;
        string selectedText_Menu = "";
        string selectedText_Page = "";

        #endregion

        #region 公开属性

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username
        {
            get
            {
                if (readedUsername)
                {
                    return username;
                }

                string dataKeyOfManagerInfo = DataKey.IWorld_ManagerInfo.ToString();
                bool hadRememberMe = IsolatedStorageSettings.ApplicationSettings
                .Any(x => x.Key == dataKeyOfManagerInfo && x.Value is LoginResult);
                if (hadRememberMe)
                {
                    LoginResult package = IsolatedStorageSettings.ApplicationSettings[dataKeyOfManagerInfo] as LoginResult;
                    username = package.Username;
                }
                readedUsername = true;
                return username;
            }
        }

        /// <summary>
        /// 用户组
        /// </summary>
        public string Group
        {
            get
            {
                if (readedGroup)
                {
                    return group;
                }

                string dataKeyOfManagerInfo = DataKey.IWorld_ManagerInfo.ToString();
                bool hadRememberMe = IsolatedStorageSettings.ApplicationSettings
                .Any(x => x.Key == dataKeyOfManagerInfo && x.Value is LoginResult);
                if (hadRememberMe)
                {
                    LoginResult package = IsolatedStorageSettings.ApplicationSettings[dataKeyOfManagerInfo] as LoginResult;
                    group = package.Group.Name;
                }
                readedGroup = true;
                return group;
            }
        }

        /// <summary>
        /// 导航选择文本
        /// </summary>
        public string SelectedText_Menu
        {
            get
            {
                return selectedText_Menu;
            }
            protected set
            {
                if (selectedText_Menu != value)
                {
                    selectedText_Menu = value;
                    OnPropertyChanged("SelectedText_Menu");
                }
            }
        }

        /// <summary>
        /// 页面选择文本
        /// </summary>
        public string SelectedText_Page
        {
            get
            {
                return selectedText_Page;
            }
            protected set
            {
                if (selectedText_Page != value)
                {
                    selectedText_Page = value;
                    OnPropertyChanged("SelectedText_Page");
                }
            }
        }

        /// <summary>
        /// 跳转命令
        /// </summary>
        public NCommand JumpCommand { get; set; }

        /// <summary>
        /// 编辑账户命令
        /// </summary>
        public NCommand EditAccountCommand { get; set; }

        /// <summary>
        /// 登出命令
        /// </summary>
        public NCommand LogoutCommand { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的操作视图的基类
        /// </summary>
        /// <param name="selectedText_Menu">导航选择文本</param>
        /// <param name="selectedText_Page">页面选择文本</param>
        public ManagerViewModelBase(string selectedText_Menu, string selectedText_Page)
        {
            //初始化属性
            SelectedText_Menu = selectedText_Menu;
            SelectedText_Page = selectedText_Page;

            //初始化命令
            JumpCommand = new NCommand(new Action<object>(Jump));
            EditAccountCommand = new NCommand(new Action<object>(EditAccount));
            LogoutCommand = new NCommand(new Action<object>(Logout));

            //注册监听系统信息的方法
            Messager.Default.RegisterRecipients(
                new List<MonitorCondition> { new MonitorCondition(ComAction.Logout, LogoutActionStatus.DoNow) },
                new RecipientDelegate(Logout_do),
                true);
            Messager.Default.RegisterRecipients(
                new List<MonitorCondition> { new MonitorCondition(ComAction.EditAccount, EditAccountActionStatus.EditNow) },
                new RecipientDelegate(EditAccount_do),
                true);
            Messager.Default.RegisterRecipients(
                new List<MonitorCondition> { new MonitorCondition(ComAction.EditAccount, EditAccountActionStatus.Done) },
                new RecipientDelegate(ReVisit),
                true);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="parameter"></param>
        void Logout(object parameter)
        {
            string prompt = "你想要退出后台管理系统吗？";
            IMessage message = Messager.Default.CreateMessage<LogoutActionStatus>(ComAction.Logout, prompt);
            Messager.Default.Send(message);
        }
        #region 执行

        /// <summary>
        /// 登出 - 执行
        /// </summary>
        /// <param name="message"></param>
        void Logout_do(IMessage message)
        {
            string dataKeyOfManagerInfo = DataKey.IWorld_ManagerInfo.ToString();
            IsolatedStorageSettings.ApplicationSettings.Remove(dataKeyOfManagerInfo);
            IsolatedStorageSettings.ApplicationSettings.Save();

            ViewModelService.Current.JumpToDefaultPage();

            message.Handle();
            Messager.Default.Send(message);
        }

        #endregion

        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="parameter">参数</param>
        void Jump(object parameter)
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            string pageName = parameter.ToString();
            Page page = ViewModelService.Current.GetPageByName(pageName);
            ViewModelService.Current.JumpTo(page);
        }

        /// <summary>
        /// 修改账户信息
        /// </summary>
        /// <param name="parameter">参数</param>
        void EditAccount(object parameter)
        {
            IMessage message = Messager.Default.CreateMessage<EditAccountActionStatus>(ComAction.EditAccount);
            Messager.Default.Send(message);
        }
        #region 执行

        /// <summary>
        /// 系统信息的临时缓存
        /// </summary>
        IMessage tMessage = null;

        /// <summary>
        /// 修改账户信息 - 执行
        /// </summary>
        /// <param name="message">系统消息</param>
        void EditAccount_do(IMessage message)
        {
            IEditAccountPackage package = (IEditAccountPackage)message.Content;
            if (package.NewPassword != package.NewPassword_Confirm)
            {
                string prompt = "两次输入的新密码不一致，请检查输入";
                message.SetStatus(EditAccountActionStatus.ShowError, prompt);
                Messager.Default.Send(message);
                return;
            }

            if (IsBusy)
            {
                return;
            }
            tMessage = message;
            IsBusy = true;
            try
            {
                ManagerServiceClient client = new ManagerServiceClient();
                client.ResetPassageCompleted += ShowEditAccountResult;
                string dataKeyOfManagerInfo = DataKey.IWorld_ManagerInfo.ToString();
                LoginResult lr = (LoginResult)IsolatedStorageSettings.ApplicationSettings[dataKeyOfManagerInfo];
                client.ResetPassageAsync(0, package.OldPassword, package.NewPassword, lr.Token);
            }
            catch (Exception)
            {
                ViewModelService.Current.JumpToDefaultPage();
            }
        }

        /// <summary>
        /// 反馈修改密码的结果
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        void ShowEditAccountResult(object sender, ResetPassageCompletedEventArgs e)
        {
            IsBusy = false;
            IMessage message = tMessage;
            if (!e.Result.Success)
            {
                message.SetStatus(EditAccountActionStatus.ShowError, e.Result.Error);
                Messager.Default.Send(message);
                return;
            }

            message.Handle("修改密码成功，请重新登陆");
            Messager.Default.Send(message);
        }

        /// <summary>
        /// 重新登陆（修改密码之后）
        /// </summary>
        /// <param name="message">系统消息</param>
        void ReVisit(IMessage message)
        {
            string dataKeyOfRememberMe = DataKey.IWorld_RememberMe.ToString();
            string dataKeyOfManagerInfo = DataKey.IWorld_ManagerInfo.ToString();
            IsolatedStorageSettings.ApplicationSettings.Remove(dataKeyOfRememberMe);
            IsolatedStorageSettings.ApplicationSettings.Remove(dataKeyOfManagerInfo);
            IsolatedStorageSettings.ApplicationSettings.Save();

            ViewModelService.Current.JumpToDefaultPage();
        }

        #endregion

        #endregion

        public class Tag
        {
            public string Value { get; set; }
        }
    }
}

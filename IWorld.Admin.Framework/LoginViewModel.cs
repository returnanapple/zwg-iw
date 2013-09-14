﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.IO.IsolatedStorage;
using IWorld.Admin.Framework.ManagerService;

namespace IWorld.Admin.Framework
{
    /// <summary>
    /// 登陆页的视图模型
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        #region 私有字段

        string _username = "";
        string _password = "";
        bool _rememberMe = false;
        string _error = "";

        #endregion

        #region 公开属性

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged("Username");
                    RememberMe = false;
                }
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged("Password");
                    RememberMe = false;
                }
            }
        }

        /// <summary>
        /// 一个布尔值 标识是否记住密码
        /// </summary>
        public bool RememberMe
        {
            get
            {
                return _rememberMe;
            }
            set
            {
                if (_rememberMe != value)
                {
                    _rememberMe = value;
                    OnPropertyChanged("RememberMe");
                }
            }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error
        {
            get
            {
                return _error;
            }
            protected set
            {
                if (_error != value)
                {
                    _error = value;
                    OnPropertyChanged("Error");
                }
            }
        }

        /// <summary>
        /// 登陆通知
        /// </summary>
        public NCommand LoginCommand { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的登陆页的视图模型
        /// </summary>
        public LoginViewModel()
        {
            this.LoginCommand = new NCommand(Login);

            string dataKeyOfRememberMe = DataKey.IWorld_RememberMe.ToString();
            bool hadRememberMe = IsolatedStorageSettings.ApplicationSettings
                .Any(x => x.Key == dataKeyOfRememberMe && x.Value is LoginPackage);
            if (hadRememberMe)
            {
                LoginPackage package = IsolatedStorageSettings.ApplicationSettings[dataKeyOfRememberMe] as LoginPackage;
                this.Username = package.Username;
                this.Password = package.Password;
                this.RememberMe = true;
            }
        }

        #endregion

        #region 私有方法

        //登陆
        void Login(object parameter)
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            //重置提示栏
            this.Error = "";

            #region 记住密码

            string dataKeyOfRememberMe = DataKey.IWorld_RememberMe.ToString();
            if (this.RememberMe == true)
            {
                LoginPackage package = new LoginPackage
                {
                    Username = this.Username,
                    Password = this.Password
                };
                IsolatedStorageSettings.ApplicationSettings[dataKeyOfRememberMe] = package;
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(dataKeyOfRememberMe);
            }
            IsolatedStorageSettings.ApplicationSettings.Save();

            #endregion

            //登陆
            ManagerServiceClient client = new ManagerServiceClient();
            client.LoginCompleted += ShowLoginResult;
            client.LoginAsync(this.Username, this.Password);
        }
        #region 登陆结果

        //处理登陆结果
        void ShowLoginResult(object sender, LoginCompletedEventArgs e)
        {
            string dataKeyOfManagerInfo = DataKey.IWorld_ManagerInfo.ToString();
            string dataKeyOfRememberMe = DataKey.IWorld_RememberMe.ToString();
            if (e.Result.Success)
            {
                IsolatedStorageSettings.ApplicationSettings[dataKeyOfManagerInfo] = e.Result;
                IsolatedStorageSettings.ApplicationSettings.Save();

                ViewModelService.Current.JumpTo(Page.IndexPage);
            }
            else
            {
                this.Error = e.Result.Error;

                IsolatedStorageSettings.ApplicationSettings.Remove(dataKeyOfManagerInfo);
                IsolatedStorageSettings.ApplicationSettings.Remove(dataKeyOfRememberMe);
                IsolatedStorageSettings.ApplicationSettings.Save();
                this.RememberMe = false;
                IsBusy = false;
            }
        }

        #endregion

        #endregion

        #region 内置类型

        /// <summary>
        /// 用户登陆信息封装
        /// </summary>
        public class LoginPackage
        {
            /// <summary>
            /// 用户名
            /// </summary>
            public string Username { get; set; }

            /// <summary>
            /// 密码
            /// </summary>
            public string Password { get; set; }
        }

        #endregion
    }
}

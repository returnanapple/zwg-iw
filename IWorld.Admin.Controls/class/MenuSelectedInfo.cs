using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace IWorld.Admin.Controls
{
    public class MenuSelectedInfo : INotifyPropertyChanged
    {
        #region 私有变量

        string username = "";
        string group = "";
        string openMenuText = "";
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
                return username;
            }
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged("Username");
                }
            }
        }

        /// <summary>
        /// 用户组
        /// </summary>
        public string Group
        {
            get
            {
                return group;
            }
            set
            {
                if (group != value)
                {
                    group = value;
                    OnPropertyChanged("Group");
                }
            }
        }

        /// <summary>
        /// 当前打开的导航区选择文本
        /// </summary>
        public string OpenMenuText
        {
            get
            {
                return openMenuText;
            }
            set
            {
                if (openMenuText != value)
                {
                    openMenuText = value;
                }
                else
                {
                    openMenuText = "";
                }
                OnPropertyChanged("OpenMenuText");
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
            set
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
            set
            {
                if (selectedText_Page != value)
                {
                    selectedText_Page = value;
                    OnPropertyChanged("SelectedText_Page");
                }
            }
        }

        #endregion

        #region 事件

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}

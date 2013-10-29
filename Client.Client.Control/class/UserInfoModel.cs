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
using Client.Client.Control.ChatService;

namespace Client.Client.Control
{
    public class UserInfoModel : ModelBase
    {
        #region 私有字段

        string username = "";
        UserShowStatus status = UserShowStatus.离线;
        UserInfoType userType = UserInfoType.用户;
        int countOfNewmessages = 0;

        #endregion

        #region 公开属性

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged("username");
                }
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public UserShowStatus Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        /// <summary>
        /// 用户类型
        /// </summary>
        public UserInfoType UserType
        {
            get { return userType; }
            set
            {
                if (userType != value)
                {
                    userType = value;
                    OnPropertyChanged("UserType");
                }
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int CountOfNewmessages
        {
            get { return countOfNewmessages; }
            set
            {
                if (countOfNewmessages != value)
                {
                    countOfNewmessages = value;
                    OnPropertyChanged("CountOfNewmessages");
                }
            }
        }

        /// <summary>
        /// 打开聊天窗口命令
        /// </summary>
        public UniversalCommand OpenTalkWindowCommand { get; set; }

        #endregion
    }
}

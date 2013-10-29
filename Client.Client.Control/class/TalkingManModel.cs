using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Client.Control
{
    public class TalkingManModel : ModelBase
    {
        #region 私有字段

        string username = "";
        bool online = false;
        bool isSelected = false;

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
                    OnPropertyChanged("Username");
                }
            }
        }

        /// <summary>
        /// 当前选择的用户
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        /// <summary>
        /// 一个布尔值 标识用户是否在线
        /// </summary>
        public bool Online
        {
            get { return online; }
            set
            {
                if (online != value)
                {
                    online = value;
                    OnPropertyChanged("Online");
                }
            }
        }

        /// <summary>
        /// 将用户移出“正在聊天”列表的命令
        /// </summary>
        public UniversalCommand RemoveTalkingUserCommand { get; set; }

        /// <summary>
        /// 选择目标聊天用户的命令
        /// </summary>
        public UniversalCommand ChooseTalkingUserCommand { get; set; }


        #endregion
    }
}

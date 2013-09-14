using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 客服账号
    /// </summary>
    public class Waiter : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 客服类型
        /// </summary>
        public CustomerType CustomerType { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的客服账号
        /// </summary>
        public Waiter()
        {
        }

        /// <summary>
        /// 实例化一个新的客服账号
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="customerType">客服类型</param>
        public Waiter(string username, string password, CustomerType customerType)
        {
            this.Username = username;
            this.Password = password;
            this.CustomerType = customerType;
        }

        #endregion
    }
}

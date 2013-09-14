using System;

namespace IWorld.Model
{
    /// <summary>
    /// 管理员
    /// </summary>
    public class Administrator : ModelBase
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
        /// 用户组
        /// </summary>
        public virtual AdministratorGroup Group { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 上次登录的网络地址
        /// </summary>
        public string LastLoginIp { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的管理员
        /// </summary>
        public Administrator()
        {
        }

        /// <summary>
        /// 实例化一个新的管理员
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="group">用户组</param>
        public Administrator(string username, string password, AdministratorGroup group)
        {
            this.Username = username;
            this.Password = password;
            this.Group = group;
            this.LastLoginTime = DateTime.Now;
            this.LastLoginIp = "127.0.0.1";
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class Author : CategoryModelBase
    {
        #region 属性

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 安全码
        /// </summary>
        public string SafeCode { get; set; }

        /// <summary>
        /// 正常返点数
        /// </summary>
        public double Rebate_Normal { get; set; }

        /// <summary>
        /// 不定位返点数
        /// </summary>
        public double Rebate_UnLocate { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserStatus Status { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 上次登录的网络地址
        /// </summary>
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 绑定信息
        /// </summary>
        public virtual UserBinding Binding { get; set; }

        /// <summary>
        /// 用户数据
        /// </summary>
        public virtual UserData Data { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的用户信息
        /// </summary>
        public Author()
        {
        }

        /// <summary>
        /// 实例化一个新的用户信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="rebate_Normal">正常返点数</param>
        /// <param name="rebate_UnLocate">不定位返点数</param>
        /// <param name="relative">父祖节点</param>
        /// <param name="tree">所从属的树</param>
        public Author(string username, string password, double rebate_Normal, double rebate_UnLocate
            , List<Relative> relative = null, string tree = "")
            : base(relative, tree)
        {
            this.Username = username;
            this.Password = password;
            this.SafeCode = password;
            this.Rebate_Normal = rebate_Normal;
            this.Rebate_UnLocate = rebate_UnLocate;
            this.Status = UserStatus.未激活;
            this.LastLoginTime = DateTime.Now;
            this.LastLoginIp = "";
            this.Binding = new UserBinding();
            this.Data = new UserData();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 声明用户登陆
        /// </summary>
        /// <param name="ip">IP</param>
        public void OnLogin(string ip)
        {
            this.LastLoginTime = DateTime.Now;
            this.LastLoginIp = ip;
        }

        #endregion
    }
}

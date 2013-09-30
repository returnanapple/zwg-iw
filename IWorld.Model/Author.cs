using System;
using System.Collections.Generic;
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
        /// 正常返点
        /// </summary>
        public double Rebate_Normal { get; set; }

        /// <summary>
        /// 不定位返点
        /// </summary>
        public double Rebate_IndefinitePosition { get; set; }

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
        /// 统计数据
        /// </summary>
        public virtual UserData Data { get; set; }

        /// <summary>
        /// 绑定信息
        /// </summary>
        public virtual UserBinding Binding { get; set; }

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
        /// <param name="rebate_Normal">普通返点</param>
        /// <param name="rebate_IndefinitePosition">不定位返点</param>
        /// <param name="relatives">父祖节点</param>
        /// <param name="tree">所从属的树</param>
        public Author(string username, string password, double rebate_Normal, double rebate_IndefinitePosition
            , List<Relative> relatives, string tree)
            : base(relatives, tree)
        {
            this.Username = username;
            this.Password = password;
            this.SafeCode = password;
            this.Rebate_Normal = rebate_Normal;
            this.Rebate_IndefinitePosition = rebate_IndefinitePosition;
            this.Status = UserStatus.未激活;
            this.LastLoginIp = "";
            this.LastLoginTime = DateTime.Now;
            this.Data = new UserData();
            this.Binding = new UserBinding();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 声明用户已就登陆
        /// </summary>
        /// <param name="ip"></param>
        public void OnLogin(string ip)
        {
            this.LastLoginIp = ip;
            this.LastLoginTime = DateTime.Now;
            this.Data.IncreaseTimesOfLogin();
        }

        #endregion
    }
}

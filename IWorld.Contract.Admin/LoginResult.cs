using System;
using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 后台登陆结果
    /// </summary>
    [DataContract]
    public class LoginResult : OperateResult
    {
        /// <summary>
        /// 身份标识
        /// </summary>
        [DataMember]
        public string Token { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string Username { get; set; }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        [DataMember]
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 用户组
        /// </summary>
        [DataMember]
        public ManagerGroupResult Group { get; set; }

        /// <summary>
        /// 实例化一个新的后台登陆结果（成功）
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <param name="username">用户名</param>
        /// <param name="lastLoginTime">最后一次登录时间</param>
        /// <param name="group">用户组</param>
        public LoginResult(string token, string username, DateTime lastLoginTime, ManagerGroupResult group)
        {
            this.Token = token;
            this.Username = username;
            this.LastLoginTime = lastLoginTime;
            this.Group = group;
        }

        /// <summary>
        /// 实例化一个新的后台登陆结果（失败）
        /// </summary>
        /// <param name="error">错误信息</param>
        public LoginResult(string error)
            : base(error)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 创建新用户结果
    /// </summary>
    [DataContract]
    public class CreateUserResult : OperateResult
    {
        #region 公开属性

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string Username { get; set; }

        /// <summary>
        /// 初始密码
        /// </summary>
        [DataMember]
        public string Password { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的创建新用户结果（成功）
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">初始密码</param>
        public CreateUserResult(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        /// <summary>
        /// 实例化一个新的创建新用户结果（失败）
        /// </summary>
        /// <param name="error">错误信息</param>
        public CreateUserResult(string error)
            : base(error)
        {
        }

        #endregion
    }
}

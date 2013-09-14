using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 登录操作的结果
    /// </summary>
    [DataContract]
    public class LoginResult : OperateResult
    {
        #region 公开属性

        /// <summary>
        /// 身份标识
        /// </summary>
        [DataMember]
        public string Token { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的登录操作的结果（成功）
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <param name="success">一个布尔值 标识操作是否成功</param>
        public LoginResult(string token, bool success)
        {
            this.Token = token;
        }

        /// <summary>
        /// 实例化一个新的登录操作的结果（失败）
        /// </summary>
        /// <param name="error">错误信息</param>
        public LoginResult(string error)
            : base(error)
        {
        }

        #endregion
    }
}

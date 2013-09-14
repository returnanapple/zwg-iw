using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 重置密码结果
    /// </summary>
    [DataContract]
    public class ResetPasswordResult : OperateResult
    {
        /// <summary>
        /// 新密码
        /// </summary>
        [DataMember]
        public string NewPassword { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [DataMember]
        public string Operator { get; set; }

        /// <summary>
        /// 实例化一个新的重置密码结果（成功）
        /// </summary>
        /// <param name="newPassword">新密码</param>
        /// <param name="_operator">操作人</param>
        public ResetPasswordResult(string newPassword, string _operator)
        {
            this.NewPassword = newPassword;
            this.Operator = _operator;
        }

        /// <summary>
        /// 实例化一个新的重置密码结果（失败）
        /// </summary>
        /// <param name="error">错误信息</param>
        public ResetPasswordResult(string error)
            : base(error)
        {
        }
    }
}

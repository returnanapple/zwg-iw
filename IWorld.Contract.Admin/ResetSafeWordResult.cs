using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 重置安全码结果
    /// </summary>
    [DataContract]
    public class ResetSafeWordResult : OperateResult
    {
        /// <summary>
        /// 新的安全码
        /// </summary>
        [DataMember]
        public string NewSafeWord { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [DataMember]
        public string Operator { get; set; }

        /// <summary>
        /// 实例化一个新的重置安全码结果（成功）
        /// </summary>
        /// <param name="newSafeWord">新的安全码</param>
        /// <param name="_operator">操作人</param>
        public ResetSafeWordResult(string newSafeWord, string _operator)
        {
            this.NewSafeWord = newSafeWord;
            this.Operator = _operator;
        }

        /// <summary>
        /// 实例化一个新的重置安全码结果（失败）
        /// </summary>
        /// <param name="error">错误信息</param>
        public ResetSafeWordResult(string error)
            : base(error)
        {
        }
    }
}

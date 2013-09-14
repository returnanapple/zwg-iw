using System.Runtime.Serialization;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 操作结果
    /// </summary>
    [DataContract]
    public class OperateResult
    {
        /// <summary>
        /// 标识操作是否成功
        /// </summary>
        [DataMember]
        public bool Success { get; set; }

        /// <summary>
        /// 错误信息（如果操作成功应为空）
        /// </summary>
        [DataMember]
        public string Error { get; set; }

        /// <summary>
        /// 实例化一个新的操作结果（成功）
        /// </summary>
        public OperateResult()
        {
            this.Success = true;
        }

        /// <summary>
        /// 实例化一个新的操作结果（失败）
        /// </summary>
        /// <param name="error">错误信息（如果操作成功应为空）</param>
        public OperateResult(string error)
        {
            this.Success = false;
            this.Error = error;
        }
    }
}

using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 未处理充值申请的数量信息
    /// </summary>
    [DataContract]
    public class UntreatedRecharCountResult : OperateResult
    {
        /// <summary>
        /// 数量
        /// </summary>
        [DataMember]
        public int Count { get; set; }

        /// <summary>
        /// 实例化一个新的未处理充值申请的数量信息（成功）
        /// </summary>
        /// <param name="count">数量</param>
        public UntreatedRecharCountResult(int count)
        {
            this.Count = count;
        }

        /// <summary>
        /// 实例化一个新的未处理充值申请的数量信息（失败）
        /// </summary>
        /// <param name="error">错误信息</param>
        public UntreatedRecharCountResult(string error)
            : base(error)
        {
        }
    }
}

using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 未处理提现申请的数量
    /// </summary>
    [DataContract]
    public class UntreatedWithdrawalCountResult : OperateResult
    {
        /// <summary>
        /// 数量
        /// </summary>
        [DataMember]
        public int Count { get; set; }

        /// <summary>
        /// 实例化一个新的未处理提现申请的数量信息（成功）
        /// </summary>
        /// <param name="count">数量</param>
        public UntreatedWithdrawalCountResult(int count)
        {
            this.Count = count;
        }

        /// <summary>
        /// 实例化一个新的未处理提现申请的数量信息（失败）
        /// </summary>
        /// <param name="error">错误信息</param>
        public UntreatedWithdrawalCountResult(string error)
            : base(error)
        {
        }
    }
}

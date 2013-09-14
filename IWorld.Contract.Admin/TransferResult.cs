using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 支取信息
    /// </summary>
    [DataContract]
    public class TransferResult
    {
        /// <summary>
        /// 支取记录的存储指针
        /// </summary>
        [DataMember]
        public int TransferId { get; set; }

        /// <summary>
        /// 操作人的存储指针
        /// </summary>
        [DataMember]
        public int OwnerId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [DataMember]
        public string Owner { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DataMember]
        public double Sum { get; set; }

        /// <summary>
        /// 支取记录
        /// </summary>
        [DataMember]
        public string Remark { get; set; }

        /// <summary>
        /// 实例化一个新的支取信息
        /// </summary>
        /// <param name="transfer">支取信息的数据封装</param>
        public TransferResult(TransferRecord transfer)
        {
            this.TransferId = transfer.Id;
            this.Owner = transfer.Owner.Username;
            this.OwnerId = transfer.Owner.Id;
            this.Sum = transfer.Sum;
            this.Remark = transfer.Remark;
        }
    }
}

using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 提现申请信息
    /// </summary>
    [DataContract]
    public class WithdrawalResult
    {
        /// <summary>
        /// 提现申请的存储指针
        /// </summary>
        [DataMember]
        public int WithdrawalId { get; set; }

        /// <summary>
        /// 申请人的存储指针
        /// </summary>
        [DataMember]
        public int OwnerId { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        [DataMember]
        public string Owner { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DataMember]
        public double Sum { get; set; }

        /// <summary>
        /// 目标银行卡的卡号
        /// </summary>
        [DataMember]
        public string Card { get; set; }

        /// <summary>
        /// 目标银行卡的开户人姓名
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 目标银行卡的开户银行
        /// </summary>
        [DataMember]
        public Bank Bank { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        [DataMember]
        public WithdrawalsStatus Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }

        /// <summary>
        /// 实例化一个新的提现申请信息
        /// </summary>
        /// <param name="withdrawals">提现申请信息的数据封装</param>
        public WithdrawalResult(WithdrawalsRecord withdrawals)
        {
            this.WithdrawalId = withdrawals.Id;
            this.Owner = withdrawals.Owner.Username;
            this.OwnerId = withdrawals.Owner.Id;
            this.Sum = withdrawals.Sum;
            this.Name = withdrawals.Name;
            this.Card = withdrawals.Card;
            this.Bank = withdrawals.Bank;
            this.Status = withdrawals.Status;
            this.Remark = withdrawals.Remark;
        }
    }
}

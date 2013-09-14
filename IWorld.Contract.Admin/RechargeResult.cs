using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 充值信息
    /// </summary>
    [DataContract]
    public class RechargeResult
    {
        /// <summary>
        /// 充值记录的存储指针
        /// </summary>
        [DataMember]
        public int RechargeId { get; set; }

        /// <summary>
        /// 目标帐户的存储指针
        /// </summary>
        [DataMember]
        public int OwnerId { get; set; }

        /// <summary>
        /// 目标账户
        /// </summary>
        [DataMember]
        public string Owner { get; set; }

        /// <summary>
        /// 支付人的存储指针
        /// </summary>
        [DataMember]
        public int PayerId { get; set; }

        /// <summary>
        /// 支付人
        /// </summary>
        [DataMember]
        public string Payer { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DataMember]
        public double Sum { get; set; }

        /// <summary>
        /// 来源卡号（交易成功前为空白）
        /// </summary>
        [DataMember]
        public string Card { get; set; }

        /// <summary>
        /// 来源卡的开户人（交易成功前为空白）
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 来源卡的开户银行（交易成功前为空白）
        /// </summary>
        [DataMember]
        public Bank Bank { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        [DataMember]
        public RechargeStatus Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }

        /// <summary>
        /// 标识码
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// 实例化一个新的充值信息
        /// </summary>
        /// <param name="recharge">充值信息的数据封装</param>
        public RechargeResult(RechargeRecord recharge)
        {
            this.RechargeId = recharge.Id;
            this.Owner = recharge.Owner.Username;
            this.OwnerId = recharge.Owner.Id;
            this.PayerId = recharge.Payer.Id;
            this.Payer = recharge.Payer.Username;
            this.Sum = recharge.Sum;
            this.Card = recharge.Card;
            this.Name = recharge.Name;
            this.Bank = recharge.Bank;
            this.Status = recharge.Status;
            this.Remark = recharge.Remark;
            this.Code = recharge.Code;
        }
    }
}

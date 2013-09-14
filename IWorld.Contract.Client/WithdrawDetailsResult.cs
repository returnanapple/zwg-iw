using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 提现明细
    /// </summary>
    [DataContract]
    public class WithdrawDetailsResult
    {
        #region 公开属性

        /// <summary>
        /// 提现记录的存储指针
        /// </summary>
        [DataMember]
        public int WithdrawalsId { get; set; }

        /// <summary>
        /// 提现金额
        /// </summary>
        [DataMember]
        public double Sum { get; set; }

        /// <summary>
        /// 提现时间
        /// </summary>
        [DataMember]
        public DateTime Time { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        [DataMember]
        public WithdrawalsStatus Status { get; set; }

        /// <summary>
        /// 目标银行卡的卡号
        /// </summary>
        [DataMember]
        public string Card { get; set; }

        /// <summary>
        /// 目标银行卡的开户人姓名
        /// </summary>
        [DataMember]
        public string Holder { get; set; }

        /// <summary>
        /// 目标银行卡的开户银行
        /// </summary>
        [DataMember]
        public Bank Bank { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的提现明细
        /// </summary>
        /// <param name="record">提现明细的数据封装</param>
        public WithdrawDetailsResult(WithdrawalsRecord record)
        {
            this.WithdrawalsId = record.Id;
            this.Sum = record.Sum;
            this.Time = record.CreatedTime;
            this.Status = record.Status;
            this.Card = record.Card;
            this.Holder = record.Name;
            this.Bank = record.Bank;
            this.Remark = record.Remark;
        }

        #endregion
    }
}

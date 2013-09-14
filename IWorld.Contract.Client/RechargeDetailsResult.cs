using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 充值明细
    /// </summary>
    [DataContract]
    public class RechargeDetailsResult
    {
        #region 公开属性

        /// <summary>
        /// 充值记录的存储指针
        /// </summary>
        [DataMember]
        public int RechargeId { get; set; }

        /// <summary>
        /// 目标账户
        /// </summary>
        [DataMember]
        public string To { get; set; }

        /// <summary>
        /// 支付人
        /// </summary>
        [DataMember]
        public string From { get; set; }

        /// <summary>
        /// 充值金额
        /// </summary>
        [DataMember]
        public double Sum { get; set; }

        /// <summary>
        /// 充值时间
        /// </summary>
        [DataMember]
        public DateTime Time { get; set; }

        /// <summary>
        /// 处理状态
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

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的充值明细
        /// </summary>
        /// <param name="record">充值明细的数据封装</param>
        public RechargeDetailsResult(RechargeRecord record)
        {
            this.RechargeId = record.Id;
            this.Sum = record.Sum;
            this.Time = record.CreatedTime;
            this.Status = record.Status;
            this.To = record.Owner.Username;
            this.From = record.Payer.Username;
            this.Code = record.Code;
            this.Remark = record.Remark;
        }

        #endregion
    }
}

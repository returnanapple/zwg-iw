using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 充值结果
    /// </summary>
    [DataContract]
    public class RechargeResult : OperateResult
    {
        #region 公开属性

        /// <summary>
        /// 开户人
        /// </summary>
        [DataMember]
        public string Holder { get; set; }

        /// <summary>
        /// 银行卡
        /// </summary>
        [DataMember]
        public string Card { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        [DataMember]
        public Bank Bank { get; set; }

        /// <summary>
        /// 标识码
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的充值结果（成功）
        /// </summary>
        /// <param name="holder">开户人</param>
        /// <param name="card">银行卡</param>
        /// <param name="bank">银行</param>
        /// <param name="code">标识码</param>
        public RechargeResult(string holder, string card, Bank bank, string code)
        {
            this.Holder = holder;
            this.Card = card;
            this.Bank = bank;
            this.Code = code;
        }

        /// <summary>
        /// 实例化一个新的充值结果（失败）
        /// </summary>
        /// <param name="error">错误信息</param>
        public RechargeResult(string error)
            : base(error)
        {
        }

        #endregion
    }
}

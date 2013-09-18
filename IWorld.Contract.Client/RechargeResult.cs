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
        /// 开户人1
        /// </summary>
        [DataMember]
        public string Holder1 { get; set; }

        /// <summary>
        /// 银行卡1
        /// </summary>
        [DataMember]
        public string Card1 { get; set; }

        /// <summary>
        /// 银行1
        /// </summary>
        [DataMember]
        public Bank Bank1 { get; set; }

        /// <summary>
        /// 开户人2
        /// </summary>
        [DataMember]
        public string Holder2 { get; set; }

        /// <summary>
        /// 银行卡2
        /// </summary>
        [DataMember]
        public string Card2 { get; set; }

        /// <summary>
        /// 银行2
        /// </summary>
        [DataMember]
        public Bank Bank2 { get; set; }

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
        /// <param name="holder1">开户人1</param>
        /// <param name="card1">银行卡1</param>
        /// <param name="bank1">银行1</param>
        /// <param name="holder1">开户人2</param>
        /// <param name="card1">银行卡2</param>
        /// <param name="bank1">银行2</param>
        /// <param name="code">标识码</param>
        public RechargeResult(string holder1, string card1, Bank bank1, string holder2, string card2
            , Bank bank2, string code)
        {
            this.Holder1 = holder1;
            this.Card1 = card1;
            this.Bank1 = bank1;
            this.Holder2 = holder2;
            this.Card2 = card2;
            this.Bank2 = bank2;
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

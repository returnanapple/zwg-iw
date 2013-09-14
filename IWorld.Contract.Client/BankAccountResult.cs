using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 官方银行帐号信息
    /// </summary>
    [DataContract]
    public class BankAccountResult : OperateResult
    {
        /// <summary>
        /// 开户人
        /// </summary>
        [DataMember]
        public string Holder { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        [DataMember]
        public string Card { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        [DataMember]
        public Bank Bank { get; set; }

        /// <summary>
        /// 实例化一个新的官方银行帐号信息（操作成功）
        /// </summary>
        /// <param name="ba">官方银行帐号信息的数据封装</param>
        public BankAccountResult(BankAccount ba)
        {
            this.Holder = ba.Name;
            this.Card = ba.Card;
            this.Bank = ba.Bank;
        }

        /// <summary>
        /// 实例化一个新的官方银行帐号信息（操作失败）
        /// </summary>
        /// <param name="error">错误信息</param>
        public BankAccountResult(string error)
            : base(error)
        {
        }
    }
}

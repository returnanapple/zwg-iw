using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 银行帐号信息
    /// </summary>
    [DataContract]
    public class BankAccountResult
    {
        /// <summary>
        /// 银行帐号的存储指针
        /// </summary>
        [DataMember]
        public int BankAccountId { get; set; }

        /// <summary>
        /// 索引字
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// 开户人
        /// </summary>
        [DataMember]
        public string Name { get; set; }

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
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }

        /// <summary>
        /// 排列系数
        /// </summary>
        [DataMember]
        public int Order { get; set; }

        /// <summary>
        /// 一个布尔值 表示是否默认对象
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 实例化一个新的银行帐号信息
        /// </summary>
        /// <param name="bankAccount">银行帐号信息的数据封装</param>
        public BankAccountResult(BankAccount bankAccount)
        {
            this.BankAccountId = bankAccount.Id;
            this.Key = bankAccount.Key;
            this.Name = bankAccount.Name;
            this.Card = bankAccount.Card;
            this.Bank = bankAccount.Bank;
            this.Remark = bankAccount.Remark;
            this.Order = bankAccount.Order;
            this.IsDefault = bankAccount.IsDefault;
        }
    }
}

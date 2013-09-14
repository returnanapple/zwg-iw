using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于更新银行帐号信息的数据集
    /// </summary>
    [DataContract]
    public class EditBankAccountImport
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
    }
}

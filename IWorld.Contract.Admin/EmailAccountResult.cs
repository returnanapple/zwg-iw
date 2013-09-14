using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 系统邮件账户信息
    /// </summary>
    [DataContract]
    public class EmailAccountResult
    {
        /// <summary>
        /// 系统邮件账户的存储指针
        /// </summary>
        [DataMember]
        public int EmailAccountId { get; set; }

        /// <summary>
        /// 索引字
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        [DataMember]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }

        /// <summary>
        /// 应该使用的服务地址
        /// </summary>
        [DataMember]
        public EmailClientResult Client { get; set; }

        /// <summary>
        /// 一个布尔值 表示是否默认对象
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 实例化一个新的系统邮件账户信息
        /// </summary>
        /// <param name="emailAccount">系统邮件账户信息的数据封装</param>
        public EmailAccountResult(EmailAccount emailAccount)
        {
            this.EmailAccountId = emailAccount.Id;
            this.Key = emailAccount.Key;
            this.Account = emailAccount.Account;
            this.Password = emailAccount.Password;
            this.Remark = emailAccount.Remark;
            this.Client = new EmailClientResult(emailAccount.Client);
            this.IsDefault = emailAccount.IsDefault;
        }
    }
}

using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 系统邮件服务地址信息
    /// </summary>
    [DataContract]
    public class EmailClientResult
    {
        /// <summary>
        /// 系统邮件服务地址的存储指针
        /// </summary>
        [DataMember]
        public int EmailClientId { get; set; }

        /// <summary>
        /// 索引字
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// 服务器地址
        /// </summary>
        [DataMember]
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        [DataMember]
        public int Port { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }

        /// <summary>
        /// 一个布尔值 表示是否默认对象
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 实例化一个新的系统邮件服务地址信息
        /// </summary>
        /// <param name="emailClient">系统邮件服务地址信息的数据封装</param>
        public EmailClientResult(EmailClient emailClient)
        {
            this.EmailClientId = emailClient.Id;
            this.Key = emailClient.Key;
            this.Host = emailClient.Host;
            this.Port = emailClient.Port;
            this.Remark = emailClient.Remark;
            this.IsDefault = emailClient.IsDefault;
        }
    }
}

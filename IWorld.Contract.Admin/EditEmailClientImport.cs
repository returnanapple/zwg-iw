using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于更新系统邮件服务地址信息的数据集
    /// </summary>
    [DataContract]
    public class EditEmailClientImport
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
    }
}

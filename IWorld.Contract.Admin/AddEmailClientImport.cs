using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于新建系统邮件服务地址的数据集
    /// </summary>
    [DataContract]
    public class AddEmailClientImport
    {
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

﻿using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于更新系统邮件账户信息的数据集
    /// </summary>
    [DataContract]
    public class EditEmailAccountImport
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
        /// 应该使用的服务地址的存储指针
        /// </summary>
        [DataMember]
        public string ClientKey { get; set; }
    }
}

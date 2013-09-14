using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 用户绑定用户初始信息的数据集
    /// </summary>
    [DataContract]
    public class UserInfoBindingImport
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        [DataMember]
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [DataMember]
        public string NewPassword { get; set; }

        /// <summary>
        /// 安全码
        /// </summary>
        [DataMember]
        public string SafeCode { get; set; }

        /// <summary>
        /// 安全邮箱
        /// </summary>
        [DataMember]
        public string Email { get; set; }

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
    }
}

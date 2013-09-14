using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    [DataContract]
    public class CustomerMessageResult
    {
        /// <summary>
        /// 聊天信息内容
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [DataMember]
        public DateTime Time { get; set; }

        /// <summary>
        /// 是否客服回应的标识
        /// </summary>
        [DataMember]
        public bool IsService { get; set; }

        /// <summary>
        /// 客服类型
        /// </summary>
        [DataMember]
        public CustomerType Type { get; set; }
    }
}

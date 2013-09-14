using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于新建虚拟排行的数据集
    /// </summary>
    [DataContract]
    public class AddVirtualTopImport
    {
        /// <summary>
        /// 对应的彩票
        /// </summary>
        [DataMember]
        public string Ticket { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DataMember]
        public double Sum { get; set; }
    }
}

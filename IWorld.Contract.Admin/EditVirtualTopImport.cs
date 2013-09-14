using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于编辑虚拟排行信息的数据集
    /// </summary>
    [DataContract]
    public class EditVirtualTopImport
    {
        /// <summary>
        /// 所要修改的虚拟排行的存储指针
        /// </summary>
        [DataMember]
        public int VirtualTopId { get; set; }

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

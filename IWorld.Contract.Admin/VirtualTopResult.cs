using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 虚拟排行信息
    /// </summary>
    [DataContract]
    public class VirtualTopResult
    {
        #region 公开属性

        /// <summary>
        /// 虚拟排行的存储指针
        /// </summary>
        [DataMember]
        public int VirtualTopId { get; set; }

        /// <summary>
        /// 对应的彩票的存储指针
        /// </summary>
        [DataMember]
        public int TicketId { get; set; }

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

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的虚拟排行信息
        /// </summary>
        /// <param name="virtualTop">虚拟排行信息的数据封装</param>
        public VirtualTopResult(VirtualTop virtualTop)
        {
            this.VirtualTopId = virtualTop.Id;
            this.TicketId = virtualTop.Ticket.Id;
            this.Ticket = virtualTop.Ticket.Name;
            this.Sum = virtualTop.Sum;
        }

        #endregion
    }
}

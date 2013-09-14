using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于更新彩票信息的数据集
    /// </summary>
    [DataContract]
    public class EditTicketImport
    {
        /// <summary>
        /// 彩票的存储指针
        /// </summary>
        [DataMember]
        public int TicketId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 排序系数
        /// </summary>
        [DataMember]
        public int Order { get; set; }
    }
}

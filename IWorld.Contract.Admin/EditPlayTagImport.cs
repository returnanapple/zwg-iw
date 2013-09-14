using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于更新玩法标签信息的数据集
    /// </summary>
    [DataContract]
    public class EditPlayTagImport
    {
        /// <summary>
        /// 玩法标签的存储指针
        /// </summary>
        [DataMember]
        public int PlayTagId { get; set; }

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

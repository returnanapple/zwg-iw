using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于修改默认活动信息的数据集（基础）
    /// </summary>
    [DataContract]
    public class EditActivityImport_Basic
    {
        /// <summary>
        /// 默认活动的存储指针
        /// </summary>
        [DataMember]
        public int ActivityId { get; set; }

        /// <summary>
        /// 持续天数
        /// </summary>
        [DataMember]
        public int Days { get; set; }

        /// <summary>
        /// 暂停显示
        /// </summary>
        [DataMember]
        public bool Hide { get; set; }

        /// <summary>
        /// 过期自动删除
        /// </summary>
        [DataMember]
        public bool AutoDelete { get; set; }
    }
}

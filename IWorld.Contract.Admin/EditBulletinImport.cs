using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于修改公告信息的数据集
    /// </summary>
    [DataContract]
    public class EditBulletinImport
    {
        /// <summary>
        /// 公告的存储指针
        /// </summary>
        [DataMember]
        public int BulletinId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        [DataMember]
        public string Context { get; set; }

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

using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于新建用户组的数据集（基础）
    /// </summary>
    [DataContract]
    public class AddUserGroupImport_Basic
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [DataMember]
        public int Grade { get; set; }

        /// <summary>
        /// 消费量下限
        /// </summary>
        [DataMember]
        public double LimitOfConsumption { get; set; }

        /// <summary>
        /// 消费量上限
        /// </summary>
        [DataMember]
        public double UpperOfConsumption { get; set; }
    }
}

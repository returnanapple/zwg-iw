using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于编辑限制条件的数据集
    /// </summary>
    [DataContract]
    public class EditConditionImport
    {
        /// <summary>
        /// 类型
        /// </summary>
        [DataMember]
        public ConditionType Type { get; set; }

        /// <summary>
        /// 下限
        /// </summary>
        [DataMember]
        public double Limit { get; set; }

        /// <summary>
        /// 上限
        /// </summary>
        [DataMember]
        public double Upper { get; set; }
    }
}

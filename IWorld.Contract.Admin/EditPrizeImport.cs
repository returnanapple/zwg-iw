using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于编辑限制条件的数据集
    /// </summary>
    [DataContract]
    public class EditPrizeImport
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// 数额
        /// </summary>
        [DataMember]
        public int Sum { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DataMember]
        public PrizeType Type { get; set; }

        /// <summary>
        /// 价值
        /// </summary>
        [DataMember]
        public double Price { get; set; }

        /// <summary>
        /// 备注（一般为实物奖品的演示链接）
        /// </summary>
        [DataMember]
        public string Remark { get; set; }
    }
}

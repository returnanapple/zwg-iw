using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于更新玩法信息的数据集
    /// </summary>
    [DataContract]
    public class EditHowToPlayImport
    {
        /// <summary>
        /// 玩法的存储指针
        /// </summary>
        [DataMember]
        public int HowToPlayId { get; set; }
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
        /// 规则
        /// </summary>
        [DataMember]
        public string Rule { get; set; }

        /// <summary>
        /// 赔率
        /// </summary>
        [DataMember]
        public double Odds { get; set; }

        /// <summary>
        /// 排序系数
        /// </summary>
        [DataMember]
        public int Order { get; set; }
    }
}

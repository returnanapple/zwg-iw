using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于更新开奖时间的数据集
    /// </summary>
    [DataContract]
    public class EditLotteryTimeImport
    {
        /// <summary>
        /// 期数
        /// </summary>
        [DataMember]
        public int Phases { get; set; }

        /// <summary>
        /// 时间的值（“小时：分”格式）
        /// </summary>
        [DataMember]
        public string TimeValue { get; set; }
    }
}

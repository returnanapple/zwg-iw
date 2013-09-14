using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 开奖时间信息
    /// </summary>
    [DataContract]
    public class LotteryTimeResult
    {
        /// <summary>
        /// 开奖时间的存储指针
        /// </summary>
        [DataMember]
        public int LotteryTimeId { get; set; }

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

        /// <summary>
        /// 实例化一个新的开奖时间信息
        /// </summary>
        /// <param name="time">开奖时间信息的数据封装</param>
        public LotteryTimeResult(LotteryTime time)
        {
            this.LotteryTimeId = time.Id;
            this.Phases = time.Phases;
            this.TimeValue = time.TimeValue;
        }
    }
}

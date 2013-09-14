using System;
using System.Linq;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 兑换活动参与记录信息
    /// </summary>
    [DataContract]
    public class ExchangeParticipateRecordResult
    {
        /// <summary>
        /// 兑换活动参与记录的存储指针
        /// </summary>
        [DataMember]
        public int ExchangeParticipateRecordId { get; set; }

        /// <summary>
        /// 参与人的存储指针
        /// </summary>
        [DataMember]
        public int OwnerId { get; set; }

        /// <summary>
        /// 参与人
        /// </summary>
        [DataMember]
        public string OwnerName { get; set; }

        /// <summary>
        /// 参与的活动的存储指针
        /// </summary>
        [DataMember]
        public int ExchangeId { get; set; }

        /// <summary>
        /// 参与的活动
        /// </summary>
        [DataMember]
        public string ExchangeName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [DataMember]
        public int Sum { get; set; }

        /// <summary>
        /// 总资金奖励
        /// </summary>
        [DataMember]
        public double RewardOfMoney { get; set; }

        /// <summary>
        /// 总积分奖励
        /// </summary>
        [DataMember]
        public double RewardOfIntegral { get; set; }

        /// <summary>
        /// 实物奖励数量
        /// </summary>
        [DataMember]
        public int CountOfGift { get; set; }

        /// <summary>
        /// 参与时间
        /// </summary>
        [DataMember]
        public DateTime ParticipatedTime { get; set; }

        /// <summary>
        /// 实例化一个新的兑换活动参与记录信息
        /// </summary>
        /// <param name="record">兑换活动参与记录信息的数据封装</param>
        public ExchangeParticipateRecordResult(ExchangeParticipateRecord record)
        {
            this.ExchangeParticipateRecordId = record.Id;
            this.OwnerId = record.Owner.Id;
            this.OwnerName = record.Owner.Username;
            this.ExchangeId = record.Exchange.Id;
            this.ExchangeName = record.Exchange.Name;
            this.Sum = record.Sum;
            this.RewardOfMoney = record.Exchange.Prizes.Where(x => x.Type == PrizeType.人民币).Sum(x => x.Price);
            this.RewardOfIntegral = record.Exchange.Prizes.Where(x => x.Type == PrizeType.积分).Sum(x => x.Price);
            this.CountOfGift = record.Exchange.Prizes.Count(x => x.Type == PrizeType.实物);
            this.ParticipatedTime = record.CreatedTime;
        }
    }
}

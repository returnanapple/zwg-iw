using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 奖品信息
    /// </summary>
    [DataContract]
    public class PrizeResult
    {
        /// <summary>
        /// 奖品的存储指针
        /// </summary>
        [DataMember]
        public int PrizeId { get; set; }

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

        /// <summary>
        /// 实例化一个新的奖品信息
        /// </summary>
        /// <param name="prize">奖品信息的数据封装</param>
        public PrizeResult(Prize prize)
        {
            this.PrizeId = prize.Id;
            this.Name = prize.Name;
            this.Description = prize.Description;
            this.Sum = prize.Sum;
            this.Type = prize.Type;
            this.Price = prize.Price;
            this.Remark = prize.Remark;
        }
    }
}

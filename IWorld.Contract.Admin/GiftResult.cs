using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 实体奖品信息
    /// </summary>
    [DataContract]
    public class GiftResult
    {
        /// <summary>
        /// 实体奖品的存储指针
        /// </summary>
        [DataMember]
        public int GiftId { get; set; }

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
        /// 状态
        /// </summary>
        [DataMember]
        public GiftStatus Status { get; set; }

        /// <summary>
        /// 实例化一个新的实体奖品信息
        /// </summary>
        /// <param name="gift">实体奖品信息的数据封装</param>
        public GiftResult(GiftRecord gift)
        {
            this.GiftId = gift.Id;
            this.Name = gift.Name;
            this.Description = gift.Description;
            this.Sum = gift.Sum;
            this.Type = gift.Type;
            this.Price = gift.Price;
            this.Remark = gift.Remark;
            this.Status = gift.Status;
        }
    }
}


namespace IWorld.Model
{
    /// <summary>
    /// 实体奖品赠送记录
    /// </summary>
    public class GiftRecord : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 对应的活动
        /// </summary>
        public virtual Exchange Exchange { get; set; }

        /// <summary>
        /// 对应的用户
        /// </summary>
        public virtual Author Owner { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 数额
        /// </summary>
        public int Sum { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public PrizeType Type { get; set; }

        /// <summary>
        /// 价值
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 备注（一般为实物奖品的演示链接）
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public GiftStatus Status { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的实体奖品赠送记录
        /// </summary>
        public GiftRecord()
        {
        }

        /// <summary>
        /// 实例化一个新的实体奖品赠送记录
        /// </summary>
        /// <param name="exchange">对应的活动</param>
        /// <param name="owner">对应的用户</param>
        /// <param name="name">名称</param>
        /// <param name="description">描述</param>
        /// <param name="sum">数额</param>
        /// <param name="type">类型</param>
        /// <param name="price">价值</param>
        /// <param name="remark">备注</param>
        public GiftRecord(Exchange exchange, Author owner, string name, string description, int sum, PrizeType type, double price, string remark)
        {
            this.Exchange = exchange;
            this.Owner = owner;
            this.Name = name;
            this.Description = description;
            this.Sum = sum;
            this.Type = type;
            this.Price = price;
            this.Remark = remark;
            this.Status = GiftStatus.未赠送;
        }

        #endregion
    }
}

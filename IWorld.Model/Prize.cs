
namespace IWorld.Model
{
    /// <summary>
    /// 奖品
    /// </summary>
    public class Prize : ModelBase
    {
        #region 公开属性

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

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的奖品
        /// </summary>
        public Prize()
        {
        }

        /// <summary>
        /// 实例化一个新的奖品
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="description">描述</param>
        /// <param name="sum">数额</param>
        /// <param name="type">类型</param>
        /// <param name="price">价值</param>
        /// <param name="remark">备注</param>
        public Prize(string name, string description, int sum, PrizeType type, double price, string remark)
        {
            this.Name = name;
            this.Description = description;
            this.Sum = sum;
            this.Type = type;
            this.Price = price;
            this.Remark = remark;
        }

        #endregion
    }
}

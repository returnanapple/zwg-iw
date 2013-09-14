
namespace IWorld.Model
{
    /// <summary>
    /// 位（开奖记录）
    /// </summary>
    public class LotterySeat : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 对应的号码
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 排序系数（继承于对应彩种的同名系数）
        /// </summary>
        public int Order { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的位（开奖记录）
        /// </summary>
        public LotterySeat()
        {
        }

        /// <summary>
        /// 实例化一个新的位（开奖记录）
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">对应的号码</param>
        /// <param name="order">排序系数（应继承于对应彩种的同名系数）</param>
        public LotterySeat(string name, string value, int order)
        {
            this.Name = name;
            this.Value = value;
            this.Order = order;
        }

        #endregion
    }
}

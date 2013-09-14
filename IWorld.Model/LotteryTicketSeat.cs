using System.Collections.Generic;
using System.Linq;

namespace IWorld.Model
{
    /// <summary>
    /// 位（彩票）
    /// </summary>
    public class LotteryTicketSeat : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表示该位是否特殊的位
        /// </summary>
        public bool IsSpecial { get; set; }

        /// <summary>
        /// 所属的号码的集合
        /// </summary>
        public string Values { get; set; }

        /// <summary>
        /// 所属的号码的集合的列表
        /// </summary>
        public List<string> ValueList
        {
            get { return this.Values.Split(new char[] { ',' }).ToList(); }
        }

        /// <summary>
        /// 排序系数
        /// </summary>
        public int Order { get; set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 实例化一个新的位（彩票）
        /// </summary>
        public LotteryTicketSeat()
        {
        }

        /// <summary>
        /// 实例化一个新的位（彩票）
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="isSpecial">表示该位是否特殊的位</param>
        /// <param name="values">所属的号码的集合</param>
        /// <param name="order">排序系数</param>
        public LotteryTicketSeat(string name, bool isSpecial, string values, int order)
        {
            this.Name = name;
            this.IsSpecial = isSpecial;
            this.Values = values;
            this.Order = order;
        }

        #endregion
    }
}

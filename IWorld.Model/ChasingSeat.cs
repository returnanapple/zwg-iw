using System.Collections.Generic;
using System.Linq;

namespace IWorld.Model
{
    /// <summary>
    /// 位（追号记录）
    /// </summary>
    public class ChasingSeat : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 对应的号码的集合
        /// </summary>
        public string Values { get; set; }

        /// <summary>
        /// 对应的号码的集合的列表（只读）
        /// </summary>
        public List<string> ValuesList
        {
            get { return this.Values.Split(new char[] { ',' }).ToList(); }
        }

        /// <summary>
        /// 排序系数（继承于对应彩种的同名系数）
        /// </summary>
        public int Order { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的位（追号记录）
        /// </summary>
        public ChasingSeat()
        {
        }

        /// <summary>
        /// 实例化一个新的位（追号记录）
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="values">对应的号码的集合</param>
        /// <param name="order">排序系数（应继承于对应彩种的同名系数）</param>
        public ChasingSeat(string name, string values, int order)
        {
            this.Name = name;
            this.Values = values;
            this.Order = order;
        }

        #endregion
    }
}

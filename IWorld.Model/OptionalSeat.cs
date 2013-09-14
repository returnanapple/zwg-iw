using System.Collections.Generic;
using System.Linq;

namespace IWorld.Model
{
    /// <summary>
    /// 可选位
    /// </summary>
    public class OptionalSeat : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 必选
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 对应的号码集合
        /// </summary>
        public string Values { get; set; }

        /// <summary>
        /// 对应的号码的集合的列表（只读）
        /// </summary>
        public List<string> ValueList
        {
            get { return this.Values.Split(new char[] { ',' }).ToList(); }
        }

        /// <summary>
        /// “大”选项所选号码集合
        /// </summary>
        public string ValuesForLarge { get; set; }

        /// <summary>
        /// “大”选项所选号码集合的列表（只读）
        /// </summary>
        public List<string> ValuesForLargeList
        {
            get { return this.ValuesForLarge.Split(new char[] { ',' }).ToList(); }
        }

        /// <summary>
        /// “小”选项所选号码集合
        /// </summary>
        public string ValuesForSmall { get; set; }

        /// <summary>
        /// “小”选项所选号码集合的列表（只读）
        /// </summary>
        public List<string> ValuesForSmallList
        {
            get { return this.ValuesForSmall.Split(new char[] { ',' }).ToList(); }
        }

        /// <summary>
        /// “单”选项所选号码集合
        /// </summary>
        public string ValuesForSingle { get; set; }

        /// <summary>
        /// “单”选项所选号码集合的列表（只读）
        /// </summary>
        public List<string> ValuesForSingleList
        {
            get { return this.ValuesForSingle.Split(new char[] { ',' }).ToList(); }
        }

        /// <summary>
        /// “双”选项所选号码集合
        /// </summary>
        public string ValuesForDouble { get; set; }

        /// <summary>
        /// “双”选项所选号码集合的列表（只读）
        /// </summary>
        public List<string> ValuesForDoubleList
        {
            get { return this.ValuesForDouble.Split(new char[] { ',' }).ToList(); }
        }

        /// <summary>
        /// 排序系数（继承于对应彩种的同名系数）
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 最少选号
        /// </summary>
        public int LimitOfPick { get; set; }

        /// <summary>
        /// 最多选号
        /// </summary>
        public int UpperOfPick { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的可选位
        /// </summary>
        public OptionalSeat()
        {
        }

        /// <summary>
        /// 实例化一个新的可选位
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="isRequired">必选</param>
        /// <param name="values">对应的号码集合</param>
        /// <param name="valuesForLarge">“大”选项所选号码集合</param>
        /// <param name="valuesForSmall">“小”选项所选号码集合</param>
        /// <param name="valuesForSingle">“单”选项所选号码集合</param>
        /// <param name="valuesForDouble">“双”选项所选号码集合</param>
        /// <param name="order">排序系数（应继承于对应彩种的同名系数）</param>
        /// <param name="limitOfPick">最少选号</param>
        /// <param name="upperOfPick">最多选号</param>
        public OptionalSeat(string name, bool isRequired, string values, string valuesForLarge, string valuesForSmall
            , string valuesForSingle, string valuesForDouble, int order, int limitOfPick, int upperOfPick)
        {
            this.Name = name;
            this.IsRequired = isRequired;
            this.Values = values;
            this.ValuesForLarge = valuesForLarge;
            this.ValuesForSmall = valuesForSmall;
            this.ValuesForSingle = valuesForSingle;
            this.ValuesForDouble = valuesForDouble;
            this.Order = order;
            this.LimitOfPick = limitOfPick;
            this.UpperOfPick = upperOfPick;
        }

        #endregion
    }
}

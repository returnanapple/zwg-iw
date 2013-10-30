using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 大鲨鱼游戏的标记
    /// </summary>
    public class MarkOfJaw : ModelBase
    {
        #region 属性

        /// <summary>
        /// 图标
        /// </summary>
        public IconOfJaw Icon { get; set; }

        /// <summary>
        /// 触发号码
        /// </summary>
        public string TouchOff { get; set; }

        /// <summary>
        /// 触发号码的集合
        /// </summary>
        public List<int> TouchOffList
        {
            get
            {
                return TouchOff.Split(new char[] { ',' }).ToList().ConvertAll(x => Convert.ToInt32(x));
            }
        }

        /// <summary>
        /// 开奖号码
        /// </summary>
        public string OpenUp { get; set; }

        /// <summary>
        /// 开奖号码的集合
        /// </summary>
        public List<int> OpenUpList
        {
            get
            {
                return OpenUp.Split(new char[] { ',' }).ToList().ConvertAll(x => Convert.ToInt32(x));
            }
        }

        /// <summary>
        /// 触发概率
        /// </summary>
        public int Probability { get; set; }

        /// <summary>
        /// 赔率
        /// </summary>
        public double Odds { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的大鲨鱼游戏的标记
        /// </summary>
        public MarkOfJaw()
        {
        }

        /// <summary>
        /// 实例化一个新的大鲨鱼游戏的标记
        /// </summary>
        /// <param name="icon">图标</param>
        /// <param name="touchOff">触发号码</param>
        /// <param name="openUp">开奖号码</param>
        /// <param name="probability">触发概率</param>
        /// <param name="odds">赔率</param>
        public MarkOfJaw(IconOfJaw icon, string touchOff, string openUp, int probability, double odds)
        {
            this.Icon = icon;
            this.TouchOff = touchOff;
            this.OpenUp = openUp;
            this.Probability = probability;
            this.Odds = odds;
        }

        #endregion
    }
}

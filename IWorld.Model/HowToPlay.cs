using System.Collections.Generic;

namespace IWorld.Model
{
    /// <summary>
    /// 玩法
    /// </summary>
    public class HowToPlay : ModelBase
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
        /// 规则
        /// </summary>
        public string Rule { get; set; }

        /// <summary>
        /// 玩法标签
        /// </summary>
        public virtual PlayTag Tag { get; set; }

        /// <summary>
        /// 选位下限
        /// </summary>
        public int LowerSeats { get; set; }

        /// <summary>
        /// 选位上限
        /// </summary>
        public int UpperSeats { get; set; }

        /// <summary>
        /// 赔率
        /// </summary>
        public double Odds { get; set; }

        /// <summary>
        /// 赔率/返点数转化率（如为0则采用系统参数）
        /// </summary>
        public double ConversionRates { get; set; }

        /// <summary>
        /// 返奖基数（如为0则采用系统参数）
        /// </summary>
        public double CardinalNumber { get; set; }

        /// <summary>
        /// 所采用的返奖接口
        /// </summary>
        public LotteryInterface Interface { get; set; }

        /// <summary>
        /// 叠位
        /// </summary>
        public bool IsStackedBit { get; set; }

        /// <summary>
        /// 允许自选位
        /// </summary>
        public bool AllowFreeSeats { get; set; }

        /// <summary>
        /// 可选位
        /// </summary>
        public virtual List<OptionalSeat> Seats { get; set; }

        /// <summary>
        /// 可选参数1
        /// </summary>
        public int Parameter1 { get; set; }

        /// <summary>
        /// 可选参数2
        /// </summary>
        public int Parameter2 { get; set; }

        /// <summary>
        /// 可选参数3
        /// </summary>
        public int Parameter3 { get; set; }

        /// <summary>
        /// 标识 | 不在前台显示
        /// </summary>
        public bool Hide { get; set; }

        /// <summary>
        /// 排序系数
        /// </summary>
        public int Order { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的玩法
        /// </summary>
        public HowToPlay()
        {
        }

        /// <summary>
        /// 实例化一个新的玩法
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="description">描述</param>
        /// <param name="rule">规则</param>
        /// <param name="tag">玩法标签</param>
        /// <param name="lowerSeats">选位下限</param>
        /// <param name="upperSeats">选位上限</param>
        /// <param name="odds">赔率（如为0则采用系统参数）</param>
        /// <param name="conversionRates">赔率/返点数转化率（如为0则采用系统参数）</param>
        /// <param name="cardinalNumber">返奖基数（如为0则采用系统参数）</param>
        /// <param name="_interface">所采用的返奖接口</param>
        /// <param name="isStackedBit">叠位</param>
        /// <param name="allowFreeSeats">允许自选位</param>
        /// <param name="seats">可选位</param>
        /// <param name="parameter1">可选参数1</param>
        /// <param name="parameter2">可选参数2</param>
        /// <param name="parameter3">可选参数3</param>
        /// <param name="order">排序系数</param>
        public HowToPlay(string name, string description, string rule, PlayTag tag, int lowerSeats, int upperSeats
            , double odds, double conversionRates, double cardinalNumber, LotteryInterface _interface, bool isStackedBit
            , bool allowFreeSeats, List<OptionalSeat> seats, int parameter1, int parameter2, int parameter3, int order)
        {
            this.Name = name;
            this.Description = description;
            this.Rule = rule;
            this.Tag = tag;
            this.LowerSeats = lowerSeats;
            this.UpperSeats = upperSeats;
            this.Odds = odds;
            this.ConversionRates = conversionRates;
            this.CardinalNumber = cardinalNumber;
            this.Interface = _interface;
            this.IsStackedBit = isStackedBit;
            this.AllowFreeSeats = allowFreeSeats;
            this.Seats = seats;
            this.Parameter1 = parameter1;
            this.Parameter2 = parameter2;
            this.Parameter3 = parameter3;
            this.Hide = false;
            this.Order = order;

            tag.HowToPlays.Add(this);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 大白鲨游戏的主要信息
    /// </summary>
    public class MainOfJaw : ModelBase
    {
        #region 属性

        /// <summary>
        /// 下一期
        /// </summary>
        public string NextPhases { get; set; }

        /// <summary>
        /// 下一期开奖时间
        /// </summary>
        public DateTime NextLotteryTime
        {
            get
            {
                char[] t = this.NextPhases.ToArray();
                int count = t.Count();
                string p = "";
                for (int i = count - 3; i < count; i++)
                {
                    p += t[i].ToString();
                }
                int phases = Convert.ToInt32(p);

                int _p = Convert.ToInt32(phases);
                DateTime _time = this.Times.FirstOrDefault(x => x.Phases == _p).Time;
                if (_time == this.Times.Min(x => x.Time)
                    && DateTime.Now > this.Times.Max(x => x.Time))
                {
                    _time.AddDays(1);
                }
                return _time;
            }
        }

        /// <summary>
        /// 开奖时间
        /// </summary>
        public virtual List<LotteryTimeOfJaw> Times { get; set; }

        #endregion

        #region 属性

        /// <summary>
        /// 实例化一个新的大白鲨游戏的主要信息
        /// </summary>
        public MainOfJaw()
        {
        }

        /// <summary>
        /// 实例化一个新的大白鲨游戏的主要信息
        /// </summary>
        /// <param name="times">开奖时间</param>
        public MainOfJaw(List<LotteryTimeOfJaw> times)
        {
            this.NextPhases = "130101001";
            this.Times = times;
        }

        #endregion
    }
}

﻿using System;

namespace IWorld.Model
{
    /// <summary>
    /// 开奖时间
    /// </summary>
    public class LotteryTime : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 所从属的彩票的存储指针
        /// </summary>
        public int TicketId { get; set; }

        /// <summary>
        /// 期数
        /// </summary>
        public int Phases { get; set; }

        /// <summary>
        /// 时间的值（“小时：分”格式）
        /// </summary>
        public string TimeValue { get; set; }

        /// <summary>
        /// 时间（本日）
        /// </summary>
        public DateTime Time
        {
            get
            {
                string[] t = TimeValue.Split(new char[] { ':', '：' });
                int tHour = Convert.ToInt32(t[0]);
                int tMinute = Convert.ToInt32(t[1]);
                return new DateTime(DateTime.Now.Year
                    , DateTime.Now.Month
                    , DateTime.Now.Day
                    , tHour
                    , tMinute
                    , 0);
            }
        }
        
        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的开奖时间
        /// </summary>
        public LotteryTime()
        {
        }

        /// <summary>
        /// 实例化一个新的开奖时间
        /// </summary>
        /// <param name="phases">期数</param>
        /// <param name="timeValue">时间的值（“小时：分”格式）</param>
        public LotteryTime(int phases, string timeValue)
        {
            this.Phases = phases;
            this.TimeValue = timeValue;
        }
        
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace IWorld.Model
{
    /// <summary>
    /// 站点统计信息（日）
    /// </summary>
    public class SiteDataAtDay : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 日
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// 登陆人数
        /// </summary>
        public int CountOfSetIn { get; set; }

        /// <summary>
        /// 注册人数
        /// </summary>
        public int CountOfSetUp { get; set; }

        /// <summary>
        /// 投注额
        /// </summary>
        public double AmountOfBets { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public double Bonus { get; set; }

        /// <summary>
        /// 返点
        /// </summary>
        public double ReturnPoints { get; set; }

        /// <summary>
        /// 活动返还
        /// </summary>
        public double Expenditures { get; set; }

        /// <summary>
        /// 盈亏
        /// </summary>
        public double GainsAndLosses
        {
            get { return this.AmountOfBets - this.ReturnPoints - this.Bonus - this.Expenditures; }
        }

        /// <summary>
        /// 充值
        /// </summary>
        public double Recharge { get; set; }

        /// <summary>
        /// 提现
        /// </summary>
        public double Withdrawal { get; set; }

        /// <summary>
        /// 支取
        /// </summary>
        public double Transfer { get; set; }

        /// <summary>
        /// 现金流
        /// </summary>
        public double Cash
        {
            get { return this.Recharge - this.Withdrawal - this.Transfer; }
        }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的站点统计信息（日）
        /// </summary>
        public SiteDataAtDay()
        {
        }

        /// <summary>
        /// 实例化一个新的站点统计信息（日）
        /// </summary>
        /// <param name="now">当前时间</param>
        public SiteDataAtDay(DateTime now)
        {
            this.Year = now.Year;
            this.Month = now.Month;
            this.Day = now.Day;
            List<string> ignore = new List<string> { "Id", "CreatedTime", "ModifiedTime", "Year", "Month", "Day" };
            typeof(SiteDataAtDay).GetProperties().ToList()
                .ForEach(x =>
                    {
                        if (!ignore.Contains(x.Name) && x.CanWrite)
                        {
                            x.SetValue(this, 0);
                        }
                    });
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace IWorld.Model
{
    /// <summary>
    /// 个人信息统计（月）
    /// </summary>
    public class PersonalDataAtMonth : ModelBase
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
        /// 用户
        /// </summary>
        public virtual Author Owner { get; set; }

        /// <summary>
        /// 下辖用户
        /// </summary>
        public int Subordinate { get; set; }

        /// <summary>
        /// 帐户余额
        /// </summary>
        public double Money { get; set; }

        /// <summary>
        /// 投注额
        /// </summary>
        public double AmountOfBets { get; set; }

        /// <summary>
        /// 返点
        /// </summary>
        public double ReturnPoints { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public double Bonus { get; set; }

        /// <summary>
        /// 活动返还
        /// </summary>
        public double Expenditures { get; set; }

        /// <summary>
        /// 盈亏
        /// </summary>
        public double GainsAndLosses
        {
            get { return this.ReturnPoints + this.Bonus + this.Expenditures - this.AmountOfBets; }
        }

        /// <summary>
        /// 充值
        /// </summary>
        public double Recharge { get; set; }

        /// <summary>
        /// 提现
        /// </summary>
        public double Withdrawal { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的个人信息统计（月）
        /// </summary>
        public PersonalDataAtMonth()
        {
        }

        /// <summary>
        /// 实例化一个新的个人信息统计（月）
        /// </summary>
        /// <param name="owner">用户</param>
        public PersonalDataAtMonth(Author owner)
        {
            this.Year = DateTime.Now.Year;
            this.Month = DateTime.Now.Month;
            this.Owner = owner;
            this.Money = owner.Money;
            List<string> ignore = new List<string> { "Id", "CreatedTime", "ModifiedTime", "Year", "Month", "Owner", "Money" };
            typeof(PersonalDataAtMonth).GetProperties().ToList()
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

using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 个人信息统计
    /// </summary>
    [DataContract]
    public class PersonalDataResult
    {
        /// <summary>
        /// 目标用户的存储指针
        /// </summary>
        [DataMember]
        public int OwnerId { get; set; }

        /// <summary>
        /// 目标用户
        /// </summary>
        [DataMember]
        public string Owner { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [DataMember]
        public int Layer { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [DataMember]
        public string Date { get; set; }

        /// <summary>
        /// 下辖用户
        /// </summary>
        [DataMember]
        public int Subordinate { get; set; }

        /// <summary>
        /// 帐户余额
        /// </summary>
        [DataMember]
        public double Money { get; set; }

        /// <summary>
        /// 投注额
        /// </summary>
        [DataMember]
        public double AmountOfBets { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        [DataMember]
        public double Bonus { get; set; }

        /// <summary>
        /// 活动返还
        /// </summary>
        [DataMember]
        public double Expenditures { get; set; }

        /// <summary>
        /// 盈亏
        /// </summary>
        [DataMember]
        public double GainsAndLosses { get; set; }

        /// <summary>
        /// 充值
        /// </summary>
        [DataMember]
        public double Recharge { get; set; }

        /// <summary>
        /// 提现
        /// </summary>
        [DataMember]
        public double Withdrawal { get; set; }

        /// <summary>
        /// 实例化一个新的个人信息统计
        /// </summary>
        /// <param name="personalDataAtDay">个人信息统计的数据封装</param>
        public PersonalDataResult(PersonalDataAtDay personalDataAtDay)
        {
            this.Date = string.Format("{0}-{1}-{2}", personalDataAtDay.Year, personalDataAtDay.Month, personalDataAtDay.Day);
            this.Owner = personalDataAtDay.Owner.Username;
            this.OwnerId = personalDataAtDay.Owner.Id;
            this.Layer = personalDataAtDay.Owner.Layer;
            this.Subordinate = personalDataAtDay.Subordinate;
            this.Money = personalDataAtDay.Money;
            this.AmountOfBets = personalDataAtDay.AmountOfBets;
            this.Bonus = personalDataAtDay.Bonus;
            this.Expenditures = personalDataAtDay.Expenditures;
            this.GainsAndLosses = personalDataAtDay.GainsAndLosses;
            this.Recharge = personalDataAtDay.Recharge;
            this.Withdrawal = personalDataAtDay.Withdrawal;
        }

        /// <summary>
        /// 实例化一个新的个人信息统计
        /// </summary>
        /// <param name="personalDataAtDay">个人信息统计的数据封装</param>
        public PersonalDataResult(PersonalDataAtMonth personalDataAtMonth)
        {
            this.Date = string.Format("{0}-{1}", personalDataAtMonth.Year, personalDataAtMonth.Month);
            this.Owner = personalDataAtMonth.Owner.Username;
            this.OwnerId = personalDataAtMonth.Owner.Id;
            this.Layer = personalDataAtMonth.Owner.Layer;
            this.Subordinate = personalDataAtMonth.Subordinate;
            this.Money = personalDataAtMonth.Money;
            this.AmountOfBets = personalDataAtMonth.AmountOfBets;
            this.Bonus = personalDataAtMonth.Bonus;
            this.Expenditures = personalDataAtMonth.Expenditures;
            this.GainsAndLosses = personalDataAtMonth.GainsAndLosses;
            this.Recharge = personalDataAtMonth.Recharge;
            this.Withdrawal = personalDataAtMonth.Withdrawal;
        }
    }
}

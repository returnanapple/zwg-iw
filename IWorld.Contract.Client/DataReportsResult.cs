using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 报表数据
    /// </summary>
    [DataContract]
    public class DataReportsResult
    {
        #region 公开属性

        /// <summary>
        /// 用户的数据库存储指针
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [DataMember]
        public string User { get; set; }

        /// <summary>
        /// 投注
        /// </summary>
        [DataMember]
        public double Bet { get; set; }

        /// <summary>
        /// 提款
        /// </summary>
        [DataMember]
        public double Withdrawal { get; set; }

        /// <summary>
        /// 充值
        /// </summary>
        [DataMember]
        public double Recharge { get; set; }

        /// <summary>
        /// 返点
        /// </summary>
        [DataMember]
        public double ReturnPoints { get; set; }

        /// <summary>
        /// 中奖
        /// </summary>
        [DataMember]
        public double Bonus { get; set; }

        /// <summary>
        /// 活动返还
        /// </summary>
        [DataMember]
        public double Expenditures { get; set; }

        /// <summary>
        /// 盈利
        /// </summary>
        [DataMember]
        public double Profit { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>
        [DataMember]
        public double Balance { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的报表数据
        /// </summary>
        /// <param name="data">报表数据的数据封装</param>
        public DataReportsResult(PersonalDataAtDay data)
        {
            this.UserId = data.Owner.Id;
            this.User = data.Owner.Username;
            this.Bet = data.AmountOfBets;
            this.Withdrawal = data.Withdrawal;
            this.Recharge = data.Recharge;
            this.ReturnPoints = data.ReturnPoints;
            this.Bonus = data.Bonus;
            this.Expenditures = data.Expenditures;
            this.Profit = data.GainsAndLosses;
            this.Balance = data.Money;
        }

        /// <summary>
        /// 实例化一个新的报表数据
        /// </summary>
        /// <param name="data">报表数据的数据封装</param>
        public DataReportsResult(PersonalDataAtMonth data)
        {
            this.UserId = data.Owner.Id;
            this.User = data.Owner.Username;
            this.Bet = data.AmountOfBets;
            this.Withdrawal = data.Withdrawal;
            this.Recharge = data.Recharge;
            this.ReturnPoints = data.ReturnPoints;
            this.Bonus = data.Bonus;
            this.Expenditures = data.Expenditures;
            this.Profit = data.GainsAndLosses;
            this.Balance = data.Money;
        }

        /// <summary>
        /// 实例化一个新的报表数据
        /// </summary>
        /// <param name="data">报表数据的数据封装</param>
        public DataReportsResult(List<PersonalDataAtDay> data)
        {
            this.UserId = data.FirstOrDefault().Owner.Id;
            this.User = data.FirstOrDefault().Owner.Username;
            this.Bet = data.Sum(x => x.AmountOfBets);
            this.Withdrawal = data.Sum(x => x.Withdrawal);
            this.Recharge = data.Sum(x => x.Recharge);
            this.ReturnPoints = data.Sum(x => x.ReturnPoints);
            this.Bonus = data.Sum(x => x.Bonus);
            this.Expenditures = data.Sum(x => x.Expenditures);
            this.Profit = data.Sum(x => x.GainsAndLosses);
            this.Balance = data.Sum(x => x.Money);
        }

        /// <summary>
        /// 实例化一个新的报表数据
        /// </summary>
        /// <param name="data">报表数据的数据封装</param>
        public DataReportsResult(List<PersonalDataAtMonth> data)
        {
            this.UserId = data.FirstOrDefault().Owner.Id;
            this.User = data.FirstOrDefault().Owner.Username;
            this.Bet = data.Sum(x => x.AmountOfBets);
            this.Withdrawal = data.Sum(x => x.Withdrawal);
            this.Recharge = data.Sum(x => x.Recharge);
            this.ReturnPoints = data.Sum(x => x.ReturnPoints);
            this.Bonus = data.Sum(x => x.Bonus);
            this.Expenditures = data.Sum(x => x.Expenditures);
            this.Profit = data.Sum(x => x.GainsAndLosses);
            this.Balance = data.Sum(x => x.Money);
        }

        #endregion
    }
}

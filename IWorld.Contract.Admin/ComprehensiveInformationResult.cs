using System.Collections.Generic;
using System.Runtime.Serialization;
using IWorld.Model;
using IWorld.Setting;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 综合信息统计
    /// </summary>
    [DataContract]
    public class ComprehensiveInformationResult : OperateResult
    {
        /// <summary>
        /// 注册用户数（日）
        /// </summary>
        [DataMember]
        public double SetUpAtDay { get; set; }

        /// <summary>
        /// 注册用户数（月）
        /// </summary>
        [DataMember]
        public double SetUpAtMonth { get; set; }

        /// <summary>
        /// 注册用户数（总）
        /// </summary>
        [DataMember]
        public double SetUpAtAll { get; set; }

        /// <summary>
        /// 登陆用户数（日）
        /// </summary>
        [DataMember]
        public double SetInAtDay { get; set; }

        /// <summary>
        /// 登陆用户数（月）
        /// </summary>
        [DataMember]
        public double SetInAtMonth { get; set; }

        /// <summary>
        /// 登陆用户数（总）
        /// </summary>
        [DataMember]
        public double SetInAtAll { get; set; }

        /// <summary>
        /// 投注额（日）
        /// </summary>
        [DataMember]
        public double AmountOfBetsAtDay { get; set; }

        /// <summary>
        /// 投注额（月）
        /// </summary>
        [DataMember]
        public double AmountOfBetsAtMonth { get; set; }

        /// <summary>
        /// 投注额（总）
        /// </summary>
        [DataMember]
        public double AmountOfBetsAtAll { get; set; }

        /// <summary>
        /// 返点（日）
        /// </summary>
        [DataMember]
        public double RebateAtDay { get; set; }

        /// <summary>
        /// 返点（月）
        /// </summary>
        [DataMember]
        public double RebateAtMonth { get; set; }

        /// <summary>
        /// 返点（总）
        /// </summary>
        [DataMember]
        public double RebateAtAll { get; set; }

        /// <summary>
        /// 奖金（日）
        /// </summary>
        [DataMember]
        public double BonusAtDay { get; set; }

        /// <summary>
        /// 奖金（月）
        /// </summary>
        [DataMember]
        public double BonusAtMonth { get; set; }

        /// <summary>
        /// 奖金（总）
        /// </summary>
        [DataMember]
        public double BonusAtAll { get; set; }

        /// <summary>
        /// 活动返还（日）
        /// </summary>
        [DataMember]
        public double ExpendituresAtDay { get; set; }

        /// <summary>
        /// 活动返还（月）
        /// </summary>
        [DataMember]
        public double ExpendituresAtMonth { get; set; }

        /// <summary>
        /// 活动返还（总）
        /// </summary>
        [DataMember]
        public double ExpendituresAtAll { get; set; }

        /// <summary>
        /// 盈亏（日）
        /// </summary>
        [DataMember]
        public double GainsAndLossesAtDay { get; set; }

        /// <summary>
        /// 盈亏（月）
        /// </summary>
        [DataMember]
        public double GainsAndLossesAtMonth { get; set; }

        /// <summary>
        /// 盈亏（总）
        /// </summary>
        [DataMember]
        public double GainsAndLossesAtAll { get; set; }

        /// <summary>
        /// 充值（日）
        /// </summary>
        [DataMember]
        public double RechargeAtDay { get; set; }

        /// <summary>
        /// 充值（月）
        /// </summary>
        [DataMember]
        public double RechargeAtMonth { get; set; }

        /// <summary>
        /// 充值（总）
        /// </summary>
        [DataMember]
        public double RechargeAtAll { get; set; }

        /// <summary>
        /// 提现（日）
        /// </summary>
        [DataMember]
        public double WithdrawalAtDay { get; set; }

        /// <summary>
        /// 提现（月）
        /// </summary>
        [DataMember]
        public double WithdrawalAtMonth { get; set; }

        /// <summary>
        /// 提现（总）
        /// </summary>
        [DataMember]
        public double WithdrawalAtAll { get; set; }

        /// <summary>
        /// 支取（日）
        /// </summary>
        [DataMember]
        public double TransferAtDay { get; set; }

        /// <summary>
        /// 支取（月）
        /// </summary>
        [DataMember]
        public double TransferAtMonth { get; set; }

        /// <summary>
        /// 支取（总）
        /// </summary>
        [DataMember]
        public double TransferAtAll { get; set; }

        /// <summary>
        /// 现金流（日）
        /// </summary>
        [DataMember]
        public double CashAtDay { get; set; }

        /// <summary>
        /// 现金流（月）
        /// </summary>
        [DataMember]
        public double CashAtMonth { get; set; }

        /// <summary>
        /// 现金流（总）
        /// </summary>
        [DataMember]
        public double CashAtAll { get; set; }

        /// <summary>
        /// 实例化一个新的综合信息统计（成功）
        /// </summary>
        /// <param name="siteDataAtDay">相关站点数据统计（日）</param>
        /// <param name="siteDataAtMonth">相关站点数据统计（月）</param>
        public ComprehensiveInformationResult(SiteDataAtDay siteDataAtDay, SiteDataAtMonth siteDataAtMonth)
        {
            ComprehensiveInformation comprehensiveInformation = new ComprehensiveInformation();

            this.SetUpAtDay = siteDataAtDay.CountOfSetUp;
            this.SetUpAtMonth = siteDataAtMonth.CountOfSetUp;
            this.SetUpAtAll = comprehensiveInformation.CountOfSetUp;

            this.SetInAtDay = siteDataAtDay.CountOfSetIn;
            this.SetInAtMonth = siteDataAtMonth.CountOfSetIn;
            this.SetInAtAll = comprehensiveInformation.CountOfSetIn;

            this.AmountOfBetsAtDay = siteDataAtDay.AmountOfBets;
            this.AmountOfBetsAtMonth = siteDataAtMonth.AmountOfBets;
            this.AmountOfBetsAtAll = comprehensiveInformation.AmountOfBets;

            this.RebateAtDay = siteDataAtDay.ReturnPoints;
            this.RebateAtMonth = siteDataAtMonth.ReturnPoints;
            this.RebateAtAll = comprehensiveInformation.ReturnPoints;

            this.BonusAtDay = siteDataAtDay.Bonus;
            this.BonusAtMonth = siteDataAtMonth.Bonus;
            this.BonusAtAll = comprehensiveInformation.Bonus;

            this.ExpendituresAtDay = siteDataAtDay.Expenditures;
            this.ExpendituresAtMonth = siteDataAtMonth.Expenditures;
            this.ExpendituresAtAll = comprehensiveInformation.Expenditures;

            this.GainsAndLossesAtDay = siteDataAtDay.GainsAndLosses;
            this.GainsAndLossesAtMonth = siteDataAtMonth.GainsAndLosses;
            this.GainsAndLossesAtAll = comprehensiveInformation.GainsAndLosses;

            this.RechargeAtDay = siteDataAtDay.Recharge;
            this.RechargeAtMonth = siteDataAtMonth.Recharge;
            this.RechargeAtAll = comprehensiveInformation.Recharge;

            this.WithdrawalAtDay = siteDataAtDay.Withdrawal;
            this.WithdrawalAtMonth = siteDataAtMonth.Withdrawal;
            this.WithdrawalAtAll = comprehensiveInformation.Withdrawal;

            this.TransferAtDay = siteDataAtDay.Transfer;
            this.TransferAtMonth = siteDataAtMonth.Transfer;
            this.TransferAtAll = comprehensiveInformation.Transfer;

            this.CashAtDay = siteDataAtDay.Cash;
            this.CashAtMonth = siteDataAtMonth.Cash;
            this.CashAtAll = comprehensiveInformation.Cash;
        }

        /// <summary>
        /// 实例化一个新的综合信息统计（失败）
        /// </summary>
        /// <param name="error">错误信息</param>
        public ComprehensiveInformationResult(string error)
            : base(error)
        {
        }
    }
}

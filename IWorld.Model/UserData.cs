using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 用户数据
    /// </summary>
    public class UserData : ModelBase
    {
        #region 属性

        /// <summary>
        /// 人民币账户余额
        /// </summary>
        public double RMB { get; set; }

        /// <summary>
        /// 被冻结的金额（人民币账户）
        /// </summary>
        public double RMB_Frozen { get; set; }

        /// <summary>
        /// 消费量（人民币）
        /// </summary>
        public double Consumption_RMB { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public double Integral { get; set; }

        /// <summary>
        /// 消费量（积分）
        /// </summary>
        public double Consumption_Integral { get; set; }

        /// <summary>
        /// 当前拥有直属下级数量
        /// </summary>
        public int CountOfSubordinates { get; set; }

        /// <summary>
        /// 投注额
        /// </summary>
        public double AmountOfBets { get; set; }

        /// <summary>
        /// 返点
        /// </summary>
        public double Rebate { get; set; }

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
        public double Profit { get; set; }

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
        /// 实例化一个新的用户数据
        /// </summary>
        public UserData()
        {
            this.RMB = 0;
            this.RMB_Frozen = 0;
            this.Consumption_RMB = 0;
            this.Integral = 0;
            this.Consumption_Integral = 0;
            this.CountOfSubordinates = 0;
            this.AmountOfBets = 0;
            this.Rebate = 0;
            this.Bonus = 0;
            this.Expenditures = 0;
            this.Recharge = 0;
            this.Withdrawal = 0;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 增减人民币账户
        /// </summary>
        /// <param name="amount">增减数额</param>
        public void IncreaseRMB(double amount)
        {
            double t = this.RMB + amount;
            if (t < 0)
            {
                throw new Exception("该操作将导致人民币账户余额小于0 操作无效");
            }
            this.RMB = t;
        }

        /// <summary>
        /// 增减被冻结的金额（人民币账户）
        /// </summary>
        /// <param name="amount">增减数额</param>
        /// <param name="linkage">一个布尔值 标识是否与人民币账户联动（默认为 是）</param>
        public void IncreaseRMB_Frozen(double amount, bool linkage = true)
        {
            double t = this.RMB_Frozen + amount;
            if (t < 0)
            {
                throw new Exception("该操作将导致被冻结的金额（人民币账户）小于0 操作无效");
            }
            if (linkage) { IncreaseRMB(-amount); }
            this.RMB_Frozen = t;
        }

        /// <summary>
        /// 增减消费量（人民币）
        /// </summary>
        /// <param name="amount">增减数额</param>
        public void IncreaseConsumption_RMB(double amount)
        {
            double t = this.Consumption_RMB + amount;
            if (t < 0)
            {
                throw new Exception("该操作将导致消费量（人民币）小于0 操作无效");
            }
            IncreaseRMB(-amount);
            this.Consumption_RMB = t;
        }

        /// <summary>
        /// 增减积分
        /// </summary>
        /// <param name="amount">增减数额</param>
        public void IncreaseIntegral(double amount)
        {
            double t = this.Integral + amount;
            if (t < 0)
            {
                throw new Exception("该操作将导致积分小于0 操作无效");
            }
            this.Integral = t;
        }

        /// <summary>
        /// 增减消费量（积分）
        /// </summary>
        /// <param name="amount">增减数额</param>
        public void IncreaseConsumption_Integral(double amount)
        {
            double t = this.Consumption_Integral + amount;
            if (t < 0)
            {
                throw new Exception("该操作将导致消费量（积分）小于0 操作无效");
            }
            IncreaseIntegral(-amount);
            this.Consumption_Integral = t;
        }

        /// <summary>
        /// 增减当前拥有直属下级数量
        /// </summary>
        /// <param name="amount">增减数额（默认为 1）</param>
        public void IncreaseSubordinates(int amount = 1)
        {
            int t = this.CountOfSubordinates + amount;
            if (t < 0)
            {
                throw new Exception("该操作将导致当前拥有直属下级数量小于0 操作无效");
            }
            this.CountOfSubordinates = t;
        }

        /// <summary>
        /// 增减投注额
        /// </summary>
        /// <param name="amount">增减数额</param>
        public void IncreaseAmountOfBets(double amount)
        {
            double t = this.AmountOfBets + amount;
            if (t < 0)
            {
                throw new Exception("该操作将导致投注额小于0 操作无效");
            }
            this.AmountOfBets = t;
            this.Profit -= amount;
        }

        /// <summary>
        /// 增减返点
        /// </summary>
        /// <param name="amount">增减数额</param>
        public void IncreaseRebate(double amount)
        {
            double t = this.Rebate + amount;
            if (t < 0)
            {
                throw new Exception("该操作将导致返点小于0 操作无效");
            }
            this.Rebate = t;
            this.Profit += amount;
        }

        /// <summary>
        /// 增减奖金
        /// </summary>
        /// <param name="amount">增减数额</param>
        public void IncreaseBonus(double amount)
        {
            double t = this.Bonus + amount;
            if (t < 0)
            {
                throw new Exception("该操作将导致奖金小于0 操作无效");
            }
            this.Bonus = t;
            this.Profit += amount;
        }

        /// <summary>
        /// 增减活动返还
        /// </summary>
        /// <param name="amount">增减数额</param>
        public void IncreaseExpenditures(double amount)
        {
            double t = this.Bonus + amount;
            if (t < 0)
            {
                throw new Exception("该操作将导致奖金小于0 操作无效");
            }
            this.Bonus = t;
            this.Profit += amount;
        }

        #endregion
    }
}

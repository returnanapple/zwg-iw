using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    public class UserData : ModelBase
    {
        #region 属性

        #region 现金账户

        /// <summary>
        /// 现金
        /// </summary>
        public double Cash { get; set; }

        /// <summary>
        /// 被冻结的现金
        /// </summary>
        public double Cash_Frozen { get; set; }

        /// <summary>
        /// 消费量（现金）
        /// </summary>
        public double Consumption_Money { get; set; }

        #endregion

        #region 积分账户

        /// <summary>
        /// 积分
        /// </summary>
        public double Integral { get; set; }

        /// <summary>
        /// 消费量（积分）
        /// </summary>
        public double Consumption_Integral { get; set; }

        #endregion

        #region 统计数据

        /// <summary>
        /// 登陆次数
        /// </summary>
        public int TimesOfLogin { get; set; }

        /// <summary>
        /// 直属下级数量
        /// </summary>
        public int Subordinate { get; set; }

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
        public double GainsAndLosses
        {
            get { return this.Rebate + this.Bonus + this.Expenditures - this.AmountOfBets; }
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
        /// 现金（理论 - 不考虑冻结）
        /// </summary>
        public double Cash_Theory { get { return this.Recharge + this.GainsAndLosses - this.Withdrawal; } }

        #endregion

        #endregion

        #region 方法

        #region 现金账户

        /// <summary>
        /// 增减现金
        /// </summary>
        /// <param name="sum">所要增减/减少的数额</param>
        public void IncreaseCash(double sum)
        {
            double t = this.Cash + sum;
            if (t < 0)
            {
                OnError("现金");
            }
            this.Cash = t;
        }

        /// <summary>
        /// 增减被冻结的现金
        /// </summary>
        /// <param name="sum">所要增减/减少的数额</param>
        public void IncreaseCash_Frozen(double sum)
        {
            double t = this.Cash_Frozen + sum;
            if (t < 0)
            {
                OnError("被冻结的现金");
            }
            IncreaseCash(-sum);
            this.Cash_Frozen = t;
        }

        /// <summary>
        /// 增减消费量（现金）
        /// </summary>
        /// <param name="sum">所要增减/减少的数额</param>
        public void IncreaseConsumption_Money(double sum)
        {
            double t = this.Consumption_Money + sum;
            if (t < 0)
            {
                OnError("消费量（现金）");
            }
            IncreaseCash(-sum);
            this.Consumption_Money = t;
        }

        #endregion

        #region 积分账户

        /// <summary>
        /// 增减积分
        /// </summary>
        /// <param name="sum">所要增减/减少的数额</param>
        public void IncreaseIntegral(double sum)
        {
            double t = this.Integral + sum;
            if (t < 0)
            {
                OnError("积分");
            }
            this.Integral = t;
        }

        /// <summary>
        /// 增减消费量（积分）
        /// </summary>
        /// <param name="sum">所要增减/减少的数额</param>
        public void IncreaseConsumption_Integral(double sum)
        {
            double t = this.Consumption_Integral + sum;
            if (t < 0)
            {
                OnError("消费量（积分）");
            }
            IncreaseIntegral(-sum);
            this.Consumption_Integral = t;
        }

        #endregion

        #region 统计数据

        /// <summary>
        /// 登陆次数+1
        /// </summary>
        public void IncreaseTimesOfLogin()
        {
            this.TimesOfLogin++;
        }

        /// <summary>
        /// 直属下级数量+1
        /// </summary>
        public void IncreaseSubordinate()
        {
            this.Subordinate++;
        }

        public void IncreaseAmountOfBets(double sum)
        {
            double t = this.Consumption_Integral + sum;
            if (t < 0)
            {
                OnError("消费量（积分）");
            }
            IncreaseIntegral(-sum);
            this.Consumption_Integral = t;
        }

        #endregion

        #endregion

        #region 私有方法

        /// <summary>
        /// 声明一个导致某属性小于0的错误发生
        /// </summary>
        /// <param name="propertyDescription">属性说明</param>
        void OnError(string propertyDescription)
        {
            string message = string.Format("该操作将导致用户的{0}小于0，操作无效", propertyDescription);
            throw new Exception(message);
        }

        #endregion
    }
}
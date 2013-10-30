using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 大白鲨游戏的投注记录
    /// </summary>
    public class BettingOfJaw : ModelBase
    {
        #region 属性

        /// <summary>
        /// 用户
        /// </summary>
        public virtual Author Owner { get; set; }

        /// <summary>
        /// 期号
        /// </summary>
        public string Issue { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public virtual List<BettingDetailOfJaw> Details { get; set; }

        /// <summary>
        /// 总投注额
        /// </summary>
        public double Pay { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public double Bonus { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public BettingStatus Status { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的大白鲨游戏的投注记录
        /// </summary>
        public BettingOfJaw()
        {
        }

        /// <summary>
        /// 实例化一个新的大白鲨游戏的投注记录
        /// </summary>
        /// <param name="owner">用户</param>
        /// <param name="issue">期号</param>
        /// <param name="details">明细</param>
        public BettingOfJaw(Author owner, string issue, List<BettingDetailOfJaw> details)
        {
            this.Owner = owner;
            this.Issue = issue;
            this.Details = details;
            this.Pay = details.Count == 0 ? 0 : details.Sum(x => x.Sum);
            this.Status = BettingStatus.等待开奖;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取奖金数额
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        /// <returns>返回奖金数额</returns>
        public double GetBonus(LotteryOfJaw lottery)
        {
            var tList = this.Details.Where(x => x.Mark.OpenUpList.Contains(lottery.Value)).ToList();
            double result = tList.Count == 0
                ? 0
                : tList.Sum(x =>
                    {
                        return x.Mark.Odds * x.Sum;
                    });
            return Math.Round(result, 2);
        }

        #endregion
    }
}

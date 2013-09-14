using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Setting;

namespace IWorld.BLL
{
    /// <summary>
    /// 站点统计信息的管理者对象
    /// </summary>
    public class SiteDataManager
    {
        #region 静态方法

        /// <summary>
        /// 统计用户注册
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadSetedUp(object sender, NEventArgs e)
        {
            SiteDataAtDay siteDataAtDay = GetSiteDataAtDay(e.Db);
            SiteDataAtMonth siteDataAtMonth = GetSiteDataAtMonth(e.Db);
            ComprehensiveInformation comprehensiveInformation = new ComprehensiveInformation();

            siteDataAtDay.CountOfSetUp += 1;
            siteDataAtMonth.CountOfSetUp += 1;
            comprehensiveInformation.CountOfSetUp += 1;

            e.Db.SaveChanges();
            comprehensiveInformation.Save();
        }

        /// <summary>
        /// 统计用户登入
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadSetedIn(object sender, AuthorManager.LoginEventArgs e)
        {
            Author author = (Author)e.State;
            DateTime now = DateTime.Now;
            int count = e.Db.Set<UserLandingRecord>().Count(x => x.Owner.Id == author.Id
                && x.CreatedTime.Year == now.Year
                && x.CreatedTime.Month == now.Month
                && x.CreatedTime.Day == now.Day);
            if (count <= 1)
            {
                SiteDataAtDay siteDataAtDay = GetSiteDataAtDay(e.Db);
                SiteDataAtMonth siteDataAtMonth = GetSiteDataAtMonth(e.Db);

                siteDataAtDay.CountOfSetIn += 1;
                siteDataAtMonth.CountOfSetIn += 1;

                e.Db.SaveChanges();
            }
        }

        /// <summary>
        /// 统计投注额
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadBeted(object sender, NEventArgs e)
        {
            SiteDataAtDay siteDataAtDay = GetSiteDataAtDay(e.Db);
            SiteDataAtMonth siteDataAtMonth = GetSiteDataAtMonth(e.Db);
            ComprehensiveInformation comprehensiveInformation = new ComprehensiveInformation();
            Betting betting = (Betting)e.State;

            siteDataAtDay.AmountOfBets += betting.Pay;
            siteDataAtMonth.AmountOfBets += betting.Pay;
            comprehensiveInformation.AmountOfBets += betting.Pay;

            e.Db.SaveChanges();
            comprehensiveInformation.Save();
        }

        /// <summary>
        /// 统计投注额
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadBeted(object sender, ChasingManager.ChangeStatusEventArgs e)
        {
            List<ChasingStatus> tStatus
                = new List<ChasingStatus> { ChasingStatus.因为中奖而终止, ChasingStatus.用户中止追号, ChasingStatus.追号结束 };
            if (tStatus.Contains(e.NewStatus))
            {
                SiteDataAtDay siteDataAtDay = GetSiteDataAtDay(e.Db);
                SiteDataAtMonth siteDataAtMonth = GetSiteDataAtMonth(e.Db);
                ComprehensiveInformation comprehensiveInformation = new ComprehensiveInformation();
                Chasing chasing = (Chasing)e.State;

                siteDataAtDay.AmountOfBets += chasing.Pay;
                siteDataAtMonth.AmountOfBets += chasing.Pay;
                comprehensiveInformation.AmountOfBets += chasing.Pay;

                e.Db.SaveChanges();
                comprehensiveInformation.Save();
            }
        }

        /// <summary>
        /// 统计返点
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadReturnedPoints(object sender, NEventArgs e)
        {
            SiteDataAtDay siteDataAtDay = GetSiteDataAtDay(e.Db);
            SiteDataAtMonth siteDataAtMonth = GetSiteDataAtMonth(e.Db);
            ComprehensiveInformation comprehensiveInformation = new ComprehensiveInformation();
            SubordinateDynamic sd = (SubordinateDynamic)e.State;

            siteDataAtDay.ReturnPoints += sd.Give;
            siteDataAtMonth.ReturnPoints += sd.Give;
            comprehensiveInformation.ReturnPoints += sd.Give;

            e.Db.SaveChanges();
            comprehensiveInformation.Save();
        }

        /// <summary>
        /// 统计奖金
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadLottery(object sender, BettingManager.ChangeStatusEventArgs e)
        {
            if (e.NewStatus == BettingStatus.中奖)
            {
                SiteDataAtDay siteDataAtDay = GetSiteDataAtDay(e.Db);
                SiteDataAtMonth siteDataAtMonth = GetSiteDataAtMonth(e.Db);
                ComprehensiveInformation comprehensiveInformation = new ComprehensiveInformation();

                siteDataAtDay.Bonus += e.Bonus;
                siteDataAtMonth.Bonus += e.Bonus;
                comprehensiveInformation.Bonus += e.Bonus;

                e.Db.SaveChanges();
                comprehensiveInformation.Save();
            }
        }

        /// <summary>
        /// 撤单时候移除投注额
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadRevoked(object sender, BettingManager.ChangeStatusEventArgs e)
        {
            if (e.NewStatus == BettingStatus.用户撤单)
            {
                SiteDataAtDay siteDataAtDay = GetSiteDataAtDay(e.Db);
                SiteDataAtMonth siteDataAtMonth = GetSiteDataAtMonth(e.Db);
                ComprehensiveInformation comprehensiveInformation = new ComprehensiveInformation();
                Betting betting = (Betting)e.State;

                siteDataAtDay.AmountOfBets -= betting.Pay;
                siteDataAtMonth.AmountOfBets -= betting.Pay;
                comprehensiveInformation.AmountOfBets -= betting.Pay;

                e.Db.SaveChanges();
                comprehensiveInformation.Save();
            }
        }

        /// <summary>
        /// 统计奖金
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadLottery(object sender, BettingForCgasingManager.ChangeStatusEventArgs e)
        {
            if (e.NewStatus == BettingStatus.中奖)
            {
                SiteDataAtDay siteDataAtDay = GetSiteDataAtDay(e.Db);
                SiteDataAtMonth siteDataAtMonth = GetSiteDataAtMonth(e.Db);
                ComprehensiveInformation comprehensiveInformation = new ComprehensiveInformation();

                siteDataAtDay.Bonus += e.Bonus;
                siteDataAtMonth.Bonus += e.Bonus;
                comprehensiveInformation.Bonus += e.Bonus;

                e.Db.SaveChanges();
                comprehensiveInformation.Save();
            }
        }

        /// <summary>
        /// 统计活动支出
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadParticipatedActivity(object sender, NEventArgs e)
        {
            SiteDataAtDay siteDataAtDay = GetSiteDataAtDay(e.Db);
            SiteDataAtMonth siteDataAtMonth = GetSiteDataAtMonth(e.Db);
            ComprehensiveInformation comprehensiveInformation = new ComprehensiveInformation();
            ActivityParticipateRecord record = (ActivityParticipateRecord)e.State;

            double sum = record.Activity.RewardValueIsAbsolute ? record.Activity.Reward
                : Math.Round(record.Activity.Reward * record.Amount, 2);

            siteDataAtDay.Expenditures += sum;
            siteDataAtMonth.Expenditures += sum;
            comprehensiveInformation.Expenditures += sum;

            e.Db.SaveChanges();
            comprehensiveInformation.Save();
        }

        /// <summary>
        /// 统计活动支出
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadParticipatedExchange(object sender, NEventArgs e)
        {
            SiteDataAtDay siteDataAtDay = GetSiteDataAtDay(e.Db);
            SiteDataAtMonth siteDataAtMonth = GetSiteDataAtMonth(e.Db);
            ComprehensiveInformation comprehensiveInformation = new ComprehensiveInformation();
            ExchangeParticipateRecord record = (ExchangeParticipateRecord)e.State;

            double sum = record.Exchange.Prizes.Where(x => x.Type == PrizeType.人民币 || x.Type == PrizeType.实物)
                .Sum(x => x.Price);

            siteDataAtDay.Expenditures += sum;
            siteDataAtMonth.Expenditures += sum;
            comprehensiveInformation.Expenditures += sum;

            e.Db.SaveChanges();
            comprehensiveInformation.Save();
        }

        /// <summary>
        /// 统计充值
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadRecharged(object sender, RechargeRecordManager.ChangeStatusEventArgs e)
        {
            if (e.NewStatus == RechargeStatus.充值成功)
            {
                SiteDataAtDay siteDataAtDay = GetSiteDataAtDay(e.Db);
                SiteDataAtMonth siteDataAtMonth = GetSiteDataAtMonth(e.Db);
                ComprehensiveInformation comprehensiveInformation = new ComprehensiveInformation();
                RechargeRecord record = (RechargeRecord)e.State;

                siteDataAtDay.Recharge += record.Sum;
                siteDataAtMonth.Recharge += record.Sum;
                comprehensiveInformation.Recharge += record.Sum;

                e.Db.SaveChanges();
                comprehensiveInformation.Save();
            }
        }

        /// <summary>
        /// 统计提现
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadWithdrawal(object sender, WithdrawalsRecordManager.ChangeStatusEventArgs e)
        {
            if (e.NewStatus == WithdrawalsStatus.提现成功)
            {
                SiteDataAtDay siteDataAtDay = GetSiteDataAtDay(e.Db);
                SiteDataAtMonth siteDataAtMonth = GetSiteDataAtMonth(e.Db);
                ComprehensiveInformation comprehensiveInformation = new ComprehensiveInformation();
                WithdrawalsRecord record = (WithdrawalsRecord)e.State;

                siteDataAtDay.Withdrawal += record.Sum;
                siteDataAtMonth.Withdrawal += record.Sum;
                comprehensiveInformation.Withdrawal += record.Sum;

                e.Db.SaveChanges();
                comprehensiveInformation.Save();
            }
        }

        /// <summary>
        /// 统计现金提取（管理员）
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadTransfer(object sender, NEventArgs e)
        {
            SiteDataAtDay siteDataAtDay = GetSiteDataAtDay(e.Db);
            SiteDataAtMonth siteDataAtMonth = GetSiteDataAtMonth(e.Db);
            ComprehensiveInformation comprehensiveInformation = new ComprehensiveInformation();
            TransferRecord record = (TransferRecord)e.State;

            siteDataAtDay.Transfer += record.Sum;
            siteDataAtMonth.Transfer += record.Sum;
            comprehensiveInformation.Transfer += record.Sum;

            e.Db.SaveChanges();
            comprehensiveInformation.Save();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取站点统计信息（日）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        /// <returns>返回站点统计信息（日）</returns>
        private static SiteDataAtDay GetSiteDataAtDay(DbContext db)
        {
            DateTime now = DateTime.Now;
            var sSet = db.Set<SiteDataAtDay>();
            Expression<Func<SiteDataAtDay, bool>> predicate = x => x.Year == now.Year && x.Month == now.Month && x.Day == now.Day;
            SiteDataAtDay siteDataAtDay = sSet.FirstOrDefault(predicate);
            if (siteDataAtDay == null)
            {
                siteDataAtDay = new SiteDataAtDay(now);
                sSet.Add(siteDataAtDay);
            }
            return siteDataAtDay;
        }

        /// <summary>
        /// 获取站点统计信息（月）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        /// <returns>返回站点统计信息（月）</returns>
        private static SiteDataAtMonth GetSiteDataAtMonth(DbContext db)
        {
            DateTime now = DateTime.Now;
            var sSet = db.Set<SiteDataAtMonth>();
            Expression<Func<SiteDataAtMonth, bool>> predicate = x => x.Year == now.Year && x.Month == now.Month;
            SiteDataAtMonth siteDataAtMonth = sSet.FirstOrDefault(predicate);
            if (siteDataAtMonth == null)
            {
                siteDataAtMonth = new SiteDataAtMonth(now);
                sSet.Add(siteDataAtMonth);
            }
            return siteDataAtMonth;
        }

        #endregion
    }
}

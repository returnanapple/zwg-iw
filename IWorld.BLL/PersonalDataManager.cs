using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 个人信息统计的管理者对象
    /// </summary>
    public class PersonalDataManager
    {
        #region 静态方法

        /// <summary>
        /// 统计投注额
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadBeted(object sender, NEventArgs e)
        {
            Betting betting = (Betting)e.State;
            PersonalDataAtDay personalDataAtDay = GetPersonalDataAtDay(e.Db, betting.Owner.Id);
            PersonalDataAtMonth personalDataAtMonth = GetPersonalDataAtMonth(e.Db, betting.Owner.Id);

            personalDataAtDay.AmountOfBets += betting.Pay;
            personalDataAtMonth.AmountOfBets += betting.Pay;

            e.Db.SaveChanges();
            if (CountedForDayEventHandler != null)
            {
                CountedForDayEventHandler(null, new NEventArgs(e.Db, personalDataAtDay));
            }
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
                Chasing chasing = (Chasing)e.State;
                PersonalDataAtDay personalDataAtDay = GetPersonalDataAtDay(e.Db, chasing.Owner.Id);
                PersonalDataAtMonth personalDataAtMonth = GetPersonalDataAtMonth(e.Db, chasing.Owner.Id);

                personalDataAtDay.AmountOfBets += chasing.Pay;
                personalDataAtMonth.AmountOfBets += chasing.Pay;

                e.Db.SaveChanges();
                if (CountedForDayEventHandler != null)
                {
                    CountedForDayEventHandler(null, new NEventArgs(e.Db, personalDataAtDay));
                }
            }
        }

        /// <summary>
        /// 统计返点
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadReturnedPoints(object sender, NEventArgs e)
        {
            SubordinateDynamic sd = (SubordinateDynamic)e.State;
            PersonalDataAtDay personalDataAtDay = GetPersonalDataAtDay(e.Db, sd.To.Id);
            PersonalDataAtMonth personalDataAtMonth = GetPersonalDataAtMonth(e.Db, sd.To.Id);

            personalDataAtDay.ReturnPoints += sd.Give;
            personalDataAtMonth.ReturnPoints += sd.Give;

            e.Db.SaveChanges();
            if (CountedForDayEventHandler != null)
            {
                CountedForDayEventHandler(null, new NEventArgs(e.Db, personalDataAtDay));
            }
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
                Betting betting = (Betting)e.State;
                PersonalDataAtDay personalDataAtDay = GetPersonalDataAtDay(e.Db, betting.Owner.Id);
                PersonalDataAtMonth personalDataAtMonth = GetPersonalDataAtMonth(e.Db, betting.Owner.Id);

                personalDataAtDay.Bonus += e.Bonus;
                personalDataAtMonth.Bonus += e.Bonus;

                e.Db.SaveChanges();
                if (CountedForDayEventHandler != null)
                {
                    CountedForDayEventHandler(null, new NEventArgs(e.Db, personalDataAtDay));
                }
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
                BettingForCgasing betting = (BettingForCgasing)e.State;
                PersonalDataAtDay personalDataAtDay = GetPersonalDataAtDay(e.Db, betting.Chasing.Owner.Id);
                PersonalDataAtMonth personalDataAtMonth = GetPersonalDataAtMonth(e.Db, betting.Chasing.Owner.Id);

                personalDataAtDay.Bonus += e.Bonus;
                personalDataAtMonth.Bonus += e.Bonus;
                e.Db.SaveChanges();
                if (CountedForDayEventHandler != null)
                {
                    CountedForDayEventHandler(null, new NEventArgs(e.Db, personalDataAtDay));
                }
            }
        }

        /// <summary>
        /// 统计活动支出
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadParticipatedActivity(object sender, NEventArgs e)
        {
            ActivityParticipateRecord record = (ActivityParticipateRecord)e.State;
            PersonalDataAtDay personalDataAtDay = GetPersonalDataAtDay(e.Db, record.Owner.Id);
            PersonalDataAtMonth personalDataAtMonth = GetPersonalDataAtMonth(e.Db, record.Owner.Id);

            double sum = record.Activity.RewardValueIsAbsolute ? record.Activity.Reward
                : Math.Round(record.Activity.Reward * record.Amount, 2);

            personalDataAtDay.Expenditures += sum;
            personalDataAtMonth.Expenditures += sum;

            e.Db.SaveChanges();
            if (CountedForDayEventHandler != null)
            {
                CountedForDayEventHandler(null, new NEventArgs(e.Db, personalDataAtDay));
            }
        }

        /// <summary>
        /// 统计活动支出
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void HadParticipatedExchange(object sender, NEventArgs e)
        {
            ExchangeParticipateRecord record = (ExchangeParticipateRecord)e.State;
            PersonalDataAtDay personalDataAtDay = GetPersonalDataAtDay(e.Db, record.Owner.Id);
            PersonalDataAtMonth personalDataAtMonth = GetPersonalDataAtMonth(e.Db, record.Owner.Id);

            double sum = record.Exchange.Prizes.Where(x => x.Type == PrizeType.人民币 || x.Type == PrizeType.实物)
                .Sum(x => x.Price);

            personalDataAtDay.Expenditures += sum;
            personalDataAtMonth.Expenditures += sum;

            e.Db.SaveChanges();
            if (CountedForDayEventHandler != null)
            {
                CountedForDayEventHandler(null, new NEventArgs(e.Db, personalDataAtDay));
            }
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
                RechargeRecord record = (RechargeRecord)e.State;
                PersonalDataAtDay personalDataAtDay = GetPersonalDataAtDay(e.Db, record.Owner.Id);
                PersonalDataAtMonth personalDataAtMonth = GetPersonalDataAtMonth(e.Db, record.Owner.Id);

                personalDataAtDay.Recharge += record.Sum;
                personalDataAtMonth.Recharge += record.Sum;

                e.Db.SaveChanges();
                if (CountedForDayEventHandler != null)
                {
                    CountedForDayEventHandler(null, new NEventArgs(e.Db, personalDataAtDay));
                }
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
                WithdrawalsRecord record = (WithdrawalsRecord)e.State;
                PersonalDataAtDay personalDataAtDay = GetPersonalDataAtDay(e.Db, record.Owner.Id);
                PersonalDataAtMonth personalDataAtMonth = GetPersonalDataAtMonth(e.Db, record.Owner.Id);

                personalDataAtDay.Withdrawal += record.Sum;
                personalDataAtMonth.Withdrawal += record.Sum;

                e.Db.SaveChanges();
                if (CountedForDayEventHandler != null)
                {
                    CountedForDayEventHandler(null, new NEventArgs(e.Db, personalDataAtDay));
                }
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取获取站点统计信息（日）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <returns>返回获取站点统计信息（日）</returns>
        private static PersonalDataAtDay GetPersonalDataAtDay(DbContext db, int ownerId)
        {
            DateTime now = DateTime.Now;
            Author owner = db.Set<Author>().Find(ownerId);
            var pSet = db.Set<PersonalDataAtDay>();
            Expression<Func<PersonalDataAtDay, bool>> predicate =
                x => x.Year == now.Year && x.Month == now.Month && x.Day == now.Day && x.Owner.Id == ownerId;
            PersonalDataAtDay result = pSet.FirstOrDefault(predicate);
            if (result == null)
            {
                result = new PersonalDataAtDay(owner);
                pSet.Add(result);
            }
            result.Subordinate = owner.Subordinate;
            result.Money = owner.Money;
            return result;
        }

        /// <summary>
        /// 获取获取站点统计信息（月）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <returns>返回获取站点统计信息（月）</returns>
        private static PersonalDataAtMonth GetPersonalDataAtMonth(DbContext db, int ownerId)
        {
            DateTime now = DateTime.Now;
            Author owner = db.Set<Author>().Find(ownerId);
            var pSet = db.Set<PersonalDataAtMonth>();
            Expression<Func<PersonalDataAtMonth, bool>> predicate =
                x => x.Year == now.Year && x.Month == now.Month && x.Owner.Id == ownerId;
            PersonalDataAtMonth result = pSet.FirstOrDefault(predicate);
            if (result == null)
            {
                result = new PersonalDataAtMonth(owner);
                pSet.Add(result);
            }
            result.Subordinate = owner.Subordinate;
            result.Money = owner.Money;
            return result;
        }

        #endregion

        #region 事件

        /// <summary>
        /// 当日统计被更新的时候将触发的时间
        /// </summary>
        public static event NDelegate CountedForDayEventHandler;

        #endregion
    }
}

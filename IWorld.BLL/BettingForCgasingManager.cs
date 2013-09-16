using System;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Setting;
using IWorld.Helper;

namespace IWorld.BLL
{
    /// <summary>
    /// 投注（追号）的管理者对象
    /// </summary>
    public class BettingForCgasingManager : SimplifyManagerBase<BettingForCgasing>, IManager<BettingForCgasing>
        , ISimplify<BettingForCgasing>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的投注（追号）的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public BettingForCgasingManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 将指定的实例从数据库中移除
        /// </summary>
        /// <param name="id">目标对象的存储指针</param>
        public override void Remove(int id)
        {
            PackageForRemove pfr = new PackageForRemove(id);
            base.Remove(pfr);
        }

        /// <summary>
        /// 改变投注记录的当前状态
        /// </summary>
        /// <param name="bettingId">目标投注记录的存储指针</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="bonus">奖金</param>
        public void ChangeStatus(int bettingId, BettingStatus newStatus, double bonus = 0)
        {
            if (newStatus == BettingStatus.等待开奖)
            {
                throw new Exception("不允许将任何记录修改为“等待开奖”状态");
            }
            NChecker.CheckEntity<BettingForCgasing>(bettingId, "投注记录", db);
            BettingForCgasing betting = db.Set<BettingForCgasing>().Find(bettingId);
            if (betting.Status == BettingStatus.未中奖 || betting.Status == BettingStatus.中奖)
            {
                throw new Exception("已经确认开奖结果的记录无法修改状态");
            }
            if (betting.Status == newStatus)
            {
                throw new Exception("目标记录已经是所要修改的状态，无需操作");
            }

            ChangeStatusEventArgs eventArgs = new ChangeStatusEventArgs(db, betting, betting.Status, newStatus, bonus);
            if (ChangingStatusEventHandler != null)
            {
                ChangingStatusEventHandler(this, eventArgs);    //触发前置事件
            }
            betting.Status = newStatus;
            betting.Bonus = bonus;
            betting.ModifiedTime = DateTime.Now;
            db.SaveChanges();
            if (ChangedStatusEventHandler != null)
            {
                ChangedStatusEventHandler(this, eventArgs); //触发后置事件
            }
        }

        #endregion

        #region 内嵌类型

        /// <summary>
        /// 用于移除投注记录的数据集
        /// </summary>
        private class PackageForRemove : IPackage<BettingForCgasing>, IRemovePackage<BettingForCgasing>
        {
            #region 公开属性

            /// <summary>
            /// 存储指针
            /// </summary>
            public int Id { get; set; }

            #endregion

            #region 构造方法

            /// <summary>
            /// 实例化一个新的用于移除投注记录的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            public PackageForRemove(int id)
            {
                this.Id = id;
            }

            #endregion

            #region 实例方法

            /// <summary>
            /// 检查数据集的内容是否符合定义
            /// </summary>
            /// <param name="db">数据库连接对象</param>
            public void CheckData(DbContext db)
            {
                NChecker.CheckEntity<BettingForCgasing>(this.Id, "投注记录", db);
                var t = (from c in db.Set<BettingForCgasing>()
                         where c.Id == this.Id
                         select new
                         {
                             c.Status,
                             c.Chasing.HowToPlay.Tag.Ticket
                         })
                         .FirstOrDefault();
                switch (t.Status)
                {
                    case BettingStatus.即将开奖:
                        throw new Exception("即将开奖的投注记录无法撤单");
                    case BettingStatus.未中奖:
                    case BettingStatus.中奖:
                        throw new Exception("已经开奖的投注记录无法撤单");
                }
            }

            /// <summary>
            /// 获取实体对象
            /// </summary>
            /// <param name="db">数据库连接对象</param>
            /// <returns>返回泛型状态所规定的实体类</returns>
            public BettingForCgasing GetEntity(DbContext db)
            {
                return db.Set<BettingForCgasing>().Find(this.Id);
            }

            #endregion
        }

        #region 监视对象

        /// <summary>
        /// 用于监视改变投注记录的当前状态动作的对象
        /// </summary>
        public class ChangeStatusEventArgs : NEventArgs
        {
            #region 公开属性

            /// <summary>
            /// 原状态
            /// </summary>
            public BettingStatus OldStatus { get; set; }

            /// <summary>
            /// 新状态
            /// </summary>
            public BettingStatus NewStatus { get; set; }

            /// <summary>
            /// 奖金
            /// </summary>
            public double Bonus { get; set; }

            #endregion

            #region 构造方法

            /// <summary>
            /// 实例化一个新的用于监视改变投注记录的当前状态动作的对象
            /// </summary>
            /// <param name="db">数据库连接对象</param>
            /// <param name="state">参数实体</param>
            /// <param name="oldStatus">原状态</param>
            /// <param name="newStatus">新状态</param>
            /// <param name="bonus">奖金</param>
            public ChangeStatusEventArgs(DbContext db, object state, BettingStatus oldStatus, BettingStatus newStatus, double bonus)
                : base(db, state)
            {
                this.OldStatus = oldStatus;
                this.NewStatus = newStatus;
                this.Bonus = bonus;
            }

            #endregion
        }

        #endregion

        #region 内置委托

        /// <summary>
        /// 改变投注记录的当前状态动作的实例委托
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public delegate void ChangeStatusDelegate(object sender, ChangeStatusEventArgs e);

        #endregion

        #endregion

        #region 有关事件

        /// <summary>
        /// 改变投注记录的当前状态前将触发的事件
        /// </summary>
        public static event ChangeStatusDelegate ChangingStatusEventHandler;

        /// <summary>
        /// 改变投注记录的当前状态后将触发的事件
        /// </summary>
        public static event ChangeStatusDelegate ChangedStatusEventHandler;

        #endregion

        #region 静态方法

        /// <summary>
        /// 系统撤单
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void SystemCancellation(object sender, ChasingManager.ChangeStatusEventArgs e)
        {
            Chasing c = (Chasing)e.State;
            if (e.NewStatus == ChasingStatus.因为所追号码已经开出而终止
                || e.NewStatus == ChasingStatus.因为中奖而终止
                || e.NewStatus == ChasingStatus.用户中止追号
                || e.NewStatus == ChasingStatus.追号结束)
            {
                c.Bettings.Where(x => x.Status == BettingStatus.等待开奖)
                    .ToList()
                    .ForEach(x =>
                    {
                        x.Status = BettingStatus.用户撤单;
                        x.ModifiedTime = DateTime.Now;
                    });
            }
        }

        /// <summary>
        /// 反馈开奖结果
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void GetResultOfLottery(object sender, NEventArgs e)
        {
            Lottery lottery = (Lottery)e.State;
            BettingForCgasingManager bettingManager = new BettingForCgasingManager(e.Db);
            WebSetting webSetting = new WebSetting();

            e.Db.Set<BettingForCgasing>().Where(betting => betting.Chasing.HowToPlay.Tag.Ticket.Id == lottery.Ticket.Id
                && betting.Phases == lottery.Phases
                && betting.Status == BettingStatus.即将开奖)
                .ToList().ForEach(betting =>
                {
                    int sum = AwardManager.AwardByChasing(lottery, betting.Chasing);
                    if (sum == 0)
                    {
                        bettingManager.ChangeStatus(betting.Id, BettingStatus.未中奖);
                    }
                    else
                    {
                        double conversionRates = betting.Chasing.HowToPlay.ConversionRates == 0
                        ? webSetting.ConversionRates : betting.Chasing.HowToPlay.ConversionRates;
                        double cardinalNumber = betting.Chasing.HowToPlay.CardinalNumber == 0
                            ? webSetting.ReferenceBonusMode : betting.Chasing.HowToPlay.CardinalNumber;
                        double coefficient = (cardinalNumber + betting.Chasing.Points * conversionRates)
                                / webSetting.ReferenceBonusMode;
                        double odds = betting.Chasing.HowToPlay.Odds;
                        if (odds == 0)
                        {
                            int tPayoutBase = webSetting.PayoutBase / 2;
                            double tSum = 1000;
                            double _tSum = 1;
                            switch (betting.Chasing.HowToPlay.Interface)
                            {
                                case LotteryInterface.任N组选:
                                    #region 组选
                                    int _tNum = betting.Chasing.HowToPlay.Seats.FirstOrDefault().ValueList.Count;
                                    if (betting.Chasing.HowToPlay.Name.Contains("组三"))
                                    {
                                        #region 组三
                                        _tSum = _tNum * (_tNum - 1);
                                        #endregion
                                    }
                                    else if (betting.Chasing.HowToPlay.Name.Contains("组六"))
                                    {
                                        #region 组六
                                        _tSum *= _tNum < 3 ? 0 : DigitalHelper.GetFactorialIn0To12(_tNum)
                                            / (DigitalHelper.GetFactorialIn0To12(3) * DigitalHelper.GetFactorialIn0To12(_tNum - 3));
                                        #endregion
                                    }
                                    else if (betting.Chasing.HowToPlay.Name.Contains("组选"))
                                    {
                                        #region 二星组选
                                        _tSum *= _tNum < 2 ? 0 : DigitalHelper.GetFactorialIn0To12(_tNum)
                                            / (DigitalHelper.GetFactorialIn0To12(2) * DigitalHelper.GetFactorialIn0To12(_tNum - 2));
                                        #endregion
                                    }
                                    #endregion
                                    break;
                                case LotteryInterface.任N直选:
                                    #region 直选
                                    betting.Chasing.HowToPlay.Seats.ForEach(x =>
                                    {
                                        _tSum *= x.ValueList.Count;
                                    });

                                    #endregion
                                    break;
                                case LotteryInterface.任N不定位:
                                case LotteryInterface.任N定位胆:
                                    #region 不定位/定位胆
                                    _tSum = betting.Chasing.HowToPlay.Seats.FirstOrDefault().ValueList.Count;
                                    #endregion
                                    break;
                            }
                            odds = tPayoutBase * _tSum / tSum;
                        }
                        odds *= coefficient;
                        double bonus = sum * odds * betting.Chasing.Multiple;
                        if (bonus > webSetting.MaximumPayout)
                        {
                            bonus = webSetting.MaximumPayout;
                        }
                        bettingManager.ChangeStatus(betting.Id, BettingStatus.中奖, bonus);
                    }
                });
        }

        /// <summary>
        /// 更新投注记录的状态
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void UpdateBettingStatus(object sender, NEventArgs e)
        {
            WebSetting webSetting = new WebSetting();
            var bSet = e.Db.Set<BettingForCgasing>();
            e.Db.Set<LotteryTicket>().ToList().ForEach(ticket =>
            {
                DateTime nextTime = ticket.NextLotteryTime.AddSeconds(-webSetting.ClosureSingleTime);
                if (DateTime.Now > nextTime)
                {
                    BettingForCgasingManager bettingManager = new BettingForCgasingManager(e.Db);
                    bSet.Where(betting => betting.Phases == ticket.NextPhases
                        && betting.Status == BettingStatus.等待开奖)
                        .ToList()
                        .ForEach(betting =>
                        {
                            bettingManager.ChangeStatus(betting.Id, BettingStatus.即将开奖);
                        });
                }
            });
            e.Db.SaveChanges();
        }

        #endregion
    }
}

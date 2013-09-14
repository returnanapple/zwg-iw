using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Setting;
using IWorld.Helper;

namespace IWorld.BLL
{
    /// <summary>
    /// 投注记录的管理者对象
    /// </summary>
    public class BettingManager : SimplifyManagerBase<Betting>, IManager<Betting>, ISimplify<Betting>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的投注记录的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public BettingManager(DbContext db)
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
            NChecker.CheckEntity<Betting>(bettingId, "投注记录", db);
            Betting betting = db.Set<Betting>().Find(bettingId);
            if (newStatus == BettingStatus.用户撤单
                && betting.Status != BettingStatus.等待开奖)
            {
                throw new Exception("不处于“等待开奖”状态中的投注禁止撤单");
            }
            if (betting.Status == BettingStatus.未中奖
                || betting.Status == BettingStatus.中奖)
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
        /// 内部工厂
        /// </summary>
        public class Factory
        {
            #region 静态方法

            /// <summary>
            /// 创建一个用于新建投注记录的数据集
            /// </summary>
            /// <param name="ownerId">投注人的存储指针</param>
            /// <param name="phases">期数</param>
            /// <param name="sum">注数</param>
            /// <param name="multiple">倍数</param>
            /// <param name="points">用于转换为赔率的点数</param>
            /// <param name="howToPlayId">玩法的存储指针</param>
            /// <param name="seats">位</param>
            /// <param name="pay">投注金额</param>
            /// <returns>返回用于新建投注记录的数据集</returns>
            public static ICreatePackage<Betting> CreatePackageForCreate(int ownerId, string phases, int sum, double multiple
                , double points, int howToPlayId, List<IPackageForSeat> seats, double pay)
            {
                return new PackageForCreate(ownerId, phases, sum, multiple, points, howToPlayId, seats, pay);
            }

            /// <summary>
            /// 创建一个用于新建位的数据集
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">对应的号码</param>
            /// <returns>返回</returns>
            public static IPackageForSeat CreatePackageForSeat(string name, string value)
            {
                return new PackageForSeat(name, value);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建投注记录的数据集
            /// </summary>
            private class PackageForCreate : IPackage<Betting>, ICreatePackage<Betting>
            {
                #region 公开属性

                /// <summary>
                /// 投注人的存储指针
                /// </summary>
                public int OwnerId { get; set; }

                /// <summary>
                /// 期数
                /// </summary>
                public string Phases { get; set; }

                /// <summary>
                /// 注数
                /// </summary>
                public int Sum { get; set; }

                /// <summary>
                /// 倍数
                /// </summary>
                public double Multiple { get; set; }

                /// <summary>
                /// 用于转换为赔率的点数
                /// </summary>
                public double Points { get; set; }

                /// <summary>
                /// 玩法的存储指针
                /// </summary>
                public int HowToPlayId { get; set; }

                /// <summary>
                /// 位
                /// </summary>
                public List<IPackageForSeat> Seats { get; set; }

                /// <summary>
                /// 投注金额
                /// </summary>
                public double Pay { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建投注记录的数据集
                /// </summary>
                /// <param name="ownerId">投注人的存储指针</param>
                /// <param name="phases">期数</param>
                /// <param name="sum">注数</param>
                /// <param name="multiple">倍数</param>
                /// <param name="points">用于转换为赔率的点数</param>
                /// <param name="howToPlayId">玩法的存储指针</param>
                /// <param name="seats">位</param>
                /// <param name="pay">投注金额</param>
                public PackageForCreate(int ownerId, string phases, int sum, double multiple, double points, int howToPlayId
                    , List<IPackageForSeat> seats, double pay)
                {
                    this.OwnerId = ownerId;
                    this.Phases = phases;
                    this.Sum = sum;
                    this.Multiple = multiple;
                    this.Points = points;
                    this.HowToPlayId = howToPlayId;
                    this.Seats = seats;
                    this.Pay = pay;
                }

                #endregion

                #region  实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    WebSetting webSetting = new WebSetting();
                    if (this.Sum < 1)
                    {
                        throw new Exception("最少投注额为 1 注");
                    }
                    NChecker.CheckEntity<Author>(this.OwnerId, "用户", db);
                    NChecker.CheckEntity<HowToPlay>(this.HowToPlayId, "玩法", db);
                    var user = (from c in db.Set<Author>()
                                where c.Id == this.OwnerId
                                select new
                                {
                                    c.NormalReturnPoints,
                                    c.UncertainReturnPoints,
                                    c.Money
                                })
                                .FirstOrDefault();
                    var howToPlay = db.Set<HowToPlay>().FirstOrDefault(x => x.Id == this.HowToPlayId);
                    double returnPoints = 0;
                    switch (howToPlay.Interface)
                    {
                        case LotteryInterface.任N不定位:
                            returnPoints = user.UncertainReturnPoints;
                            break;
                        default:
                            returnPoints = user.NormalReturnPoints;
                            break;
                    }
                    if (this.Pay > user.Money)
                    {
                        throw new Exception("资金余额不足");
                    }
                    if (this.Points > returnPoints)
                    {
                        throw new Exception("声明所要用于转换为赔率的点数大于用户本身拥有的最大返点数");
                    }
                    bool hadLottery = db.Set<Lottery>().Any(x => x.Phases == this.Phases && x.Ticket.Id == howToPlay.Tag.Ticket.Id);
                    if (hadLottery)
                    {
                        throw new Exception("已经开奖 不能投注");
                    }
                    bool hadClosure = DateTime.Now > howToPlay.Tag.Ticket.NextLotteryTime.AddMinutes(-webSetting.ClosureSingleTime);
                    if (hadClosure)
                    {
                        throw new Exception("已经封单 不能投注");
                    }
                    this.Seats.ForEach(x =>
                        {
                            x.CheckSeat(howToPlay);
                        });
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public Betting GetEntity(DbContext db)
                {
                    Author owner = db.Set<Author>().Find(this.OwnerId);
                    HowToPlay howToPlay = db.Set<HowToPlay>().Find(this.HowToPlayId);
                    List<BettingSeat> seats = new List<BettingSeat>();
                    this.Seats.ForEach(x =>
                        {
                            seats.Add(x.GetSeat(howToPlay));
                        });

                    return new Betting(owner, this.Phases, this.Sum, this.Multiple, this.Points, howToPlay, seats, this.Pay);
                }

                #endregion
            }

            /// <summary>
            /// 用于新建位的数据集
            /// </summary>
            private class PackageForSeat : IPackageForSeat
            {
                #region 公开属性

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 对应的号码的集合
                /// </summary>
                public string Values { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建位的数据集
                /// </summary>
                /// <param name="name">名称</param>
                /// <param name="values">对应的号码的集合</param>
                public PackageForSeat(string name, string values)
                {
                    this.Name = name;
                    this.Values = values;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查位信息的合法性
                /// </summary>
                /// <param name="howToPlay">玩法</param>
                public void CheckSeat(HowToPlay howToPlay)
                {
                    if (howToPlay.Parameter1 == 1)
                    {
                        if (!howToPlay.Seats.Any(x => x.Name == this.Name))
                        {
                            throw new Exception(string.Format("位：{0} 并不属于玩法：{1}（{2}-{3}）", this.Name
                                , howToPlay.Name, howToPlay.Tag.Ticket.Name, howToPlay.Tag.Name));
                        }
                        var seat = howToPlay.Seats.FirstOrDefault(x => x.Name == this.Name);
                        List<string> tValues = this.Values.Split(new char[] { ',' }).ToList();
                        if (tValues.Count < seat.LimitOfPick
                            || tValues.Count > seat.UpperOfPick)
                        {
                            throw new Exception(string.Format("选号数量（{0}）超出规定范围：{1} - {2}"
                                , tValues.Count, seat.LimitOfPick, seat.UpperOfPick));
                        }
                        tValues.ForEach(value =>
                            {
                                if (!seat.ValueList.Any(x => x == value))
                                {
                                    throw new Exception(string.Format("位：{0} 中并不包括值：{1}", this.Name, value));
                                }
                            });
                    }
                }

                /// <summary>
                /// 获取位（投注记录）
                /// </summary>
                /// <param name="howToPlay">玩法</param>
                /// <returns>返回位（投注记录）的封装</returns>
                public BettingSeat GetSeat(HowToPlay howToPlay)
                {
                    var seat = howToPlay.Tag.Ticket.Seats.FirstOrDefault(x => x.Name == this.Name);

                    return new BettingSeat(this.Name, this.Values, seat.Order);
                }

                #endregion
            }

            #endregion
        }

        /// <summary>
        /// 用于移除投注记录的数据集
        /// </summary>
        private class PackageForRemove : IPackage<Betting>, IRemovePackage<Betting>
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
                NChecker.CheckEntity<Betting>(this.Id, "投注记录", db);
                var t = (from c in db.Set<Betting>()
                         where c.Id == this.Id
                         select new
                         {
                             c.Status,
                             c.HowToPlay.Tag.Ticket
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
            public Betting GetEntity(DbContext db)
            {
                return db.Set<Betting>().Find(this.Id);
            }

            #endregion
        }

        #region 内嵌接口

        /// <summary>
        /// 定义用于新建位的数据集
        /// </summary>
        public interface IPackageForSeat
        {
            /// <summary>
            /// 检查位信息的合法性
            /// </summary>
            /// <param name="howToPlay">玩法</param>
            void CheckSeat(HowToPlay howToPlay);

            /// <summary>
            /// 获取位（投注记录）
            /// </summary>
            /// <param name="howToPlay">玩法</param>
            /// <returns>返回位（投注记录）的封装</returns>
            BettingSeat GetSeat(HowToPlay howToPlay);
        }

        #endregion

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
        /// 反馈开奖结果
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void GetResultOfLottery(object sender, NEventArgs e)
        {
            Lottery lottery = (Lottery)e.State;
            BettingManager bettingManager = new BettingManager(e.Db);
            WebSetting webSetting = new WebSetting();

            e.Db.Set<Betting>().Where(betting => betting.HowToPlay.Tag.Ticket.Id == lottery.Ticket.Id
                && betting.Phases == lottery.Phases
                && (betting.Status == BettingStatus.即将开奖 || betting.Status == BettingStatus.等待开奖))
                .ToList().ForEach(betting =>
            {
                int sum = AwardManager.AwardByBetting(lottery, betting);
                if (sum == 0)
                {
                    bettingManager.ChangeStatus(betting.Id, BettingStatus.未中奖);
                }
                else
                {
                    double conversionRates = betting.HowToPlay.ConversionRates == 0
                        ? webSetting.ConversionRates : betting.HowToPlay.ConversionRates;
                    double cardinalNumber = betting.HowToPlay.CardinalNumber == 0
                        ? webSetting.PayoutBase : betting.HowToPlay.CardinalNumber;
                    double coefficient = (cardinalNumber + betting.Points * conversionRates) / webSetting.PayoutBase;
                    double odds = betting.HowToPlay.Odds;
                    if (odds == 0)
                    {
                        double tSum = 1000;
                        double _tSum = 1;
                        switch (betting.HowToPlay.Interface)
                        {
                            case LotteryInterface.任N组选:
                                #region 组选
                                int _tNum = betting.HowToPlay.Seats.FirstOrDefault().ValueList.Count;
                                if (betting.HowToPlay.Name.Contains("组三"))
                                {
                                    #region 组三
                                    _tSum = _tNum * (_tNum - 1);
                                    #endregion
                                }
                                else if (betting.HowToPlay.Name.Contains("组六"))
                                {
                                    #region 组六
                                    _tSum *= _tNum < 3 ? 0 : DigitalHelper.GetFactorialIn0To12(_tNum)
                                        / (DigitalHelper.GetFactorialIn0To12(3) * DigitalHelper.GetFactorialIn0To12(_tNum - 3));
                                    #endregion
                                }
                                else if (betting.HowToPlay.Name.Contains("组选"))
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
                                betting.HowToPlay.Seats.ForEach(x =>
                                    {
                                        _tSum *= x.ValueList.Count;
                                    });

                                #endregion
                                break;
                            case LotteryInterface.任N不定位:
                            case LotteryInterface.任N定位胆:
                                #region 不定位/定位胆
                                _tSum = betting.HowToPlay.Seats.First().ValueList.Count;
                                #endregion
                                break;
                        }
                        odds = webSetting.ReferenceBonusMode * _tSum / tSum;
                    }
                    odds = odds * coefficient;
                    double bonus = sum * odds * betting.Multiple;
                    if (bonus > webSetting.MaximumPayout)
                    {
                        bonus = webSetting.MaximumPayout;
                    }
                    bettingManager.ChangeStatus(betting.Id, BettingStatus.中奖, bonus);
                }
            });
            e.Db.SaveChanges();
        }

        /// <summary>
        /// 更新投注记录的状态
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void UpdateBettingStatus(object sender, NEventArgs e)
        {
            WebSetting webSetting = new WebSetting();
            var bSet = e.Db.Set<Betting>();
            e.Db.Set<LotteryTicket>().ToList().ForEach(ticket =>
                {
                    DateTime nextTime = ticket.NextLotteryTime.AddMinutes(-webSetting.ClosureSingleTime);
                    if (DateTime.Now > nextTime)
                    {
                        BettingManager bettingManager = new BettingManager(e.Db);
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

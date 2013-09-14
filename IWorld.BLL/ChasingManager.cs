using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 追号记录的管理者对象
    /// </summary>
    public class ChasingManager : SimplifyManagerBase<Chasing>, IManager<Chasing>, ISimplify<Chasing>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的追号记录的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public ChasingManager(DbContext db)
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
        /// 改变追号记录的当前状态
        /// </summary>
        /// <param name="chasingId">目标追号记录的存储指针</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="bonus">奖金</param>
        public void ChangeStatus(int chasingId, ChasingStatus newStatus, double bonus = 0)
        {
            if (newStatus == ChasingStatus.未开始)
            {
                throw new Exception("不允许将任何记录修改为“未开始”状态");
            }
            NChecker.CheckEntity<Chasing>(chasingId, "追号记录", db);
            Chasing chasing = db.Set<Chasing>().Find(chasingId);
            if (chasing.Status == ChasingStatus.因为所追号码已经开出而终止
                || chasing.Status == ChasingStatus.因为中奖而终止
                || chasing.Status == ChasingStatus.用户中止追号
                || chasing.Status == ChasingStatus.追号结束)
            {
                throw new Exception("已经被终止的记录无法修改状态");
            }

            ChangeStatusEventArgs eventArgs = new ChangeStatusEventArgs(db, chasing, chasing.Status, newStatus, bonus);
            if (ChangingStatusEventHandler != null)
            {
                ChangingStatusEventHandler(this, eventArgs);    //触发前置事件
            }
            chasing.Status = newStatus;
            chasing.Bonus += bonus;
            chasing.ModifiedTime = DateTime.Now;
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
        public class Fanctory
        {
            #region 静态方法

            /// <summary>
            /// 创建一个用于新建追号记录的数据集
            /// </summary>
            /// <param name="ownerId">投注人的存储指针</param>
            /// <param name="postpone">顺延期数</param>
            /// <param name="continuance">持续</param>
            /// <param name="sum">注数</param>
            /// <param name="multiple">倍数</param>
            /// <param name="points">用于转换为赔率的点数</param>
            /// <param name="howToPlayId">玩法的存储指针</param>
            /// <param name="seats">位</param>
            /// <param name="bettings">投注记录</param>
            /// <param name="pay">投注总金额</param>
            /// <param name="endIfLotteryBeforeBegin">标识|如果在开始追号之前就开除号码 追号中止</param>
            /// <param name="endIfLotteryAtGoing">标识|如果在开始追号过程中中奖 追号中止</param>
            /// <returns>返回用于新建追号记录的数据集</returns>
            public static ICreatePackage<Chasing> CreatePackageForCreate(int ownerId, int postpone, int continuance, int sum
                , double multiple, double points, int howToPlayId, List<IPackageForSeat> seats, List<IPackageForBetting> bettings
                , double pay, bool endIfLotteryBeforeBegin, bool endIfLotteryAtGoing)
            {
                return new PackageForCreate(ownerId, postpone, continuance, sum, multiple, points, howToPlayId, seats, bettings
                    , pay, endIfLotteryBeforeBegin, endIfLotteryAtGoing);
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

            /// <summary>
            /// 创建一个用于新建投注（追号）的数据集
            /// </summary>
            /// <param name="phases">期号</param>
            /// <param name="exponent">倍数指数</param>
            /// <param name="pay">投注金额</param>
            /// <returns>返回用于新建投注（追号）的数据集</returns>
            public static IPackageForBetting CreatePackageForBetting(string phases, double exponent, double pay)
            {
                return new PackageForBetting(phases, exponent, pay);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建追号记录的数据集
            /// </summary>
            private class PackageForCreate : IPackage<Chasing>, ICreatePackage<Chasing>
            {
                #region 公开属性

                /// <summary>
                /// 投注人的存储指针
                /// </summary>
                public int OwnerId { get; set; }

                /// <summary>
                /// 顺延期数
                /// </summary>
                public int Postpone { get; set; }

                /// <summary>
                /// 持续期数
                /// </summary>
                public int Continuance { get; set; }

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
                /// 投注记录
                /// </summary>
                public List<IPackageForBetting> Bettings { get; set; }

                /// <summary>
                /// 投注总金额
                /// </summary>
                public double Pay { get; set; }

                /// <summary>
                /// 标识|如果在开始追号之前就开出号码 追号中止
                /// </summary>
                public bool EndIfLotteryBeforeBegin { get; set; }

                /// <summary>
                /// 标识|如果在开始追号过程中中奖 追号中止
                /// </summary>
                public bool EndIfLotteryAtGoing { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建追号记录的数据集
                /// </summary>
                /// <param name="ownerId">投注人的存储指针</param>
                /// <param name="postpone">顺延期数</param>
                /// <param name="continuance">持续</param>
                /// <param name="sum">注数</param>
                /// <param name="multiple">倍数</param>
                /// <param name="points">用于转换为赔率的点数</param>
                /// <param name="howToPlayId">玩法的存储指针</param>
                /// <param name="seats">位</param>
                /// <param name="bettings">投注记录</param>
                /// <param name="pay">投注总金额</param>
                /// <param name="endIfLotteryBeforeBegin">标识|如果在开始追号之前就开除号码 追号中止</param>
                /// <param name="endIfLotteryAtGoing">标识|如果在开始追号过程中中奖 追号中止</param>
                public PackageForCreate(int ownerId, int postpone, int continuance, int sum, double multiple, double points
                    , int howToPlayId, List<IPackageForSeat> seats, List<IPackageForBetting> bettings, double pay
                    , bool endIfLotteryBeforeBegin, bool endIfLotteryAtGoing)
                {
                    this.OwnerId = ownerId;
                    this.Postpone = postpone;
                    this.Continuance = continuance;
                    this.Sum = sum;
                    this.Multiple = multiple;
                    this.Points = points;
                    this.HowToPlayId = howToPlayId;
                    this.Seats = seats;
                    this.Bettings = bettings;
                    this.Pay = pay;
                    this.EndIfLotteryBeforeBegin = endIfLotteryBeforeBegin;
                    this.EndIfLotteryAtGoing = endIfLotteryAtGoing;
                }

                #endregion

                #region  实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    if (this.Sum < 1)
                    {
                        throw new Exception("最少投注额为 1 注");
                    }
                    if (this.Postpone < 1)
                    {
                        throw new Exception("最少顺延期数为 1 期");
                    }
                    if (this.Continuance < 1)
                    {
                        throw new Exception("最少持续期数为 1 期");
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
                    if (this.Pay > user.Money)
                    {
                        throw new Exception("资金余额不足");
                    }
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
                    if (this.Points > returnPoints)
                    {
                        throw new Exception("声明所要用于转换为赔率的点数大于用户本身拥有的最大返点数");
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
                public Chasing GetEntity(DbContext db)
                {
                    Author owner = db.Set<Author>().Find(this.OwnerId);
                    HowToPlay howToPlay = db.Set<HowToPlay>().Find(this.HowToPlayId);
                    List<ChasingSeat> seats = this.Seats.ConvertAll(x => x.GetSeat(howToPlay));
                    List<BettingForCgasing> bettings = this.Bettings.ConvertAll(x => x.GetBetting());

                    return new Chasing(owner, this.Postpone, this.Continuance, this.Sum, this.Multiple, this.Points, howToPlay
                        , seats, bettings, this.Pay, this.EndIfLotteryBeforeBegin, this.EndIfLotteryAtGoing);
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
                    if (howToPlay.Parameter3 == 1)
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
                /// 获取位（追号记录）
                /// </summary>
                /// <param name="howToPlay">玩法</param>
                /// <returns>返回位（追号记录）的封装</returns>
                public ChasingSeat GetSeat(HowToPlay howToPlay)
                {
                    var seat = howToPlay.Tag.Ticket.Seats.FirstOrDefault(x => x.Name == this.Name);

                    return new ChasingSeat(this.Name, this.Values, seat.Order);
                }

                #endregion
            }

            /// <summary>
            /// 用于新建投注（追号）的数据集
            /// </summary>
            private class PackageForBetting : IPackageForBetting
            {
                #region 公开属性

                /// <summary>
                /// 期数
                /// </summary>
                public string Phases { get; set; }

                /// <summary>
                /// 翻倍指数
                /// </summary>
                public double Exponent { get; set; }

                /// <summary>
                /// 投注金额
                /// </summary>
                public double Pay { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建投注（追号）的数据集
                /// </summary>
                /// <param name="phases">期数</param>
                /// <param name="exponent">翻倍指数</param>
                /// <param name="pay">投注金额</param>
                public PackageForBetting(string phases, double exponent, double pay)
                {
                    this.Phases = phases;
                    this.Exponent = exponent;
                    this.Pay = pay;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 获取投注（追号）
                /// </summary>
                /// <returns>返回投注（追号）的封装</returns>
                public BettingForCgasing GetBetting()
                {
                    return new BettingForCgasing(this.Phases, this.Exponent, this.Pay);
                }

                #endregion
            }

            #endregion

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
                /// 获取位（追号记录）
                /// </summary>
                /// <param name="howToPlay">玩法</param>
                /// <returns>返回位（追号记录）的封装</returns>
                ChasingSeat GetSeat(HowToPlay howToPlay);
            }

            /// <summary>
            /// 定义用于新建投注（追号）的数据集
            /// </summary>
            public interface IPackageForBetting
            {
                /// <summary>
                /// 获取投注（追号）
                /// </summary>
                /// <returns>返回投注（追号）的封装</returns>
                BettingForCgasing GetBetting();
            }

            #endregion
        }

        /// <summary>
        /// 用于移除追号记录的数据集
        /// </summary>
        private class PackageForRemove : IPackage<Chasing>, IRemovePackage<Chasing>
        {
            #region 公开属性

            /// <summary>
            /// 存储指针
            /// </summary>
            public int Id { get; set; }

            #endregion

            #region 构造方法

            /// <summary>
            /// 实例化一个新的用于移除追号记录的数据集
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
                NChecker.CheckEntity<Chasing>(this.Id, "追号记录", db);
                var t = (from c in db.Set<Chasing>()
                         where c.Id == this.Id
                         select new
                         {
                             c.Status,
                             c.HowToPlay.Tag.Ticket
                         })
                         .FirstOrDefault();
                switch (t.Status)
                {
                    case ChasingStatus.因为所追号码已经开出而终止:
                    case ChasingStatus.因为中奖而终止:
                    case ChasingStatus.用户中止追号:
                    case ChasingStatus.追号结束:
                        throw new Exception("已经结束的追号记录无法撤单");
                }
            }

            /// <summary>
            /// 获取实体对象
            /// </summary>
            /// <param name="db">数据库连接对象</param>
            /// <returns>返回泛型状态所规定的实体类</returns>
            public Chasing GetEntity(DbContext db)
            {
                return db.Set<Chasing>().Find(this.Id);
            }

            #endregion
        }

        #region 监视对象

        // <summary>
        /// 用于监视改变追号记录的当前状态动作的对象
        /// </summary>
        public class ChangeStatusEventArgs : NEventArgs
        {
            #region 公开属性

            /// <summary>
            /// 原状态
            /// </summary>
            public ChasingStatus OldStatus { get; set; }

            /// <summary>
            /// 新状态
            /// </summary>
            public ChasingStatus NewStatus { get; set; }

            /// <summary>
            /// 奖金
            /// </summary>
            public double Bonus { get; set; }

            #endregion

            #region 构造方法

            /// <summary>
            /// 实例化一个新的用于监视改变追号记录的当前状态动作的对象
            /// </summary>
            /// <param name="db">数据库连接对象</param>
            /// <param name="state">参数实体</param>
            /// <param name="oldStatus">原状态</param>
            /// <param name="newStatus">新状态</param>
            /// <param name="bonus">奖金</param>
            public ChangeStatusEventArgs(DbContext db, object state, ChasingStatus oldStatus, ChasingStatus newStatus, double bonus)
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
        /// 改变追号记录的当前状态动作的实例委托
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public delegate void ChangeStatusDelegate(object sender, ChangeStatusEventArgs e);

        #endregion

        #endregion

        #region 有关事件

        /// <summary>
        /// 改变追号记录的当前状态前将触发的事件
        /// </summary>
        public static event ChangeStatusDelegate ChangingStatusEventHandler;

        /// <summary>
        /// 改变追号记录的当前状态后将触发的事件
        /// </summary>
        public static event ChangeStatusDelegate ChangedStatusEventHandler;

        #endregion

        #region 静态方法

        /// <summary>
        /// 更新追号记录的状态
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void UpdateChasingStatus(object sender, NEventArgs e)
        {
            Lottery lottery = (Lottery)e.State;
            ChasingManager chasingManager = new ChasingManager(e.Db);
            var cSet = e.Db.Set<Chasing>();

            cSet.Where(chasing => chasing.Status == ChasingStatus.未开始
                && !chasing.Bettings.Any(betting => betting.Phases == lottery.Phases)
                && chasing.EndIfLotteryBeforeBegin == true)
                .ToList()
                .ForEach(chasing =>
                    {
                        int sum = AwardManager.AwardByChasing(lottery, chasing);
                        if (sum > 1)
                        {
                            chasingManager.ChangeStatus(chasing.Id, ChasingStatus.因为所追号码已经开出而终止);
                        }
                    });

            cSet.Where(chasing => chasing.Status == ChasingStatus.未开始
                && chasing.Bettings.Any(b => b.Phases == lottery.Phases))
                .ToList()
                .ForEach(chasing =>
                    {
                        int sum = AwardManager.AwardByChasing(lottery, chasing);
                        if (sum > 1 && chasing.EndIfLotteryAtGoing == true)
                        {
                            chasingManager.ChangeStatus(chasing.Id, ChasingStatus.因为中奖而终止);
                        }
                        else if (chasing.Continuance == 1)
                        {
                            chasingManager.ChangeStatus(chasing.Id, ChasingStatus.追号结束);
                        }
                        else
                        {
                            chasingManager.ChangeStatus(chasing.Id, ChasingStatus.追号中);
                        }
                    });

            cSet.Where(chasing => chasing.Status == ChasingStatus.追号中)
                .ToList()
                .ForEach(chasing =>
                    {
                        int tBettingCount = chasing.Bettings.Count(betting => betting.Status == BettingStatus.等待开奖);
                        if (tBettingCount == 0)
                        {
                            chasingManager.ChangeStatus(chasing.Id, ChasingStatus.追号结束);
                        }
                        else
                        {
                            int sum = AwardManager.AwardByChasing(lottery, chasing);
                            if (chasing.EndIfLotteryAtGoing == true)
                            {
                                chasingManager.ChangeStatus(chasing.Id, ChasingStatus.因为中奖而终止);
                            }
                        }
                    });
        }

        #endregion
    }
}

using IWorld.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using IWorld.Setting;

namespace IWorld.BLL
{
    /// <summary>
    /// 大白鲨游戏的投注记录的管理对象
    /// </summary>
    public class BettingOfJawManager : SimplifyManagerBase<BettingOfJaw>
    {
        #region 构造方法

        /// <summary>
        /// 大白鲨游戏的投注记录的管理对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public BettingOfJawManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="bettingId">订单的存储指针</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="bonus">奖金</param>
        public void ChangeStatus(int bettingId, BettingStatus newStatus, double bonus = 0)
        {
            if (newStatus == BettingStatus.等待开奖)
            {
                throw new Exception("不允许将订单状态改变为等待开奖");
            }
            NChecker.CheckEntity<BettingOfJaw>(bettingId, "投注记录", db);
            BettingOfJaw b = db.Set<BettingOfJaw>().Find(bettingId);
            List<BettingStatus> ignoreStatus = new List<BettingStatus> { BettingStatus.未中奖, BettingStatus.中奖 };
            if (ignoreStatus.Contains(b.Status))
            {
                throw new Exception("已经开奖的订单不允许修改");
            }

            if (b.Status == newStatus) { return; }
            ChangeStatusEventArgs e = new ChangeStatusEventArgs(db, b, b.Status, newStatus);

            if (ChangingStatusEventHandler != null)
            {
                ChangingStatusEventHandler(this, e);
            }

            b.Status = newStatus;
            b.Bonus = bonus;
            db.SaveChanges();

            if (ChangedStatusEventHandler != null)
            {
                ChangedStatusEventHandler(this, e);
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
            /// 创建一个用于新建大白鲨游戏的投注记录的数据集
            /// </summary>
            /// <param name="ownerId">用户</param>
            /// <param name="issue">期号</param>
            /// <param name="details">明细</param>
            /// <returns>返回用于新建大白鲨游戏的投注记录的数据集</returns>
            public static ICreatePackage<BettingOfJaw> CreatePackageForCreate(int ownerId, string issue
                , List<IPackageForCreateDetail> details)
            {
                return new PackageForCreate(ownerId, issue, details);
            }

            /// <summary>
            /// 创建一个用于新建大白鲨游戏的投注明细的数据集
            /// </summary>
            /// <param name="icon">目标标识</param>
            /// <param name="sum">投注金额</param>
            /// <returns>返回用于新建大白鲨游戏的投注明细的数据集</returns>
            public static IPackageForCreateDetail CreatePackageForCreateDetail(IconOfJaw icon, double sum)
            {
                return new PackageForCreateDetail(icon, sum);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建大白鲨游戏的投注记录的数据集
            /// </summary>
            class PackageForCreate : IPackage<BettingOfJaw>, ICreatePackage<BettingOfJaw>
            {
                #region 公开属性

                /// <summary>
                /// 用户
                /// </summary>
                public int OwnerId { get; set; }

                /// <summary>
                /// 期号
                /// </summary>
                public string Issue { get; set; }

                /// <summary>
                /// 明细
                /// </summary>
                public List<IPackageForCreateDetail> Details { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建大白鲨游戏的投注记录的数据集
                /// </summary>
                /// <param name="ownerId">用户</param>
                /// <param name="issue">期号</param>
                /// <param name="details">明细</param>
                public PackageForCreate(int ownerId, string issue, List<IPackageForCreateDetail> details)
                {
                    this.OwnerId = ownerId;
                    this.Issue = issue;
                    this.Details = details;
                }

                #endregion

                #region 方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    NChecker.CheckEntity<Author>(this.OwnerId, "用户", db);
                    bool hadLotteried = db.Set<LotteryOfJaw>().Any(x => x.Issue == this.Issue);
                    if (hadLotteried)
                    {
                        throw new Exception("已经开奖 不能投注");
                    }
                    MainOfJaw moj = db.Set<MainOfJaw>().First();
                    if (this.Issue != moj.NextPhases)
                    {
                        throw new Exception("目标期号不正确");
                    }
                    WebSetting webSetting = new WebSetting();
                    DateTime dt = moj.NextLotteryTime.AddSeconds(-webSetting.ClosureSingleTime);
                    if (DateTime.Now > dt)
                    {
                        throw new Exception("已经封单，禁止投注");
                    }
                    if (this.Details.Count <= 0)
                    {
                        throw new Exception("投注明细不能小于0");
                    }
                    int cDuplicate = this.Details.GroupBy(x => x.Icon).Count(x => x.Count() > 1);
                    if (cDuplicate > 0)
                    {
                        throw new Exception("投注明细指向的标识不能重复");
                    }
                    if (this.Details.Any(x => x.Sum < 0))
                    {
                        throw new Exception("投注金额不能小于0");
                    }
                    double aSum = this.Details.Sum(x => x.Sum);
                    double myMoney = db.Set<Author>().Where(x => x.Id == this.OwnerId).Select(x => x.Money).First();
                    if (aSum > myMoney)
                    {
                        throw new Exception("所要投注的金额大约用户本人的资金余额 投注无效");
                    }
                    List<IconOfJaw> icons = new List<IconOfJaw> { IconOfJaw.金色鲨鱼, IconOfJaw.通赔, IconOfJaw.通杀 };
                    if (this.Details.Any(x => icons.Contains(x.Icon)))
                    {
                        throw new Exception("目标标识为不能投注的图标 投注无效");
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public BettingOfJaw GetEntity(DbContext db)
                {
                    List<BettingDetailOfJaw> bdojs = this.Details.ConvertAll(d =>
                        {
                            MarkOfJaw moj = db.Set<MarkOfJaw>().First(x => x.Icon == d.Icon);
                            return new BettingDetailOfJaw(moj, d.Sum);
                        });
                    Author owner = db.Set<Author>().Find(this.OwnerId);
                    return new BettingOfJaw(owner, this.Issue, bdojs);
                }

                #endregion
            }

            /// <summary>
            /// 用于新建大白鲨游戏的投注明细的数据集
            /// </summary>
            class PackageForCreateDetail : IPackageForCreateDetail
            {
                #region 属性

                /// <summary>
                /// 目标标识
                /// </summary>
                public IconOfJaw Icon { get; set; }

                /// <summary>
                /// 投注金额
                /// </summary>
                public double Sum { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建大白鲨游戏的投注明细的数据集
                /// </summary>
                /// <param name="icon">目标标识</param>
                /// <param name="sum">投注金额</param>
                public PackageForCreateDetail(IconOfJaw icon, double sum)
                {
                    this.Icon = icon;
                    this.Sum = sum;
                }

                #endregion
            }

            #endregion
        }

        /// <summary>
        /// 定义用于创建投注明细的数据集
        /// </summary>
        public interface IPackageForCreateDetail
        {
            /// <summary>
            /// 目标标识
            /// </summary>
            IconOfJaw Icon { get; set; }

            /// <summary>
            /// 投注金额
            /// </summary>
            double Sum { get; set; }
        }

        #region 监视对象

        /// <summary>
        /// 改变状态动作的监视者对象
        /// </summary>
        public class ChangeStatusEventArgs : NEventArgs
        {
            #region 属性

            /// <summary>
            /// 原状态
            /// </summary>
            public BettingStatus OldStatus { get; set; }

            /// <summary>
            /// 新状态
            /// </summary>
            public BettingStatus NewStatus { get; set; }

            #endregion

            #region 构造方法

            /// <summary>
            /// 实例化一个新的改变状态动作的监视者对象
            /// </summary>
            /// <param name="db">数据库连接对象</param>
            /// <param name="state">参数实体</param>
            /// <param name="oldStatus">原状态</param>
            /// <param name="newStatus">新状态</param>
            public ChangeStatusEventArgs(DbContext db, object state, BettingStatus oldStatus, BettingStatus newStatus)
                : base(db, state)
            {
                this.OldStatus = oldStatus;
                this.NewStatus = newStatus;
            }

            #endregion
        }

        #endregion

        #region 委托

        /// <summary>
        /// 关于改变状态动作的委托
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public delegate void ChangeStatusDelegate(object sender, ChangeStatusEventArgs e);

        #endregion

        #endregion

        #region 事件

        /// <summary>
        /// 改变订单状态之前将触发的事件
        /// </summary>
        public static event ChangeStatusDelegate ChangingStatusEventHandler;

        /// <summary>
        /// 改变订单状态之后将触发的事件
        /// </summary>
        public static event ChangeStatusDelegate ChangedStatusEventHandler;

        #endregion
    }
}

using System;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 默认活动的参与记录的管理者对象
    /// </summary>
    public class ActivityParticipateRecordManager : SimplifyManagerBase<ActivityParticipateRecord>
        , IManager<ActivityParticipateRecord>, ISimplify<ActivityParticipateRecord>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的默认活动的参与记录的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public ActivityParticipateRecordManager(DbContext db)
            : base(db)
        {
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
            /// 创建一个用于新建默认活动的参与记录的数据集
            /// </summary>
            /// <param name="ownerId">参与人的存储指针</param>
            /// <param name="activityId">参与的活动的存储指针</param>
            /// <param name="amount">目标对象的数额</param>
            /// <returns>返回用于新建默认活动的参与记录的数据集</returns>
            public static ICreatePackage<ActivityParticipateRecord> CreatePackageForCreate(int ownerId, int activityId, double amount = 0)
            {
                return new PackageForCreate(ownerId, activityId, amount);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建默认活动的参与记录的数据集
            /// </summary>
            private class PackageForCreate : IPackage<ActivityParticipateRecord>, ICreatePackage<ActivityParticipateRecord>
            {
                #region 公开属性

                /// <summary>
                /// 参与人的存储指针
                /// </summary>
                public int OwnerId { get; set; }

                /// <summary>
                /// 参与的活动的存储指针
                /// </summary>
                public int ActivityId { get; set; }

                /// <summary>
                /// 目标对象的数额
                /// </summary>
                public double Amount { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建默认活动的参与记录的数据集
                /// </summary>
                /// <param name="ownerId">参与人的存储指针</param>
                /// <param name="activityId">参与的活动的存储指针</param>
                /// <param name="amount">目标对象的数额</param>
                public PackageForCreate(int ownerId, int activityId, double amount)
                {
                    this.OwnerId = ownerId;
                    this.ActivityId = activityId;
                    this.Amount = amount;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    NChecker.CheckEntity<Author>(this.OwnerId, "用户", db);
                    NChecker.CheckEntity<Activity>(this.ActivityId, "活动", db);
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public ActivityParticipateRecord GetEntity(DbContext db)
                {
                    Author owner = db.Set<Author>().Find(this.OwnerId);
                    Activity activity = db.Set<Activity>().Find(this.ActivityId);
                    activity.Conditions.ForEach(x =>
                        {
                            x.Audit(owner);
                        });

                    return new ActivityParticipateRecord(owner, activity, this.Amount);
                }

                #endregion
            }

            #endregion
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 用户注册时候自动创建活动参与记录（假如有相关活动的话）
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void CreateRecordWhenUserCreated(object sender, NEventArgs e)
        {
            Author owner = (Author)e.State;
            ActivityParticipateRecordManager aprManager = new ActivityParticipateRecordManager(e.Db);
            e.Db.Set<Activity>().Where(activity => activity.Type == ActivityType.注册返点
                && activity.Hide == false
                && activity.BeginTime < DateTime.Now
                && activity.EndTime > DateTime.Now)
                .ToList()
                .ForEach(activity =>
                    {
                        ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                            .CreatePackageForCreate(owner.Id, activity.Id);
                        aprManager.Create(pfc);
                    });
        }

        /// <summary>
        /// 成功充值时候自动创建活动参与记录（假如有相关活动的话）
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void CreateRecordWhenRecharge(object sender, RechargeRecordManager.ChangeStatusEventArgs e)
        {
            if (e.NewStatus == RechargeStatus.充值成功)
            {
                RechargeRecord rechargeRecord = (RechargeRecord)e.State;
                ActivityParticipateRecordManager aprManager = new ActivityParticipateRecordManager(e.Db);
                var aSet = e.Db.Set<Activity>();
                aSet.Where(activity => activity.Type == ActivityType.充值返点
                    && activity.MinRestrictionValue <= rechargeRecord.Sum
                    && activity.MaxRestrictionValues >= rechargeRecord.Sum
                    && activity.Hide == false
                    && activity.BeginTime < DateTime.Now
                    && activity.EndTime > DateTime.Now)
                    .ToList()
                    .ForEach(activity =>
                        {
                            int _c = e.Db.Set<RechargeRecord>().Count(x => x.Owner.Id == rechargeRecord.Owner.Id);
                            if (_c > 1) { return; }
                            ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                                .CreatePackageForCreate(rechargeRecord.Owner.Id, activity.Id, rechargeRecord.Sum);
                            aprManager.Create(pfc);
                        });
            }
        }

        /// <summary>
        /// 封单时候自动创建相应的消费活动记录（假如有相关活动的话）
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void CreateRecordWhenBetting(object sender, BettingManager.ChangeStatusEventArgs e)
        {
            if (e.NewStatus == BettingStatus.即将开奖)
            {
                Betting betting = (Betting)e.State;
                ActivityParticipateRecordManager aprManager = new ActivityParticipateRecordManager(e.Db);
                var aSet = e.Db.Set<Activity>();
                aSet.Where(activity => activity.Type == ActivityType.消费返点
                    && activity.MinRestrictionValue <= betting.Pay
                    && activity.MaxRestrictionValues >= betting.Pay
                    && activity.Hide == false
                    && activity.BeginTime < DateTime.Now
                    && activity.EndTime > DateTime.Now)
                    .ToList()
                    .ForEach(activity =>
                    {
                        ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                            .CreatePackageForCreate(betting.Owner.Id, activity.Id, betting.Pay);
                        aprManager.Create(pfc);
                    });
                if (betting.Owner.Layer > 1)
                {
                    Author parent = new AuthorManager(e.Db).GetParent(betting.Owner);
                    aSet.Where(activity => activity.Type == ActivityType.下级用户消费返点
                        && activity.MinRestrictionValue <= betting.Pay
                        && activity.MaxRestrictionValues >= betting.Pay
                        && activity.Hide == false
                        && activity.BeginTime < DateTime.Now
                        && activity.EndTime > DateTime.Now)
                        .ToList()
                        .ForEach(activity =>
                        {
                            ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                                .CreatePackageForCreate(parent.Id, activity.Id, betting.Pay);
                            aprManager.Create(pfc);
                        });
                    if (betting.Owner.Layer > 2)
                    {
                        Author _parent = new AuthorManager(e.Db).GetParent(parent);
                        aSet.Where(activity => activity.Type == ActivityType.下下级用户消费返点
                            && activity.MinRestrictionValue <= betting.Pay
                            && activity.MaxRestrictionValues >= betting.Pay
                            && activity.Hide == false
                            && activity.BeginTime < DateTime.Now
                            && activity.EndTime > DateTime.Now)
                            .ToList()
                            .ForEach(activity =>
                            {
                                ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                                    .CreatePackageForCreate(_parent.Id, activity.Id, betting.Pay);
                                aprManager.Create(pfc);
                            });
                    }
                }
            }
        }

        /// <summary>
        /// 开奖之后自动创建相应的亏损补贴活动记录（假如有相关活动的话）
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void CreateRecordWhenLottery(object sender, BettingManager.ChangeStatusEventArgs e)
        {
            if (e.NewStatus == BettingStatus.未中奖)
            {
                Betting betting = (Betting)e.State;
                ActivityParticipateRecordManager aprManager = new ActivityParticipateRecordManager(e.Db);
                var aSet = e.Db.Set<Activity>();
                aSet.Where(activity => activity.Type == ActivityType.亏损返点
                    && activity.MinRestrictionValue <= betting.Pay
                    && activity.MaxRestrictionValues >= betting.Pay
                    && activity.Hide == false
                    && activity.BeginTime < DateTime.Now
                    && activity.EndTime > DateTime.Now)
                    .ToList()
                    .ForEach(activity =>
                    {
                        ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                            .CreatePackageForCreate(betting.Owner.Id, activity.Id, betting.Pay);
                        aprManager.Create(pfc);
                    });
                if (betting.Owner.Layer > 1)
                {
                    Author parent = new AuthorManager(e.Db).GetParent(betting.Owner);
                    aSet.Where(activity => activity.Type == ActivityType.下级用户亏损返点
                        && activity.MinRestrictionValue <= betting.Pay
                        && activity.MaxRestrictionValues >= betting.Pay
                        && activity.Hide == false
                        && activity.BeginTime < DateTime.Now
                        && activity.EndTime > DateTime.Now)
                        .ToList()
                        .ForEach(activity =>
                        {
                            ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                                .CreatePackageForCreate(parent.Id, activity.Id, betting.Pay);
                            aprManager.Create(pfc);
                        });
                    if (betting.Owner.Layer > 2)
                    {
                        Author _parent = new AuthorManager(e.Db).GetParent(parent);
                        aSet.Where(activity => activity.Type == ActivityType.下下级用户亏损返点
                            && activity.MinRestrictionValue <= betting.Pay
                            && activity.MaxRestrictionValues >= betting.Pay
                            && activity.Hide == false
                            && activity.BeginTime < DateTime.Now
                            && activity.EndTime > DateTime.Now)
                            .ToList()
                            .ForEach(activity =>
                            {
                                ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                                    .CreatePackageForCreate(_parent.Id, activity.Id, betting.Pay);
                                aprManager.Create(pfc);
                            });
                    }
                }
            }
        }

        /// <summary>
        /// 追号终止时自动创建消费活动（假如有相关活动的话）
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void CreateRecordWhenChasing(object sender, ChasingManager.ChangeStatusEventArgs e)
        {
            if (e.NewStatus == ChasingStatus.追号结束
                || e.NewStatus == ChasingStatus.因为中奖而终止
                || e.NewStatus == ChasingStatus.用户中止追号)
            {
                Chasing chasing = (Chasing)e.State;
                ActivityParticipateRecordManager aprManager = new ActivityParticipateRecordManager(e.Db);
                var aSet = e.Db.Set<Activity>();
                double sum = chasing.Bettings.Where(x => x.Status == BettingStatus.即将开奖
                    || x.Status == BettingStatus.未中奖
                    || x.Status == BettingStatus.中奖)
                    .Sum(x => x.Pay);
                aSet.Where(activity => activity.Type == ActivityType.消费返点
                    && activity.MinRestrictionValue <= chasing.Pay
                    && activity.MaxRestrictionValues >= chasing.Pay
                    && activity.Hide == false
                    && activity.BeginTime < DateTime.Now
                    && activity.EndTime > DateTime.Now)
                    .ToList()
                    .ForEach(activity =>
                    {
                        ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                            .CreatePackageForCreate(chasing.Owner.Id, activity.Id, sum);
                        aprManager.Create(pfc);
                    });
                if (chasing.Owner.Layer > 1)
                {
                    Author parent = new AuthorManager(e.Db).GetParent(chasing.Owner);
                    aSet.Where(activity => activity.Type == ActivityType.下级用户消费返点
                        && activity.MinRestrictionValue <= chasing.Pay
                        && activity.MaxRestrictionValues >= chasing.Pay
                        && activity.Hide == false
                        && activity.BeginTime < DateTime.Now
                        && activity.EndTime > DateTime.Now)
                        .ToList()
                        .ForEach(activity =>
                        {
                            ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                                .CreatePackageForCreate(parent.Id, activity.Id, sum);
                            aprManager.Create(pfc);
                        });
                    if (chasing.Owner.Layer > 2)
                    {
                        Author _parent = new AuthorManager(e.Db).GetParent(parent);
                        aSet.Where(activity => activity.Type == ActivityType.下级用户消费返点
                            && activity.MinRestrictionValue <= chasing.Pay
                            && activity.MaxRestrictionValues >= chasing.Pay
                            && activity.Hide == false
                            && activity.BeginTime < DateTime.Now
                            && activity.EndTime > DateTime.Now)
                            .ToList()
                            .ForEach(activity =>
                            {
                                ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                                    .CreatePackageForCreate(_parent.Id, activity.Id, sum);
                                aprManager.Create(pfc);
                            });
                    }
                }
            }
        }

        /// <summary>
        /// 在当日统计更新后自动创建跟统计额有关的活动
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void CreateRecordWhenCountedForDay(object sender, NEventArgs e)
        {
            PersonalDataAtDay pd = (PersonalDataAtDay)e.State;
            ActivityParticipateRecordManager aprManager = new ActivityParticipateRecordManager(e.Db);
            var aSet = e.Db.Set<Activity>();
            #region 消费额相关

            int _dayOfWeek = (int)DateTime.Now.DayOfWeek;
            ActivityType _type_ = (_dayOfWeek == 6 || _dayOfWeek == 0)
                ? ActivityType.当日累计消费奖励_周末 : ActivityType.当日累计消费奖励_非周末;
            aSet.Where(x => x.Type == _type_
                && x.MinRestrictionValue <= pd.AmountOfBets
                && x.MaxRestrictionValues >= pd.AmountOfBets
                && x.Hide == false
                && x.BeginTime < DateTime.Now
                && x.EndTime > DateTime.Now)
                .ToList().ForEach(x =>
                    {
                        int _tNum_ = (int)Math.Floor(pd.AmountOfBets / x.MinRestrictionValue);
                        int hadJoin = e.Db.Set<ActivityParticipateRecord>().Count(apr => apr.Activity.Id == x.Id
                            && apr.Owner.Id == pd.Owner.Id
                            && apr.CreatedTime.Year == DateTime.Now.Year
                            && apr.CreatedTime.Month == DateTime.Now.Month
                            && apr.CreatedTime.Day == DateTime.Now.Day);
                        if (hadJoin >= _tNum_) { return; }
                        ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                            .CreatePackageForCreate(pd.Owner.Id, x.Id, x.MinRestrictionValue);
                        aprManager.Create(pfc);
                    });
            if (pd.Owner.Layer > 1)
            {
                Author p1 = new AuthorManager(e.Db).GetParent(pd.Owner);
                aSet.Where(x => x.Type == ActivityType.下级用户当日累计消费奖励
                    && x.MinRestrictionValue <= pd.AmountOfBets
                    && x.MaxRestrictionValues >= pd.AmountOfBets
                    && x.Hide == false
                    && x.BeginTime < DateTime.Now
                    && x.EndTime > DateTime.Now)
                    .ToList().ForEach(x =>
                        {
                            bool hadJoin = e.Db.Set<ActivityParticipateRecord>().Any(apr => apr.Activity.Id == x.Id
                                && apr.Owner.Id == p1.Id
                                && apr.CreatedTime.Year == DateTime.Now.Year
                                && apr.CreatedTime.Month == DateTime.Now.Month
                                && apr.CreatedTime.Day == DateTime.Now.Day);
                            if (hadJoin) { return; }
                            ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                                .CreatePackageForCreate(p1.Id, x.Id, x.MinRestrictionValue);
                            aprManager.Create(pfc);
                        });
                if (pd.Owner.Layer > 2)
                {
                    Author p2 = new AuthorManager(e.Db).GetParent(p1);
                    aSet.Where(x => x.Type == ActivityType.下下级用户当日累计消费奖励
                        && x.MinRestrictionValue <= pd.AmountOfBets
                        && x.MaxRestrictionValues >= pd.AmountOfBets
                        && x.Hide == false
                        && x.BeginTime < DateTime.Now
                        && x.EndTime > DateTime.Now)
                        .ToList().ForEach(x =>
                        {
                            bool hadJoin = e.Db.Set<ActivityParticipateRecord>().Any(apr => apr.Activity.Id == x.Id
                                && apr.Owner.Id == p2.Id
                                && apr.CreatedTime.Year == DateTime.Now.Year
                                && apr.CreatedTime.Month == DateTime.Now.Month
                                && apr.CreatedTime.Day == DateTime.Now.Day);
                            if (hadJoin) { return; }
                            ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                                .CreatePackageForCreate(p2.Id, x.Id, x.MinRestrictionValue);
                            aprManager.Create(pfc);
                        });
                }
            }

            #endregion
            #region 充值额相关

            aSet.Where(x => x.Type == ActivityType.当日累计充值奖励
                && x.MinRestrictionValue <= pd.Recharge
                && x.MaxRestrictionValues >= pd.Recharge
                && x.Hide == false
                && x.BeginTime < DateTime.Now
                && x.EndTime > DateTime.Now)
                .ToList().ForEach(x =>
                {
                    bool hadJoin = e.Db.Set<ActivityParticipateRecord>().Any(apr => apr.Activity.Id == x.Id
                        && apr.Owner.Id == pd.Owner.Id
                        && apr.CreatedTime.Year == DateTime.Now.Year
                        && apr.CreatedTime.Month == DateTime.Now.Month
                        && apr.CreatedTime.Day == DateTime.Now.Day);
                    if (hadJoin) { return; }
                    ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                        .CreatePackageForCreate(pd.Owner.Id, x.Id, x.MinRestrictionValue);
                    aprManager.Create(pfc);
                });
            if (pd.Owner.Layer > 1)
            {
                Author p1 = new AuthorManager(e.Db).GetParent(pd.Owner);
                aSet.Where(x => x.Type == ActivityType.下级用户当日累计充值奖励
                    && x.MinRestrictionValue <= pd.Recharge
                    && x.MaxRestrictionValues >= pd.Recharge
                    && x.Hide == false
                    && x.BeginTime < DateTime.Now
                    && x.EndTime > DateTime.Now)
                    .ToList().ForEach(x =>
                    {
                        bool hadJoin = e.Db.Set<ActivityParticipateRecord>().Any(apr => apr.Activity.Id == x.Id
                                && apr.Owner.Id == p1.Id
                                && apr.CreatedTime.Year == DateTime.Now.Year
                                && apr.CreatedTime.Month == DateTime.Now.Month
                                && apr.CreatedTime.Day == DateTime.Now.Day);
                        if (hadJoin) { return; }
                        ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                            .CreatePackageForCreate(p1.Id, x.Id, x.MinRestrictionValue);
                        aprManager.Create(pfc);
                    });
                if (pd.Owner.Layer > 2)
                {
                    Author p2 = new AuthorManager(e.Db).GetParent(p1);
                    aSet.Where(x => x.Type == ActivityType.下下级用户当日累计充值奖励
                        && x.MinRestrictionValue <= pd.Recharge
                        && x.MaxRestrictionValues >= pd.Recharge
                        && x.Hide == false
                        && x.BeginTime < DateTime.Now
                        && x.EndTime > DateTime.Now)
                        .ToList().ForEach(x =>
                        {
                            bool hadJoin = e.Db.Set<ActivityParticipateRecord>().Any(apr => apr.Activity.Id == x.Id
                                && apr.Owner.Id == p2.Id
                                && apr.CreatedTime.Year == DateTime.Now.Year
                                && apr.CreatedTime.Month == DateTime.Now.Month
                                && apr.CreatedTime.Day == DateTime.Now.Day);
                            if (hadJoin) { return; }
                            ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                                .CreatePackageForCreate(p2.Id, x.Id, x.MinRestrictionValue);
                            aprManager.Create(pfc);
                        });
                }
            }

            #endregion
            #region 亏损额相关

            if (pd.GainsAndLosses >= 0) { return; }
            double _gainsAndLosses = -pd.GainsAndLosses;
            aSet.Where(x => x.Type == ActivityType.当日累计亏损补贴
                && x.MinRestrictionValue <= _gainsAndLosses
                && x.MaxRestrictionValues >= _gainsAndLosses
                && x.Hide == false
                && x.BeginTime < DateTime.Now
                && x.EndTime > DateTime.Now)
                .ToList().ForEach(x =>
                {
                    bool hadJoin = e.Db.Set<ActivityParticipateRecord>().Any(apr => apr.Activity.Id == x.Id
                                && apr.Owner.Id == pd.Owner.Id
                                && apr.CreatedTime.Year == DateTime.Now.Year
                                && apr.CreatedTime.Month == DateTime.Now.Month
                                && apr.CreatedTime.Day == DateTime.Now.Day);
                    if (hadJoin) { return; }
                    ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                        .CreatePackageForCreate(pd.Owner.Id, x.Id, x.MinRestrictionValue);
                    aprManager.Create(pfc);
                });
            if (pd.Owner.Layer > 1)
            {
                Author p1 = new AuthorManager(e.Db).GetParent(pd.Owner);
                aSet.Where(x => x.Type == ActivityType.下级用户当日累计亏损补贴
                    && x.MinRestrictionValue <= _gainsAndLosses
                    && x.MaxRestrictionValues >= _gainsAndLosses
                    && x.Hide == false
                    && x.BeginTime < DateTime.Now
                    && x.EndTime > DateTime.Now)
                    .ToList().ForEach(x =>
                    {
                        bool hadJoin = e.Db.Set<ActivityParticipateRecord>().Any(apr => apr.Activity.Id == x.Id
                                && apr.Owner.Id == p1.Id
                                && apr.CreatedTime.Year == DateTime.Now.Year
                                && apr.CreatedTime.Month == DateTime.Now.Month
                                && apr.CreatedTime.Day == DateTime.Now.Day);
                        if (hadJoin) { return; }
                        ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                            .CreatePackageForCreate(p1.Id, x.Id, x.MinRestrictionValue);
                        aprManager.Create(pfc);
                    });
                if (pd.Owner.Layer > 2)
                {
                    Author p2 = new AuthorManager(e.Db).GetParent(p1);
                    aSet.Where(x => x.Type == ActivityType.下下级用户当日累计亏损补贴
                        && x.MinRestrictionValue <= _gainsAndLosses
                        && x.MaxRestrictionValues >= _gainsAndLosses
                        && x.Hide == false
                        && x.BeginTime < DateTime.Now
                        && x.EndTime > DateTime.Now)
                        .ToList().ForEach(x =>
                        {
                            bool hadJoin = e.Db.Set<ActivityParticipateRecord>().Any(apr => apr.Activity.Id == x.Id
                                && apr.Owner.Id == p2.Id
                                && apr.CreatedTime.Year == DateTime.Now.Year
                                && apr.CreatedTime.Month == DateTime.Now.Month
                                && apr.CreatedTime.Day == DateTime.Now.Day);
                            if (hadJoin) { return; }
                            ICreatePackage<ActivityParticipateRecord> pfc = ActivityParticipateRecordManager.Factory
                                .CreatePackageForCreate(p2.Id, x.Id, x.MinRestrictionValue);
                            aprManager.Create(pfc);
                        });
                }
            }

            #endregion
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Helper;

namespace IWorld.BLL
{
    /// <summary>
    /// 默认活动的管理者对象
    /// </summary>
    public class ActivityManager : SimplifyManagerBase<Activity>, IManager<Activity>, ISimplify<Activity>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的默认活动的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public ActivityManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 编辑限制条件
        /// </summary>
        /// <param name="activityId">目标活动的存储指针</param>
        /// <param name="conditions">限制条件列表</param>
        public void EditCondition(int activityId, List<Factory.IPackageForCondition> conditions)
        {
            NChecker.CheckEntity<Activity>(activityId, "活动", db);
            Activity activity = db.Set<Activity>().Find(activityId);
            if (activity.BeginTime < DateTime.Now)
            {
                throw new Exception("已经开始的活动不能编辑相关信息");
            }
            conditions.ForEach(x =>
                {
                    x.CheckCondition();
                });
            List<ActivityCondition> _conditions = conditions.ConvertAll(x => x.GetCondition());
            var acSet = db.Set<ActivityCondition>();

            //分支：新列表的限制条件数量小于或等于原限制条件
            if (activity.Conditions.Count >= _conditions.Count)
            {
                List<ActivityCondition> rConditions = activity.Conditions.OrderBy(x => x.Id)
                    .Skip(_conditions.Count)
                    .ToList();
                activity.Conditions.RemoveAll(x => rConditions.Contains(x));    //从限制条件中移除多余的条件记录
                rConditions.ForEach(x =>
                    {
                        acSet.Remove(x);    //从数据库从移除多余的条件记录
                    });
                //修改原限制条件至新状态
                for (int i = 0; i < _conditions.Count; i++)
                {
                    activity.Conditions[i].Type = _conditions[i].Type;
                    activity.Conditions[i].Limit = _conditions[i].Limit;
                    activity.Conditions[i].Upper = _conditions[i].Upper;
                    activity.Conditions[i].ModifiedTime = DateTime.Now;
                }
            }
            //分支：新列表的限制条件数量大于原限制条件
            else
            {
                IEnumerable<ActivityCondition> aConditions = _conditions.OrderBy(x => x.Type)
                    .Skip(activity.Conditions.Count);
                List<ActivityCondition> eConditions = _conditions.Where(x => !aConditions.Contains(x)).ToList();
                //修改原限制条件至新状态
                for (int i = 0; i < activity.Conditions.Count; i++)
                {
                    activity.Conditions[i].Type = eConditions[i].Type;
                    activity.Conditions[i].Limit = eConditions[i].Limit;
                    activity.Conditions[i].Upper = eConditions[i].Upper;
                    activity.Conditions[i].ModifiedTime = DateTime.Now;
                }
                activity.Conditions.AddRange(aConditions);  //将新添加的添加记录添加到限制条件列表
            }

            activity.ModifiedTime = DateTime.Now;
            db.SaveChanges();
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
            /// 创建一个用于新建默认活动的数据集
            /// </summary>
            /// <param name="title">标题</param>
            /// <param name="type">活动类型</param>
            /// <param name="minRestrictionValue">活动涉及指标的最小值</param>
            /// <param name="maxRestrictionValues">活动涉及指标的最小值</param>
            /// <param name="rewardType">奖励类型</param>
            /// <param name="rewardValueIsAbsolute">奖励数额类型（绝对值/百分比）</param>
            /// <param name="reward">奖励数额</param>
            /// <param name="conditions">限制条件</param>
            /// <param name="beginTime">开始时间</param>
            /// <param name="days">持续天数</param>
            /// <param name="autoDelete">过期自动删除</param>
            /// <returns>返回用于新建默认活动的数据集</returns>
            public static ICreatePackage<Activity> CreatePackageForCreate(string title, string type, double minRestrictionValue
                , double maxRestrictionValues, string rewardType, bool rewardValueIsAbsolute, double reward
                , List<IPackageForCondition> conditions, string beginTime, int days, bool autoDelete)
            {
                ActivityType _type = EnumHelper.Parse<ActivityType>(type);
                ActivityRewardType _rewardType = EnumHelper.Parse<ActivityRewardType>(rewardType);

                return new PackageForCreate(title, _type, minRestrictionValue, maxRestrictionValues, _rewardType, rewardValueIsAbsolute
                    , reward, conditions, beginTime, days, autoDelete);
            }

            /// <summary>
            /// 创建一个用于修改默认活动信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="title">标题</param>
            /// <param name="type">活动类型</param>
            /// <param name="minRestrictionValue">活动涉及指标的最小值</param>
            /// <param name="maxRestrictionValues">活动涉及指标的最小值</param>
            /// <param name="rewardType">奖励类型</param>
            /// <param name="rewardValueIsAbsolute">奖励数额类型（绝对值/百分比）</param>
            /// <param name="reward">奖励数额</param>
            /// <param name="days">持续天数</param>
            /// <param name="hide">暂停显示</param>
            /// <param name="autoDelete">过期自动删除</param>
            /// <returns>返回用于修改默认活动信息的数据集</returns>
            public static IUpdatePackage<Activity> CreatePackageForUpdate(int id, string title, string type, double minRestrictionValue
                , double maxRestrictionValues, string rewardType, bool rewardValueIsAbsolute, double reward, int days, bool hide
                , bool autoDelete)
            {
                ActivityType _type = EnumHelper.Parse<ActivityType>(type);
                ActivityRewardType _rewardType = EnumHelper.Parse<ActivityRewardType>(rewardType);

                return new PackageForUpdate(id, title, _type, minRestrictionValue, maxRestrictionValues, _rewardType, rewardValueIsAbsolute
                    , reward, days, hide, autoDelete);
            }

            /// <summary>
            /// 创建一个用于修改默认活动信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="days">持续天数</param>
            /// <param name="hide">暂停显示</param>
            /// <param name="autoDelete">过期自动删除</param>
            /// <returns>返回用于修改默认活动信息的数据集</returns>
            public static IUpdatePackage<Activity> CreatePackageForUpdate(int id, int days, bool hide, bool autoDelete)
            {

                return new PackageForUpdate_basic(id, days, hide, autoDelete);
            }

            /// <summary>
            /// 创建一个用于修改默认活动信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="hide">暂停显示</param>
            /// <returns>返回用于修改默认活动信息的数据集</returns>
            public static IUpdatePackage<Activity> CreatePackageForUpdate(int id, bool hide)
            {

                return new PackageForUpdate_showOrHide(id, hide);
            }

            /// <summary>
            /// 创建一个用于新建限制条件的数据集
            /// </summary>
            /// <param name="type">类型</param>
            /// <param name="limit">下限</param>
            /// <param name="upper">上限</param>
            /// <returns>返回用于新建限制条件的数据集</returns>
            public static IPackageForCondition CreatePackageForCondition(string type, double limit, double upper)
            {
                ConditionType _type = EnumHelper.Parse<ConditionType>(type);

                return new PackageForCondition(_type, limit, upper);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建默认活动的数据集
            /// </summary>
            private class PackageForCreate : IPackage<Activity>, ICreatePackage<Activity>
            {
                #region 公开属性

                /// <summary>
                /// 标题
                /// </summary>
                public string Title { get; set; }

                /// <summary>
                /// 活动类型
                /// </summary>
                public ActivityType Type { get; set; }

                /// <summary>
                /// 活动涉及指标的最小值
                /// </summary>
                public double MinRestrictionValue { get; set; }

                /// <summary>
                /// 活动涉及指标的最大值
                /// </summary>
                public double MaxRestrictionValues { get; set; }

                /// <summary>
                /// 奖励类型
                /// </summary>
                public ActivityRewardType RewardType { get; set; }

                /// <summary>
                /// 奖励数额类型（绝对值/百分比）
                /// </summary>
                public bool RewardValueIsAbsolute { get; set; }

                /// <summary>
                /// 奖励数额
                /// </summary>
                public double Reward { get; set; }

                /// <summary>
                /// 限制条件
                /// </summary>
                public List<IPackageForCondition> Conditions { get; set; }

                /// <summary>
                /// 开始时间
                /// </summary>
                public string BeginTime { get; set; }

                /// <summary>
                /// 持续天数
                /// </summary>
                public int Days { get; set; }

                /// <summary>
                /// 过期自动删除
                /// </summary>
                public bool AutoDelete { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建默认活动的数据集
                /// </summary>
                /// <param name="title">标题</param>
                /// <param name="type">活动类型</param>
                /// <param name="minRestrictionValue">活动涉及指标的最小值</param>
                /// <param name="maxRestrictionValues">活动涉及指标的最小值</param>
                /// <param name="rewardType">奖励类型</param>
                /// <param name="rewardValueIsAbsolute">奖励数额类型（绝对值/百分比）</param>
                /// <param name="reward">奖励数额</param>
                /// <param name="conditions">限制条件</param>
                /// <param name="beginTime">开始时间</param>
                /// <param name="days">持续天数</param>
                /// <param name="autoDelete">过期自动删除</param>
                public PackageForCreate(string title, ActivityType type, double minRestrictionValue, double maxRestrictionValues
                    , ActivityRewardType rewardType, bool rewardValueIsAbsolute, double reward
                    , List<IPackageForCondition> conditions, string beginTime, int days, bool autoDelete)
                {
                    this.Title = title;
                    this.Type = type;
                    this.MinRestrictionValue = minRestrictionValue;
                    this.MaxRestrictionValues = maxRestrictionValues;
                    this.RewardType = rewardType;
                    this.RewardValueIsAbsolute = rewardValueIsAbsolute;
                    this.Reward = reward;
                    this.Conditions = conditions;
                    this.BeginTime = beginTime;
                    this.Days = days;
                    this.AutoDelete = autoDelete;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    if (this.MinRestrictionValue < 0)
                    {
                        throw new Exception("活动涉及指标的最小值不能小于0");
                    }
                    if (this.MinRestrictionValue > this.MaxRestrictionValues)
                    {
                        throw new Exception("活动涉及指标的最小值不能大于动涉及指标的最大值");
                    }
                    this.Conditions.ForEach(x =>
                        {
                            x.CheckCondition();
                        });
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public Activity GetEntity(DbContext db)
                {
                    List<ActivityCondition> conditions = this.Conditions.ConvertAll(x => x.GetCondition());
                    List<int> t = this.BeginTime.Split(new char[] { '-' }).ToList().ConvertAll(x => Convert.ToInt32(x));
                    DateTime beginTime = new DateTime(t[0], t[1], t[2]);

                    return new Activity(this.Title, this.Type, this.MinRestrictionValue, this.MaxRestrictionValues, this.RewardType
                        , this.RewardValueIsAbsolute, this.Reward, conditions, beginTime, this.Days, this.AutoDelete);
                }

                #endregion
            }

            /// <summary>
            /// 用于修改默认活动信息的数据集
            /// </summary>
            private class PackageForUpdate : PackageForUpdateBase<Activity>, IPackage<Activity>, IUpdatePackage<Activity>
            {
                #region 公开属性

                /// <summary>
                /// 标题
                /// </summary>
                public string Title { get; set; }

                /// <summary>
                /// 活动类型
                /// </summary>
                public ActivityType Type { get; set; }

                /// <summary>
                /// 活动涉及指标的最小值
                /// </summary>
                public double MinRestrictionValue { get; set; }

                /// <summary>
                /// 活动涉及指标的最大值
                /// </summary>
                public double MaxRestrictionValues { get; set; }

                /// <summary>
                /// 奖励类型
                /// </summary>
                public ActivityRewardType RewardType { get; set; }

                /// <summary>
                /// 奖励数额类型（绝对值/百分比）
                /// </summary>
                public bool RewardValueIsAbsolute { get; set; }

                /// <summary>
                /// 奖励数额
                /// </summary>
                public double Reward { get; set; }

                /// <summary>
                /// 持续天数
                /// </summary>
                public int Days { get; set; }

                /// <summary>
                /// 暂停显示
                /// </summary>
                public bool Hide { get; set; }

                /// <summary>
                /// 过期自动删除
                /// </summary>
                public bool AutoDelete { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于修改默认活动信息的数据集
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="title">标题</param>
                /// <param name="type">活动类型</param>
                /// <param name="minRestrictionValue">活动涉及指标的最小值</param>
                /// <param name="maxRestrictionValues">活动涉及指标的最小值</param>
                /// <param name="rewardType">奖励类型</param>
                /// <param name="rewardValueIsAbsolute">奖励数额类型（绝对值/百分比）</param>
                /// <param name="reward">奖励数额</param>
                /// <param name="days">持续天数</param>
                /// <param name="hide">暂停显示</param>
                /// <param name="autoDelete">过期自动删除</param>
                public PackageForUpdate(int id, string title, ActivityType type, double minRestrictionValue, double maxRestrictionValues
                    , ActivityRewardType rewardType, bool rewardValueIsAbsolute, double reward, int days, bool hide
                    , bool autoDelete)
                    : base(id)
                {
                    this.Title = title;
                    this.Type = type;
                    this.MinRestrictionValue = minRestrictionValue;
                    this.MaxRestrictionValues = maxRestrictionValues;
                    this.RewardType = rewardType;
                    this.RewardValueIsAbsolute = rewardValueIsAbsolute;
                    this.Reward = reward;
                    this.Days = days;
                    this.Hide = hide;
                    this.AutoDelete = autoDelete;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public override void CheckData(DbContext db)
                {
                    base.CheckData(db);
                    var entity = db.Set<Activity>().Find(this.Id);
                    if (entity.BeginTime < DateTime.Now)
                    {
                        throw new Exception("已经超过开始时间的活动不能修改活动的详细内容");
                    }
                    if (this.MinRestrictionValue < 0)
                    {
                        throw new Exception("活动涉及指标的最小值不能小于0");
                    }
                    if (this.MinRestrictionValue > this.MaxRestrictionValues)
                    {
                        throw new Exception("活动涉及指标的最小值不能大于动涉及指标的最大值");
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override Activity GetEntity(DbContext db)
                {
                    var t = db.Set<Activity>().Where(x => x.Id == this.Id)
                           .Select(x => new { x.Days, x.EndTime }).FirstOrDefault();
                    DateTime endTime = t.EndTime.AddDays(this.Days - t.Days);

                    this.AddToUpdating("Title", this.Title);
                    this.AddToUpdating("Type", this.Type);
                    this.AddToUpdating("MinRestrictionValue", this.MinRestrictionValue);
                    this.AddToUpdating("MaxRestrictionValues", this.MaxRestrictionValues);
                    this.AddToUpdating("RewardType", this.RewardType);
                    this.AddToUpdating("RewardValueIsAbsolute", this.RewardValueIsAbsolute);
                    this.AddToUpdating("Reward", this.Reward);
                    this.AddToUpdating("Days", this.Days);
                    this.AddToUpdating("EndTime", endTime);
                    this.AddToUpdating("Hide", this.Hide);
                    this.AddToUpdating("AutoDelete", this.AutoDelete);

                    return base.GetEntity(db);
                }

                #endregion
            }

            /// <summary>
            /// 用于修改默认活动信息的数据集（基础）
            /// </summary>
            private class PackageForUpdate_basic : PackageForUpdateBase<Activity>, IPackage<Activity>, IUpdatePackage<Activity>
            {
                #region 公开属性

                /// <summary>
                /// 持续天数
                /// </summary>
                public int Days { get; set; }

                /// <summary>
                /// 暂停显示
                /// </summary>
                public bool Hide { get; set; }

                /// <summary>
                /// 过期自动删除
                /// </summary>
                public bool AutoDelete { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于修改默认活动信息的数据集（基础）
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="days">持续天数</param>
                /// <param name="hide">暂停显示</param>
                /// <param name="autoDelete">过期自动删除</param>
                public PackageForUpdate_basic(int id, int days, bool hide, bool autoDelete)
                    : base(id)
                {
                    this.Days = days;
                    this.Hide = hide;
                    this.AutoDelete = autoDelete;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override Activity GetEntity(DbContext db)
                {
                    this.AddToUpdating("Days", this.Days);
                    this.AddToUpdating("Hide", this.Hide);
                    this.AddToUpdating("AutoDelete", this.AutoDelete);

                    return base.GetEntity(db);
                }

                #endregion
            }

            /// <summary>
            /// 用于修改默认活动信息的数据集（暂停/重新开始）
            /// </summary>
            private class PackageForUpdate_showOrHide : PackageForUpdateBase<Activity>, IPackage<Activity>, IUpdatePackage<Activity>
            {
                #region 公开属性

                /// <summary>
                /// 暂停显示
                /// </summary>
                public bool Hide { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于修改默认活动信息的数据集（暂停/重新开始）
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="hide">暂停显示</param>
                public PackageForUpdate_showOrHide(int id, bool hide)
                    : base(id)
                {
                    this.Hide = hide;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override Activity GetEntity(DbContext db)
                {
                    this.AddToUpdating("Hide", this.Hide);

                    return base.GetEntity(db);
                }

                #endregion
            }

            /// <summary>
            /// 用于新建限制条件的数据集
            /// </summary>
            private class PackageForCondition : IPackageForCondition
            {
                #region 公开属性

                /// <summary>
                /// 类型
                /// </summary>
                public ConditionType Type { get; set; }

                /// <summary>
                /// 下限
                /// </summary>
                public double Limit { get; set; }

                /// <summary>
                /// 上限
                /// </summary>
                public double Upper { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建限制条件的数据集
                /// </summary>
                /// <param name="type">类型</param>
                /// <param name="limit">下限</param>
                /// <param name="upper">上限</param>
                public PackageForCondition(ConditionType type, double limit, double upper)
                {
                    this.Type = type;
                    this.Limit = limit;
                    this.Upper = upper;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查用于新建限制条件的输入的合法性
                /// </summary>
                public void CheckCondition()
                {
                    if (this.Limit > this.Upper)
                    {
                        throw new Exception("最小值不能大于最大值");
                    }
                }

                /// <summary>
                /// 获取限制条件（默认活动）
                /// </summary>
                /// <returns>返回限制条件（默认活动）的封装</returns>
                public ActivityCondition GetCondition()
                {
                    return new ActivityCondition(this.Type, this.Limit, this.Upper);
                }

                #endregion
            }

            #endregion

            #region 内嵌接口

            /// <summary>
            /// 定义用于新建限制条件的数据集
            /// </summary>
            public interface IPackageForCondition
            {
                /// <summary>
                /// 检查用于新建限制条件的输入的合法性
                /// </summary>
                void CheckCondition();

                /// <summary>
                /// 获取限制条件（默认活动）
                /// </summary>
                /// <returns>返回限制条件（默认活动）的封装</returns>
                ActivityCondition GetCondition();
            }

            #endregion
        }

        #endregion
    }
}

using System;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 用户组的管理者对象
    /// </summary>
    public class UserGroupManager : SimplifyManagerBase<UserGroup>, IManager<UserGroup>, ISimplify<UserGroup>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的用户组的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public UserGroupManager(DbContext db)
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
            /// 创建一个用于新建用户组的数据集
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="grade">等级</param>
            /// <param name="limitOfConsumption">消费量下限</param>
            /// <param name="upperOfConsumption">消费量上限</param>
            /// <returns>返回用于新建用户组的数据集</returns>
            public static ICreatePackage<UserGroup> CreatePackageForCreate(string name, int grade, double limitOfConsumption
                , double upperOfConsumption)
            {
                return new PackageForCreate(name, "", grade, limitOfConsumption, upperOfConsumption, 0, 0, 0, 0, 0, false, 0);
            }

            /// <summary>
            /// 创建一个用于新建用户组的数据集
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="colorOfName">名称的显示颜色</param>
            /// <param name="grade">等级</param>
            /// <param name="limitOfConsumption">消费量下限</param>
            /// <param name="upperOfConsumption">消费量上限</param>
            /// <param name="withdrawals">每日允许提现次数</param>
            /// <param name="minimumWithdrawalAmount">单笔最低取款金额</param>
            /// <param name="maximumWithdrawalAmount">单笔最高取款金额</param>
            /// <param name="minimumRechargeAmount">最小充值额度</param>
            /// <param name="maximumRechargeAmount">最大充值额度</param>
            /// <param name="withdrawalsAtAnyTime">随时提现</param>
            /// <param name="maxOfSubordinate">最多拥有直属下级数量限制</param>
            /// <returns>返回用于新建用户组的数据集</returns>
            public static ICreatePackage<UserGroup> CreatePackageForCreate(string name, string colorOfName, int grade
                , double limitOfConsumption, double upperOfConsumption, int withdrawals, double minimumWithdrawalAmount
                , double maximumWithdrawalAmount, double minimumRechargeAmount, double maximumRechargeAmount
                , bool withdrawalsAtAnyTime, int maxOfSubordinate)
            {
                return new PackageForCreate(name, colorOfName, grade, limitOfConsumption, upperOfConsumption, withdrawals
                    , minimumWithdrawalAmount, maximumWithdrawalAmount, minimumRechargeAmount, maximumRechargeAmount
                    , withdrawalsAtAnyTime, maxOfSubordinate);
            }

            /// <summary>
            /// 创建一个用于更新用户组信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="grade">等级</param>
            /// <param name="name">名称</param>
            /// <param name="limitOfConsumption">消费量下限</param>
            /// <param name="upperOfConsumption">消费量上限</param>
            /// <returns>返回用于更新用户组信息的数据集</returns>
            public static IUpdatePackage<UserGroup> CreatePackageForUpdate(int id, int grade, string name
                , double limitOfConsumption, double upperOfConsumption)
            {
                return new PackageForUpdate_basic(id, grade, name, limitOfConsumption, upperOfConsumption);
            }

            /// <summary>
            /// 创建一个用于更新用户组信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="name">名称</param>
            /// <param name="colorOfName">名称的显示颜色</param>
            /// <param name="grade">等级</param>
            /// <param name="limitOfConsumption">消费量下限</param>
            /// <param name="upperOfConsumption">消费量上限</param>
            /// <param name="withdrawals">每日允许提现次数</param>
            /// <param name="minimumWithdrawalAmount">单笔最低取款金额</param>
            /// <param name="maximumWithdrawalAmount">单笔最高取款金额</param>
            /// <param name="minimumRechargeAmount">最小充值额度</param>
            /// <param name="maximumRechargeAmount">最大充值额度</param>
            /// <param name="withdrawalsAtAnyTime">随时提现</param>
            /// <param name="maxOfSubordinate">最多拥有直属下级数量限制</param>
            /// <returns>返回用于更新用户组信息的数据集</returns>
            public static IUpdatePackage<UserGroup> CreatePackageForUpdate(int id, string name, string colorOfName, int grade
                , double limitOfConsumption, double upperOfConsumption, int withdrawals, double minimumWithdrawalAmount
                , double maximumWithdrawalAmount, double minimumRechargeAmount, double maximumRechargeAmount
                , bool withdrawalsAtAnyTime, int maxOfSubordinate)
            {
                return new PackageForUpdate(id, name, colorOfName, grade, limitOfConsumption, upperOfConsumption, withdrawals
                    , minimumWithdrawalAmount, maximumWithdrawalAmount, minimumRechargeAmount, maximumRechargeAmount
                    , withdrawalsAtAnyTime, maxOfSubordinate);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建用户组的数据集
            /// </summary>
            private class PackageForCreate : IPackage<UserGroup>, ICreatePackage<UserGroup>
            {
                #region 公开属性

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 等级
                /// </summary>
                public int Grade { get; set; }

                /// <summary>
                /// 名称的显示颜色（如为空则显示系统默认颜色）
                /// </summary>
                public string ColorOfName { get; set; }

                /// <summary>
                /// 消费量下限
                /// </summary>
                public double LimitOfConsumption { get; set; }

                /// <summary>
                /// 消费量上限
                /// </summary>
                public double UpperOfConsumption { get; set; }

                /// <summary>
                /// 每日允许提现次数（如为0则采用系统参数）
                /// </summary>
                public int Withdrawals { get; set; }

                /// <summary>
                /// 单笔最低取款金额（如为0则采用系统参数）
                /// </summary>
                public double MinimumWithdrawalAmount { get; set; }

                /// <summary>
                /// 单笔最高取款金额（如为0则采用系统参数）
                /// </summary>
                public double MaximumWithdrawalAmount { get; set; }

                /// <summary>
                /// 最小充值额度（如为0则采用系统参数）
                /// </summary>
                public double MinimumRechargeAmount { get; set; }

                /// <summary>
                /// 最大充值额度（如为0则采用系统参数）
                /// </summary>
                public double MaximumRechargeAmount { get; set; }

                /// <summary>
                /// 随时提现
                /// </summary>
                public bool WithdrawalsAtAnyTime { get; set; }

                /// <summary>
                /// 最多拥有直属下级数量限制
                /// </summary>
                public int MaxOfSubordinate { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建用户组的数据集
                /// </summary>
                /// <param name="name">名称</param>
                /// <param name="colorOfName">名称的显示颜色</param>
                /// <param name="grade">等级</param>
                /// <param name="limitOfConsumption">消费量下限</param>
                /// <param name="upperOfConsumption">消费量上限</param>
                /// <param name="withdrawals">每日允许提现次数</param>
                /// <param name="minimumWithdrawalAmount">单笔最低取款金额</param>
                /// <param name="maximumWithdrawalAmount">单笔最高取款金额</param>
                /// <param name="minimumRechargeAmount">最小充值额度</param>
                /// <param name="maximumRechargeAmount">最大充值额度</param>
                /// <param name="withdrawalsAtAnyTime">随时提现</param>
                /// <param name="maxOfSubordinate">最多拥有直属下级数量限制</param>
                public PackageForCreate(string name, string colorOfName, int grade, double limitOfConsumption
                    , double upperOfConsumption, int withdrawals, double minimumWithdrawalAmount, double maximumWithdrawalAmount
                    , double minimumRechargeAmount, double maximumRechargeAmount, bool withdrawalsAtAnyTime, int maxOfSubordinate)
                {
                    this.Name = name;
                    this.Grade = grade;
                    this.ColorOfName = colorOfName;
                    this.LimitOfConsumption = limitOfConsumption;
                    this.UpperOfConsumption = upperOfConsumption;
                    this.Withdrawals = withdrawals;
                    this.MinimumWithdrawalAmount = minimumWithdrawalAmount;
                    this.MaximumWithdrawalAmount = maximumWithdrawalAmount;
                    this.MinimumRechargeAmount = minimumRechargeAmount;
                    this.MaximumRechargeAmount = maximumRechargeAmount;
                    this.WithdrawalsAtAnyTime = withdrawalsAtAnyTime;
                    this.MaxOfSubordinate = maxOfSubordinate;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    var ugSet = db.Set<UserGroup>();
                    bool hadUsedName = ugSet.Any(x => x.Name == this.Name);
                    if (hadUsedName)
                    {
                        throw new Exception(string.Format("已经存在同名的用户组：{0}", this.Name));
                    }
                    if (this.Grade < 1
                        || this.Grade > 255)
                    {
                        throw new Exception("用户组等级必须为 1 - 255 的正整数");
                    }
                    bool hadUsedGrade = ugSet.Any(x => x.Grade == this.Grade);
                    if (hadUsedGrade)
                    {
                        throw new Exception(string.Format("用户等级不能重复 已经存在等级 {0}", this.Grade));
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public UserGroup GetEntity(DbContext db)
                {
                    return new UserGroup(this.Name, this.ColorOfName, this.Grade, this.LimitOfConsumption, this.UpperOfConsumption
                        , this.Withdrawals, this.MinimumWithdrawalAmount, this.MaximumWithdrawalAmount, this.MinimumRechargeAmount
                        , this.MaximumRechargeAmount, this.WithdrawalsAtAnyTime, this.MaxOfSubordinate);
                }

                #endregion
            }

            /// <summary>
            /// 用于更新用户组信息的数据集
            /// </summary>
            private class PackageForUpdate_basic : PackageForUpdateBase<UserGroup>, IPackage<UserGroup>, IUpdatePackage<UserGroup>
            {
                #region 公开属性

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 等级
                /// </summary>
                public int Grade { get; set; }

                /// <summary>
                /// 消费量下限
                /// </summary>
                public double LimitOfConsumption { get; set; }

                /// <summary>
                /// 消费量上限
                /// </summary>
                public double UpperOfConsumption { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于更新用户组信息的数据集
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="grade">等级</param>
                /// <param name="name">名称</param>
                /// <param name="limitOfConsumption">消费量下限</param>
                /// <param name="upperOfConsumption">消费量上限</param>
                public PackageForUpdate_basic(int id, int grade, string name, double limitOfConsumption, double upperOfConsumption)
                    : base(id)
                {
                    this.Grade = grade;
                    this.Name = name;
                    this.LimitOfConsumption = limitOfConsumption;
                    this.UpperOfConsumption = upperOfConsumption;
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
                    var ugSet = db.Set<UserGroup>();

                    bool hadUsedName = ugSet.Any(x => x.Id != this.Id && x.Name == this.Name);
                    if (hadUsedName)
                    {
                        throw new Exception("已经存在同名的用户组");
                    }
                    if (this.Grade < 1
                        || this.Grade > 255)
                    {
                        throw new Exception("用户组等级必须为 1 - 255 的正整数");
                    }
                    bool hadUsedGrade = ugSet.Any(x => x.Id != this.Id && x.Grade == this.Grade);
                    if (hadUsedGrade)
                    {
                        throw new Exception(string.Format("用户等级不能重复 已经存在等级 {0}", this.Grade));
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override UserGroup GetEntity(DbContext db)
                {
                    this.AddToUpdating("Name", this.Name);
                    this.AddToUpdating("Grade", this.Grade);
                    this.AddToUpdating("LimitOfConsumption", this.LimitOfConsumption);
                    this.AddToUpdating("UpperOfConsumption", this.UpperOfConsumption);

                    return base.GetEntity(db);
                }

                #endregion
            }

            /// <summary>
            /// 用于更新用户组信息的数据集
            /// </summary>
            private class PackageForUpdate : PackageForUpdateBase<UserGroup>, IPackage<UserGroup>, IUpdatePackage<UserGroup>
            {
                #region 公开属性

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 等级
                /// </summary>
                public int Grade { get; set; }

                /// <summary>
                /// 名称的显示颜色（如为空则显示系统默认颜色）
                /// </summary>
                public string ColorOfName { get; set; }

                /// <summary>
                /// 消费量下限
                /// </summary>
                public double LimitOfConsumption { get; set; }

                /// <summary>
                /// 消费量上限
                /// </summary>
                public double UpperOfConsumption { get; set; }

                /// <summary>
                /// 每日允许提现次数（如为0则采用系统参数）
                /// </summary>
                public int Withdrawals { get; set; }

                /// <summary>
                /// 单笔最低取款金额（如为0则采用系统参数）
                /// </summary>
                public double MinimumWithdrawalAmount { get; set; }

                /// <summary>
                /// 单笔最高取款金额（如为0则采用系统参数）
                /// </summary>
                public double MaximumWithdrawalAmount { get; set; }

                /// <summary>
                /// 最小充值额度（如为0则采用系统参数）
                /// </summary>
                public double MinimumRechargeAmount { get; set; }

                /// <summary>
                /// 最大充值额度（如为0则采用系统参数）
                /// </summary>
                public double MaximumRechargeAmount { get; set; }

                /// <summary>
                /// 随时提现
                /// </summary>
                public bool WithdrawalsAtAnyTime { get; set; }

                /// <summary>
                /// 最多拥有直属下级数量限制
                /// </summary>
                public int MaxOfSubordinate { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于用于更新用户组信息的数据集
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="name">名称</param>
                /// <param name="colorOfName">名称的显示颜色</param>
                /// <param name="grade">等级</param>
                /// <param name="limitOfConsumption">消费量下限</param>
                /// <param name="upperOfConsumption">消费量上限</param>
                /// <param name="withdrawals">每日允许提现次数</param>
                /// <param name="minimumWithdrawalAmount">单笔最低取款金额</param>
                /// <param name="maximumWithdrawalAmount">单笔最高取款金额</param>
                /// <param name="minimumRechargeAmount">最小充值额度</param>
                /// <param name="maximumRechargeAmount">最大充值额度</param>
                /// <param name="withdrawalsAtAnyTime">随时提现</param>
                /// <param name="maxOfSubordinate">最多拥有直属下级数量限制</param>
                public PackageForUpdate(int id, string name, string colorOfName, int grade, double limitOfConsumption
                    , double upperOfConsumption, int withdrawals, double minimumWithdrawalAmount, double maximumWithdrawalAmount
                    , double minimumRechargeAmount, double maximumRechargeAmount, bool withdrawalsAtAnyTime, int maxOfSubordinate)
                    : base(id)
                {
                    this.Name = name;
                    this.Grade = grade;
                    this.ColorOfName = colorOfName;
                    this.LimitOfConsumption = limitOfConsumption;
                    this.UpperOfConsumption = upperOfConsumption;
                    this.Withdrawals = withdrawals;
                    this.MinimumWithdrawalAmount = minimumWithdrawalAmount;
                    this.MaximumWithdrawalAmount = maximumWithdrawalAmount;
                    this.MinimumRechargeAmount = minimumRechargeAmount;
                    this.MaximumRechargeAmount = maximumRechargeAmount;
                    this.WithdrawalsAtAnyTime = withdrawalsAtAnyTime;
                    this.MaxOfSubordinate = maxOfSubordinate;
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
                    var ugSet = db.Set<UserGroup>();

                    bool hadUsedName = ugSet.Any(x => x.Id != this.Id && x.Name == this.Name);
                    if (hadUsedName)
                    {
                        throw new Exception("已经存在同名的用户组");
                    }
                    if (this.Grade < 1
                        || this.Grade > 255)
                    {
                        throw new Exception("用户组等级必须为 1 - 255 的正整数");
                    }
                    bool hadUsedGrade = ugSet.Any(x => x.Id != this.Id && x.Grade == this.Grade);
                    if (hadUsedGrade)
                    {
                        throw new Exception(string.Format("用户等级不能重复 已经存在等级 {0}", this.Grade));
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override UserGroup GetEntity(DbContext db)
                {
                    this.AddToUpdating("Name", this.Name);
                    this.AddToUpdating("Grade", this.Grade);
                    this.AddToUpdating("ColorOfName", this.ColorOfName);
                    this.AddToUpdating("LimitOfConsumption", this.LimitOfConsumption);
                    this.AddToUpdating("UpperOfConsumption", this.UpperOfConsumption);
                    this.AddToUpdating("Withdrawals", this.Withdrawals);
                    this.AddToUpdating("MinimumWithdrawalAmount", this.MinimumWithdrawalAmount);
                    this.AddToUpdating("MaximumWithdrawalAmount", this.MaximumWithdrawalAmount);
                    this.AddToUpdating("MinimumRechargeAmount", this.MinimumRechargeAmount);
                    this.AddToUpdating("MaximumRechargeAmount", this.MaximumRechargeAmount);
                    this.AddToUpdating("WithdrawalsAtAnyTime", this.WithdrawalsAtAnyTime);
                    this.AddToUpdating("MaxOfSubordinate", this.MaxOfSubordinate);

                    return base.GetEntity(db);
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}

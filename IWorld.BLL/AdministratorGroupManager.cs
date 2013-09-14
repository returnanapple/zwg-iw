using System;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 管理用户组的管理者对象
    /// </summary>
    public class AdministratorGroupManager : SimplifyManagerBase<AdministratorGroup>, IManager<AdministratorGroup>
        , ISimplify<AdministratorGroup>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的管理用户组的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public AdministratorGroupManager(DbContext db)
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
            /// 创建一个用于新建管理员用户组的数据集
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="grade">等级</param>
            /// <param name="canViewUsers">允许查看前台用户列表</param>
            /// <param name="canEditUsers">允许创建和修改用户信息</param>
            /// <param name="canViewTickets">允许查看彩票信息</param>
            /// <param name="canEditTickets">允许修改彩票信息</param>
            /// <param name="canViewActivities">允许查看活动信息</param>
            /// <param name="canEditActivities">允许修改活动信息</param>
            /// <param name="canSettingSite">允许查看和修改系统设置</param>
            /// <param name="canViewDataReports">允许查看数据报表</param>
            /// <param name="canViewAndAddFundsReports">允许查看并添加资金支取记录</param>
            /// <param name="canViewAndEditMessageBox">允许参看消息盒子</param>
            /// <param name="canViewAndEditManagers">允许查看和管理“管理员”以其用户组并查看相关登陆、操作信息</param>
            /// <returns>返回用于新建管理员用户组的数据集</returns>
            public static ICreatePackage<AdministratorGroup> CreatePackageForCreate(string name, int grade, bool canViewUsers
                , bool canEditUsers, bool canViewTickets, bool canEditTickets, bool canViewActivities, bool canEditActivities
                , bool canSettingSite, bool canViewDataReports, bool canViewAndAddFundsReports, bool canViewAndEditMessageBox
                , bool canViewAndEditManagers)
            {
                return new PackageForCreate(name, grade, canViewUsers, canEditUsers, canViewTickets, canEditTickets
                    , canViewActivities, canEditActivities, canSettingSite, canViewDataReports, canViewAndAddFundsReports
                    , canViewAndEditMessageBox, canViewAndEditManagers);
            }

            /// <summary>
            /// 创建一个用于修改管理员用户组信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="name">名称</param>
            /// <param name="grade">等级</param>
            /// <param name="canViewUsers">允许查看前台用户列表</param>
            /// <param name="canEditUsers">允许创建和修改用户信息</param>
            /// <param name="canViewTickets">允许查看彩票信息</param>
            /// <param name="canEditTickets">允许修改彩票信息</param>
            /// <param name="canViewActivities">允许查看活动信息</param>
            /// <param name="canEditActivities">允许修改活动信息</param>
            /// <param name="canSettingSite">允许查看和修改系统设置</param>
            /// <param name="canViewDataReports">允许查看数据报表</param>
            /// <param name="canViewAndAddFundsReports">允许查看并添加资金支取记录</param>
            /// <param name="canViewAndEditMessageBox">允许参看消息盒子</param>
            /// <param name="canViewAndEditManagers">允许查看和管理“管理员”以其用户组并查看相关登陆、操作信息</param>
            /// <returns>返回用于修改管理员用户组信息的数据集</returns>
            public static IUpdatePackage<AdministratorGroup> CreatePackageForUpdate(int id, string name, int grade, bool canViewUsers
                , bool canEditUsers, bool canViewTickets, bool canEditTickets, bool canViewActivities, bool canEditActivities
                , bool canSettingSite, bool canViewDataReports, bool canViewAndAddFundsReports, bool canViewAndEditMessageBox
                , bool canViewAndEditManagers = false)
            {
                return new PackageForUpdate(id, name, grade, canViewUsers, canEditUsers, canViewTickets, canEditTickets
                    , canViewActivities, canEditActivities, canSettingSite, canViewDataReports, canViewAndAddFundsReports
                    , canViewAndEditMessageBox, canViewAndEditManagers);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建管理员用户组的数据集
            /// </summary>
            private class PackageForCreate : IPackage<AdministratorGroup>, ICreatePackage<AdministratorGroup>
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
                /// 允许查看前台用户列表
                /// </summary>
                public bool CanViewUsers { get; set; }

                /// <summary>
                /// 允许创建和修改用户信息
                /// </summary>
                public bool CanEditUsers { get; set; }

                /// <summary>
                /// 允许查看彩票信息
                /// </summary>
                public bool CanViewTickets { get; set; }

                /// <summary>
                /// 允许修改彩票信息
                /// </summary>
                public bool CanEditTickets { get; set; }

                /// <summary>
                /// 允许查看活动信息
                /// </summary>
                public bool CanViewActivities { get; set; }

                /// <summary>
                /// 允许修改活动信息
                /// </summary>
                public bool CanEditActivities { get; set; }

                /// <summary>
                /// 允许查看和修改系统设置
                /// </summary>
                public bool CanSettingSite { get; set; }

                /// <summary>
                /// 允许查看数据报表
                /// </summary>
                public bool CanViewDataReports { get; set; }

                /// <summary>
                /// 允许查看并添加资金支取记录
                /// </summary>
                public bool CanViewAndAddFundsReports { get; set; }

                /// <summary>
                /// 允许参看消息盒子
                /// </summary>
                public bool CanViewAndEditMessageBox { get; set; }

                /// <summary>
                /// 允许查看和管理“管理员”以其用户组并查看相关登陆、操作信息
                /// </summary>
                public bool CanViewAndEditManagers { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建管理员用户组的数据集
                /// </summary>
                /// <param name="name">名称</param>
                /// <param name="grade">等级</param>
                /// <param name="canViewUsers">允许查看前台用户列表</param>
                /// <param name="canEditUsers">允许创建和修改用户信息</param>
                /// <param name="canViewTickets">允许查看彩票信息</param>
                /// <param name="canEditTickets">允许修改彩票信息</param>
                /// <param name="canViewActivities">允许查看活动信息</param>
                /// <param name="canEditActivities">允许修改活动信息</param>
                /// <param name="canSettingSite">允许查看和修改系统设置</param>
                /// <param name="canViewDataReports">允许查看数据报表</param>
                /// <param name="canViewAndAddFundsReports">允许查看并添加资金支取记录</param>
                /// <param name="canViewAndEditMessageBox">允许参看消息盒子</param>
                /// <param name="canViewAndEditManagers">允许查看和管理“管理员”以其用户组并查看相关登陆、操作信息</param>
                public PackageForCreate(string name, int grade, bool canViewUsers, bool canEditUsers, bool canViewTickets
                    , bool canEditTickets, bool canViewActivities, bool canEditActivities, bool canSettingSite
                    , bool canViewDataReports, bool canViewAndAddFundsReports, bool canViewAndEditMessageBox
                    , bool canViewAndEditManagers)
                {
                    this.Name = name;
                    this.Grade = grade;
                    this.CanViewUsers = canViewUsers;
                    this.CanEditUsers = canEditUsers;
                    this.CanViewTickets = canViewTickets;
                    this.CanEditTickets = canEditTickets;
                    this.CanViewActivities = canViewActivities;
                    this.CanEditActivities = canEditActivities;
                    this.CanSettingSite = canSettingSite;
                    this.CanViewDataReports = canViewDataReports;
                    this.CanViewAndAddFundsReports = canViewAndAddFundsReports;
                    this.CanViewAndEditMessageBox = canViewAndEditMessageBox;
                    this.CanViewAndEditManagers = canViewAndEditManagers;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    var agSet = db.Set<AdministratorGroup>();
                    bool hadUsedName = agSet.Any(x => x.Name == this.Name);
                    if (hadUsedName)
                    {
                        throw new Exception(string.Format("已经存在同名的用户组：{0}", this.Name));
                    }
                    if (this.Grade < 1
                        || this.Grade > 255)
                    {
                        throw new Exception("用户组等级必须为 1 - 255 的正整数");
                    }
                    bool hadUsedGrade = agSet.Any(x => x.Grade == this.Grade);
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
                public AdministratorGroup GetEntity(DbContext db)
                {
                    return new AdministratorGroup(this.Name, this.Grade, this.CanViewUsers, this.CanEditUsers, this.CanViewTickets
                        , this.CanEditTickets, this.CanViewActivities, this.CanEditActivities, this.CanSettingSite
                        , this.CanViewDataReports, this.CanViewAndAddFundsReports, this.CanViewAndEditMessageBox
                        , this.CanViewAndEditManagers);
                }

                #endregion
            }

            /// <summary>
            /// 用于修改管理员用户组信息的数据集
            /// </summary>
            private class PackageForUpdate : PackageForUpdateBase<AdministratorGroup>, IPackage<AdministratorGroup>
                , IUpdatePackage<AdministratorGroup>
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
                /// 允许查看前台用户列表
                /// </summary>
                public bool CanViewUsers { get; set; }

                /// <summary>
                /// 允许创建和修改用户信息
                /// </summary>
                public bool CanEditUsers { get; set; }

                /// <summary>
                /// 允许查看彩票信息
                /// </summary>
                public bool CanViewTickets { get; set; }

                /// <summary>
                /// 允许修改彩票信息
                /// </summary>
                public bool CanEditTickets { get; set; }

                /// <summary>
                /// 允许查看活动信息
                /// </summary>
                public bool CanViewActivities { get; set; }

                /// <summary>
                /// 允许修改活动信息
                /// </summary>
                public bool CanEditActivities { get; set; }

                /// <summary>
                /// 允许查看和修改系统设置
                /// </summary>
                public bool CanSettingSite { get; set; }

                /// <summary>
                /// 允许查看数据报表
                /// </summary>
                public bool CanViewDataReports { get; set; }

                /// <summary>
                /// 允许查看并添加资金支取记录
                /// </summary>
                public bool CanViewAndAddFundsReports { get; set; }

                /// <summary>
                /// 允许参看消息盒子
                /// </summary>
                public bool CanViewAndEditMessageBox { get; set; }

                /// <summary>
                /// 允许查看和管理“管理员”以其用户组并查看相关登陆、操作信息
                /// </summary>
                public bool CanViewAndEditManagers { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于修改管理员用户组信息的数据集
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="name">名称</param>
                /// <param name="grade">等级</param>
                /// <param name="canViewUsers">允许查看前台用户列表</param>
                /// <param name="canEditUsers">允许创建和修改用户信息</param>
                /// <param name="canViewTickets">允许查看彩票信息</param>
                /// <param name="canEditTickets">允许修改彩票信息</param>
                /// <param name="canViewActivities">允许查看活动信息</param>
                /// <param name="canEditActivities">允许修改活动信息</param>
                /// <param name="canSettingSite">允许查看和修改系统设置</param>
                /// <param name="canViewDataReports">允许查看数据报表</param>
                /// <param name="canViewAndAddFundsReports">允许查看并添加资金支取记录</param>
                /// <param name="canViewAndEditMessageBox">允许参看消息盒子</param>
                /// <param name="canViewAndEditManagers">允许查看和管理“管理员”以其用户组并查看相关登陆、操作信息</param>
                public PackageForUpdate(int id, string name, int grade, bool canViewUsers, bool canEditUsers, bool canViewTickets
                    , bool canEditTickets, bool canViewActivities, bool canEditActivities, bool canSettingSite
                    , bool canViewDataReports, bool canViewAndAddFundsReports, bool canViewAndEditMessageBox
                    , bool canViewAndEditManagers)
                    : base(id)
                {
                    this.Name = name;
                    this.Grade = grade;
                    this.CanViewUsers = canViewUsers;
                    this.CanEditUsers = canEditUsers;
                    this.CanViewTickets = canViewTickets;
                    this.CanEditTickets = canEditTickets;
                    this.CanViewActivities = canViewActivities;
                    this.CanEditActivities = canEditActivities;
                    this.CanSettingSite = canSettingSite;
                    this.CanViewDataReports = canViewDataReports;
                    this.CanViewAndAddFundsReports = canViewAndAddFundsReports;
                    this.CanViewAndEditMessageBox = canViewAndEditMessageBox;
                    this.CanViewAndEditManagers = canViewAndEditManagers;
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
                    var agSet = db.Set<AdministratorGroup>();
                    bool hadUsedName = agSet.Any(x => x.Name == this.Name);
                    if (hadUsedName)
                    {
                        throw new Exception(string.Format("已经存在同名的用户组：{0}", this.Name));
                    }
                    if (this.Grade < 1
                        || this.Grade > 255)
                    {
                        throw new Exception("用户组等级必须为 1 - 255 的正整数");
                    }
                    bool hadUsedGrade = agSet.Any(x => x.Grade == this.Grade);
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
                public override AdministratorGroup GetEntity(DbContext db)
                {
                    this.AddToUpdating("Name", this.Name);
                    this.AddToUpdating("Grade", this.Grade);
                    this.AddToUpdating("CanViewUsers", this.CanViewUsers);
                    this.AddToUpdating("CanEditUsers", this.CanEditUsers);
                    this.AddToUpdating("CanViewTickets", this.CanViewTickets);
                    this.AddToUpdating("CanEditTickets", this.CanEditTickets);
                    this.AddToUpdating("CanViewActivities", this.CanViewActivities);
                    this.AddToUpdating("CanEditActivities", this.CanEditActivities);
                    this.AddToUpdating("CanSettingSite", this.CanSettingSite);
                    this.AddToUpdating("CanViewDataReports", this.CanViewDataReports);
                    this.AddToUpdating("CanViewAndAddFundsReports", this.CanViewAndAddFundsReports);
                    this.AddToUpdating("CanViewAndEditMessageBox", this.CanViewAndEditMessageBox);
                    this.AddToUpdating("CanViewAndEditManagers", this.CanViewAndEditManagers);

                    return base.GetEntity(db);
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}

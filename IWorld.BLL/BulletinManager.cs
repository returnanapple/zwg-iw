using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 公告的管理者对象
    /// </summary>
    public class BulletinManager : IWorld.BLL.SimplifyManagerBase<Bulletin>, IWorld.BLL.IManager<Bulletin>, IWorld.BLL.ISimplify<Bulletin>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的公告的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public BulletinManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 内嵌类型

        /// <summary>
        /// 内置工厂
        /// </summary>
        public class Factory
        {
            #region 静态方法

            /// <summary>
            /// 创建一个用于新建公告的数据集
            /// </summary>
            /// <param name="title">标题</param>
            /// <param name="context">正文</param>
            /// <param name="beginTime">开始时间</param>
            /// <param name="days">持续天数</param>
            /// <param name="autoDelete">过期自动删除</param>
            /// <returns>返回用于新建公告的数据集</returns>
            public static ICreatePackage<Bulletin> CreatePackageForCreate(string title, string context, string beginTime, int days
                , bool autoDelete)
            {
                return new PackageForCreate(title, context, beginTime, days, autoDelete);
            }

            /// <summary>
            /// 创建一个用于修改公告信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="title">标题</param>
            /// <param name="context">正文</param>
            /// <param name="days">持续天数</param>
            /// <param name="hide">暂停显示</param>
            /// <param name="autoDelete">过期自动删除</param>
            /// <returns>返回用于新建公告的数据集</returns>
            public static IUpdatePackage<Bulletin> CreatePackageForUpdate(int id, string title, string context, int days, bool hide
                , bool autoDelete)
            {
                return new PackageForUpdate(id, title, context, days, hide, autoDelete);
            }

            /// <summary>
            /// 创建一个用于修改公告信息的数据集（暂停/重新开始）
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="hide">暂停显示</param>
            /// <returns>返回用于新建公告的数据集</returns>
            public static IUpdatePackage<Bulletin> CreatePackageForUpdate(int id, bool hide)
            {
                return new PackageForUpdate_ShowOrHide(id, hide);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建公告的数据集
            /// </summary>
            private class PackageForCreate : IPackage<Bulletin>, ICreatePackage<Bulletin>
            {
                #region 公开属性

                /// <summary>
                /// 标题
                /// </summary>
                public string Title { get; set; }

                /// <summary>
                /// 正文
                /// </summary>
                public string Context { get; set; }

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
                /// 实例化一个新的用于新建公告的数据集
                /// </summary>
                /// <param name="title">标题</param>
                /// <param name="context">正文</param>
                /// <param name="beginTime">开始时间</param>
                /// <param name="days">持续天数</param>
                /// <param name="autoDelete">过期自动删除</param>
                public PackageForCreate(string title, string context, string beginTime, int days, bool autoDelete)
                {
                    this.Title = title;
                    this.Context = context;
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
                    if (this.Days < 1)
                    {
                        throw new Exception("持续天数至少为1天");
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public Bulletin GetEntity(DbContext db)
                {
                    List<int> t = this.BeginTime.Split(new char[] { '-' }).ToList().ConvertAll(x => Convert.ToInt32(x));
                    DateTime beginTime = new DateTime(t[0], t[1], t[2]);

                    return new Bulletin(this.Title, this.Context, beginTime, this.Days, this.AutoDelete);
                }

                #endregion
            }

            /// <summary>
            /// 用于修改公告信息的数据集
            /// </summary>
            private class PackageForUpdate : PackageForUpdateBase<Bulletin>, IPackage<Bulletin>, IUpdatePackage<Bulletin>
            {
                #region 公开属性

                /// <summary>
                /// 标题
                /// </summary>
                public string Title { get; set; }

                /// <summary>
                /// 正文
                /// </summary>
                public string Context { get; set; }

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
                /// 实例化一个新的用于修改公告信息的数据集
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="title">标题</param>
                /// <param name="context">正文</param>
                /// <param name="days">持续天数</param>
                /// <param name="hide">暂停显示</param>
                /// <param name="autoDelete">过期自动删除</param>
                public PackageForUpdate(int id, string title, string context, int days, bool hide, bool autoDelete)
                    : base(id)
                {
                    this.Title = title;
                    this.Context = context;
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
                public override Bulletin GetEntity(DbContext db)
                {
                    var t = db.Set<Bulletin>().Where(x => x.Id == this.Id)
                        .Select(x => new { x.Days, x.EndTime }).FirstOrDefault();
                    DateTime endTime = t.EndTime.AddDays(this.Days - t.Days);

                    this.AddToUpdating("Title", this.Title);
                    this.AddToUpdating("Context", this.Context);
                    this.AddToUpdating("Days", this.Days);
                    this.AddToUpdating("EndTime", endTime);
                    this.AddToUpdating("Hide", this.Hide);
                    this.AddToUpdating("AutoDelete", this.AutoDelete);

                    return base.GetEntity(db);
                }

                #endregion
            }

            /// <summary>
            /// 用于修改公告信息的数据集（暂停/重新开始）
            /// </summary>
            private class PackageForUpdate_ShowOrHide : PackageForUpdateBase<Bulletin>, IPackage<Bulletin>, IUpdatePackage<Bulletin>
            {
                #region 公开属性

                /// <summary>
                /// 暂停显示
                /// </summary>
                public bool Hide { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于修改公告信息的数据集（暂停/重新开始）
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="hide">暂停显示</param>
                public PackageForUpdate_ShowOrHide(int id, bool hide)
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
                public override Bulletin GetEntity(DbContext db)
                {
                    this.AddToUpdating("Hide", this.Hide);

                    return base.GetEntity(db);
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}

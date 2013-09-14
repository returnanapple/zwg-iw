using System;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Setting;

namespace IWorld.BLL
{
    /// <summary>
    /// 推广信息的管理者对象
    /// </summary>
    public class SpreadManager : SimplifyManagerBase<Spread>, IManager<Spread>, ISimplify<Spread>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的推广信息的管理者对象
        /// </summary>
        /// <param name="db"></param>
        public SpreadManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 使用推广标识码
        /// </summary>
        /// <param name="code">标识码</param>
        /// <returns>返回改标识码所对应的推广记录的推广人的信息封装</returns>
        public Author Use(string code)
        {
            Spread spread = db.Set<Spread>().FirstOrDefault(x => x.Code == code);
            if (spread == null) { throw new Exception("指定的标识码没有相对应的推广记录"); }
            if (spread.Used) { throw new Exception("指定的标识码所相对应的推广记录已经被使用"); }
            if (spread.ExpiredTime < DateTime.Now) { throw new Exception("指定的标识码所相对应的推广记录已过期"); }

            NEventArgs eventArgs = new NEventArgs(db, spread);
            if (UsingEventHandler != null)
            {
                UsingEventHandler(this, eventArgs); //触发前置事件
            }
            spread.Used = true;
            db.SaveChanges();
            if (UsedEventHandler != null)
            {
                UsedEventHandler(this, eventArgs); //触发后置事件
            }

            return spread.Owner;
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
            /// 创建一个用于新建推广记录的数据集
            /// </summary>
            /// <param name="ownerId">推广人的存储指针</param>
            /// <param name="normalReturnPoints">正常返点数</param>
            /// <param name="uncertainReturnPoints">不定位返点数</param>
            /// <returns>返回用于新建推广记录的数据集</returns>
            public static ICreatePackage<Spread> CreatePackageForCreate(int ownerId, double normalReturnPoints
                , double uncertainReturnPoints)
            {
                return new PackageForCreate(ownerId, normalReturnPoints, uncertainReturnPoints);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建推广记录的数据集
            /// </summary>
            private class PackageForCreate : IPackage<Spread>, ICreatePackage<Spread>
            {
                #region 公开属性

                /// <summary>
                /// 推广人的存储指针
                /// </summary>
                public int OwnerId { get; set; }

                /// <summary>
                /// 正常返点数
                /// </summary>
                public double NormalReturnPoints { get; set; }

                /// <summary>
                /// 不定位返点数
                /// </summary>
                public double UncertainReturnPoints { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建推广记录的数据集
                /// </summary>
                /// <param name="ownerId">推广人的存储指针</param>
                /// <param name="normalReturnPoints">正常返点数</param>
                /// <param name="uncertainReturnPoints">不定位返点数</param>
                public PackageForCreate(int ownerId, double normalReturnPoints, double uncertainReturnPoints)
                {
                    this.OwnerId = ownerId;
                    this.NormalReturnPoints = normalReturnPoints;
                    this.UncertainReturnPoints = uncertainReturnPoints;
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
                    var owner = db.Set<Author>().Where(x => x.Id == this.OwnerId)
                        .Select(x => new { x.MaxOfSubordinate, x.Subordinate })
                        .FirstOrDefault();
                    if (owner.MaxOfSubordinate <= owner.Subordinate)
                    {
                        throw new Exception("直属下级的数量已经达到上限");
                    }
                    NChecker.CheckerReturnPoints(this.NormalReturnPoints, this.UncertainReturnPoints, this.OwnerId, db);
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public Spread GetEntity(DbContext db)
                {
                    Author owner = db.Set<Author>().Find(this.OwnerId);
                    int hours = new WebSetting().SpreadKeepTime;
                    DateTime endTime = DateTime.Now.AddHours(hours);

                    return new Spread(owner, this.NormalReturnPoints, this.UncertainReturnPoints, endTime);
                }

                #endregion
            }

            #endregion
        }

        #endregion

        #region 有关事件

        /// <summary>
        /// 使用前将触发的事件
        /// </summary>
        public static event NDelegate UsingEventHandler;

        /// <summary>
        /// 使用后将触发的事件
        /// </summary>
        public static event NDelegate UsedEventHandler;

        #endregion
    }
}

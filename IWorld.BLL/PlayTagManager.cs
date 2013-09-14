using System;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 玩法标签的管理者对象
    /// </summary>
    public class PlayTagManager : SimplifyManagerBase<PlayTag>, IManager<PlayTag>, ISimplify<PlayTag>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的玩法标签的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public PlayTagManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 显示玩法标签
        /// </summary>
        /// <param name="tagId">目标玩法标签的存储指针</param>
        public void Show(int tagId)
        {
            NChecker.CheckEntity<PlayTag>(tagId, "玩法标签", db);
            PlayTag tag = db.Set<PlayTag>().Find(tagId);
            if (!tag.Hide)
            {
                throw new Exception("该玩法标签没有被隐藏");
            }

            tag.Hide = false;
            tag.ModifiedTime = DateTime.Now;
            db.SaveChanges();
        }

        /// <summary>
        /// 隐藏玩法标签
        /// </summary>
        /// <param name="tagId">目标玩法标签的存储指针</param>
        public void Hide(int tagId)
        {
            NChecker.CheckEntity<PlayTag>(tagId, "玩法标签", db);
            PlayTag tag = db.Set<PlayTag>().Find(tagId);
            if (tag.Hide)
            {
                throw new Exception("该玩法标签已经被隐藏");
            }

            tag.Hide = true;
            tag.ModifiedTime = DateTime.Now;
            db.SaveChanges();
        }

        #endregion

        #region 内嵌类型

        /// <summary>
        /// 内部工厂
        /// </summary>
        public class Fantory
        {
            #region 静态方法

            /// <summary>
            /// 创建一个新的用于新建玩法标签的数据集
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="ticketId">所属彩票的存储指针</param>
            /// <param name="order">排序系数</param>
            /// <returns>返回用于新建玩法标签的数据集</returns>
            public static ICreatePackage<PlayTag> CreatePackageForCreate(string name, int ticketId, int order)
            {
                return new PackageForCreate(name, ticketId, order);
            }

            /// <summary>
            /// 创建一个用于更新玩法标签信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="name">名称</param>
            /// <param name="order">排序系数</param>
            /// <returns>返回用于更新玩法标签信息的数据集</returns>
            public static IUpdatePackage<PlayTag> CreatePackageForUpdate(int id, string name, int order)
            {
                return new PackageForUpdate(id, name, order);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建玩法标签的数据集
            /// </summary>
            private class PackageForCreate : IPackage<PlayTag>, ICreatePackage<PlayTag>
            {
                #region 公开属性

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 所属彩票的存储指针
                /// </summary>
                public int TicketId { get; set; }

                /// <summary>
                /// 排序系数
                /// </summary>
                public int Order { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建玩法标签的数据集
                /// </summary>
                /// <param name="name">名称</param>
                /// <param name="ticketId">所属彩票的存储指针</param>
                /// <param name="order">排序系数</param>
                public PackageForCreate(string name, int ticketId, int order)
                {
                    this.Name = name;
                    this.TicketId = ticketId;
                    this.Order = order;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    NChecker.CheckEntity<LotteryTicket>(this.TicketId, "彩票", db);
                    bool usedName = db.Set<PlayTag>().Any(x => x.Name == this.Name
                        && x.Ticket.Id == this.TicketId);
                    if (usedName) { throw new Exception("制定的彩票中已经下辖同名的标签"); }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public PlayTag GetEntity(DbContext db)
                {
                    LotteryTicket ticket = db.Set<LotteryTicket>().Find(this.TicketId);

                    return new PlayTag(this.Name, ticket, this.Order);
                }

                #endregion
            }

            /// <summary>
            /// 用于更新玩法标签信息的数据集
            /// </summary>
            private class PackageForUpdate : PackageForUpdateBase<PlayTag>, IPackage<PlayTag>, IUpdatePackage<PlayTag>
            {
                #region 公开属性

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 排序系数
                /// </summary>
                public int Order { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于更新玩法标签信息的数据集
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="name">名称</param>
                /// <param name="order">排序系数</param>
                public PackageForUpdate(int id, string name, int order)
                    : base(id)
                {
                    this.Name = name;
                    this.Order = order;
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
                    bool usedName = db.Set<PlayTag>().Any(x => x.Name == this.Name
                        && x.Id != this.Id);
                    if (usedName) { throw new Exception("制定的彩票中已经下辖同名的标签"); }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override PlayTag GetEntity(DbContext db)
                {
                    this.AddToUpdating("Name", this.Name);
                    this.AddToUpdating("Order", this.Order);

                    return base.GetEntity(db);
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}

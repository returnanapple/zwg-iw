using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 虚拟排行的管理者对象
    /// </summary>
    public class VirtualTopManager : IWorld.BLL.SimplifyManagerBase<VirtualTop>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的虚拟排行的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public VirtualTopManager(DbContext db)
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
            /// 创建一个用于新建虚拟排行的数据集
            /// </summary>
            /// <param name="ticketId">对应的彩票的存储指针</param>
            /// <param name="sum">金额</param>
            /// <returns>返回用于新建虚拟排行的数据集</returns>
            public static ICreatePackage<VirtualTop> CreatePackageForCreate(int ticketId, double sum)
            {
                return new PackageForCreate(ticketId, sum);
            }

            /// <summary>
            /// 创建一个用户更新虚拟排行信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="ticketId">对应的彩票的存储指针</param>
            /// <param name="sum">金额</param>
            /// <returns>返回用户更新虚拟排行信息的数据集</returns>
            public static IUpdatePackage<VirtualTop> CreatePackageForUpdate(int id, int ticketId, double sum)
            {
                return new PackageForUpdate(id, ticketId, sum);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建虚拟排行的数据集
            /// </summary>
            private class PackageForCreate : IPackage<VirtualTop>, ICreatePackage<VirtualTop>
            {
                #region 公开属性

                /// <summary>
                /// 对应的彩票的存储指针
                /// </summary>
                public int TicketId { get; set; }

                /// <summary>
                /// 金额
                /// </summary>
                public double Sum { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建虚拟排行的数据集
                /// </summary>
                /// <param name="ticketId">对应的彩票的存储指针</param>
                /// <param name="sum">金额</param>
                public PackageForCreate(int ticketId, double sum)
                {
                    this.TicketId = ticketId;
                    this.Sum = sum;
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
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public VirtualTop GetEntity(DbContext db)
                {
                    LotteryTicket ticket = db.Set<LotteryTicket>().Find(this.TicketId);

                    return new VirtualTop(ticket, this.Sum);
                }

                #endregion
            }

            /// <summary>
            /// 用户更新虚拟排行信息的数据集
            /// </summary>
            private class PackageForUpdate : PackageForUpdateBase<VirtualTop>, IPackage<VirtualTop>, IUpdatePackage<VirtualTop>
            {
                #region 公开属性

                /// <summary>
                /// 对应的彩票的存储指针
                /// </summary>
                public int TicketId { get; set; }

                /// <summary>
                /// 金额
                /// </summary>
                public double Sum { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用户更新虚拟排行信息的数据集
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="ticketId">对应的彩票的存储指针</param>
                /// <param name="sum">金额</param>
                public PackageForUpdate(int id, int ticketId, double sum)
                    : base(id)
                {
                    this.TicketId = ticketId;
                    this.Sum = sum;
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
                    NChecker.CheckEntity<LotteryTicket>(this.TicketId, "彩票", db);
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override VirtualTop GetEntity(DbContext db)
                {
                    LotteryTicket ticket = db.Set<LotteryTicket>().Find(this.TicketId);

                    this.AddToUpdating("Ticket", ticket);
                    this.AddToUpdating("Sum", this.Sum);

                    return base.GetEntity(db);
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}

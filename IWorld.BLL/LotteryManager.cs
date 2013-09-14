using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 开奖记录的管理者对象
    /// </summary>
    public class LotteryManager : SimplifyManagerBase<Lottery>, IManager<Lottery>, ISimplify<Lottery>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的开奖记录的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public LotteryManager(DbContext db)
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
            /// 创建一个用于新建开奖记录的数据集
            /// </summary>
            /// <param name="phases">期数</param>
            /// <param name="sources">来源</param>
            /// <param name="ticketId">彩种的存储指针</param>
            /// <param name="seats">位</param>
            /// <param name="operatorId">操作人的存储指针</param>
            /// <returns>返回用于新建开奖记录的数据集</returns>
            public static ICreatePackage<Lottery> CreatePackageForCreate(string phases, LotterySources sources, int ticketId
                , List<IPackageForSeat> seats, int operatorId = 0)
            {
                return new PackageForCreate(phases, sources, operatorId, ticketId, seats);
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

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建开奖记录的数据集
            /// </summary>
            private class PackageForCreate : IPackage<Lottery>, ICreatePackage<Lottery>
            {
                #region 公开属性

                /// <summary>
                /// 期数
                /// </summary>
                public string Phases { get; set; }

                /// <summary>
                /// 来源
                /// </summary>
                public LotterySources Sources { get; set; }

                /// <summary>
                /// 操作人的存储指针
                /// </summary>
                public int OperatorId { get; set; }

                /// <summary>
                /// 彩种的存储指针
                /// </summary>
                public int TicketId { get; set; }

                /// <summary>
                /// 位
                /// </summary>
                public List<IPackageForSeat> Seats { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建开奖记录的数据集
                /// </summary>
                /// <param name="phases">期数</param>
                /// <param name="sources">来源</param>
                /// <param name="operatorId">操作人的存储指针</param>
                /// <param name="ticketId">彩种的存储指针</param>
                /// <param name="seats">位</param>
                public PackageForCreate(string phases, LotterySources sources, int operatorId, int ticketId
                    , List<IPackageForSeat> seats)
                {
                    this.Phases = phases;
                    this.Sources = sources;
                    this.OperatorId = operatorId;
                    this.TicketId = ticketId;
                    this.Seats = seats;
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
                    bool hadUsedPhases = db.Set<Lottery>().Any(x => x.Phases == this.Phases && x.Ticket.Id == this.TicketId);
                    if (hadUsedPhases)
                    {
                        throw new Exception("该期已经开奖");
                    }
                    if (this.OperatorId != 0)
                    {
                        NChecker.CheckEntity<Administrator>(this.OperatorId, "管理员账户", db);
                    }
                    LotteryTicket ticket = db.Set<LotteryTicket>().Find(this.TicketId);
                    this.Seats.ForEach(x =>
                        {
                            x.CheckSeat(ticket);
                        });
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public Lottery GetEntity(DbContext db)
                {
                    Administrator _operator = null;
                    if (this.OperatorId != 0)
                    {
                        _operator = db.Set<Administrator>().Find(this.OperatorId);
                    }
                    LotteryTicket ticket = db.Set<LotteryTicket>().Find(this.TicketId);
                    List<LotterySeat> seats = new List<LotterySeat>();
                    this.Seats.ForEach(x =>
                        {
                            seats.Add(x.GetSeat(ticket));
                        });

                    return new Lottery(this.Phases, this.Sources, _operator, ticket, seats);
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
                /// 对应的号码
                /// </summary>
                public string Value { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建位的数据集
                /// </summary>
                /// <param name="name">名称</param>
                /// <param name="value">对应的号码</param>
                public PackageForSeat(string name, string value)
                {
                    this.Name = name;
                    this.Value = value;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查位信息的合法性
                /// </summary>
                /// <param name="ticket">彩票</param>
                public void CheckSeat(LotteryTicket ticket)
                {
                    if (!ticket.Seats.Any(x => x.Name == this.Name))
                    {
                        throw new Exception(string.Format("位：{0} 并不属于彩票：{1}", this.Name, ticket.Name));
                    }
                    var seat = ticket.Seats.FirstOrDefault(x => x.Name == this.Name);
                    if (!seat.ValueList.Any(x => x == this.Value))
                    {
                        throw new Exception(string.Format("位：{0} 中并不包括值：{1}", this.Name, this.Value));
                    }
                }

                /// <summary>
                /// 获取位（开奖记录）
                /// </summary>
                /// <param name="ticket">彩票</param>
                /// <returns>返回位（开奖记录）的封装</returns>
                public LotterySeat GetSeat(LotteryTicket ticket)
                {
                    var seat = ticket.Seats.FirstOrDefault(x => x.Name == this.Name);

                    return new LotterySeat(this.Name, this.Value, seat.Order);
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
                /// <param name="ticket">彩票</param>
                void CheckSeat(LotteryTicket ticket);

                /// <summary>
                /// 获取位（开奖记录）
                /// </summary>
                /// <param name="ticket">彩票</param>
                /// <returns>返回位（开奖记录）的封装</returns>
                LotterySeat GetSeat(LotteryTicket ticket);
            }

            #endregion
        }

        #endregion
    }
}

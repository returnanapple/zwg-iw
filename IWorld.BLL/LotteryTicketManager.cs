using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 彩票的管理者对象
    /// </summary>
    public class LotteryTicketManager : SimplifyManagerBase<LotteryTicket>, IManager<LotteryTicket>, ISimplify<LotteryTicket>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的彩票的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public LotteryTicketManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 编辑开奖时间
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="times">新的开奖时间的集合</param>
        public void EditLotteryTimes(int ticketId, List<Factory.IPackageForTime> times)
        {
            NChecker.CheckEntity<LotteryTicket>(ticketId, "彩票", db);
            LotteryTicket ticket = db.Set<LotteryTicket>().Find(ticketId);
            List<LotteryTime> _times = new List<LotteryTime>();
            times.ForEach(x =>
                {
                    _times.Add(x.GetTime());
                });
            var ltSet = db.Set<LotteryTime>();

            List<LotteryTime> rTimes = ticket.Times.Where(x => !_times.Any(t => t.Phases == x.Phases)).ToList();
            ticket.Times.RemoveAll(x => rTimes.Contains(x));    //从列表中移除被删除的项
            rTimes.ForEach(x =>
                {
                    ltSet.Remove(x);    //将被删除的项从数据库中移除
                });
            ticket.Times.ForEach(x =>
                {
                    var t = _times.FirstOrDefault(_t => _t.Phases == x.Phases);
                    x.TimeValue = t.TimeValue;  //更新现有项为新数据
                    t.ModifiedTime = DateTime.Now;
                });
            _times.Where(x => !ticket.Times.Any(t => t.Phases == x.Phases)).ToList().ForEach(x =>
                {
                    ticket.Times.Add(x);    //添加新建项目
                });
            ticket.ModifiedTime = DateTime.Now;
            db.SaveChanges();
        }

        /// <summary>
        /// 显示彩票
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        public void Show(int ticketId)
        {
            NChecker.CheckEntity<LotteryTicket>(ticketId, "彩票", db);
            LotteryTicket ticket = db.Set<LotteryTicket>().Find(ticketId);
            if (!ticket.Hide)
            {
                throw new Exception("该彩票没有被隐藏");
            }

            ticket.Hide = false;
            ticket.ModifiedTime = DateTime.Now;
            db.SaveChanges();
        }

        /// <summary>
        /// 隐藏彩票
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        public void Hide(int ticketId)
        {
            NChecker.CheckEntity<LotteryTicket>(ticketId, "彩票", db);
            LotteryTicket ticket = db.Set<LotteryTicket>().Find(ticketId);
            if (ticket.Hide)
            {
                throw new Exception("该彩票已经被隐藏");
            }

            ticket.Hide = true;
            ticket.ModifiedTime = DateTime.Now;
            db.SaveChanges();
        }

        public void ChangeNextPhases(int ticketId, string nextPhases)
        {
            NChecker.CheckEntity<LotteryTicket>(ticketId, "彩票", db);
            LotteryTicket ticket = db.Set<LotteryTicket>().Find(ticketId);
            ticket.NextPhases = nextPhases;
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
            /// 创建一个用于新建彩票的数据集
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="times">开奖时间</param>
            /// <param name="seats">位</param>
            /// <param name="order">排序系数</param>
            /// <returns>返回用于新建彩票的数据集</returns>
            public static ICreatePackage<LotteryTicket> CreatePackageForCreate(string name, List<IPackageForTime> times
                , List<IPackageForSeat> seats, int order)
            {
                return new PackageForCreate(name, times, seats, order);
            }

            /// <summary>
            /// 创建一个修改彩票名称的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="name">名称</param>
            /// <param name="order">排序系数</param>
            /// <returns>返回修改彩票名称的数据集</returns>
            public static IUpdatePackage<LotteryTicket> CreatePackageForUpdateName(int id, string name, int order)
            {
                return new PackageForUpdate(id, name, "", order);
            }

            /// <summary>
            /// 创建一个修改彩票下期开奖期号的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="nextPhases">下期期号</param>
            /// <returns>返回修改彩票下期开奖期号的数据集</returns>
            public static IUpdatePackage<LotteryTicket> CreatePackageForUpdateNextPhases(int id, string nextPhases)
            {
                return new PackageForUpdate(id, "", nextPhases, 0);
            }

            /// <summary>
            /// 创建一个用于新建位的数据集
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="isSpecial">表示该位是否特殊的位</param>
            /// <param name="values">所属的号码的集合</param>
            /// <param name="order">排序系数</param>
            /// <returns>返回用于新建位的数据集</returns>
            public static IPackageForSeat CreatePackageForSeat(string name, bool isSpecial, string values, int order)
            {
                return new PackageForSeat(name, isSpecial, values, order);
            }

            /// <summary>
            /// 创建一个用于新建开奖时间的数据集
            /// </summary>
            /// <param name="phases">期数</param>
            /// <param name="timeValue">时间的值（“小时：分”格式）</param>
            /// <returns>返回用于新建开奖时间的数据集</returns>
            public static IPackageForTime CreatePackageForTime(int phases, string timeValue)
            {
                return new PackageForTime(phases, timeValue);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建彩票的数据集
            /// </summary>
            private class PackageForCreate : IPackage<LotteryTicket>, ICreatePackage<LotteryTicket>
            {
                #region 公开属性

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 开奖时间
                /// </summary>
                public List<IPackageForTime> Times { get; set; }

                /// <summary>
                /// 位
                /// </summary>
                public List<IPackageForSeat> Seats { get; set; }

                /// <summary>
                /// 排序系数
                /// </summary>
                public int Order { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建彩票的数据集
                /// </summary>
                /// <param name="name">名称</param>
                /// <param name="times">开奖时间</param>
                /// <param name="seats">位</param>
                /// <param name="order">排序系数</param>
                public PackageForCreate(string name, List<IPackageForTime> times, List<IPackageForSeat> seats, int order)
                {
                    this.Name = name;
                    this.Times = times;
                    this.Seats = seats;
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
                    bool hadUsedName = db.Set<LotteryTicket>().Any(x => x.Name == this.Name);
                    if (hadUsedName)
                    {
                        throw new Exception("已经存在同名的彩票");
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public LotteryTicket GetEntity(DbContext db)
                {
                    List<LotteryTime> times = new List<LotteryTime>();
                    this.Times.ForEach(x =>
                        {
                            times.Add(x.GetTime());
                        });
                    List<LotteryTicketSeat> seats = new List<LotteryTicketSeat>();
                    this.Seats.ForEach(x =>
                        {
                            seats.Add(x.GetSeat());
                        });

                    return new LotteryTicket(this.Name, times, seats, this.Order);
                }

                #endregion
            }

            /// <summary>
            /// 用于更新彩票信息的数据集
            /// </summary>
            private class PackageForUpdate : PackageForUpdateBase<LotteryTicket>, IPackage<LotteryTicket>
                , IUpdatePackage<LotteryTicket>
            {
                #region 公开属性

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 下一期
                /// </summary>
                public string NextPhases { get; set; }

                /// <summary>
                /// 排序系数
                /// </summary>
                public int Order { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于更新彩票信息的数据集
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="name">名称</param>
                /// <param name="order">排序系数</param>
                /// <param name="nextPhases">下一期</param>
                public PackageForUpdate(int id, string name, string nextPhases, int order)
                    : base(id)
                {
                    this.Name = name;
                    this.NextPhases = nextPhases;
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
                    if (this.Name != "")
                    {
                        bool hadUsedName = db.Set<LotteryTicket>().Any(x => x.Id != this.Id && x.Name == this.Name);
                        if (hadUsedName)
                        {
                            throw new Exception("已经存在同名的彩票");
                        }
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override LotteryTicket GetEntity(DbContext db)
                {
                    if (this.Name != "")
                    {
                        this.AddToUpdating("Name", this.Name);
                        this.AddToUpdating("Order", this.Order);
                    }
                    if (this.NextPhases != "")
                    {
                        this.AddToUpdating("NextPhases", this.NextPhases);
                    }

                    return base.GetEntity(db);
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
                /// 表示该位是否特殊的位
                /// </summary>
                public bool IsSpecial { get; set; }

                /// <summary>
                /// 所属的号码的集合
                /// </summary>
                public string Values { get; set; }

                /// <summary>
                /// 排序系数
                /// </summary>
                public int Order { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建位的数据集
                /// </summary>
                /// <param name="name">名称</param>
                /// <param name="isSpecial">表示该位是否特殊的位</param>
                /// <param name="values">所属的号码的集合</param>
                /// <param name="order">排序系数</param>
                public PackageForSeat(string name, bool isSpecial, string values, int order)
                {
                    this.Name = name;
                    this.IsSpecial = isSpecial;
                    this.Values = values;
                    this.Order = order;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 获取位（彩票）
                /// </summary>
                /// <returns>返回位（彩票）的封装</returns>
                public LotteryTicketSeat GetSeat()
                {
                    return new LotteryTicketSeat(this.Name, this.IsSpecial, this.Values, this.Order);
                }

                #endregion
            }

            /// <summary>
            /// 用于新建开奖时间的数据集
            /// </summary>
            private class PackageForTime : IPackageForTime
            {
                #region 公开属性

                /// <summary>
                /// 期数
                /// </summary>
                public int Phases { get; set; }

                /// <summary>
                /// 时间的值（“小时：分”格式）
                /// </summary>
                public string TimeValue { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建开奖时间的数据集
                /// </summary>
                /// <param name="phases">期数</param>
                /// <param name="timeValue">时间的值（“小时：分”格式）</param>
                public PackageForTime(int phases, string timeValue)
                {
                    this.Phases = phases;
                    this.TimeValue = timeValue;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 获取开奖时间
                /// </summary>
                /// <returns>返回开奖时间的封装</returns>
                public LotteryTime GetTime()
                {
                    return new LotteryTime(this.Phases, this.TimeValue);
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
                /// 获取位（彩票）
                /// </summary>
                /// <returns>返回位（彩票）的封装</returns>
                LotteryTicketSeat GetSeat();
            }

            /// <summary>
            /// 定义用于新建开奖时间的数据集
            /// </summary>
            public interface IPackageForTime
            {
                /// <summary>
                /// 获取开奖时间
                /// </summary>
                /// <returns>返回开奖时间的封装</returns>
                LotteryTime GetTime();
            }

            #endregion
        }

        #endregion

        #region 静态方法

        static List<string> _hadUpdateTickets = new List<string>();
        /// <summary>
        /// 更新实际开奖时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void UpdateLotteryTime(object sender, NEventArgs e)
        {
            Lottery lottery = (Lottery)e.State;
            int phases = 0;
            #region 确认当前开奖是当天的第几期
            if (lottery.Ticket.Name == "重庆时时彩" || lottery.Ticket.Name == "江西时时彩")
            {
                char[] t = lottery.Phases.ToArray();
                int count = t.Count();
                string p = "";
                for (int i = count - 3; i < count; i++)
                {
                    p += t[i].ToString();
                }
                phases = Convert.ToInt32(p);
            }
            else if (lottery.Ticket.Name == "新疆时时彩" || lottery.Ticket.Name == "十一夺运金"
                || lottery.Ticket.Name == "广东十一选五" || lottery.Ticket.Name == "上海时时乐")
            {
                char[] t = lottery.Phases.ToArray();
                int count = t.Count();
                string p = "";
                for (int i = count - 2; i < count; i++)
                {
                    p += t[i].ToString();
                }
                phases = Convert.ToInt32(p);
            }
            else if (lottery.Ticket.Name == "福彩3D" || lottery.Ticket.Name == "排列三")
            {
                phases = 1;
            }
            #endregion

            #region 更新开奖时间

            if (phases == 0)
            {
                return;
            }
            if (!_hadUpdateTickets.Contains(lottery.Ticket.Name))
            {
                _hadUpdateTickets.Add(lottery.Ticket.Name);
                return;
            }
            LotteryTime lotteryTime = e.Db.Set<LotteryTime>().FirstOrDefault(x => x.TicketId == lottery.Ticket.Id
                && x.Phases == phases);
            if (lotteryTime == null)
            {
                return;
            }
            lotteryTime.TimeValue = string.Format("{0}:{1}", DateTime.Now.Hour, DateTime.Now.Minute);
            e.Db.SaveChanges();

            #endregion
        }

        #endregion
    }
}

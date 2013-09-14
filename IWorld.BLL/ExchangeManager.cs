using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Helper;

namespace IWorld.BLL
{
    /// <summary>
    /// 兑换活动的管理者对象
    /// </summary>
    public class ExchangeManager : SimplifyManagerBase<Exchange>, IManager<Exchange>, ISimplify<Exchange>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的兑换活动的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public ExchangeManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 编辑奖品列表
        /// </summary>
        /// <param name="exchangeId">目测活动的存储主子很</param>
        /// <param name="prizes">奖品列表</param>
        public void EditPrize(int exchangeId, List<Factory.IPackageForPrize> prizes)
        {
            NChecker.CheckEntity<Exchange>(exchangeId, "活动", db);
            Exchange exchange = db.Set<Exchange>().Find(exchangeId);
            if (exchange.BeginTime < DateTime.Now)
            {
                throw new Exception("已经开始的活动不允许编辑相关信息");
            }
            prizes.ForEach(x =>
                {
                    x.CheckPrize();
                });
            List<Prize> _prizes = prizes.ConvertAll(x => x.GetPrize());
            var pSet = db.Set<Prize>();

            //分支：新列表的奖品数量小于或等于原奖品
            if (exchange.Prizes.Count >= _prizes.Count)
            {
                List<Prize> rPrizes = exchange.Prizes.OrderBy(x => x.Id)
                    .Skip(_prizes.Count)
                    .ToList();
                exchange.Prizes.RemoveAll(x => rPrizes.Contains(x));    //从奖品中移除多余的条件记录
                rPrizes.ForEach(x =>
                {
                    pSet.Remove(x);    //从数据库从移除多余的条件记录
                });
                //修改原奖品至新状态
                for (int i = 0; i < _prizes.Count; i++)
                {
                    exchange.Prizes[i].Name = _prizes[0].Name;
                    exchange.Prizes[i].Description = _prizes[0].Description;
                    exchange.Prizes[i].Sum = _prizes[0].Sum;
                    exchange.Prizes[i].Type = _prizes[0].Type;
                    exchange.Prizes[i].Price = _prizes[0].Price;
                    exchange.Prizes[i].Remark = _prizes[0].Remark;
                    exchange.Prizes[i].ModifiedTime = DateTime.Now;
                }
            }
            //分支：新列表的奖品数量大于原奖品
            else
            {
                IEnumerable<Prize> aPrizes = _prizes.OrderBy(x => x.Type)
                    .Skip(exchange.Prizes.Count);
                List<Prize> ePrizes = _prizes.Where(x => !aPrizes.Contains(x)).ToList();
                //修改原奖品至新状态
                for (int i = 0; i < exchange.Conditions.Count; i++)
                {
                    exchange.Prizes[i].Name = _prizes[0].Name;
                    exchange.Prizes[i].Description = _prizes[0].Description;
                    exchange.Prizes[i].Sum = _prizes[0].Sum;
                    exchange.Prizes[i].Type = _prizes[0].Type;
                    exchange.Prizes[i].Price = _prizes[0].Price;
                    exchange.Prizes[i].Remark = _prizes[0].Remark;
                    exchange.Prizes[i].ModifiedTime = DateTime.Now;
                }
                exchange.Prizes.AddRange(aPrizes);  //将新添加的添加记录添加到奖品列表
            }

            exchange.ModifiedTime = DateTime.Now;
            db.SaveChanges();
        }

        /// <summary>
        /// 编辑限制条件
        /// </summary>
        /// <param name="exchangeId">目标活动的存储指针</param>
        /// <param name="conditions">限制条件列表</param>
        public void EditCondition(int exchangeId, List<Factory.IPackageForCondition> conditions)
        {
            NChecker.CheckEntity<Exchange>(exchangeId, "活动", db);
            Exchange exchange = db.Set<Exchange>().Find(exchangeId);
            if (exchange.BeginTime < DateTime.Now)
            {
                throw new Exception("已经开始的活动不能编辑相关信息");
            }
            conditions.ForEach(x =>
            {
                x.CheckCondition();
            });
            List<ExchangeCondition> _conditions = conditions.ConvertAll(x => x.GetCondition());
            var ecSet = db.Set<ExchangeCondition>();

            //分支：新列表的限制条件数量小于或等于原限制条件
            if (exchange.Conditions.Count >= _conditions.Count)
            {
                List<ExchangeCondition> rConditions = exchange.Conditions.OrderBy(x => x.Id)
                    .Skip(_conditions.Count)
                    .ToList();
                exchange.Conditions.RemoveAll(x => rConditions.Contains(x));    //从限制条件中移除多余的条件记录
                rConditions.ForEach(x =>
                {
                    ecSet.Remove(x);    //从数据库从移除多余的条件记录
                });
                //修改原限制条件至新状态
                for (int i = 0; i < _conditions.Count; i++)
                {
                    exchange.Conditions[i].Type = _conditions[i].Type;
                    exchange.Conditions[i].Limit = _conditions[i].Limit;
                    exchange.Conditions[i].Upper = _conditions[i].Upper;
                    exchange.Conditions[i].ModifiedTime = DateTime.Now;
                }
            }
            //分支：新列表的限制条件数量大于原限制条件
            else
            {
                IEnumerable<ExchangeCondition> aConditions = _conditions.OrderBy(x => x.Type)
                    .Skip(exchange.Conditions.Count);
                List<ExchangeCondition> eConditions = _conditions.Where(x => !aConditions.Contains(x)).ToList();
                //修改原限制条件至新状态
                for (int i = 0; i < exchange.Conditions.Count; i++)
                {
                    exchange.Conditions[i].Type = eConditions[i].Type;
                    exchange.Conditions[i].Limit = eConditions[i].Limit;
                    exchange.Conditions[i].Upper = eConditions[i].Upper;
                    exchange.Conditions[i].ModifiedTime = DateTime.Now;
                }
                exchange.Conditions.AddRange(aConditions);  //将新添加的添加记录添加到限制条件列表
            }

            exchange.ModifiedTime = DateTime.Now;
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
            /// 创建一个用于新建兑换活动的数据集
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="places">名额</param>
            /// <param name="unitPrice">单价</param>
            /// <param name="eachPersonCanExchangeTheNumberOfTimes">每人每次允许兑换数量</param>
            /// <param name="eachPersonCanExchangeTheTimesOfDays">每人每天允许兑换次数</param>
            /// <param name="eachPersonCanExchangeTheNumberOfDays">每人每天允许兑换总数量</param>
            /// <param name="eachPersonCanExchangeTheTimesOfAll">每人允许参与总次数</param>
            /// <param name="eachPersonCanExchangeTheNumberOfAll">每人允许兑换总数量</param>
            /// <param name="prizes">奖品</param>
            /// <param name="conditions">参与条件</param>
            /// <param name="beginTime">开始时间</param>
            /// <param name="days">持续天数</param>
            /// <param name="autoDelete">过期自动删除</param>
            /// <returns>返回用于新建兑换活动的数据集</returns>
            public static ICreatePackage<Exchange> CreatePackageForCreate(string name, int places, double unitPrice
                , int eachPersonCanExchangeTheNumberOfTimes, int eachPersonCanExchangeTheTimesOfDays, int eachPersonCanExchangeTheNumberOfDays
                , int eachPersonCanExchangeTheTimesOfAll, int eachPersonCanExchangeTheNumberOfAll, List<IPackageForPrize> prizes
                , List<IPackageForCondition> conditions, string beginTime, int days, bool autoDelete)
            {
                return new PackageForCreate(name, places, unitPrice, eachPersonCanExchangeTheNumberOfTimes, eachPersonCanExchangeTheTimesOfDays
                    , eachPersonCanExchangeTheNumberOfDays, eachPersonCanExchangeTheTimesOfAll, eachPersonCanExchangeTheNumberOfAll, prizes
                    , conditions, beginTime, days, autoDelete);
            }

            /// <summary>
            /// 创建一个用于更新兑换活动信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="name">名称</param>
            /// <param name="places">名额</param>
            /// <param name="unitPrice">单价</param>
            /// <param name="eachPersonCanExchangeTheNumberOfTimes">每人每次允许兑换数量</param>
            /// <param name="eachPersonCanExchangeTheTimesOfDays">每人每天允许兑换次数</param>
            /// <param name="eachPersonCanExchangeTheNumberOfDays">每人每天允许兑换总数量</param>
            /// <param name="eachPersonCanExchangeTheTimesOfAll">每人允许参与总次数</param>
            /// <param name="eachPersonCanExchangeTheNumberOfAll">每人允许兑换总数量</param>
            /// <param name="days">持续天数</param>
            /// <param name="hide">暂停显示</param>
            /// <param name="autoDelete">过期自动删除</param>
            /// <returns>返回用于更新兑换活动信息的数据集</returns>
            public static IUpdatePackage<Exchange> CreatePackageForUpdate(int id, string name, int places, double unitPrice
                , int eachPersonCanExchangeTheNumberOfTimes, int eachPersonCanExchangeTheTimesOfDays, int eachPersonCanExchangeTheNumberOfDays
                , int eachPersonCanExchangeTheTimesOfAll, int eachPersonCanExchangeTheNumberOfAll, int days, bool hide, bool autoDelete)
            {
                return new PackageForUpdate(id, name, places, unitPrice, eachPersonCanExchangeTheNumberOfTimes
                    , eachPersonCanExchangeTheTimesOfDays, eachPersonCanExchangeTheNumberOfDays, eachPersonCanExchangeTheTimesOfAll
                    , eachPersonCanExchangeTheNumberOfAll, days, hide, autoDelete);
            }

            /// <summary>
            /// 创建一个用于更新兑换活动信息的数据集（基础）
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="days">持续天数</param>
            /// <param name="hide">暂停显示</param>
            /// <param name="autoDelete">过期自动删除</param>
            /// <returns>返回用于更新兑换活动信息的数据集</returns>
            public static IUpdatePackage<Exchange> CreatePackageForUpdate(int id, int days, bool hide, bool autoDelete)
            {
                return new PackageForUpdate_Basic(id, days, hide, autoDelete);
            }

            /// <summary>
            /// 创建一个用于更新兑换活动信息的数据集（暂停/重新开始）
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="hide">暂停显示</param>
            /// <returns>返回用于更新兑换活动信息的数据集</returns>
            public static IUpdatePackage<Exchange> CreatePackageForUpdate(int id, bool hide)
            {
                return new PackageForUpdate_ShowOrHide(id, hide);
            }

            /// <summary>
            /// 创建一个用于新建奖品的数据集
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="description">描述</param>
            /// <param name="sum">数额</param>
            /// <param name="type">类型</param>
            /// <param name="price">价值</param>
            /// <param name="remark">备注</param>
            /// <returns>返回用于新建奖品的数据集</returns>
            public static IPackageForPrize CreatePackageForPrize(string name, string description, int sum, string type, double price
                , string remark = "")
            {
                PrizeType _type = EnumHelper.Parse<PrizeType>(type);

                return new PackageForPrize(name, description, sum, _type, price, remark);
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
            /// 用于新建兑换活动的数据集
            /// </summary>
            private class PackageForCreate : IPackage<Exchange>, ICreatePackage<Exchange>
            {
                #region 公开属性

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 名额
                /// </summary>
                public int Places { get; set; }

                /// <summary>
                /// 单价
                /// </summary>
                public double UnitPrice { get; set; }

                /// <summary>
                /// 每人每次允许兑换数量
                /// </summary>
                public int EachPersonCanExchangeTheNumberOfTimes { get; set; }

                /// <summary>
                /// 每人每天允许兑换次数
                /// </summary>
                public int EachPersonCanExchangeTheTimesOfDays { get; set; }

                /// <summary>
                /// 每人每天允许兑换总数量
                /// </summary>
                public int EachPersonCanExchangeTheNumberOfDays { get; set; }

                /// <summary>
                /// 每人允许参与总次数
                /// </summary>
                public int EachPersonCanExchangeTheTimesOfAll { get; set; }

                /// <summary>
                /// 每人允许兑换总数量
                /// </summary>
                public int EachPersonCanExchangeTheNumberOfAll { get; set; }

                /// <summary>
                /// 奖品
                /// </summary>
                public List<IPackageForPrize> Prizes { get; set; }

                /// <summary>
                /// 参与条件
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
                /// 实例化一个新的用于新建兑换活动的数据集
                /// </summary>
                /// <param name="name">名称</param>
                /// <param name="places">名额</param>
                /// <param name="unitPrice">单价</param>
                /// <param name="eachPersonCanExchangeTheNumberOfTimes">每人每次允许兑换数量</param>
                /// <param name="eachPersonCanExchangeTheTimesOfDays">每人每天允许兑换次数</param>
                /// <param name="eachPersonCanExchangeTheNumberOfDays">每人每天允许兑换总数量</param>
                /// <param name="eachPersonCanExchangeTheTimesOfAll">每人允许参与总次数</param>
                /// <param name="eachPersonCanExchangeTheNumberOfAll">每人允许兑换总数量</param>
                /// <param name="prizes">奖品</param>
                /// <param name="conditions">参与条件</param>
                /// <param name="beginTime">开始时间</param>
                /// <param name="days">持续天数</param>
                /// <param name="autoDelete">过期自动删除</param>
                public PackageForCreate(string name, int places, double unitPrice, int eachPersonCanExchangeTheNumberOfTimes
                    , int eachPersonCanExchangeTheTimesOfDays, int eachPersonCanExchangeTheNumberOfDays, int eachPersonCanExchangeTheTimesOfAll
                    , int eachPersonCanExchangeTheNumberOfAll, List<IPackageForPrize> prizes, List<IPackageForCondition> conditions
                    , string beginTime, int days, bool autoDelete)
                {
                    this.Name = name;
                    this.Places = places;
                    this.UnitPrice = unitPrice;
                    this.EachPersonCanExchangeTheNumberOfTimes = eachPersonCanExchangeTheNumberOfTimes;
                    this.EachPersonCanExchangeTheTimesOfDays = eachPersonCanExchangeTheTimesOfDays;
                    this.EachPersonCanExchangeTheNumberOfDays = eachPersonCanExchangeTheNumberOfDays;
                    this.EachPersonCanExchangeTheTimesOfAll = eachPersonCanExchangeTheTimesOfAll;
                    this.EachPersonCanExchangeTheNumberOfAll = eachPersonCanExchangeTheNumberOfAll;
                    this.Prizes = prizes;
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
                    if (this.Places < 1)
                    {
                        throw new Exception("名额不能小于1");
                    }
                    if (this.UnitPrice < 0)
                    {
                        throw new Exception("兑换单价不能小于0");
                    }
                    if (this.EachPersonCanExchangeTheNumberOfTimes < 1)
                    {
                        throw new Exception("每人每次允许兑换数量不能小于1");
                    }
                    if (this.EachPersonCanExchangeTheTimesOfDays < 1)
                    {
                        throw new Exception("每人每天允许兑换总数量不能小于1");
                    }
                    if (this.EachPersonCanExchangeTheNumberOfDays < this.EachPersonCanExchangeTheNumberOfTimes)
                    {
                        throw new Exception("每人每天允许兑换总数量不能小于单次兑换数量的最高额");
                    }
                    if (this.EachPersonCanExchangeTheTimesOfAll < this.EachPersonCanExchangeTheTimesOfDays)
                    {
                        throw new Exception("每人允许参与总次数不能小于单日兑换次数的最高额");
                    }
                    if (this.EachPersonCanExchangeTheNumberOfAll < this.EachPersonCanExchangeTheNumberOfTimes)
                    {
                        throw new Exception("每人允许兑换总数量不能小于单次兑换数量的最高额");
                    }
                    if (this.Prizes.Count < 1)
                    {
                        throw new Exception("奖品数量不能小于1");
                    }
                    if (this.Days < 1)
                    {
                        throw new Exception("持续天数不能小于1");
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public Exchange GetEntity(DbContext db)
                {
                    List<int> t = this.BeginTime.Split(new char[] { '-' }).ToList().ConvertAll(x => Convert.ToInt32(x));
                    DateTime beginTime = new DateTime(t[0], t[1], t[2]);
                    List<Prize> prizes = this.Prizes.ConvertAll(x => x.GetPrize());
                    List<ExchangeCondition> conditions = this.Conditions.ConvertAll(x => x.GetCondition());

                    return new Exchange(this.Name, this.Places, this.UnitPrice, this.EachPersonCanExchangeTheNumberOfTimes
                        , this.EachPersonCanExchangeTheTimesOfDays, this.EachPersonCanExchangeTheNumberOfDays
                        , this.EachPersonCanExchangeTheTimesOfAll, this.EachPersonCanExchangeTheNumberOfAll, prizes, conditions
                        , beginTime, this.Days, this.AutoDelete);
                }

                #endregion
            }

            /// <summary>
            /// 用于更新兑换活动信息的数据集
            /// </summary>
            private class PackageForUpdate : PackageForUpdateBase<Exchange>, IPackage<Exchange>, IUpdatePackage<Exchange>
            {
                #region 公开属性

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 名额
                /// </summary>
                public int Places { get; set; }

                /// <summary>
                /// 单价
                /// </summary>
                public double UnitPrice { get; set; }

                /// <summary>
                /// 每人每次允许兑换数量
                /// </summary>
                public int EachPersonCanExchangeTheNumberOfTimes { get; set; }

                /// <summary>
                /// 每人每天允许兑换次数
                /// </summary>
                public int EachPersonCanExchangeTheTimesOfDays { get; set; }

                /// <summary>
                /// 每人每天允许兑换总数量
                /// </summary>
                public int EachPersonCanExchangeTheNumberOfDays { get; set; }

                /// <summary>
                /// 每人允许参与总次数
                /// </summary>
                public int EachPersonCanExchangeTheTimesOfAll { get; set; }

                /// <summary>
                /// 每人允许兑换总数量
                /// </summary>
                public int EachPersonCanExchangeTheNumberOfAll { get; set; }

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
                /// 实例化一个新的用于更新兑换活动信息的数据集
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="name">名称</param>
                /// <param name="places">名额</param>
                /// <param name="unitPrice">单价</param>
                /// <param name="eachPersonCanExchangeTheNumberOfTimes">每人每次允许兑换数量</param>
                /// <param name="eachPersonCanExchangeTheTimesOfDays">每人每天允许兑换次数</param>
                /// <param name="eachPersonCanExchangeTheNumberOfDays">每人每天允许兑换总数量</param>
                /// <param name="eachPersonCanExchangeTheTimesOfAll">每人允许参与总次数</param>
                /// <param name="eachPersonCanExchangeTheNumberOfAll">每人允许兑换总数量</param>
                /// <param name="days">持续天数</param>
                /// <param name="hide">暂停显示</param>
                /// <param name="autoDelete">过期自动删除</param>
                public PackageForUpdate(int id, string name, int places, double unitPrice, int eachPersonCanExchangeTheNumberOfTimes
                    , int eachPersonCanExchangeTheTimesOfDays, int eachPersonCanExchangeTheNumberOfDays, int eachPersonCanExchangeTheTimesOfAll
                    , int eachPersonCanExchangeTheNumberOfAll, int days, bool hide, bool autoDelete)
                    : base(id)
                {
                    this.Name = name;
                    this.Places = places;
                    this.UnitPrice = unitPrice;
                    this.EachPersonCanExchangeTheNumberOfTimes = eachPersonCanExchangeTheNumberOfTimes;
                    this.EachPersonCanExchangeTheTimesOfDays = eachPersonCanExchangeTheTimesOfDays;
                    this.EachPersonCanExchangeTheNumberOfDays = eachPersonCanExchangeTheNumberOfDays;
                    this.EachPersonCanExchangeTheTimesOfAll = eachPersonCanExchangeTheTimesOfAll;
                    this.EachPersonCanExchangeTheNumberOfAll = eachPersonCanExchangeTheNumberOfAll;
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
                    DateTime beginTime = db.Set<Exchange>().Where(x => x.Id == this.Id)
                        .Select(x => x.BeginTime).FirstOrDefault();
                    if (beginTime < DateTime.Now)
                    {
                        throw new Exception("已经开始的活动不能编辑相关信息");
                    }
                    if (this.Places < 1)
                    {
                        throw new Exception("名额不能小于1");
                    }
                    if (this.UnitPrice < 0)
                    {
                        throw new Exception("兑换单价不能小于0");
                    }
                    if (this.EachPersonCanExchangeTheNumberOfTimes < 1)
                    {
                        throw new Exception("每人每次允许兑换数量不能小于1");
                    }
                    if (this.EachPersonCanExchangeTheTimesOfDays < 1)
                    {
                        throw new Exception("每人每天允许兑换总数量不能小于1");
                    }
                    if (this.EachPersonCanExchangeTheNumberOfDays < this.EachPersonCanExchangeTheNumberOfTimes)
                    {
                        throw new Exception("每人每天允许兑换总数量不能小于单次兑换数量的最高额");
                    }
                    if (this.EachPersonCanExchangeTheTimesOfAll < this.EachPersonCanExchangeTheTimesOfDays)
                    {
                        throw new Exception("每人允许参与总次数不能小于单日兑换次数的最高额");
                    }
                    if (this.EachPersonCanExchangeTheNumberOfAll < this.EachPersonCanExchangeTheNumberOfTimes)
                    {
                        throw new Exception("每人允许兑换总数量不能小于单次兑换数量的最高额");
                    }
                    if (this.Days < 1)
                    {
                        throw new Exception("持续天数不能小于1");
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override Exchange GetEntity(DbContext db)
                {
                    var t = db.Set<Bulletin>().Where(x => x.Id == this.Id)
                        .Select(x => new { x.Days, x.EndTime }).FirstOrDefault();
                    DateTime endTime = t.EndTime.AddDays(this.Days - t.Days);

                    this.AddToUpdating("Name", this.Name);
                    this.AddToUpdating("Places", this.Places);
                    this.AddToUpdating("UnitPrice", this.UnitPrice);
                    this.AddToUpdating("EachPersonCanExchangeTheNumberOfTimes", this.EachPersonCanExchangeTheNumberOfTimes);
                    this.AddToUpdating("EachPersonCanExchangeTheTimesOfDays", this.EachPersonCanExchangeTheTimesOfDays);
                    this.AddToUpdating("EachPersonCanExchangeTheNumberOfDays", this.EachPersonCanExchangeTheNumberOfDays);
                    this.AddToUpdating("EachPersonCanExchangeTheTimesOfAll", this.EachPersonCanExchangeTheTimesOfAll);
                    this.AddToUpdating("EachPersonCanExchangeTheNumberOfAll", this.EachPersonCanExchangeTheNumberOfAll);
                    this.AddToUpdating("Days", this.Days);
                    this.AddToUpdating("EndTime", endTime);
                    this.AddToUpdating("Hide", this.Hide);
                    this.AddToUpdating("AutoDelete", this.AutoDelete);

                    return base.GetEntity(db);
                }

                #endregion
            }

            /// <summary>
            /// 用于更新兑换活动信息的数据集（基础）
            /// </summary>
            private class PackageForUpdate_Basic : PackageForUpdateBase<Exchange>, IPackage<Exchange>, IUpdatePackage<Exchange>
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
                /// 实例化一个新的用于更新兑换活动信息的数据集（基础）
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="days">持续天数</param>
                /// <param name="hide">暂停显示</param>
                /// <param name="autoDelete">过期自动删除</param>
                public PackageForUpdate_Basic(int id, int days, bool hide, bool autoDelete)
                    : base(id)
                {
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
                    if (this.Days < 1)
                    {
                        throw new Exception("持续天数不能小于1");
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override Exchange GetEntity(DbContext db)
                {
                    var t = db.Set<Bulletin>().Where(x => x.Id == this.Id)
                        .Select(x => new { x.Days, x.EndTime }).FirstOrDefault();
                    DateTime endTime = t.EndTime.AddDays(this.Days - t.Days);

                    this.AddToUpdating("Days", this.Days);
                    this.AddToUpdating("EndTime", endTime);
                    this.AddToUpdating("Hide", this.Hide);
                    this.AddToUpdating("AutoDelete", this.AutoDelete);

                    return base.GetEntity(db);
                }

                #endregion
            }

            /// <summary>
            /// 用于更新兑换活动信息的数据集（暂停/重新开始）
            /// </summary>
            private class PackageForUpdate_ShowOrHide : PackageForUpdateBase<Exchange>, IPackage<Exchange>, IUpdatePackage<Exchange>
            {
                #region 公开属性

                /// <summary>
                /// 暂停显示
                /// </summary>
                public bool Hide { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于更新兑换活动信息的数据集（暂停/重新开始）
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
                public override Exchange GetEntity(DbContext db)
                {
                    this.AddToUpdating("Hide", this.Hide);

                    return base.GetEntity(db);
                }

                #endregion
            }

            /// <summary>
            /// 用于新建奖品的数据集
            /// </summary>
            private class PackageForPrize : IPackageForPrize
            {
                #region 公开属性

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 描述
                /// </summary>
                public string Description { get; set; }

                /// <summary>
                /// 数额
                /// </summary>
                public int Sum { get; set; }

                /// <summary>
                /// 类型
                /// </summary>
                public PrizeType Type { get; set; }

                /// <summary>
                /// 价值
                /// </summary>
                public double Price { get; set; }

                /// <summary>
                /// 备注（一般为实物奖品的演示链接）
                /// </summary>
                public string Remark { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个用于用于新建奖品的数据集
                /// </summary>
                /// <param name="name">名称</param>
                /// <param name="description">描述</param>
                /// <param name="sum">数额</param>
                /// <param name="type">类型</param>
                /// <param name="price">价值</param>
                /// <param name="remark">备注</param>
                public PackageForPrize(string name, string description, int sum, PrizeType type, double price, string remark)
                {
                    this.Name = name;
                    this.Description = description;
                    this.Sum = sum;
                    this.Type = type;
                    this.Price = price;
                    this.Remark = remark;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查用于新建奖品的输入是否合法
                /// </summary>
                public void CheckPrize()
                {
                    if (this.Sum < 1)
                    {
                        throw new Exception("数额不能小于1");
                    }
                    if (this.Price < 0)
                    {
                        throw new Exception("价值不能小于0");
                    }
                }

                /// <summary>
                /// 获取奖品
                /// </summary>
                /// <returns>返回奖品的封装</returns>
                public Prize GetPrize()
                {
                    return new Prize(this.Name, this.Description, this.Sum, this.Type, this.Price, this.Remark);
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
                /// 检查用于新建限制条件的输入是否合法
                /// </summary>
                public void CheckCondition()
                {
                    if (this.Limit > this.Upper)
                    {
                        throw new Exception("下限不能大于上限");
                    }
                }

                /// <summary>
                /// 获取限制条件（兑换活动）
                /// </summary>
                /// <returns>返回限制条件（兑换活动）的封装</returns>
                public ExchangeCondition GetCondition()
                {
                    return new ExchangeCondition(this.Type, this.Limit, this.Upper);
                }

                #endregion
            }

            #endregion

            #region 内嵌接口

            /// <summary>
            /// 定义用于新建奖品的数据集
            /// </summary>
            public interface IPackageForPrize
            {
                /// <summary>
                /// 检查用于新建奖品的输入是否合法
                /// </summary>
                void CheckPrize();

                /// <summary>
                /// 获取奖品
                /// </summary>
                /// <returns>返回奖品的封装</returns>
                Prize GetPrize();
            }

            /// <summary>
            /// 定义用于新建限制条件的数据集
            /// </summary>
            public interface IPackageForCondition
            {
                /// <summary>
                /// 检查用于新建限制条件的输入是否合法
                /// </summary>
                void CheckCondition();

                /// <summary>
                /// 获取限制条件（兑换活动）
                /// </summary>
                /// <returns>返回限制条件（兑换活动）的封装</returns>
                ExchangeCondition GetCondition();
            }

            #endregion
        }

        #endregion
    }
}

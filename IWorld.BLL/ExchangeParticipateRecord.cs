using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 兑换记录的参与记录的管理者对象
    /// </summary>
    public class ExchangeParticipateRecordManager : SimplifyManagerBase<ExchangeParticipateRecord>
        , IManager<ExchangeParticipateRecord>, ISimplify<ExchangeParticipateRecord>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的兑换记录的参与记录的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public ExchangeParticipateRecordManager(DbContext db)
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
            /// 创建一个用于新建兑换活动的参与记录的数据集
            /// </summary>
            /// <param name="ownerId">参与人的存储指针</param>
            /// <param name="exchangeId">参与的活动的存储指针</param>
            /// <param name="sum">数量</param>
            /// <returns>返回用于新建兑换活动的参与记录的数据集</returns>
            public static ICreatePackage<ExchangeParticipateRecord> CreatePackageForCreate(int ownerId, int exchangeId, int sum)
            {
                return new PackageForCreate(ownerId, exchangeId, sum);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建兑换活动的参与记录的数据集
            /// </summary>
            private class PackageForCreate : IPackage<ExchangeParticipateRecord>, ICreatePackage<ExchangeParticipateRecord>
            {
                #region 公开属性

                /// <summary>
                /// 参与人的存储指针
                /// </summary>
                public int OwnerId { get; set; }

                /// <summary>
                /// 参与的活动的存储指针
                /// </summary>
                public int ExchangeId { get; set; }

                /// <summary>
                /// 数量
                /// </summary>
                public int Sum { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建兑换活动的参与记录的数据集
                /// </summary>
                /// <param name="ownerId">参与人的存储指针</param>
                /// <param name="exchangeId">参与的活动的存储指针</param>
                /// <param name="sum">数量</param>
                public PackageForCreate(int ownerId, int exchangeId, int sum)
                {
                    this.OwnerId = ownerId;
                    this.ExchangeId = exchangeId;
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
                    NChecker.CheckEntity<Author>(this.OwnerId, "用户", db);
                    NChecker.CheckEntity<Author>(this.ExchangeId, "活动", db);
                    if (this.Sum < 1)
                    {
                        throw new Exception("兑换数量不能小于1");
                    }
                    var exchange = db.Set<Exchange>().Where(x => x.Id == this.ExchangeId)
                        .Select(x =>
                            new
                            {
                                x.EachPersonCanExchangeTheNumberOfTimes,
                                x.EachPersonCanExchangeTheTimesOfDays,
                                x.EachPersonCanExchangeTheNumberOfDays,
                                x.EachPersonCanExchangeTheTimesOfAll,
                                x.EachPersonCanExchangeTheNumberOfAll
                            })
                        .FirstOrDefault();
                    if (exchange.EachPersonCanExchangeTheNumberOfTimes > 0)
                    {
                        if (this.Sum > exchange.EachPersonCanExchangeTheNumberOfTimes)
                        {
                            throw new Exception(string.Format("兑换数量不能大于系统限定：{0}"
                                , exchange.EachPersonCanExchangeTheNumberOfTimes));
                        }
                    }
                    var eprSet = db.Set<ExchangeParticipateRecord>();
                    if (exchange.EachPersonCanExchangeTheTimesOfDays > 0)
                    {
                        int countOfTimesInToday = eprSet.Count(x => x.Owner.Id == this.OwnerId
                            && x.Exchange.Id == this.ExchangeId
                            && x.CreatedTime.Year == DateTime.Now.Year
                            && x.CreatedTime.Month == DateTime.Now.Month
                            && x.CreatedTime.Day == DateTime.Now.Day);
                        if (countOfTimesInToday >= exchange.EachPersonCanExchangeTheTimesOfDays)
                        {
                            throw new Exception(string.Format("每日兑换次数不能大于系统限定：{0} 已兑换：{1}"
                                , exchange.EachPersonCanExchangeTheNumberOfTimes
                                , countOfTimesInToday));
                        }
                    }
                    if (exchange.EachPersonCanExchangeTheNumberOfDays > 0)
                    {
                        int countOfSumInToday = eprSet.Where(x => x.Owner.Id == this.OwnerId
                            && x.Exchange.Id == this.ExchangeId
                            && x.CreatedTime.Year == DateTime.Now.Year
                            && x.CreatedTime.Month == DateTime.Now.Month
                            && x.CreatedTime.Day == DateTime.Now.Day)
                            .Sum(x => x.Sum);
                        if (countOfSumInToday + this.Sum > exchange.EachPersonCanExchangeTheNumberOfDays)
                        {
                            throw new Exception(string.Format("每日兑换奖品数量不能大于系统限定：{0} 已兑换：{1} 要求兑换：{2}"
                                , exchange.EachPersonCanExchangeTheNumberOfTimes
                                , countOfSumInToday
                                , this.Sum));
                        }
                    }
                    if (exchange.EachPersonCanExchangeTheTimesOfAll > 0)
                    {
                        int countOfTimesAtAll = eprSet.Count(x => x.Owner.Id == this.OwnerId
                            && x.Exchange.Id == this.ExchangeId);
                        if (countOfTimesAtAll >= exchange.EachPersonCanExchangeTheTimesOfDays)
                        {
                            throw new Exception(string.Format("总兑换次数不能大于系统限定：{0} 已兑换：{1}"
                                , exchange.EachPersonCanExchangeTheNumberOfTimes
                                , countOfTimesAtAll));
                        }
                    }
                    if (exchange.EachPersonCanExchangeTheNumberOfAll > 0)
                    {
                        int countOfSumAtAll = eprSet.Where(x => x.Owner.Id == this.OwnerId
                            && x.Exchange.Id == this.ExchangeId)
                            .Sum(x => x.Sum);
                        if (countOfSumAtAll + this.Sum > exchange.EachPersonCanExchangeTheNumberOfDays)
                        {
                            throw new Exception(string.Format("总兑换奖品数量不能大于系统限定：{0} 已兑换：{1} 要求兑换：{2}"
                                , exchange.EachPersonCanExchangeTheNumberOfTimes
                                , countOfSumAtAll
                                , this.Sum));
                        }
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public ExchangeParticipateRecord GetEntity(DbContext db)
                {
                    Author owner = db.Set<Author>().Find(this.OwnerId);
                    Exchange exchange = db.Set<Exchange>().Find(this.ExchangeId);
                    exchange.Conditions.ForEach(x =>
                        {
                            x.Audit(owner);
                        });
                    List<GiftRecord> gifts = new List<GiftRecord>();
                    exchange.Prizes.Where(x => x.Type == PrizeType.实物).ToList().ForEach(x =>
                        {
                            GiftRecord gift = new GiftRecord(exchange, owner, x.Name, x.Description, x.Sum, x.Type, x.Price, x.Remark);
                            gifts.Add(gift);
                        });

                    return new ExchangeParticipateRecord(owner, exchange, this.Sum, gifts);
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}

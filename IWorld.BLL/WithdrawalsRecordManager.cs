using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Setting;
using IWorld.Helper;

namespace IWorld.BLL
{
    /// <summary>
    /// 提现记录的管理者对象
    /// </summary>
    public class WithdrawalsRecordManager : SimplifyManagerBase<WithdrawalsRecord>, IManager<WithdrawalsRecord>
        , ISimplify<WithdrawalsRecord>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的提现记录的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public WithdrawalsRecordManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 将指定的实例从数据库中移除
        /// </summary>
        /// <param name="id">目标对象的存储指针</param>
        public override void Remove(int id)
        {
            PackageForRemove packageForRemove = new PackageForRemove(id);
            base.Remove(packageForRemove);
        }

        /// <summary>
        /// 改变提现记录的当前状态
        /// </summary>
        /// <param name="recordId"目标记录的存储指针></param>
        /// <param name="newStatus">新状态</param>
        /// <param name="remark">备注</param>
        public void ChangeStatus(int recordId, WithdrawalsStatus newStatus, string remark = "")
        {
            if (newStatus == WithdrawalsStatus.处理中)
            {
                throw new Exception("不允许将提现记录的状态改为“处理中”");
            }
            NChecker.CheckEntity<WithdrawalsRecord>(recordId, "提现记录", db);
            WithdrawalsRecord record = db.Set<WithdrawalsRecord>().Find(recordId);
            if (record.Status != WithdrawalsStatus.处理中)
            {
                throw new Exception("这条记录已经被处理过");
            }

            ChangeStatusEventArgs eventArgs = new ChangeStatusEventArgs(db, record, record.Status, newStatus, remark);
            if (ChangingStatusEventHandler != null)
            {
                ChangingStatusEventHandler(this, eventArgs);    //触发前置事件
            }
            record.Status = newStatus;
            record.Remark = remark;
            db.SaveChanges();
            if (ChangedStatusEventHandler != null)
            {
                ChangedStatusEventHandler(this, eventArgs);    //触发后置事件
            }
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
            /// 创建一个用于新建提现记录的数据集
            /// </summary>
            /// <param name="ownerId">申请人的存储指针</param>
            /// <param name="sum">金额</param>
            /// <param name="card">目标银行卡的卡号</param>
            /// <param name="name">目标银行卡的开户人姓名</param>
            /// <param name="bank">目标银行卡的开户银行</param>
            /// <returns>返回用于新建提现记录的数据集</returns>
            public static ICreatePackage<WithdrawalsRecord> CreatePackageForCreate(int ownerId, double sum, string card, string name
                , string bank)
            {
                Bank _bank = EnumHelper.Parse<Bank>(bank);

                return new PackageForCreate(ownerId, sum, card, name, _bank);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建提现记录的数据集
            /// </summary>
            private class PackageForCreate : IPackage<WithdrawalsRecord>, ICreatePackage<WithdrawalsRecord>
            {
                #region 公开属性

                /// <summary>
                /// 申请人的存储指针
                /// </summary>
                public int OwnerId { get; set; }

                /// <summary>
                /// 金额
                /// </summary>
                public double Sum { get; set; }

                /// <summary>
                /// 目标银行卡的卡号
                /// </summary>
                public string Card { get; set; }

                /// <summary>
                /// 目标银行卡的开户人姓名
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 目标银行卡的开户银行
                /// </summary>
                public Bank Bank { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个用于新建提现记录的数据集
                /// </summary>
                /// <param name="ownerId">申请人的存储指针</param>
                /// <param name="sum">金额</param>
                /// <param name="card">目标银行卡的卡号</param>
                /// <param name="name">目标银行卡的开户人姓名</param>
                /// <param name="bank">目标银行卡的开户银行</param>
                public PackageForCreate(int ownerId, double sum, string card, string name, Bank bank)
                {
                    this.OwnerId = ownerId;
                    this.Sum = sum;
                    this.Card = card;
                    this.Name = name;
                    this.Bank = bank;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    WebSetting webSetting = new WebSetting();
                    NChecker.CheckEntity<Author>(this.OwnerId, "用户", db);
                    var t = (from c in db.Set<Author>()
                             where c.Id == this.OwnerId
                             select new
                             {
                                 c.Group.Withdrawals,
                                 c.Group.MinimumWithdrawalAmount,
                                 c.Group.MaximumWithdrawalAmount,
                                 c.Money
                             })
                             .FirstOrDefault();
                    int times = db.Set<WithdrawalsRecord>().Count(x => x.Owner.Id == this.OwnerId
                        && x.CreatedTime.Year == DateTime.Now.Year
                        && x.CreatedTime.Month == DateTime.Now.Month
                        && x.CreatedTime.Day == DateTime.Now.Day);
                    int withdrawals = t.Withdrawals != 0 ? t.Withdrawals : webSetting.Withdrawals;
                    if (times >= withdrawals)
                    {
                        throw new Exception(string.Format("当日提款的次数已经达到系统规定的上限（{0}次）", withdrawals));
                    }
                    double minimumWithdrawalAmount = t.MinimumWithdrawalAmount != 0 ? t.MinimumWithdrawalAmount
                        : webSetting.MinimumWithdrawalAmount;
                    if (this.Sum < minimumWithdrawalAmount)
                    {
                        throw new Exception(string.Format("提款金额不得小于系统规定的下限（{0}元）", minimumWithdrawalAmount));
                    }
                    double maximumWithdrawalAmount = t.MaximumWithdrawalAmount != 0 ? t.MaximumWithdrawalAmount
                        : webSetting.MinimumBonusMode;
                    if (this.Sum > maximumWithdrawalAmount)
                    {
                        throw new Exception(string.Format("提款金额不得大于系统规定的上限（{0}元）", maximumWithdrawalAmount));
                    }
                    if (this.Sum > t.Money)
                    {
                        throw new Exception("提款金额不得大于用户的资金余额");
                    }
                    List<string> banks = webSetting.Banks.Split(new char[] { ',' }).ToList();
                    if (!banks.Contains(this.Bank.ToString()))
                    {
                        throw new Exception(string.Format("指定的银行不在系统支持的列表中（{0}）", webSetting.Banks));
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public WithdrawalsRecord GetEntity(DbContext db)
                {
                    Author owner = db.Set<Author>().Find(this.OwnerId);

                    return new WithdrawalsRecord(owner, this.Sum, this.Card, this.Name, this.Bank);
                }

                #endregion
            }

            #endregion
        }

        /// <summary>
        /// 用于删除提现记录的数据集
        /// </summary>
        protected class PackageForRemove : IPackage<WithdrawalsRecord>, IRemovePackage<WithdrawalsRecord>
        {
            #region 公开属性

            /// <summary>
            /// 存储指针
            /// </summary>
            public int Id { get; set; }

            #endregion

            #region 构造方法

            /// <summary>
            /// 实例化一个新的用于删除提现记录的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            public PackageForRemove(int id)
            {
                this.Id = id;
            }

            #endregion

            #region 实例方法

            /// <summary>
            /// 检查数据集的内容是否符合定义
            /// </summary>
            /// <param name="db">数据库连接对象</param>
            public void CheckData(DbContext db)
            {
                WithdrawalsStatus status = (from c in db.Set<WithdrawalsRecord>()
                                            where c.Id == this.Id
                                            select c.Status)
                                            .FirstOrDefault();
                if (status != WithdrawalsStatus.处理中)
                {
                    throw new Exception("已经被处理的提现记录无法进行撤单操作");
                }
            }

            /// <summary>
            /// 获取实体对象
            /// </summary>
            /// <param name="db">数据库连接对象</param>
            /// <returns>返回泛型状态所规定的实体类</returns>
            public WithdrawalsRecord GetEntity(DbContext db)
            {
                return db.Set<WithdrawalsRecord>().Find(this.Id);
            }

            #endregion
        }

        #region 监视对象

        /// <summary>
        /// 用于监视改变提现记录的当前状态动作的对象
        /// </summary>
        public class ChangeStatusEventArgs : NEventArgs
        {
            #region 公开属性

            /// <summary>
            /// 原状态
            /// </summary>
            public WithdrawalsStatus OldStatus { get; set; }

            /// <summary>
            /// 新状态
            /// </summary>
            public WithdrawalsStatus NewStatus { get; set; }

            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }

            #endregion

            #region 构造方法

            /// <summary>
            /// 实例化一个新的用于监视改变提现记录的当前状态动作的对象
            /// </summary>
            /// <param name="db">数据库连接对象</param>
            /// <param name="state">参数实体</param>
            /// <param name="oldStatus">原状态</param>
            /// <param name="newStatus">新状态</param>
            /// <param name="remark">备注</param>
            public ChangeStatusEventArgs(DbContext db, object state, WithdrawalsStatus oldStatus, WithdrawalsStatus newStatus
                , string remark)
                : base(db, state)
            {
                this.OldStatus = oldStatus;
                this.NewStatus = newStatus;
                this.Remark = remark;
            }

            #endregion
        }

        #endregion

        #region 内置委托

        /// <summary>
        /// 改变提现记录的当前状态动作的实例委托
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public delegate void ChangeStatusDelegate(object sender, ChangeStatusEventArgs e);

        #endregion

        #endregion

        #region 有关事件

        /// <summary>
        /// 改变提现记录的当前状态前将触发的事件
        /// </summary>
        public static event ChangeStatusDelegate ChangingStatusEventHandler;

        /// <summary>
        /// 改变提现记录的当前状态后将触发的事件
        /// </summary>
        public static event ChangeStatusDelegate ChangedStatusEventHandler;

        #endregion
    }
}

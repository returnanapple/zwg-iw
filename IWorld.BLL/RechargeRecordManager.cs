using System;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Setting;

namespace IWorld.BLL
{
    /// <summary>
    /// 充值记录的管理者对象
    /// </summary>
    public class RechargeRecordManager : SimplifyManagerBase<RechargeRecord>, IManager<RechargeRecord>, ISimplify<RechargeRecord>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的充值记录的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public RechargeRecordManager(DbContext db)
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
            PackageForRemove pfr = new PackageForRemove(id);
            base.Remove(pfr);
        }

        /// <summary>
        /// 改变充值记录的当前状态
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="newStatus"></param>
        /// <param name="remark"></param>
        /// <param name="card"></param>
        /// <param name="name"></param>
        /// <param name="bank"></param>
        public void ChangeStatus(int recordId, RechargeStatus newStatus, string remark = "", string card = "", string name = ""
            , Bank bank = Bank.无)
        {
            if (newStatus == RechargeStatus.等待支付)
            {
                throw new Exception("不允许将充值记录的状态改为“等待支付”");
            }
            NChecker.CheckEntity<RechargeRecord>(recordId, "提现记录", db);
            RechargeRecord record = db.Set<RechargeRecord>().Find(recordId);
            if (record.Status == RechargeStatus.充值成功 || record.Status == RechargeStatus.失败)
            {
                throw new Exception("这条记录已经被处理过");
            }

            ChangeStatusEventArgs eventArgs
                = new ChangeStatusEventArgs(db, record, record.Status, newStatus, remark, card, name, bank);
            if (ChangingStatusEventHandler != null)
            {
                ChangingStatusEventHandler(this, eventArgs);    //触发前置事件
            }
            record.Status = newStatus;
            record.Remark = remark;
            record.Card = card;
            record.Name = name;
            record.Bank = bank;
            db.SaveChanges();
            if (ChangedStatusEventHandler != null)
            {
                ChangedStatusEventHandler(this, eventArgs);    //触发前置事件
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
            /// 创建一个用于新建充值记录的数据集
            /// </summary>
            /// <param name="ownerId">目标账户的存储指针</param>
            /// <param name="payerId">支付人的存储指针</param>
            /// <param name="sum">金额</param>
            /// <returns>返回用于新建充值记录的数据集</returns>
            public static ICreatePackage<RechargeRecord> CreatePackageForCreate(int ownerId, int payerId, double sum)
            {
                return new PackageForCreate(ownerId, payerId, sum);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建充值记录的数据集
            /// </summary>
            private class PackageForCreate : IPackage<RechargeRecord>, ICreatePackage<RechargeRecord>
            {
                #region 公开属性

                /// <summary>
                /// 目标账户的存储指针
                /// </summary>
                public int OwnerId { get; set; }

                /// <summary>
                /// 支付人的存储指针
                /// </summary>
                public int PayerId { get; set; }

                /// <summary>
                /// 金额
                /// </summary>
                public double Sum { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建充值记录的数据集
                /// </summary>
                /// <param name="ownerId">目标账户的存储指针</param>
                /// <param name="payerId">支付人的存储指针</param>
                /// <param name="sum">金额</param>
                public PackageForCreate(int ownerId, int payerId, double sum)
                {
                    this.OwnerId = ownerId;
                    this.PayerId = payerId;
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
                    WebSetting webSetting = new WebSetting();
                    NChecker.CheckEntity<Author>(this.OwnerId, "用户", db);
                    NChecker.CheckEntity<Author>(this.PayerId, "用户", db);
                    var t = (from c in db.Set<Author>()
                             where c.Id == this.OwnerId
                             select new
                             {
                                 c.Group.MinimumRechargeAmount,
                                 c.Group.MaximumRechargeAmount
                             })
                             .FirstOrDefault();
                    double minimumRechargeAmount = t.MinimumRechargeAmount != 0 ? t.MinimumRechargeAmount
                        : webSetting.MinimumRechargeAmount;
                    if (this.Sum < minimumRechargeAmount)
                    {
                        throw new Exception(string.Format("充值金额不得小于系统规定的下限（{0}元）", minimumRechargeAmount));
                    }
                    double maximumRechargeAmount = t.MaximumRechargeAmount != 0 ? t.MaximumRechargeAmount
                        : webSetting.MaximumRechargeAmount;
                    if (this.Sum > maximumRechargeAmount)
                    {
                        throw new Exception(string.Format("充值金额不得大于系统规定的上限（{0}元）", maximumRechargeAmount));
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public RechargeRecord GetEntity(DbContext db)
                {
                    Author owner = db.Set<Author>().Find(this.OwnerId);
                    Author payer = db.Set<Author>().Find(this.PayerId);

                    return new RechargeRecord(owner, payer, this.Sum);
                }

                #endregion
            }

            #endregion
        }

        /// <summary>
        /// 用于删除提现记录的数据集
        /// </summary>
        protected class PackageForRemove : IPackage<RechargeRecord>, IRemovePackage<RechargeRecord>
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
                RechargeStatus status = (from c in db.Set<RechargeRecord>()
                                         where c.Id == this.Id
                                         select c.Status)
                                         .FirstOrDefault();
                if (status != RechargeStatus.等待支付)
                {
                    throw new Exception("已经被处理的充值记录无法进行取消操作");
                }
            }

            /// <summary>
            /// 获取实体对象
            /// </summary>
            /// <param name="db">数据库连接对象</param>
            /// <returns>返回泛型状态所规定的实体类</returns>
            public RechargeRecord GetEntity(DbContext db)
            {
                return db.Set<RechargeRecord>().Find(this.Id);
            }

            #endregion
        }

        #region 监视对象

        /// <summary>
        /// 用于监视改变充值记录的当前状态动作的对象
        /// </summary>
        public class ChangeStatusEventArgs : NEventArgs
        {
            #region 公开属性

            /// <summary>
            /// 原状态
            /// </summary>
            public RechargeStatus OldStatus { get; set; }

            /// <summary>
            /// 新状态
            /// </summary>
            public RechargeStatus NewStatus { get; set; }

            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }

            /// <summary>
            /// 来源卡号
            /// </summary>
            public string Card { get; set; }

            /// <summary>
            /// 来源卡的开户人
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 来源卡的开户银行
            /// </summary>
            public Bank Bank { get; set; }

            #endregion

            #region 构造方法

            /// <summary>
            /// 实例化一个新的用于监视改变充值记录的当前状态动作的对象
            /// </summary>
            /// <param name="db">数据库连接对象</param>
            /// <param name="state">参数实体</param>
            /// <param name="oldStatus">原状态</param>
            /// <param name="newStatus">新状态</param>
            /// <param name="remark">备注</param>
            /// <param name="card">来源卡号</param>
            /// <param name="name">来源卡的开户人</param>
            /// <param name="bank">来源卡的开户银行</param>
            public ChangeStatusEventArgs(DbContext db, object state, RechargeStatus oldStatus, RechargeStatus newStatus
                , string remark, string card, string name, Bank bank)
                : base(db, state)
            {
                this.OldStatus = oldStatus;
                this.NewStatus = newStatus;
                this.Remark = remark;
                this.Card = card;
                this.Name = name;
                this.Bank = bank;
            }

            #endregion
        }

        #endregion

        #region 内置委托

        /// <summary>
        /// 改变充值记录的当前状态动作的实例委托
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public delegate void ChangeStatusDelegate(object sender, ChangeStatusEventArgs e);

        #endregion

        #endregion

        #region 有关事件

        /// <summary>
        /// 改变充值记录的当前状态前将触发的事件
        /// </summary>
        public static event ChangeStatusDelegate ChangingStatusEventHandler;

        /// <summary>
        /// 改变充值记录的当前状态后将触发的事件
        /// </summary>
        public static event ChangeStatusDelegate ChangedStatusEventHandler;

        #endregion
    }
}

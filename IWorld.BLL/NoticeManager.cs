using System;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 通知的管理者对象
    /// </summary>
    public class NoticeManager : SimplifyManagerBase<Notice>, IManager<Notice>, ISimplify<Notice>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的通知的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public NoticeManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 声明目标通知已经被目标用户所阅读
        /// </summary>
        /// <param name="noticeId">目标通知的存储指针</param>
        /// <param name="userId">目标用户的存储指针</param>
        public void Read(int noticeId, int userId)
        {
            NChecker.CheckEntity<Notice>(noticeId, "通知", db);
            NChecker.CheckEntity<Author>(userId, "用户", db);
            string token = string.Format("[{0}]", userId);
            Notice notice = db.Set<Notice>().Find(noticeId);
            if (notice.To.Id != userId)
            {
                throw new Exception("这个通知不是发给这个用户的");
            }
            if (notice.IsReaded)
            {
                throw new Exception("该用户已经阅读过这个通知");
            }

            notice.IsReaded = true;
            notice.ModifiedTime = DateTime.Now;
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
            /// 创建一个用于新建通知的数据集
            /// </summary>
            /// <param name="toId">接收人的存储指针</param>
            /// <param name="context">正文</param>
            /// <param name="type">通知类型</param>
            /// <param name="targetId">目标对象的存储指针</param>
            /// <returns>返回用于新建通知的数据集</returns>
            public static ICreatePackage<Notice> CreatePackageForCreate(int toId, string context, NoticeType type, int targetId)
            {
                return new PackageForCreate(toId, context, type, targetId);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建通知的数据集
            /// </summary>
            private class PackageForCreate : IPackage<Notice>, ICreatePackage<Notice>
            {
                #region 公开属性

                /// <summary>
                /// 接收人的存储指针
                /// </summary>
                public int ToId { get; set; }

                /// <summary>
                /// 正文
                /// </summary>
                public string Context { get; set; }

                /// <summary>
                /// 通知类型
                /// </summary>
                public NoticeType Type { get; set; }

                /// <summary>
                /// 目标对象的存储指针
                /// </summary>
                public int TargetId { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建通知的数据集
                /// </summary>
                /// <param name="toId">接收人的存储指针</param>
                /// <param name="context">正文</param>
                /// <param name="type">通知类型</param>
                /// <param name="targetId">目标对象的存储指针</param>
                public PackageForCreate(int toId, string context, NoticeType type, int targetId)
                {
                    this.ToId = toId;
                    this.Context = context;
                    this.Type = type;
                    this.TargetId = targetId;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    NChecker.CheckEntity<Author>(this.ToId, "用户", db);
                    switch (this.Type)
                    {
                        case NoticeType.充值反馈:
                            NChecker.CheckEntity<RechargeRecord>(this.TargetId, "充值记录", db);
                            break;
                        case NoticeType.开奖提醒:
                            NChecker.CheckEntity<RechargeRecord>(this.TargetId, "投注记录", db);
                            break;
                        case NoticeType.提现反馈:
                            NChecker.CheckEntity<RechargeRecord>(this.TargetId, "提现记录", db);
                            break;
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public Notice GetEntity(DbContext db)
                {
                    Author _to = db.Set<Author>().Find(this.ToId);

                    return new Notice(_to, this.Context, this.Type, this.TargetId);
                }

                #endregion
            }

            #endregion
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 反馈开奖结果（投注）
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void CreateNoticeWhenLottery(object sender, BettingManager.ChangeStatusEventArgs e)
        {
            Betting betting = (Betting)e.State;
            NoticeManager noticeManager = new NoticeManager(e.Db);
            if (e.NewStatus == BettingStatus.中奖)
            {
                string context = string.Format("{0} 第 {1} 期已经开奖。恭喜，您的投注：{2}-{3}（{4}注）中奖 {5} 元"
                    , betting.HowToPlay.Tag.Ticket.Name
                    , betting.Phases
                    , betting.HowToPlay.Tag.Name
                    , betting.HowToPlay.Name
                    , betting.Sum
                    , e.Bonus);
                ICreatePackage<Notice> pfc = NoticeManager.Factory.CreatePackageForCreate(betting.Owner.Id, context
                    , NoticeType.开奖提醒, betting.Id);
                noticeManager.Create(pfc);
            }
            else if (e.NewStatus == BettingStatus.未中奖)
            {
                string context = string.Format("{0} 第 {1} 期已经开奖。很遗憾，您的投注：{2}-{3}（{4}注）未能中奖"
                    , betting.HowToPlay.Tag.Ticket.Name
                    , betting.Phases
                    , betting.HowToPlay.Tag.Name
                    , betting.HowToPlay.Name
                    , betting.Sum);
                ICreatePackage<Notice> pfc = NoticeManager.Factory.CreatePackageForCreate(betting.Owner.Id, context
                    , NoticeType.开奖提醒, betting.Id);
                noticeManager.Create(pfc);
            }
        }

        /// <summary>
        /// 反馈充值结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CreateNoticeWhenRechargeHadResult(object sender, RechargeRecordManager.ChangeStatusEventArgs e)
        {
            RechargeRecord rr = (RechargeRecord)e.State;
            NoticeManager noticeManager = new NoticeManager(e.Db);
            if (e.NewStatus == RechargeStatus.充值成功)
            {
                string context = string.Format("您的订单号为 {0} 的 金额为 {1} 的充值操作已经成功到账"
                    , rr.Id
                    , rr.Sum);
                ICreatePackage<Notice> pfc = NoticeManager.Factory.CreatePackageForCreate(rr.Owner.Id, context
                    , NoticeType.充值反馈, rr.Id);
                noticeManager.Create(pfc);
            }
            else if (e.NewStatus == RechargeStatus.失败)
            {
                string context = string.Format("您的订单号为 {0} 的 金额为 {1} 的充值操作失败了"
                    , rr.Id
                    , rr.Sum);
                ICreatePackage<Notice> pfc = NoticeManager.Factory.CreatePackageForCreate(rr.Owner.Id, context
                    , NoticeType.充值反馈, rr.Id);
                noticeManager.Create(pfc);
            }
        }

        /// <summary>
        /// 反馈提现结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CreateNoticeWhenWithdrawalsHadResult(object sender, WithdrawalsRecordManager.ChangeStatusEventArgs e)
        {
            WithdrawalsRecord wr = (WithdrawalsRecord)e.State;
            NoticeManager noticeManager = new NoticeManager(e.Db);
            if (e.NewStatus == WithdrawalsStatus.提现成功)
            {
                string context = string.Format("您的订单号为 {0} 的 金额为 {1} 的提现已经转账成功"
                    , wr.Id
                    , wr.Sum);
                ICreatePackage<Notice> pfc = NoticeManager.Factory.CreatePackageForCreate(wr.Owner.Id, context
                    , NoticeType.提现反馈, wr.Id);
                noticeManager.Create(pfc);
            }
            else if (e.NewStatus == WithdrawalsStatus.失败)
            {
                string context = string.Format("您的订单号为 {0} 的 金额为 {1} 的提现失败了"
                    , wr.Id
                    , wr.Sum);
                ICreatePackage<Notice> pfc = NoticeManager.Factory.CreatePackageForCreate(wr.Owner.Id, context
                    , NoticeType.提现反馈, wr.Id);
                noticeManager.Create(pfc);
            }
        }

        /// <summary>
        /// 反馈开奖结果（追号）
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void CreateNoticeWhenLottery(object sender, BettingForCgasingManager.ChangeStatusEventArgs e)
        {
            //BettingForCgasing betting = (BettingForCgasing)e.State;
            //NoticeManager noticeManager = new NoticeManager(e.Db);
            //if (e.NewStatus == BettingStatus.中奖)
            //{
            //    string context = string.Format("{0} 第 {1} 期已经开奖。恭喜，您的投注：{2}-{3}（{4}注）中奖 {5} 元"
            //        , betting.Chasing.HowToPlay.Tag.Ticket.Name
            //        , betting.Phases
            //        , betting.Chasing.HowToPlay.Tag.Name
            //        , betting.Chasing.HowToPlay.Name
            //        , betting.Chasing.Sum
            //        , e.Bonus);
            //    ICreatePackage<Notice> pfc = NoticeManager.Factory.CreatePackageForCreate(betting.Chasing.Owner.Id, context);
            //    noticeManager.Create(pfc);
            //}
            //else if (e.NewStatus == BettingStatus.未中奖)
            //{
            //    string context = string.Format("{0} 第 {1} 期已经开奖。很遗憾，您的投注：{2}-{3}（{4}注）未能中奖"
            //        , betting.Chasing.HowToPlay.Tag.Ticket.Name
            //        , betting.Phases
            //        , betting.Chasing.HowToPlay.Tag.Name
            //        , betting.Chasing.HowToPlay.Name
            //        , betting.Chasing.Sum);
            //    ICreatePackage<Notice> pfc = NoticeManager.Factory.CreatePackageForCreate(betting.Chasing.Owner.Id, context);
            //    noticeManager.Create(pfc);
            //}
        }

        #endregion
    }
}

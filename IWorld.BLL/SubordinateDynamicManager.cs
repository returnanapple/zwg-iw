using System;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Helper;

namespace IWorld.BLL
{
    /// <summary>
    /// 下级用户动态的管理者对象
    /// </summary>
    public class SubordinateDynamicManager : SimplifyManagerBase<SubordinateDynamic>, IManager<SubordinateDynamic>
        , ISimplify<SubordinateDynamic>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的下级用户动态的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public SubordinateDynamicManager(DbContext db)
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
            /// 创建一个用于新建下级用户动态的数据集
            /// </summary>
            /// <param name="fromId">“谁”的存储指针</param>
            /// <param name="done">做了什么</param>
            /// <param name="toId">“给谁”的存储指针</param>
            /// <param name="give">给予了多少</param>
            /// <param name="currency">货币单位</param>
            /// <returns>返回用于新建下级用户动态的数据集</returns>
            public static ICreatePackage<SubordinateDynamic> CreatePackageForCreate(int fromId, string done, int toId, double give
                , string currency = "元")
            {
                Currency _currency = EnumHelper.Parse<Currency>(currency);

                return new PackageForCreate(fromId, done, toId, give, _currency);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建下级用户动态的数据集
            /// </summary>
            private class PackageForCreate : IPackage<SubordinateDynamic>, ICreatePackage<SubordinateDynamic>
            {
                #region 公开属性

                /// <summary>
                /// “谁”的存储指针
                /// </summary>
                public int FromId { get; set; }

                /// <summary>
                /// 做了什么
                /// </summary>
                public string Done { get; set; }

                /// <summary>
                /// “给谁”的存储指针
                /// </summary>
                public int ToId { get; set; }

                /// <summary>
                /// 给予了多少
                /// </summary>
                public double Give { get; set; }

                /// <summary>
                /// 货币单位
                /// </summary>
                public Currency Currency { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建下级用户动态的数据集
                /// </summary>
                /// <param name="fromId">“谁”的存储指针</param>
                /// <param name="done">做了什么</param>
                /// <param name="toId">“给谁”的存储指针</param>
                /// <param name="give">给予了多少</param>
                /// <param name="currency">货币单位</param>
                public PackageForCreate(int fromId, string done, int toId, double give, Currency currency)
                {
                    this.FromId = fromId;
                    this.Done = done;
                    this.ToId = toId;
                    this.Give = give;
                    this.Currency = currency;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    NChecker.CheckEntity<Author>(this.FromId, "用户", db);
                    NChecker.CheckEntity<Author>(this.ToId, "用户", db);
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public SubordinateDynamic GetEntity(DbContext db)
                {
                    var aSet = db.Set<Author>();
                    Author _from = aSet.Find(this.FromId);
                    Author _to = aSet.Find(this.ToId);

                    return new SubordinateDynamic(_from, this.Done, _to, this.Give, this.Currency);
                }

                #endregion
            }

            #endregion
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 生成对应的返点记录
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void CreateSubordinateDynamicsForBetting(object sender, BettingManager.ChangeStatusEventArgs e)
        {
            Betting b = (Betting)e.State;
            Author owner = e.Db.Set<Author>().Find(b.Owner.Id);
            if ((e.NewStatus == BettingStatus.未中奖 || e.NewStatus == BettingStatus.中奖)
                && owner.Layer >= 1)
            {
                SubordinateDynamicManager sdm = new SubordinateDynamicManager(e.Db);
                #region 自身

                double _returnPoints = b.HowToPlay.Interface == LotteryInterface.任N不定位
                    ? owner.UncertainReturnPoints : owner.NormalReturnPoints;
                _returnPoints -= b.Points;
                double _amount = Math.Round(b.Pay * _returnPoints / 100, 2);
                string _done = string.Format("在 {0} {1} {2} 中投注了 {3} 元"
                        , b.HowToPlay.Tag.Ticket.Name
                        , b.HowToPlay.Tag.Name
                        , b.HowToPlay.Name
                        , b.Pay);
                ICreatePackage<SubordinateDynamic> _sdPfc = SubordinateDynamicManager.Factory
                        .CreatePackageForCreate(owner.Id, _done, owner.Id, _amount);
                sdm.Create(_sdPfc);


                #endregion
                #region 上级

                AuthorManager am = new AuthorManager(e.Db);
                Author tOwner = owner;
                int t = 0;
                while (tOwner.Layer > 1)
                {
                    Author parent = am.GetParent(tOwner);
                    if (parent == null)
                    {
                        throw new Exception(string.Format("严重错误，用户 {0} 没有对应的上级用户", tOwner.Username));
                    }
                    double returnPoints = b.HowToPlay.Interface == LotteryInterface.任N不定位 ?
                        parent.UncertainReturnPoints - tOwner.UncertainReturnPoints :
                        parent.NormalReturnPoints - tOwner.NormalReturnPoints;
                    double amount = Math.Round(b.Pay * returnPoints / 100, 2);
                    string done = t == 0 ? "" : "的下级用户";
                    done += string.Format("在 {0} {1} {2} 中投注了 {3} 元"
                        , b.HowToPlay.Tag.Ticket.Name
                        , b.HowToPlay.Tag.Name
                        , b.HowToPlay.Name
                        , b.Pay);
                    ICreatePackage<SubordinateDynamic> sdPfc = SubordinateDynamicManager.Factory
                        .CreatePackageForCreate(tOwner.Id, done, parent.Id, amount);
                    sdm.Create(sdPfc);

                    t++;
                    tOwner = parent;
                }

                #endregion
            }
        }

        #endregion
    }
}

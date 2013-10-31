using IWorld.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace IWorld.BLL
{
    /// <summary>
    /// 大白鲨游戏的投注记录的管理对象
    /// </summary>
    public class BettingOfJawManager : SimplifyManagerBase<BettingOfJaw>
    {
        #region 构造方法

        /// <summary>
        /// 大白鲨游戏的投注记录的管理对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public BettingOfJawManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        #endregion

        #region 内嵌类型

        /// <summary>
        /// 内部工厂
        /// </summary>
        public class Factory
        {
            #region 静态方法

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建大白鲨游戏的投注记录的数据集
            /// </summary>
            class PackageForCreate : IPackage<BettingOfJaw>, ICreatePackage<BettingOfJaw>
            {
                #region 公开属性

                /// <summary>
                /// 用户
                /// </summary>
                public int OwnerId { get; set; }

                /// <summary>
                /// 期号
                /// </summary>
                public string Issue { get; set; }

                /// <summary>
                /// 明细
                /// </summary>
                public List<IPackageForCreateDetail> Details { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建大白鲨游戏的投注记录的数据集
                /// </summary>
                /// <param name="ownerId">用户</param>
                /// <param name="issue">期号</param>
                /// <param name="details">明细</param>
                public PackageForCreate(int ownerId, string issue, List<IPackageForCreateDetail> details)
                {
                    this.OwnerId = ownerId;
                    this.Issue = issue;
                    this.Details = details;
                }

                #endregion

                #region 方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    NChecker.CheckEntity<Author>(this.OwnerId, "用户", db);
                    bool hadLotteried=db.Set<Lottery
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public BettingOfJaw GetEntity(DbContext db)
                {
                    throw new NotImplementedException();
                }

                #endregion
            }

            #endregion
        }

        /// <summary>
        /// 定义用于创建投注明细的数据集
        /// </summary>
        public interface IPackageForCreateDetail : IPackage<BettingDetailOfJaw>
        {
            IconOfJaw Icon { get; set; }
        }

        #endregion
    }
}

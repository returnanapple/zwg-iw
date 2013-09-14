using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 支取记录的管理者对象
    /// </summary>
    public class TransferRecordManager : IWorld.BLL.SimplifyManagerBase<TransferRecord>, IWorld.BLL.IManager<TransferRecord>, IWorld.BLL.ISimplify<TransferRecord>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的支取记录的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public TransferRecordManager(DbContext db)
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
            /// 创建一个用于新建支取记录的数据集
            /// </summary>
            /// <param name="ownerId">用户的存储指针</param>
            /// <param name="sum">金额</param>
            /// <param name="remark">支取记录</param>
            /// <returns>返回用于新建支取记录的数据集</returns>
            public static ICreatePackage<TransferRecord> CreatePackageForCreate(int ownerId, double sum, string remark)
            {
                return new PackageForCreate(ownerId, sum, remark);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建支取记录的数据集
            /// </summary>
            private class PackageForCreate : IPackage<TransferRecord>, ICreatePackage<TransferRecord>
            {
                #region 公开属性

                /// <summary>
                /// 用户的存储指针
                /// </summary>
                public int OwnerId { get; set; }

                /// <summary>
                /// 金额
                /// </summary>
                public double Sum { get; set; }

                /// <summary>
                /// 支取记录
                /// </summary>
                public string Remark { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建支取记录的数据集
                /// </summary>
                /// <param name="ownerId">用户的存储指针</param>
                /// <param name="sum">金额</param>
                /// <param name="remark">支取记录</param>
                public PackageForCreate(int ownerId, double sum, string remark)
                {
                    this.OwnerId = ownerId;
                    this.Sum = sum;
                    this.Remark = remark;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    NChecker.CheckEntity<Administrator>(this.OwnerId, "管理员", db);
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public TransferRecord GetEntity(DbContext db)
                {
                    Administrator owner = db.Set<Administrator>().Find(this.OwnerId);

                    return new TransferRecord(owner, this.Sum, this.Remark);
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}

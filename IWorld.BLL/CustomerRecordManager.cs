using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 客户服务记录的管理者对象
    /// </summary>
    public class CustomerRecordManager : SimplifyManagerBase<CustomerRecord>, IManager<CustomerRecord>, ISimplify<CustomerRecord>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的客户服务记录的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public CustomerRecordManager(DbContext db)
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
            /// 创建一个用于新建客服服务记录的数据集
            /// </summary>
            /// <param name="userId">目标用户的存储指针</param>
            /// <param name="type">客服类型</param>
            /// <param name="isService">标识 | 是否系统客服响应</param>
            /// <param name="message">聊天内容</param>
            /// <returns>返回用于新建客服服务记录的数据集</returns>
            public static ICreatePackage<CustomerRecord> CreatePackageForCreate(int userId, CustomerType type, bool isService
                , string message)
            {
                return new PackageForCreate(userId, type, isService, message);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建客服服务记录的数据集
            /// </summary>
            private class PackageForCreate : IPackage<CustomerRecord>, ICreatePackage<CustomerRecord>
            {
                #region 公开属性

                /// <summary>
                /// 目标用户的存储指针
                /// </summary>
                public int UserId { get; set; }

                /// <summary>
                /// 客服类型
                /// </summary>
                public CustomerType Type { get; set; }

                /// <summary>
                /// 标识 | 是否系统客服响应
                /// </summary>
                public bool IsService { get; set; }

                /// <summary>
                /// 聊天内容
                /// </summary>
                public string Message { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建客服服务记录的数据集
                /// </summary>
                /// <param name="userId">目标用户的存储指针</param>
                /// <param name="type">客服类型</param>
                /// <param name="isService">标识 | 是否系统客服响应</param>
                /// <param name="message">聊天内容</param>
                public PackageForCreate(int userId, CustomerType type, bool isService, string message)
                {
                    this.UserId = userId;
                    this.Type = type;
                    this.IsService = IsService;
                    this.Message = message;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    NChecker.CheckEntity<Author>(this.UserId, "用户", db);
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public CustomerRecord GetEntity(DbContext db)
                {
                    Author user = db.Set<Author>().Find(this.UserId);

                    return new CustomerRecord(user, this.Type, this.IsService, this.Message);
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}

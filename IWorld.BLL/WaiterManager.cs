using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Helper;

namespace IWorld.BLL
{
    /// <summary>
    /// 客服账户的管理者对象
    /// </summary>
    public class WaiterManager : IWorld.BLL.SimplifyManagerBase<Waiter>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的客服账户的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public WaiterManager(DbContext db)
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
            /// 创建一个用于新建客服账号的数据集
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="password">密码</param>
            /// <param name="customerType">客服类型</param>
            /// <returns>返回用于新建客服账号的数据集</returns>
            public static ICreatePackage<Waiter> CreatePackageForCreate(string username, string password, CustomerType customerType)
            {
                return new PackageForCreate(username, password, customerType);
            }

            /// <summary>
            /// 创建一个用于修改客服账号信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="oldPassword">原密码</param>
            /// <param name="newPassword">新密码</param>
            /// <returns>返回用于修改客服账号信息的数据集</returns>
            public static IUpdatePackage<Waiter> CreatePackageForUpdate(int id, string oldPassword, string newPassword)
            {
                return new PackageForUpdatePassword(id, oldPassword, newPassword);
            }

            /// <summary>
            /// 创建一个用于修改客服账号信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="customerType">客服类型</param>
            /// <returns>返回用于修改客服账号信息的数据集</returns>
            public static IUpdatePackage<Waiter> CreatePackageForUpdate(int id, CustomerType customerType)
            {
                return new PackageForUpdate(id, customerType);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建客服账号的数据集
            /// </summary>
            private class PackageForCreate : IPackage<Waiter>, ICreatePackage<Waiter>
            {
                #region 公开属性

                /// <summary>
                /// 用户名
                /// </summary>
                public string Username { get; set; }

                /// <summary>
                /// 密码
                /// </summary>
                public string Password { get; set; }

                /// <summary>
                /// 客服类型
                /// </summary>
                public CustomerType CustomerType { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建客服账号的数据集
                /// </summary>
                /// <param name="username">用户名</param>
                /// <param name="password">密码</param>
                /// <param name="customerType">客服类型</param>
                public PackageForCreate(string username, string password, CustomerType customerType)
                {
                    this.Username = username;
                    this.Password = password;
                    this.CustomerType = customerType;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    TextHelper.Check(this.Username, TextHelper.Key.Nickname);
                    TextHelper.Check(this.Password, TextHelper.Key.Password);
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public Waiter GetEntity(DbContext db)
                {
                    return new Waiter(this.Username, this.Password, this.CustomerType);
                }

                #endregion
            }

            /// <summary>
            /// 用于修改客服账号密码的数据集
            /// </summary>
            private class PackageForUpdatePassword : PackageForUpdateBase<Waiter>, IPackage<Waiter>, IUpdatePackage<Waiter>
            {
                #region 公开属性

                /// <summary>
                /// 原密码
                /// </summary>
                public string OldPassword { get; set; }

                /// <summary>
                /// 新密码
                /// </summary>
                public string NewPassword { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于修改客服账号密码的数据集
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="oldPassword">原密码</param>
                /// <param name="newPassword">新密码</param>
                public PackageForUpdatePassword(int id, string oldPassword, string newPassword)
                    : base(id)
                {
                    this.OldPassword = oldPassword;
                    this.NewPassword = newPassword;
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
                    TextHelper.Check(this.OldPassword, TextHelper.Key.Password);
                    TextHelper.Check(this.NewPassword, TextHelper.Key.Password);
                    var tWaiter = db.Set<Waiter>().Where(x => x.Id == this.Id)
                        .Select(x => new { x.Password })
                        .FirstOrDefault();
                    string tOldPassword = EncryptHelper.EncryptByMd5(this.OldPassword);
                    if (tOldPassword != tWaiter.Password)
                    {
                        throw new Exception("原密码不匹配");
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override Waiter GetEntity(DbContext db)
                {
                    string newPassword = EncryptHelper.EncryptByMd5(this.NewPassword);
                    this.AddToUpdating("Password", newPassword);

                    return base.GetEntity(db);
                }

                #endregion
            }

            /// <summary>
            /// 用于修改客服账号信息的数据集
            /// </summary>
            private class PackageForUpdate : PackageForUpdateBase<Waiter>, IPackage<Waiter>, IUpdatePackage<Waiter>
            {
                #region 公开属性

                /// <summary>
                /// 客服类型
                /// </summary>
                public CustomerType CustomerType { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 用于修改客服账号信息的数据集
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="customerType">客服类型</param>
                public PackageForUpdate(int id, CustomerType customerType)
                    : base(id)
                {
                    this.CustomerType = customerType;
                }
                
                #endregion

                #region 实例方法

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override Waiter GetEntity(DbContext db)
                {
                    this.AddToUpdating("CustomerType", this.CustomerType);

                    return base.GetEntity(db);
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}

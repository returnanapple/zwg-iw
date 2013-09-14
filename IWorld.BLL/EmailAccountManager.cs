using System;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 系统邮件账户的管理者对象
    /// </summary>
    public class EmailAccountManager : SimplifyManagerBase<EmailAccount>, IManager<EmailAccount>, ISimplify<EmailAccount>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的系统邮件账户的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public EmailAccountManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 设置默认对象
        /// </summary>
        /// <param name="emailAccountId">要设置为默认对象的对象的存储指针</param>
        public void SetDefault(int emailAccountId)
        {
            var eaSet = db.Set<EmailAccount>();
            bool isDefault = eaSet.Any(x => x.Id == emailAccountId && x.IsDefault);
            if (!isDefault)
            {
                eaSet.Where(x => x.IsDefault).ToList()
                    .ForEach(x => x.IsDefault = false);
                eaSet.Find(emailAccountId).IsDefault = true;

                db.SaveChanges();
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
            /// 创建一个用于新建系统邮件账户的数据集
            /// </summary>
            /// <param name="key">索引字</param>
            /// <param name="account">帐号</param>
            /// <param name="password">密码</param>
            /// <param name="remark">备注</param>
            /// <param name="clientId">应该使用的服务地址的存储指针</param>
            /// <returns>返回用于新建系统邮件账户的数据集</returns>
            public static ICreatePackage<EmailAccount> CreatePackageForCreate(string key, string account, string password, string remark
                , int clientId)
            {
                return new PackageForCreate(key, account, password, remark, clientId);
            }

            /// <summary>
            /// 创建一个用于更新系统邮件账户信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="key">索引字</param>
            /// <param name="account">帐号</param>
            /// <param name="password">密码</param>
            /// <param name="remark">备注</param>
            /// <param name="clientId">应该使用的服务地址的存储指针</param>
            /// <returns>返回用于更新系统邮件账户信息的数据集</returns>
            public static IUpdatePackage<EmailAccount> CreatePackageForUpdate(int id, string key, string account, string password
                , string remark, int clientId)
            {
                return new PackageForUpdate(id, key, account, password, remark, clientId);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建系统邮件账户的数据集
            /// </summary>
            private class PackageForCreate : IPackage<EmailAccount>, ICreatePackage<EmailAccount>
            {
                #region 公开属性

                /// <summary>
                /// 索引字
                /// </summary>
                public string Key { get; set; }

                /// <summary>
                /// 帐号
                /// </summary>
                public string Account { get; set; }

                /// <summary>
                /// 密码
                /// </summary>
                public string Password { get; set; }

                /// <summary>
                /// 备注
                /// </summary>
                public string Remark { get; set; }

                /// <summary>
                /// 应该使用的服务地址的存储指针
                /// </summary>
                public int ClientId { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建系统邮件账户的数据集
                /// </summary>
                /// <param name="key">索引字</param>
                /// <param name="account">帐号</param>
                /// <param name="password">密码</param>
                /// <param name="remark">备注</param>
                /// <param name="clientId">应该使用的服务地址的存储指针</param>
                public PackageForCreate(string key, string account, string password, string remark, int clientId)
                {
                    this.Key = key;
                    this.Account = account;
                    this.Password = password;
                    this.Remark = remark;
                    this.ClientId = clientId;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    bool hadUsedKey = db.Set<EmailAccount>().Any(x => x.Key == this.Key);
                    if (hadUsedKey)
                    {
                        throw new Exception("这个索引字已经被使用");
                    }
                    NChecker.CheckEntity<EmailClient>(this.ClientId, "服务端记录", db);
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public EmailAccount GetEntity(DbContext db)
                {
                    EmailClient client = db.Set<EmailClient>().Find(this.ClientId);
                    bool anyIsDefault = db.Set<EmailAccount>().Any(x => x.IsDefault);

                    return new EmailAccount(this.Key, this.Account, this.Password, this.Remark, client, !anyIsDefault);
                }

                #endregion
            }

            /// <summary>
            /// 用于更新系统邮件账户信息的数据集
            /// </summary>
            private class PackageForUpdate : PackageForUpdateBase<EmailAccount>, IPackage<EmailAccount>, IUpdatePackage<EmailAccount>
            {
                #region 公开属性

                /// <summary>
                /// 索引字
                /// </summary>
                public string Key { get; set; }

                /// <summary>
                /// 帐号
                /// </summary>
                public string Account { get; set; }

                /// <summary>
                /// 密码
                /// </summary>
                public string Password { get; set; }

                /// <summary>
                /// 备注
                /// </summary>
                public string Remark { get; set; }

                /// <summary>
                /// 应该使用的服务地址的存储指针
                /// </summary>
                public int ClientId { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建系统邮件账户的数据集
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="key">索引字</param>
                /// <param name="account">帐号</param>
                /// <param name="password">密码</param>
                /// <param name="remark">备注</param>
                /// <param name="clientId">应该使用的服务地址的存储指针</param>
                public PackageForUpdate(int id, string key, string account, string password, string remark, int clientId)
                    : base(id)
                {
                    this.Key = key;
                    this.Account = account;
                    this.Password = password;
                    this.Remark = remark;
                    this.ClientId = clientId;
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
                    bool hadUsedKey = db.Set<EmailAccount>().Any(x => x.Key == this.Key && x.Id != this.Id);
                    if (hadUsedKey)
                    {
                        throw new Exception("这个索引字已经被使用");
                    }
                    NChecker.CheckEntity<EmailClient>(this.ClientId, "服务端记录", db);
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override EmailAccount GetEntity(DbContext db)
                {
                    EmailClient client = db.Set<EmailClient>().Find(this.ClientId);

                    this.AddToUpdating("Key", this.Key);
                    this.AddToUpdating("Account", this.Account);
                    this.AddToUpdating("Password", this.Password);
                    this.AddToUpdating("Remark", this.Remark);
                    this.AddToUpdating("Client", client);

                    return base.GetEntity(db);
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}

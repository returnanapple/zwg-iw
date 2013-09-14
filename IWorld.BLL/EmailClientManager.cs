using System;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 系统邮件服务地址的管理者对象
    /// </summary>
    public class EmailClientManager : SimplifyManagerBase<EmailClient>, IManager<EmailClient>, ISimplify<EmailClient>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的系统邮件服务地址的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public EmailClientManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 设置默认对象
        /// </summary>
        /// <param name="emailClientId">要设置为默认对象的对象的存储指针</param>
        public void SetDefault(int emailClientId)
        {
            var ecSet = db.Set<EmailClient>();
            bool isDefault = ecSet.Any(x => x.Id == emailClientId && x.IsDefault);
            if (!isDefault)
            {
                ecSet.Where(x => x.IsDefault).ToList()
                    .ForEach(x => x.IsDefault = false);
                ecSet.Find(emailClientId).IsDefault = true;

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
            /// 创建一个用于新建系统邮件服务地址的数据集
            /// </summary>
            /// <param name="key">索引字</param>
            /// <param name="host">服务器地址</param>
            /// <param name="port">端口</param>
            /// <param name="remark">备注</param>
            /// <returns>返回用于新建系统邮件服务地址的数据集</returns>
            public static ICreatePackage<EmailClient> CreatePackageForCreate(string key, string host, int port, string remark)
            {
                return new PackageForCreate(key, host, port, remark);
            }

            /// <summary>
            /// 创建一个用于更新系统邮件服务地址信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="key">索引字</param>
            /// <param name="host">服务器地址</param>
            /// <param name="port">端口</param>
            /// <param name="remark">备注</param>
            /// <returns>返回用于更新系统邮件服务地址信息的数据集</returns>
            public static IUpdatePackage<EmailClient> CreatePackageForUpdate(int id, string key, string host, int port, string remark)
            {
                return new PackageForUpdate(id, key, host, port, remark);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建系统邮件服务地址的数据集
            /// </summary>
            private class PackageForCreate : IPackage<EmailClient>, ICreatePackage<EmailClient>
            {
                #region 公开属性

                /// <summary>
                /// 索引字
                /// </summary>
                public string Key { get; set; }

                /// <summary>
                /// 服务器地址
                /// </summary>
                public string Host { get; set; }

                /// <summary>
                /// 端口
                /// </summary>
                public int Port { get; set; }

                /// <summary>
                /// 备注
                /// </summary>
                public string Remark { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建系统邮件服务地址的数据集
                /// </summary>
                /// <param name="key">索引字</param>
                /// <param name="host">服务器地址</param>
                /// <param name="port">端口</param>
                /// <param name="remark">备注</param>
                public PackageForCreate(string key, string host, int port, string remark)
                {
                    this.Key = key;
                    this.Host = host;
                    this.Port = port;
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
                    bool hadUsedKey = db.Set<EmailClient>().Any(x => x.Key == this.Key);
                    if (hadUsedKey)
                    {
                        throw new Exception("这个索引字已经被使用");
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public EmailClient GetEntity(DbContext db)
                {
                    bool anyIsDefault = db.Set<EmailClient>().Any(x => x.IsDefault);
                    return new EmailClient(this.Key, this.Host, this.Port, this.Remark, !anyIsDefault);
                }

                #endregion
            }

            /// <summary>
            /// 用于更新系统邮件服务地址信息的数据集
            /// </summary>
            private class PackageForUpdate : PackageForUpdateBase<EmailClient>, IPackage<EmailClient>, IUpdatePackage<EmailClient>
            {
                #region 公开属性

                /// <summary>
                /// 索引字
                /// </summary>
                public string Key { get; set; }

                /// <summary>
                /// 服务器地址
                /// </summary>
                public string Host { get; set; }

                /// <summary>
                /// 端口
                /// </summary>
                public int Port { get; set; }

                /// <summary>
                /// 备注
                /// </summary>
                public string Remark { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建系统邮件服务地址的数据集
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="key">索引字</param>
                /// <param name="host">服务器地址</param>
                /// <param name="port">端口</param>
                /// <param name="remark">备注</param>
                public PackageForUpdate(int id, string key, string host, int port, string remark)
                    : base(id)
                {
                    this.Key = key;
                    this.Host = host;
                    this.Port = port;
                    this.Remark = remark;
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
                    bool hadUsedKey = db.Set<EmailClient>().Any(x => x.Key == this.Key && x.Id != this.Id);
                    if (hadUsedKey)
                    {
                        throw new Exception("这个索引字已经被使用");
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override EmailClient GetEntity(DbContext db)
                {
                    this.AddToUpdating("Key", this.Key);
                    this.AddToUpdating("Host", this.Host);
                    this.AddToUpdating("Port", this.Port);
                    this.AddToUpdating("Remark", this.Remark);

                    return base.GetEntity(db);
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}

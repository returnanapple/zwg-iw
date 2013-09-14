using System;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Helper;

namespace IWorld.BLL
{
    /// <summary>
    /// 银行帐号的管理者对象
    /// </summary>
    public class BankAccountManager : SimplifyManagerBase<BankAccount>, IManager<BankAccount>, ISimplify<BankAccount>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的银行帐号的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public BankAccountManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 设置默认对象
        /// </summary>
        /// <param name="bankAccountId">要设置为默认对象的对象的存储指针</param>
        public void SetDefault(int bankAccountId)
        {
            var baSet = db.Set<BankAccount>();
            bool isDefault = baSet.Any(x => x.Id == bankAccountId && x.IsDefault);
            if (!isDefault)
            {
                baSet.Where(x => x.IsDefault).ToList()
                    .ForEach(x => x.IsDefault = false);
                baSet.Find(bankAccountId).IsDefault = true;

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
            /// 创建一个用于新建银行帐号的数据集
            /// </summary>
            /// <param name="key">索引字</param>
            /// <param name="name">开户人</param>
            /// <param name="card">卡号</param>
            /// <param name="bank">银行</param>
            /// <param name="remark">备注</param>
            /// <param name="order">排列系数</param>
            /// <returns>返回用于新建银行帐号的数据集</returns>
            public static ICreatePackage<BankAccount> CreatePackageForCreate(string key, string name, string card, string bank, string remark
                , int order)
            {
                Bank _bank = EnumHelper.Parse<Bank>(bank);
                return new PackageForCreate(key, name, card, _bank, remark, order);
            }

            /// <summary>
            /// 创建一个用于更新银行帐号信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="key">索引字</param>
            /// <param name="name">开户人</param>
            /// <param name="card">卡号</param>
            /// <param name="bank">银行</param>
            /// <param name="remark">备注</param>
            /// <param name="order">排列系数</param>
            /// <returns>返回用于更新银行帐号信息的数据集</returns>
            public static IUpdatePackage<BankAccount> CreatePackageForUpdate(int id, string key, string name, string card, string bank
                , string remark, int order)
            {
                Bank _bank = EnumHelper.Parse<Bank>(bank);
                return new PackageForUpdate(id, key, name, card, _bank, remark, order);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建银行帐号的数据集
            /// </summary>
            private class PackageForCreate : IPackage<BankAccount>, ICreatePackage<BankAccount>
            {
                #region 公开属性

                /// <summary>
                /// 索引字
                /// </summary>
                public string Key { get; set; }

                /// <summary>
                /// 开户人
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 卡号
                /// </summary>
                public string Card { get; set; }

                /// <summary>
                /// 银行
                /// </summary>
                public Bank Bank { get; set; }

                /// <summary>
                /// 备注
                /// </summary>
                public string Remark { get; set; }

                /// <summary>
                /// 排列系数
                /// </summary>
                public int Order { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建银行帐号的数据集
                /// </summary>
                /// <param name="key">索引字</param>
                /// <param name="name">开户人</param>
                /// <param name="card">卡号</param>
                /// <param name="bank">银行</param>
                /// <param name="remark">备注</param>
                /// <param name="order">排列系数</param>
                public PackageForCreate(string key, string name, string card, Bank bank, string remark, int order)
                {
                    this.Key = key;
                    this.Name = name;
                    this.Card = card;
                    this.Bank = bank;
                    this.Remark = remark;
                    this.Order = order;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    bool hadUsedKey = db.Set<BankAccount>().Any(x => x.Key == this.Key);
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
                public BankAccount GetEntity(DbContext db)
                {
                    bool anyIsDefault = db.Set<BankAccount>().Any(x => x.IsDefault);
                    return new BankAccount(this.Key, this.Name, this.Card, this.Bank, this.Remark, this.Order, !anyIsDefault);
                }

                #endregion
            }

            /// <summary>
            /// 用于更新银行帐号信息的数据集
            /// </summary>
            private class PackageForUpdate : PackageForUpdateBase<BankAccount>, IPackage<BankAccount>, IUpdatePackage<BankAccount>
            {
                #region 公开属性

                /// <summary>
                /// 索引字
                /// </summary>
                public string Key { get; set; }

                /// <summary>
                /// 开户人
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 卡号
                /// </summary>
                public string Card { get; set; }

                /// <summary>
                /// 银行
                /// </summary>
                public Bank Bank { get; set; }

                /// <summary>
                /// 备注
                /// </summary>
                public string Remark { get; set; }

                /// <summary>
                /// 排列系数
                /// </summary>
                public int Order { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建银行帐号的数据集
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="key">索引字</param>
                /// <param name="name">开户人</param>
                /// <param name="card">卡号</param>
                /// <param name="bank">银行</param>
                /// <param name="remark">备注</param>
                /// <param name="order">排列系数</param>
                public PackageForUpdate(int id, string key, string name, string card, Bank bank, string remark, int order)
                    : base(id)
                {
                    this.Key = key;
                    this.Name = name;
                    this.Card = card;
                    this.Bank = bank;
                    this.Remark = remark;
                    this.Order = order;
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
                    bool hadUsedKey = db.Set<BankAccount>().Any(x => x.Key == this.Key && x.Id != this.Id);
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
                public override BankAccount GetEntity(DbContext db)
                {
                    this.AddToUpdating("Key", this.Key);
                    this.AddToUpdating("Name", this.Name);
                    this.AddToUpdating("Card", this.Card);
                    this.AddToUpdating("Bank", this.Bank);
                    this.AddToUpdating("Remark", this.Remark);
                    this.AddToUpdating("Order", this.Order);

                    return base.GetEntity(db);
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}

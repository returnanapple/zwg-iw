using IWorld.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace IWorld.BLL
{
    /// <summary>
    /// 大白鲨游戏的开奖记录的管理者对性
    /// </summary>
    public class LotteryOfJawManager : SimplifyManagerBase<LotteryOfJaw>
    {
        #region 构造方法

        /// <summary>
        /// 大白鲨游戏的开奖记录的管理者对性
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public LotteryOfJawManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 内嵌类型

        public class Factory
        {
            #region 静态方法

            /// <summary>
            /// 创建一个用于新建大白鲨游戏的开奖记录的数据集
            /// </summary>
            /// <param name="issue">期号</param>
            /// <param name="value">开奖号码</param>
            /// <returns>返回用于新建大白鲨游戏的开奖记录的数据集</returns>
            public static ICreatePackage<LotteryOfJaw> CreatePackageForCreate(string issue, int value)
            {
                return new PackageForCreate(issue, value);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建大白鲨游戏的开奖记录的数据集
            /// </summary>
            class PackageForCreate : IPackage<LotteryOfJaw>, ICreatePackage<LotteryOfJaw>
            {
                #region 属性

                /// <summary>
                /// 期号
                /// </summary>
                public string Issue { get; set; }

                /// <summary>
                /// 开奖号码
                /// </summary>
                public int Value { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 用于新建大白鲨游戏的开奖记录的数据集
                /// </summary>
                /// <param name="issue">期号</param>
                /// <param name="value">开奖号码</param>
                public PackageForCreate(string issue, int value)
                {
                    this.Issue = issue;
                    this.Value = value;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    bool hadLotteyied = db.Set<LotteryOfJaw>().Any(x => x.Issue == this.Issue);
                    if (hadLotteyied)
                    {
                        throw new Exception("当期已经开奖");
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public LotteryOfJaw GetEntity(DbContext db)
                {
                    return new LotteryOfJaw(this.Issue, this.Value);
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}

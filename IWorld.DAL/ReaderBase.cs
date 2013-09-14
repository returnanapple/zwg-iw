using System.Data.Entity;

namespace IWorld.DAL
{
    /// <summary>
    /// 用于继承的阅读者对象的基础类
    /// </summary>
    public abstract class ReaderBase
    {
        #region 保护字段

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        protected DbContext db;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的阅读者对象
        /// </summary>
        public ReaderBase(DbContext db)
        {
            this.db = db;
        }

        #endregion
    }
}

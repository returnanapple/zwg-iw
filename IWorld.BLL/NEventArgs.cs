using System;
using System.Data.Entity;

namespace IWorld.BLL
{
    /// <summary>
    /// 通用的监视者对象
    /// </summary>
    public class NEventArgs : EventArgs
    {
        #region 公开属性

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public DbContext Db { get; set; }

        /// <summary>
        /// 参数实体
        /// </summary>
        public object State { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的通常的监视者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        /// <param name="State">参数实体</param>
        public NEventArgs(DbContext db, object state)
        {
            this.Db = db;
            this.State = state;
        }

        #endregion
    }
}

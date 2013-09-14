using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 操作记录的管理者对象
    /// </summary>
    public class OperateRecordManager
    {
        #region 保护字段

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        protected DbContext db;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的操作记录的管理者对象
        /// </summary>
        public OperateRecordManager(DbContext db)
        {
            this.db = db;
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 写入操作记录
        /// </summary>
        /// <param name="administratorId">操作人的存储指针</param>
        /// <param name="_operate">操作</param>
        public void Write(int administratorId, string _operate)
        {
            NChecker.CheckEntity<Administrator>(administratorId, "用户", db);
            Administrator adminer = db.Set<Administrator>().Find(administratorId);
            OperateRecord or = new OperateRecord(adminer, _operate);
            db.Set<OperateRecord>().Add(or);
            db.SaveChanges();
        }

        #endregion
    }
}

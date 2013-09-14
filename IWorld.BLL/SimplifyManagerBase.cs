using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 实现简化操作方案的用于继承的管理者对象的基类
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public abstract class SimplifyManagerBase<T> : ManagerBase<T>, IManager<T>, ISimplify<T>
        where T : ModelBase
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的实现简化操作方案的管理者对象的基类
        /// </summary>
        /// <param name="db"></param>
        public SimplifyManagerBase(DbContext db)
            : base(db)
        {
        }


        #endregion

        #region 操作对象

        /// <summary>
        /// 将指定的实例从数据库中移除
        /// </summary>
        /// <param name="id">目标对象的存储指针</param>
        public virtual void Remove(int id)
        {
            NPackageForRemove<T> pfr = new NPackageForRemove<T>(id);
            Remove(pfr);
        }

        #endregion
    }
}

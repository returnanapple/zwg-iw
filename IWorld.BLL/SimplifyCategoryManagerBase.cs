using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 实现简化操作方案的用于继承的类目相关的管理者对象的基类
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public abstract class SimplifyCategoryManagerBase<T> : CategoryManaerBase<T>, IManager<T>, ICategoryManager<T>, ISimplify<T>
        where T : CategoryBase
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个虚拟的实现简化操作方案的类目相关的管理者对象的基类
        /// </summary>
        /// <param name="db"></param>
        public SimplifyCategoryManagerBase(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 操作对象

        /// <summary>
        /// 将指定的实例从数据库中移除
        /// </summary>
        /// <param name="id">目标对象的存储指针</param>
        public void Remove(int id)
        {
            NPackageForRemove<T> pfr = new NPackageForRemove<T>(id);
            Remove(pfr);
        }

        #endregion
    }
}

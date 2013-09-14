using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 用于继承的用于创建类目相关的实例的数据集基础对象
    /// （注：请务必在派生类中显示实现 GetEntity 方法！）
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public abstract class PackageForCreateCategoryBase<T> : IPackage<T>, ICreateCategoryPackagr<T>
        where T : CategoryBase
    {
        #region 公开属性

        /// <summary>
        /// 父节点的存储指针
        /// </summary>
        public int ParentId { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个用于创建类目相关的实例的数据集基础对象
        /// </summary>
        /// <param name="parentId">父节点的存储指针</param>
        public PackageForCreateCategoryBase(int parentId)
        {
            this.ParentId = parentId;
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 获取指定的父类目
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        /// <returns>返回父类目的实例</returns>
        public T GetParent(DbContext db)
        {
            return this.ParentId == -1 ? null : db.Set<T>().Find(this.ParentId);
        }

        /// <summary>
        /// 检查数据集的内容是否符合定义
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public virtual void CheckData(DbContext db)
        {
            if (this.ParentId != -1)
            {
                NChecker.CheckEntity<T>(this.ParentId, "父节点", db);
            }
        }

        /// <summary>
        /// 获取实体对象（必须在派生类中重写）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        /// <returns>返回泛型状态所规定的实体类</returns>
        public abstract T GetEntity(DbContext db);

        #endregion
    }
}

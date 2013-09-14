using System;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 通用的用于删除实例的数据集
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public class NPackageForRemove<T> : IPackage<T>, IRemovePackage<T>
        where T : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 实例的存储指针
        /// </summary>
        public int Id { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的通用的用于删除实例的数据集
        /// </summary>
        /// <param name="id">实例的存储指针</param>
        public NPackageForRemove(int id)
        {
            this.Id = id;
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 检查数据集的内容是否符合定义
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public void CheckData(DbContext db)
        {
            bool HaveEntity = db.Set<T>().Any(x => x.Id == this.Id);
            if (!HaveEntity) { throw new Exception("指定的实例不存在"); }
        }

        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        /// <returns>返回泛型状态所规定的实体类</returns>
        public T GetEntity(DbContext db)
        {
            return db.Set<T>().Find(this.Id);
        }

        #endregion
    }
}

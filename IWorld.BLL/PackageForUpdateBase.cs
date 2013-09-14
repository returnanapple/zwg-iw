using System.Collections.Generic;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 用于继承的用于更新实例的数据集基础对象
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public abstract class PackageForUpdateBase<T> : IPackage<T>, IUpdatePackage<T>
        where T : ModelBase
    {
        #region 私有字段

        /// <summary>
        /// 要修改的属性值的集合
        /// </summary>
        private Dictionary<string, object> properties = new Dictionary<string, object>();

        #endregion

        #region 公开属性

        /// <summary>
        /// 存储指针
        /// </summary>
        public int Id { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的用于更新实例的数据集基础对象
        /// </summary>
        /// <param name="id">目标对象的存储指针</param>
        public PackageForUpdateBase(int id)
        {
            this.Id = id;
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 检查数据集的内容是否符合定义
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public virtual void CheckData(DbContext db)
        {
            NChecker.CheckEntity<T>(this.Id, "实例", db);
        }

        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        /// <returns>返回泛型状态所规定的实体类</returns>
        public virtual T GetEntity(DbContext db)
        {
            return db.Set<T>().Find(this.Id);
        }

        /// <summary>
        /// 获取要修改的属性值的集合
        /// </summary>
        /// <returns>返回包含所有要修改的属性值的字典集</returns>
        public Dictionary<string, object> GetPropertieList()
        {
            return this.properties;
        }

        #endregion

        #region 保护方法

        /// <summary>
        /// 向系统声明一个属性需要修改为指定的值
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">修改后的值</param>
        protected void AddToUpdating(string propertyName, object value)
        {
            this.properties.Add(propertyName, value);
        }

        #endregion
    }
}

using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 定义用于新建类目类实例的数据集
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public interface ICreateCategoryPackagr<T> : ICreatePackage<T>
        where T : CategoryBase
    {
        /// <summary>
        /// 获取指定的父类目
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        /// <returns>返回父类目的实例</returns>
        T GetParent(DbContext db);
    }
}

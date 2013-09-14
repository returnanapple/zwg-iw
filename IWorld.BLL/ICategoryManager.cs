using IWorld.Model;
using System.Collections.Generic;

namespace IWorld.BLL
{
    /// <summary>
    /// 定义类目相关的模型的管理者对象的基本方法
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public interface ICategoryManager<T> : IManager<T>
        where T : CategoryBase
    {
        /// <summary>
        /// 将实例添加到数据库
        /// </summary>
        /// <param name="package">所要用于添加实例的数据集</param>
        /// <returns>返回被添加的实例的副本</returns>
        T Create(ICreateCategoryPackagr<T> package);

        /// <summary>
        /// 获取父类目
        /// </summary>
        /// <param name="entity">目标类目</param>
        /// <returns>返回父类目的实例</returns>
        T GetParent(CategoryBase entity);

        /// <summary>
        /// 返回所有的上级类目
        /// </summary>
        /// <param name="entity">目标类目</param>
        /// <returns>返回所有的上级类目的列表</returns>
        List<T> GetElders(CategoryBase entity);

        /// <summary>
        /// 获取子类目
        /// </summary>
        /// <param name="entity">目标类目</param>
        /// <returns>返回子类目的列表</returns>
        List<T> GetChildren(CategoryBase entity);

        /// <summary>
        /// 获取所有子孙类目
        /// </summary>
        /// <param name="entity">目标类目</param>
        /// <returns></returns>
        List<T> GetOffspring(CategoryBase entity);

        /// <summary>
        /// 获取整个家族类目树
        /// </summary>
        /// <param name="entity">目标类目</param>
        /// <returns>返回家族树的列表</returns>
        List<T> GetClan(CategoryBase entity);
    }
}

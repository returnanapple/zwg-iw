using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 定义管理者对象的基本操作方法
    /// </summary>
    public interface IManager<T>
        where T : ModelBase
    {
        /// <summary>
        /// 将实例添加到数据库
        /// </summary>
        /// <param name="package">所要用于添加实例的数据集</param>
        /// <returns>返回被添加的实例的副本</returns>
        T Create(ICreatePackage<T> package);

        /// <summary>
        /// 将实例的改变更新到数据库
        /// </summary>
        /// <param name="package">所要用于修改实例的数据集</param>
        void Update(IUpdatePackage<T> package);

        /// <summary>
        /// 将指定的实例从数据库中移除
        /// </summary>
        /// <param name="package">所要用于移除实例的数据集</param>
        void Remove(IRemovePackage<T> package);
    }
}

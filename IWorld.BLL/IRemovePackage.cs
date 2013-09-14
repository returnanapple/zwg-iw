using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 定义用于删除实例的数据集
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public interface IRemovePackage<T> : IPackage<T>
        where T : ModelBase
    {
    }
}

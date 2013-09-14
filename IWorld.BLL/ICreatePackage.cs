using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 定于用于新建实例的数据集
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public interface ICreatePackage<T> : IPackage<T>
        where T : ModelBase
    {
    }
}

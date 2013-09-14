using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 定义管理者对象的简化操作方案
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISimplify<T>
        where T : ModelBase
    {
        /// <summary>
        /// 将指定的实例从数据库中移除
        /// </summary>
        /// <param name="id">目标对象的存储指针</param>
        void Remove(int id);
    }
}

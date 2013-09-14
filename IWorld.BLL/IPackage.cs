using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 定义用于向管理者对象传递数据的数据集
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public interface IPackage<T>
        where T : ModelBase
    {
        /// <summary>
        /// 检查数据集的内容是否符合定义
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        void CheckData(DbContext db);

        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        /// <returns>返回泛型状态所规定的实体类</returns>
        T GetEntity(DbContext db);
    }
}

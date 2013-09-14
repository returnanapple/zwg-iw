using System.Collections.Generic;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 定义用于更新实例信息的数据集
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public interface IUpdatePackage<T> : IPackage<T>
        where T : ModelBase
    {
        /// <summary>
        /// 获取要修改的属性值的集合
        /// </summary>
        /// <returns>返回包含所有要修改的属性值的字典集</returns>
        Dictionary<string, object> GetPropertieList();
    }
}

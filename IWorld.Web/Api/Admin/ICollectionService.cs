using System.Collections.Generic;
using System.ServiceModel;
using IWorld.Contract.Admin;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 定义开奖结果采集的数据服务（后台）
    /// </summary>
    [ServiceContract]
    public interface ICollectionService
    {
        /// <summary>
        /// 获取近期采集结果的列表
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <param name="key">已读的最新采集结果的键</param>
        /// <returns>返回近期采集结果的列表</returns>
        [OperationContract]
        List<CollectionResult> GetCollectionResult(string token, string key = "");
    }
}

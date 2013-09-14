using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using IWorld.Contract.Client;

namespace IWorld.Web.Api
{
    /// <summary>
    /// 定义活动相关的数据服务
    /// </summary>
    [ServiceContract]
    public interface IActivityService
    {
        /// <summary>
        /// 获取默认活动列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回默认活动的分页列表</returns>
        [OperationContract]
        PaginationList<NormalActivitiesResult> GetNormalActivities(int page, string token);

        /// <summary>
        /// 获取兑换活动列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回兑换活动的分页列表</returns>
        [OperationContract]
        PaginationList<ExchangeActivitiesResult> GetExchangeActivities(int page, string token);

        /// <summary>
        /// 兑换
        /// </summary>
        /// <param name="exchangeId">兑换活动的数据库存储指针</param>
        /// <param name="sum">兑换数量</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult Exchange(int exchangeId, int sum, string token);
    }
}

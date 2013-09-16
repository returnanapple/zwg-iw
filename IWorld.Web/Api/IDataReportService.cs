using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using IWorld.Contract.Client;
using IWorld.Model;

namespace IWorld.Web.Api
{
    /// <summary>
    /// 定义数据报表相关的数据服务
    /// </summary>
    [ServiceContract]
    public interface IDataReportService
    {
        /// <summary>
        /// 获取数据报表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="type">类型</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回报表数据的分页列表</returns>
        [OperationContract]
        PaginationList<DataReportsResult> GetReports(string beginTime, string endTime, ReportsType type, int page, string token);
    }
}

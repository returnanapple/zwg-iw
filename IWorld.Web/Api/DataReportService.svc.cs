using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using IWorld.Contract.Client;
using IWorld.Model;
using IWorld.BLL;
using IWorld.DAL;
using IWorld.Setting;
using IWorld.Helper;

namespace IWorld.Web.Api
{
    /// <summary>
    /// 数据报表相关的数据服务
    /// </summary>
    public class DataReportService : IDataReportService
    {
        /// <summary>
        /// 获取数据报表
        /// </summary>
        /// <param name="selectType">筛选类型</param>
        /// <param name="type">类型</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回报表数据的分页列表</returns>
        public PaginationList<DataReportsResult> GetReports(ReportsSelectType selectType, ReportsType type, int page, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new PaginationList<DataReportsResult>("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    ClientDataReportReader reader = new ClientDataReportReader(db);
                    return reader.ReadReports(userId, selectType, type, page);
                }
            }
            catch (Exception ex)
            {
                return new PaginationList<DataReportsResult>(ex.Message);
            }
        }
    }
}

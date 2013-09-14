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
    /// 活动相关的数据服务
    /// </summary>
    public class ActivityService : IActivityService
    {
        /// <summary>
        /// 获取默认活动列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回默认活动的分页列表</returns>
        public PaginationList<NormalActivitiesResult> GetNormalActivities(int page, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new PaginationList<NormalActivitiesResult>("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    ClientActivityReader reader = new ClientActivityReader(db);
                    return reader.ReadNormalActivities(page);
                }
            }
            catch (Exception ex)
            {
                return new PaginationList<NormalActivitiesResult>(ex.Message);
            }
        }

        /// <summary>
        /// 获取兑换活动列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回兑换活动的分页列表</returns>
        public PaginationList<ExchangeActivitiesResult> GetExchangeActivities(int page, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new PaginationList<ExchangeActivitiesResult>("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    ClientActivityReader reader = new ClientActivityReader(db);
                    return reader.ReadExchangeActivities(page);
                }
            }
            catch (Exception ex)
            {
                return new PaginationList<ExchangeActivitiesResult>(ex.Message);
            }
        }

        /// <summary>
        /// 兑换
        /// </summary>
        /// <param name="exchangeId">兑换活动的数据库存储指针</param>
        /// <param name="sum">兑换数量</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult Exchange(int exchangeId, int sum, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new OperateResult("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    ICreatePackage<ExchangeParticipateRecord> pfc = ExchangeParticipateRecordManager.Factory
                        .CreatePackageForCreate(userId, exchangeId, sum);
                    new ExchangeParticipateRecordManager(db).Create(pfc);

                    return new OperateResult();
                }
            }
            catch (Exception ex)
            {
                return new OperateResult(ex.Message);
            }
        }
    }
}

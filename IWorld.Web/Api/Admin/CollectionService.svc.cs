using System;
using System.Collections.Generic;
using IWorld.Contract.Admin;
using IWorld.BLL;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 开奖结果采集的数据服务（后台）
    /// </summary>
    public class CollectionService : ICollectionService
    {
        /// <summary>
        /// 获取近期采集结果的列表
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <param name="key">已读的最新采集结果的键</param>
        /// <returns>返回近期采集结果的列表</returns>
        public List<CollectionResult> GetCollectionResult(string token, string key = "")
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return null;
                }

                List<CacheManager.ICollectionResult> tList = CacheManager.GetCollectionResults(key);
                return tList.ConvertAll(x => new CollectionResult(x.GetKey(), x.GetMessage(), x.GetTime()));
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

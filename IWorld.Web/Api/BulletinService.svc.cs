using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using IWorld.Contract.Client;
using IWorld.DAL;
using IWorld.BLL;

namespace IWorld.Web.Api
{
    /// <summary>
    /// 公告信息的数据服务
    /// </summary>
    public class BulletinService : IBulletinService
    {
        /// <summary>
        /// 获取公告列表
        /// </summary>
        /// <returns>返回公告列表</returns>
        public NormalList<BulletinResult> GetBulletins()
        {
            try
            {
                using (WebMapContext db = new WebMapContext())
                {
                    ClientBulletinReader reader = new ClientBulletinReader(db);
                    return reader.ReadBulletins();
                }
            }
            catch (Exception ex)
            {
                return new NormalList<BulletinResult>(ex.Message);
            }
        }

        /// <summary>
        /// 获取未读通知
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回未读通知</returns>
        public NoticeResult GetNotice(string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return null;
                }

                using (WebMapContext db = new WebMapContext())
                {
                    ClientBulletinReader reader = new ClientBulletinReader(db);
                    var result = reader.ReadNotice(userId);
                    new NoticeManager(db).Read(result.NoticeId, userId);
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

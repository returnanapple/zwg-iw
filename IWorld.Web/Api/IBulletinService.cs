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
    /// 定义公告信息的数据服务
    /// </summary>
    [ServiceContract]
    public interface IBulletinService
    {
        /// <summary>
        /// 获取公告列表
        /// </summary>
        /// <returns>返回公告列表</returns>
        [OperationContract]
        NormalList<BulletinResult> GetBulletins();

        /// <summary>
        /// 获取未读通知
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回未读通知</returns>
        [OperationContract]
        NoticeResult GetNotice(string token);
    }
}

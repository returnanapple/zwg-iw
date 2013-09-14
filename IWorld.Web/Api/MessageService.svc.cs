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
    /// 站内消息相关的数据服务
    /// </summary>
    public class MessageService : IMessageService
    {
        /// <summary>
        /// 获取站内短消息列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回站内短消息的分页列表</returns>
        public PaginationList<MessageResult> GetMessages(int page, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new PaginationList<MessageResult>("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    ClientMessageReader reader = new ClientMessageReader(db);
                    return reader.ReadMessages(userId, page);
                }
            }
            catch (Exception ex)
            {
                return new PaginationList<MessageResult>(ex.Message);
            }
        }

        /// <summary>
        /// 将站内消息标记为“已阅”
        /// </summary>
        /// <param name="messageId">站内短消息的数据库存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult Read(int messageId, string token)
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
                    new MessageManager(db).Read(messageId, userId);

                    return new OperateResult();
                }
            }
            catch (Exception ex)
            {
                return new OperateResult(ex.Message);
            }
        }

        /// <summary>
        /// 删除站内信息
        /// </summary>
        /// <param name="messageId">站内短消息的数据库存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult Delete(int messageId, string token)
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
                    new MessageManager(db).Delete(messageId, userId);

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

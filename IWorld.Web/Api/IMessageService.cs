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
    /// 定义站内消息相关的数据服务
    /// </summary>
    [ServiceContract]
    public interface IMessageService
    {
        /// <summary>
        /// 获取站内短消息列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回站内短消息的分页列表</returns>
        [OperationContract]
        PaginationList<MessageResult> GetMessages(int page, string token);

        /// <summary>
        /// 将站内消息标记为“已阅”
        /// </summary>
        /// <param name="messageId">站内短消息的数据库存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult Read(int messageId, string token);

        /// <summary>
        /// 删除站内信息
        /// </summary>
        /// <param name="messageId">站内短消息的数据库存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult Delete(int messageId, string token);
    }
}

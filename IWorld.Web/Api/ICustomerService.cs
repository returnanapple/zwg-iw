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
    /// 定义客户服务相关的数据服务
    /// </summary>
    [ServiceContract]
    public interface ICustomerService
    {
        /// <summary>
        /// 获取聊天记录
        /// </summary>
        /// <param name="type">客服类型</param>
        /// <param name="id">当前已经获取的最新信息的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回聊天信息的普通列表</returns>
        [OperationContract]
        NormalList<CustomerMessageResult> GetMessages(CustomerType type, int id, string token);

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="type">客服类型</param>
        /// <param name="message">聊天内容</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult Send(CustomerType type, string message, string token);
    }
}

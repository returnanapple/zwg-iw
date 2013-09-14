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
    /// 客户服务相关的数据服务
    /// </summary>
    public class CustomerService : ICustomerService
    {
        /// <summary>
        /// 获取聊天记录
        /// </summary>
        /// <param name="type">客服类型</param>
        /// <param name="id">当前已经获取的最新信息的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回聊天信息的普通列表</returns>
        public NormalList<CustomerMessageResult> GetMessages(CustomerType type, int id, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new NormalList<CustomerMessageResult>("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {

                    return null;
                }
            }
            catch (Exception ex)
            {
                return new NormalList<CustomerMessageResult>(ex.Message);
            }
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="type">客服类型</param>
        /// <param name="message">聊天内容</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult Send(CustomerType type, string message, string token)
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
                    ICreatePackage<CustomerRecord> pfc = CustomerRecordManager.Factory
                        .CreatePackageForCreate(userId, type, false, message);
                    new CustomerRecordManager(db).Create(pfc);

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

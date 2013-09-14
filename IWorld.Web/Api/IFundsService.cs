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
    /// 定义资金相关的数据服务
    /// </summary>
    [ServiceContract]
    public interface IFundsService
    {
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="userId">用户的数据库存储指针</param>
        /// <param name="sum">充值金额</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回充值结果</returns>
        [OperationContract]
        RechargeResult Recharge(int userId, double sum, string token);

        /// <summary>
        /// 获取充值明细
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回充值明细的分页列表</returns>
        [OperationContract]
        PaginationList<RechargeDetailsResult> GetRechargeDetails(int page, string token);

        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="sum">提现金额</param>
        /// <param name="safeWord">安全码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult Withdraw(double sum, string safeWord, string token);

        /// <summary>
        /// 获取提现明细
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回提现明细的分页列表</returns>
        [OperationContract]
        PaginationList<WithdrawDetailsResult> GetWithdrawDetails(int page, string token);
    }
}

using System.ServiceModel;
using IWorld.Contract.Admin;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 定义兑换活动管理的数据服务（后台）
    /// </summary>
    [ServiceContract]
    public interface IExchangeService
    {
        /// <summary>
        /// 获取兑换活动的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回兑换活动的分页列表</returns>
        [OperationContract]
        PaginationList<ExchangeResult> GetExchangeList(string keyword, RegularlyStatusSelectType status, int page, string token);

        /// <summary>
        /// 获取兑换活动的参与记录的分页列表
        /// </summary>
        /// <param name="exchangeId">目标活动的存储指针</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回兑换活动的参与记录的分页列表</returns>
        [OperationContract]
        PaginationList<ExchangeParticipateRecordResult> GetParticipateRecordList(int exchangeId, int ownerId, string beginTime
            , string endTime, int page, string token);

        /// <summary>
        /// 添加兑换活动
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult AddExchange(AddExchangeImport import, string token);

        /// <summary>
        /// 编辑兑换活动的基本信息
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditExchange_Basic(EditExchangeImport_Basic import, string token);

        /// <summary>
        /// 编辑兑换活动
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditExchange(EditExchangeImport import, string token);

        /// <summary>
        /// 删除兑换活动
        /// </summary>
        /// <param name="exchangeId">目标活动的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult RemoveExchange(int exchangeId, string token);

        /// <summary>
        /// 隐藏兑换活动
        /// </summary>
        /// <param name="exchangeId">目标活动的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult HideExchange(int exchangeId, string token);

        /// <summary>
        /// 显示兑换活动
        /// </summary>
        /// <param name="exchangeId">目标活动的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult ShowExchange(int exchangeId, string token);

        /// <summary>
        /// 获取实体奖品赠送记录的分页列表
        /// </summary>
        /// <param name="exchangeId">目标活动的存储指针</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <param name="status">赠送状态</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回实体奖品赠送记录的分页列表</returns>
        [OperationContract]
        PaginationList<GiftResult> GetGiftList(int exchangeId, int ownerId, GiftStatusSelectType status, int page, string token);

        /// <summary>
        /// 赠送实体奖品
        /// </summary>
        /// <param name="giftId">实体奖品赠送记录的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult HandselGift(int giftId, string token);
    }
}

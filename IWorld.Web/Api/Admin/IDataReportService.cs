using System.ServiceModel;
using IWorld.Contract.Admin;
using IWorld.Model;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 定义报表的数据服务（后台）
    /// </summary>
    [ServiceContract]
    public interface IDataReportService
    {
        /// <summary>
        /// 获取站点综合信息
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回站点综合信息</returns>
        [OperationContract]
        ComprehensiveInformationResult GetComprehensiveInformation(string token);

        /// <summary>
        /// 获取站点信息统计的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="timePeriod">时间段</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回站点信息统计的分页列表</returns>
        [OperationContract]
        PaginationList<SiteDataResult> GetSiteDataList(string beginTime, string endTime, TimePeriodSelectType timePeriod
            , int page, string token);

        /// <summary>
        /// 获取个人信息统计的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="timePeriod">时间段</param>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="username">用户名</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回个人信息统计的分页列表</returns>
        [OperationContract]
        PaginationList<PersonalDataResult> GetPersonalDataList(string beginTime, string endTime, TimePeriodSelectType timePeriod
            , int userId, string username, int page, string token);

        /// <summary>
        /// 获取投注信息的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <param name="username">用户名</param>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="tagId">目标玩法标签的存储指针</param>
        /// <param name="howToPlatId">目标玩法的存储指针</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回投注信息的分页列表</returns>
        [OperationContract]
        PaginationList<BettingResult> GetBettingList(string beginTime, string endTime, int ownerId, string username
            , int ticketId, int tagId, int howToPlatId, BettingStatusSelectType status, int page, string token);

        /// <summary>
        /// 获取追号信息的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="tagId">目标玩法标签的存储指针</param>
        /// <param name="howToPlatId">目标玩法的存储指针</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回追号信息的分页列表</returns>
        [OperationContract]
        PaginationList<ChasingResult> GetChasingList(string beginTime, string endTime, int ownerId, int ticketId, int tagId
            , int howToPlatId, ChasingStatusSelectType status, int page, string token);

        /// <summary>
        /// 获取投注（追号）信息的分页列表
        /// </summary>
        /// <param name="chasingId">目标追号记录的存储指针</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回投注（追号）信息的分页列表</returns>
        [OperationContract]
        PaginationList<BettingForChasingResult> GetBettingForChasingList(int chasingId, BettingStatusSelectType status, int page
            , string token);

        /// <summary>
        /// 获取充值申请的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="status">状态</param>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="userId">用户名</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回充值申请的分页列表</returns>
        [OperationContract]
        PaginationList<RechargeResult> GetRechargeList(string beginTime, string endTime, RechargeStatusSelectType status
            , int userId, string username, int page, string token);

        /// <summary>
        /// 获取未处理的充值申请的数量
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回未处理的充值申请的数量</returns>
        [OperationContract]
        UntreatedRecharCountResult GetUntreatedRecharCount(string token);

        /// <summary>
        /// 确认充值申请
        /// </summary>
        /// <param name="rechargeId">目标充值申请的存储指针</param>
        /// <param name="card">来源卡号</param>
        /// <param name="name">来源卡的开户人</param>
        /// <param name="bank">来源卡的开户银行</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult DeterminetRecharge(int rechargeId, string card, string name, Bank bank, string token);

        /// <summary>
        /// 否决充值申请
        /// </summary>
        /// <param name="rechargeId">目标充值申请的存储指针</param>
        /// <param name="remark">备注</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult NegativeRecharge(int rechargeId, string remark, string token);

        /// <summary>
        /// 获取提现申请的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="status">状态</param>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="username">用户名</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回提现申请的分页列表</returns>
        [OperationContract]
        PaginationList<WithdrawalResult> GetWithdrawalList(string beginTime, string endTime, WithdrawalsStatusSelectType status
            , int userId, string username, int page, string token);

        /// <summary>
        /// 获取未处理的提现申请的数量
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        UntreatedWithdrawalCountResult GetUntreatedWithdrawalCount(string token);

        /// <summary>
        /// 确认提现申请
        /// </summary>
        /// <param name="withdrawalId">目标提现申请的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult DeterminetWithdrawal(int withdrawalId, string token);

        /// <summary>
        /// 否决提现申请
        /// </summary>
        /// <param name="withdrawalId">目标提现申请的存储指针</param>
        /// <param name="remark">备注</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult NegativeWithdrawal(int withdrawalId, string remark, string token);

        /// <summary>
        /// 获取支取记录的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回支取记录的分页列表</returns>
        [OperationContract]
        PaginationList<TransferResult> GetTransferList(string beginTime, string endTime, int userId, int page, string token);

        /// <summary>
        /// 添加支取记录
        /// </summary>
        /// <param name="sum">数额</param>
        /// <param name="remark">备注</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult AddTransfer(double sum, string remark, string token);

        /// <summary>
        /// 设置投注记录是否作别
        /// </summary>
        /// <param name="bettingId">目标投注记录的存储指针</param>
        /// <param name="cheat">一个布尔值 标识投注记录是否作弊</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult SetCheatForBetting(int bettingId, bool cheat, string token);
    }
}

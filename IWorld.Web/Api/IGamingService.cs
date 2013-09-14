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
    /// 定义博彩相关的数据服务
    /// </summary>
    [ServiceContract]
    public interface IGamingService
    {
        /// <summary>
        /// 获取彩票和彩票的最新动态
        /// </summary>
        /// <returns>返回彩票信息的普通列表</returns>
        [OperationContract]
        NormalList<LotteryTicketResult> GetLotteryTickets();

        /// <summary>
        /// 获取玩法信息
        /// </summary>
        /// <returns>返回彩票信息的普通列表</returns>
        [OperationContract]
        NormalList<LotteryTicketResult> GetHowToPlays();

        /// <summary>
        /// 投注
        /// </summary>
        /// <param name="bettingImport">用于投注的数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        BettingActionResult Betting(BettingImport bettingImport, string token);

        /// <summary>
        /// 删除投注
        /// </summary>
        /// <param name="bettingId">投注记录的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult RemoveBetting(int bettingId, string token);

        /// <summary>
        /// 获取投注明细
        /// </summary>
        /// <param name="selectType">筛选类型</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回投注明细的分页列表</returns>
        [OperationContract]
        PaginationList<BettingDetailsResult> GetBettingDetails(BettingDetailsSelectType selectType, string beginTime
            , string endTime, int page, string token);

        /// <summary>
        /// 获取中奖排行
        /// </summary>
        /// <param name="ticketId">目标裁判的存储指针</param>
        /// <returns>返回中奖排行信息的普通列表</returns>
        [OperationContract]
        NormalList<RankingResult> GetRanking(int ticketId);

        /// <summary>
        /// 获取开奖历史
        /// </summary>
        /// <param name="ticketId">目标裁判的存储指针</param>
        /// <returns>返回开奖历史的普通列表</returns>
        [OperationContract]
        NormalList<HistoryOfLotteryResult> GetHistoryOfLottery(int ticketId);

        /// <summary>
        /// 添加临时的彩票单式的位信息
        /// </summary>
        /// <param name="values">投注</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        AddBettingValuesResult AddBettingValues(List<string> values, string token);
    }
}

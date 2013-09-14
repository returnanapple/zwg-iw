using System.Collections.Generic;
using System.ServiceModel;
using IWorld.Contract.Admin;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 定义彩票管理的数据服务（后台）
    /// </summary>
    [ServiceContract]
    public interface ILotteryTicketService
    {
        /// <summary>
        /// 获取彩票信息的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="isHide">隐藏</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回彩票信息的分页列表</returns>
        [OperationContract]
        PaginationList<TicketResult> GetTicketList(string keyword, HideOrNotSelectType isHide, int page, string token);

        /// <summary>
        /// 获取开奖时间的分页列表
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回开奖时间的分页列表</returns>
        [OperationContract]
        PaginationList<LotteryTimeResult> GetLotteryTimeList(int ticketId, int page, string token);

        /// <summary>
        /// 获取开奖记录的分页列表
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="sources">来源</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回开奖记录的分页列表</returns>
        [OperationContract]
        PaginationList<LotteryResult> GetLotteryList(int ticketId, LotterySourcesSelectType sources, int page, string token);

        /// <summary>
        /// 修改彩票信息
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EdotTicket(EditTicketImport import, string token);

        /// <summary>
        /// 修改开奖时间集
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="imports">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditLotteryTime(int ticketId, List<EditLotteryTimeImport> imports, string token);

        /// <summary>
        /// 隐藏彩票
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult HideTicket(int ticketId, string token);

        /// <summary>
        /// 显示彩票
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult ShowTicket(int ticketId, string token);

        /// <summary>
        /// 获取玩法标签信息的分页列表
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="keyword">关键字</param>
        /// <param name="isHide">隐藏</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回玩法标签信息的分页列表</returns>
        [OperationContract]
        PaginationList<PlayTagResult> GetPlayTagList(int ticketId, string keyword, HideOrNotSelectType isHide, int page
            , string token);

        /// <summary>
        /// 修改玩法标签
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditPlayTag(EditPlayTagImport import, string token);

        /// <summary>
        /// 隐藏玩法标签
        /// </summary>
        /// <param name="playTagId">目标玩法标签的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult HidePlayTag(int playTagId, string token);

        /// <summary>
        /// 显示玩法标签
        /// </summary>
        /// <param name="playTagId">目标玩法标签的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult ShowPlayTag(int playTagId, string token);

        /// <summary>
        /// 获取玩法的分页列表
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="playTagId">目标玩法标签的存储指针</param>
        /// <param name="keyword">关键字</param>
        /// <param name="isHide">隐藏</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回玩法的分页列表</returns>
        [OperationContract]
        PaginationList<HowToPlayResult> GetHowToPlayList(int ticketId, int playTagId, string keyword, HideOrNotSelectType isHide
            , int page, string token);

        /// <summary>
        /// 修改玩法信息
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditHowToPlay(EditHowToPlayImport import, string token);

        /// <summary>
        /// 隐藏玩法
        /// </summary>
        /// <param name="howToPlayId">目标玩法的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult HideHowToPlay(int howToPlayId, string token);

        /// <summary>
        /// 显示玩法
        /// </summary>
        /// <param name="howToPlayId">目标玩法的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult ShowHowToPlay(int howToPlayId, string token);

        /// <summary>
        /// 获取虚拟排行信息的分页列表
        /// </summary>
        /// <param name="ticketId">对应的彩票的存储指针</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回虚拟排行信息的分页列表</returns>
        [OperationContract]
        PaginationList<VirtualTopResult> GetVirtualTopList(int ticketId, int page, string token);

        /// <summary>
        /// 添加虚拟排行信息
        /// </summary>
        /// <param name="import"数据集></param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult AddVirtualTop(AddVirtualTopImport import, string token);

        /// <summary>
        /// 编辑虚拟排行信息
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns><返回操作结果/returns>
        [OperationContract]
        OperateResult EditVirtualTop(EditVirtualTopImport import, string token);

        /// <summary>
        /// 移除虚拟排行
        /// </summary>
        /// <param name="virtualTopId">对应的虚拟排行信息</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult RemoveVirtualTop(int virtualTopId, string token);
    }
}

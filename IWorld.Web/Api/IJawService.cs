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
    /// 定义大白鲨游戏的服务
    /// </summary>
    [ServiceContract]
    public interface IJawService
    {
        /// <summary>
        /// 获取大白鲨游戏的主显示信息
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回大白鲨游戏的主显示信息</returns>
        [OperationContract]
        MainOfJawResult GetMainOfJaw(string token);

        /// <summary>
        /// 获取大白鲨游戏的开奖记录（前十期）
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回大白鲨游戏的开奖记录（前十期）</returns>
        [OperationContract]
        List<LotteryOfJawResult> GetLotterys(string token);

        /// <summary>
        /// 投注
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult Bet(BettingOfJawImport import, string token);

        /// <summary>
        /// 撤销投注
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult Revocation(string token);
    }
}


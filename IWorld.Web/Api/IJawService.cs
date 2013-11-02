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
        [OperationContract]
        MainOfJawResult GetMainOfJaw(string token);

        [OperationContract]
        List<LotteryOfJawResult> GetLotterys(string token);

        [OperationContract]
        OperateResult Bet(BettingOfJawImport import, string token);

        [OperationContract]
        OperateResult Revocation(string token);
    }
}


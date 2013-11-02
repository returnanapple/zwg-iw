using IWorld.Contract.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace IWorld.Web.Api
{
    /// <summary>
    /// 大白鲨游戏的服务
    /// </summary>
    public class JawService : IJawService
    {
        public MainOfJawResult GetMainOfJaw(string token)
        {
            throw new NotImplementedException();
        }

        public List<LotteryOfJawResult> GetLotterys(string token)
        {
            throw new NotImplementedException();
        }

        public OperateResult Bet(BettingOfJawImport import, string token)
        {
            throw new NotImplementedException();
        }

        public OperateResult Revocation(string token)
        {
            throw new NotImplementedException();
        }
    }
}

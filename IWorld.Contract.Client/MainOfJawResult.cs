using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IWorld.Model;
using System.Runtime.Serialization;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 大白鲨游戏的主要信息
    /// </summary>
    [DataContract]
    public class MainOfJawResult : OperateResult
    {
        /// <summary>
        /// 当期期号
        /// </summary>
        [DataMember]
        public string Phases { get; set; }

        /// <summary>
        /// 开奖号码
        /// </summary>
        [DataMember]
        public int LoteryValue { get; set; }

        /// <summary>
        /// 下一期期号
        /// </summary>
        [DataMember]
        public string NextPhases { get; set; }

        /// <summary>
        /// 下一期开奖时间
        /// </summary>
        [DataMember]
        public DateTime NextLotteryTime { get; set; }

        /// <summary>
        /// 当前期用户是否已经投注
        /// </summary>
        [DataMember]
        public bool HadLottery { get; set; }

        /// <summary>
        /// 整体盈亏
        /// </summary>
        [DataMember]
        public double Profit { get; set; }

        public MainOfJawResult()
        {
        }

        public MainOfJawResult(string error)
            : base(error)
        {
        }
    }
}

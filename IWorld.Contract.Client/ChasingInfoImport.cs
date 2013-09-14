using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 用于封装追号信息的数据集
    /// </summary>
    [DataContract]
    public class ChasingInfoImport
    {
        /// <summary>
        /// 一个布尔值 标识当次投注是否进行追号
        /// </summary>
        [DataMember]
        public bool IsChasing { get; set; }

        /// <summary>
        /// 顺延期数
        /// </summary>
        [DataMember]
        public int Postpone { get; set; }

        /// <summary>
        /// 持续期数
        /// </summary>
        [DataMember]
        public int Continuance { get; set; }

        /// <summary>
        /// 一个布尔值 标识如果在开始追号之前就开除号码 是否中止追号
        /// </summary>
        [DataMember]
        public bool EndIfLotteryBeforeBegin { get; set; }

        /// <summary>
        /// 一个布尔值 标识如果在开始追号过程中中奖 是否中止追号
        /// </summary>
        [DataMember]
        public bool EndIfLotteryAtGoing { get; set; }

        /// <summary>
        /// 投注信息
        /// </summary>
        [DataMember]
        public List<BettingForChasingInfo> Bettings { get; set; }
    }
}

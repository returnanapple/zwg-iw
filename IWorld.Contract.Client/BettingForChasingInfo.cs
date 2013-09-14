using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 用于封装投注（追号）信息的数据集
    /// </summary>
    [DataContract]
    public class BettingForChasingInfo
    {
        /// <summary>
        /// 期数
        /// </summary>
        [DataMember]
        public string Phases { get; set; }

        /// <summary>
        /// 倍数指数
        /// </summary>
        [DataMember]
        public double Exponent { get; set; }
    }
}

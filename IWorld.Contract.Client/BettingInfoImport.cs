using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 用于封装投注信息的数据集
    /// </summary>
    [DataContract]
    public class BettingInfoImport
    {
        /// <summary>
        /// 期数
        /// </summary>
        [DataMember]
        public string Phases { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        [DataMember]
        public double Multiple { get; set; }

        /// <summary>
        /// 用于转换为赔率的点数
        /// </summary>
        [DataMember]
        public double Points { get; set; }

        /// <summary>
        /// 玩法
        /// </summary>
        [DataMember]
        public int HowToPlayId { get; set; }
    }
}

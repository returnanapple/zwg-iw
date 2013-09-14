using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 用于投注的数据集
    /// </summary>
    [DataContract]
    public class BettingImport
    {
        /// <summary>
        /// 投注信息
        /// </summary>
        [DataMember]
        public BettingInfoImport BettingInfo { get; set; }

        /// <summary>
        /// 追号信息
        /// </summary>
        [DataMember]
        public ChasingInfoImport ChasingInfo { get; set; }

        /// <summary>
        /// 投注号码
        /// </summary>
        [DataMember]
        public List<BettingValuesImport> Values { get; set; }

        /// <summary>
        /// 临时信息的标识码
        /// </summary>
        [DataMember]
        public List<string> Codes { get; set; }
    }
}

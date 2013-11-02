using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 投注信息
    /// </summary>
    [DataContract]
    public class BettingOfJawImport
    {
        /// <summary>
        /// 期号
        /// </summary>
        [DataMember]
        public string Issue { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        [DataMember]
        public List<BettingDetailOfJawImport> Details { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 用于封装投注号码的数据集
    /// </summary>
    [DataContract]
    public class BettingValuesImport
    {
        /// <summary>
        /// 位
        /// </summary>
        [DataMember]
        public string Seat { get; set; }

        /// <summary>
        /// 所选号码
        /// </summary>
        [DataMember]
        public List<string> Values { get; set; }
    }
}

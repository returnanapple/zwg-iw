using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 大白鲨游戏的投注明细
    /// </summary>
    [DataContract]
    public class BettingDetailOfJawImport
    {
        /// <summary>
        /// 图标
        /// </summary>
        [DataMember]
        public IconOfJaw Icon { get; set; }

        /// <summary>
        /// 投注金额
        /// </summary>
        [DataMember]
        public double Sum { get; set; }
    }
}

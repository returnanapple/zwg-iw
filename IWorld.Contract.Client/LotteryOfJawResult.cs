using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IWorld.Model;
using System.Runtime.Serialization;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 大白鲨游戏的开奖记录
    /// </summary>
    [DataContract]
    public class LotteryOfJawResult
    {
        /// <summary>
        /// 期号
        /// </summary>
        [DataMember]
        public string Issue { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [DataMember]
        public IconOfJaw Icon { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 开奖历史
    /// </summary>
    [DataContract]
    public class HistoryOfLotteryResult
    {
        #region 公开属性

        /// <summary>
        /// 期号
        /// </summary>
        [DataMember]
        public string Phases { get; set; }

        /// <summary>
        /// 中奖号码
        /// </summary>
        [DataMember]
        public List<string> Values { get; set; }
        
        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的开奖历史
        /// </summary>
        /// <param name="lottery">开奖历史的数据封装</param>
        public HistoryOfLotteryResult(Lottery lottery)
        {
            this.Phases = lottery.Phases;
            this.Values = lottery.Seats.OrderBy(x => x.Order).ToList().ConvertAll(x => x.Value);
        }

        #endregion
    }
}

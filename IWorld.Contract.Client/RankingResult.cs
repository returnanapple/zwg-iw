using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 中奖排行信息
    /// </summary>
    [DataContract]
    public class RankingResult
    {
        #region 公开属性

        /// <summary>
        /// 存储指针
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        [DataMember]
        public double Bonus { get; set; }

        /// <summary>
        /// 一个布尔值 标识该排行信息是否真实
        /// </summary>
        [DataMember]
        public bool IsRealBetting { get; set; }
        
        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的中奖排行信息
        /// </summary>
        /// <param name="betting">投注记录的数据封装</param>
        public RankingResult(Betting betting)
        {
            this.Id = betting.Id;
            this.Bonus = betting.Bonus;
            this.IsRealBetting = true;
        }

        /// <summary>
        /// 实例化一个新的中奖排行信息
        /// </summary>
        /// <param name="betting">投注记录的数据封装</param>
        public RankingResult(VirtualTop virtualTop)
        {
            this.Id = virtualTop.Id;
            this.Bonus = virtualTop.Sum;
            this.IsRealBetting = false;
        }
        
        #endregion
    }
}

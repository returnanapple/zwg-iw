using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 玩法信息
    /// </summary>
    [DataContract]
    public class HowToPlayResult
    {
        #region 公开属性

        /// <summary>
        /// 玩法的数据库存储指针
        /// </summary>
        [DataMember]
        public int HowToPlayId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// 规则
        /// </summary>
        [DataMember]
        public string Rule { get; set; }

        /// <summary>
        /// 赔率
        /// </summary>
        [DataMember]
        public double Odds { get; set; }

        /// <summary>
        /// 所采用的返奖接口
        /// </summary>
        [DataMember]
        public LotteryInterface Interface { get; set; }

        /// <summary>
        /// 一个布尔值 标记是否叠位
        /// </summary>
        [DataMember]
        public bool IsStackedBit { get; set; }

        /// <summary>
        /// 可选位
        /// </summary>
        [DataMember]
        public List<OptionalSeatResult> Seats { get; set; }

        /// <summary>
        /// 一个布尔值 标识该玩法是否单式玩法
        /// </summary>
        [DataMember]
        public bool IsSingle { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的玩法信息
        /// </summary>
        /// <param name="play">玩法信息的数据封装</param>
        public HowToPlayResult(HowToPlay play)
        {
            this.HowToPlayId = play.Id;
            this.Name = play.Name;
            this.Description = play.Description;
            this.Rule = play.Rule;
            this.Odds = play.Odds;
            this.Interface = play.Interface;
            this.IsStackedBit = play.IsStackedBit;
            this.Seats = play.Seats.ConvertAll(x => new OptionalSeatResult(x));
            this.IsSingle = play.Parameter3 == 0;
        }

        #endregion
    }
}

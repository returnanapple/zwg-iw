using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 奖品信息
    /// </summary>
    [DataContract]
    public class PrizesResult
    {
        #region 公开属性

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
        /// 数额
        /// </summary>
        [DataMember]
        public int Sum { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DataMember]
        public PrizeType Type { get; set; }

        /// <summary>
        /// 价值
        /// </summary>
        [DataMember]
        public double Price { get; set; }

        /// <summary>
        /// 备注（一般为实物奖品的演示链接）
        /// </summary>
        [DataMember]
        public string Remark { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的奖品信息
        /// </summary>
        /// <param name="prize">奖品信息的数据封装</param>
        public PrizesResult(Prize prize)
        {
            this.Name = prize.Name;
            this.Description = prize.Description;
            this.Sum = prize.Sum;
            this.Type = prize.Type;
            this.Price = prize.Price;
            this.Remark = prize.Remark;
        }

        #endregion
    }
}

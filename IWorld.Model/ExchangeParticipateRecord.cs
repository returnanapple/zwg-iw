using System.Collections.Generic;

namespace IWorld.Model
{
    /// <summary>
    /// 兑换活动的参与记录
    /// </summary>
    public class ExchangeParticipateRecord : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 参与人
        /// </summary>
        public virtual Author Owner { get; set; }

        /// <summary>
        /// 参与的活动
        /// </summary>
        public virtual Exchange Exchange { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Sum { get; set; }

        /// <summary>
        /// 实体奖品赠送记录
        /// </summary>
        public virtual List<GiftRecord> Gifts { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的兑换活动的参与记录
        /// </summary>
        public ExchangeParticipateRecord()
        {
        }

        /// <summary>
        /// 实例化一个新的兑换活动的参与记录
        /// </summary>
        /// <param name="owner">参与人</param>
        /// <param name="exchange">参与的活动</param>
        /// <param name="sum">数量</param>
        /// <param name="gifts">实体奖品赠送记录</param>
        public ExchangeParticipateRecord(Author owner, Exchange exchange, int sum, List<GiftRecord> gifts)
        {
            this.Owner = owner;
            this.Exchange = exchange;
            this.Sum = sum;
            this.Gifts = gifts;
        }
        
        #endregion
    }
}

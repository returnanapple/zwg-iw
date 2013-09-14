
namespace IWorld.Model
{
    /// <summary>
    /// 投注记录（追号）
    /// </summary>
    public class BettingForCgasing : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 期数
        /// </summary>
        public string Phases { get; set; }

        /// <summary>
        /// 翻倍指数
        /// </summary>
        public double Exponent { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public BettingStatus Status { get; set; }

        /// <summary>
        /// 投注金额
        /// </summary>
        public double Pay { get; set; }

        /// <summary>
        /// 中奖金额（未中奖时候为0）
        /// </summary>
        public double Bonus { get; set; }

        /// <summary>
        /// 所从属的追号记录
        /// </summary>
        public virtual Chasing Chasing { get; set; }
        
        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的投注记录（追号）
        /// </summary>
        public BettingForCgasing()
        {
        }

        /// <summary>
        /// 实例化一个新的投注记录（追号）
        /// </summary>
        /// <param name="phases">期数</param>
        /// <param name="exponent">翻倍指数</param>
        /// <param name="pay">投注金额</param>
        public BettingForCgasing(string phases, double exponent, double pay)
        {
            this.Phases = phases;
            this.Exponent = exponent;
            this.Pay = pay;
            this.Status = BettingStatus.等待开奖;
            this.Bonus = 0;
        }
        
        #endregion
    }
}

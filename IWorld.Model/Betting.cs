using System.Collections.Generic;

namespace IWorld.Model
{
    /// <summary>
    /// 投注记录
    /// </summary>
    public class Betting : ModelBase, IConsumption
    {
        #region 公开属性

        /// <summary>
        /// 投注人
        /// </summary>
        public virtual Author Owner { get; set; }

        /// <summary>
        /// 期数
        /// </summary>
        public string Phases { get; set; }

        /// <summary>
        /// 注数
        /// </summary>
        public int Sum { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public double Multiple { get; set; }

        /// <summary>
        /// 用于转换为赔率的点数
        /// </summary>
        public double Points { get; set; }

        /// <summary>
        /// 玩法
        /// </summary>
        public virtual HowToPlay HowToPlay { get; set; }

        /// <summary>
        /// 位
        /// </summary>
        public virtual List<BettingSeat> Seats { get; set; }

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

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的投注记录
        /// </summary>
        public Betting()
        {
        }

        /// <summary>
        /// 实例化一个新的投注记录
        /// </summary>
        /// <param name="owner">投注人</param>
        /// <param name="phases">期数</param>
        /// <param name="sum">注数</param>
        /// <param name="multiple">倍数</param>
        /// <param name="points">用于转换为赔率的点数</param>
        /// <param name="howToPlay">玩法</param>
        /// <param name="seats">位</param>
        /// <param name="pay">投注金额</param>
        public Betting(Author owner, string phases, int sum, double multiple, double points, HowToPlay howToPlay, List<BettingSeat> seats
            , double pay)
        {
            this.Owner = owner;
            this.Phases = phases;
            this.Sum = sum;
            this.Multiple = multiple;
            this.Points = points;
            this.HowToPlay = howToPlay;
            this.Seats = seats;
            this.Status = BettingStatus.等待开奖;
            this.Pay = pay;
            this.Bonus = 0;
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 获取消费量
        /// </summary>
        /// <returns>返回消费量</returns>
        public double GetConsumption()
        {
            return this.Pay;
        }
        
        #endregion
    }
}

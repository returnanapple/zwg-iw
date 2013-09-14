using System.Collections.Generic;

namespace IWorld.Model
{
    /// <summary>
    /// 追号记录
    /// </summary>
    public class Chasing : ModelBase, IConsumption
    {
        #region 公开属性

        /// <summary>
        /// 投注人
        /// </summary>
        public virtual Author Owner { get; set; }

        /// <summary>
        /// 顺延期数
        /// </summary>
        public int Postpone { get; set; }

        /// <summary>
        /// 持续期数
        /// </summary>
        public int Continuance { get; set; }

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
        public virtual List<ChasingSeat> Seats { get; set; }

        /// <summary>
        /// 投注记录
        /// </summary>
        public virtual List<BettingForCgasing> Bettings { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public ChasingStatus Status { get; set; }

        /// <summary>
        /// 投注总金额
        /// </summary>
        public double Pay { get; set; }

        /// <summary>
        /// 中奖金额（未中奖时候为0）
        /// </summary>
        public double Bonus { get; set; }

        /// <summary>
        /// 标识|如果在开始追号之前就开出号码 追号中止
        /// </summary>
        public bool EndIfLotteryBeforeBegin { get; set; }

        /// <summary>
        /// 标识|如果在开始追号过程中中奖 追号中止
        /// </summary>
        public bool EndIfLotteryAtGoing { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的追号记录
        /// </summary>
        public Chasing()
        {
        }

        /// <summary>
        /// 实例化一个新的追号记录
        /// </summary>
        /// <param name="owner">投注人</param>
        /// <param name="postpone">顺延期数</param>
        /// <param name="continuance">持续期数</param>
        /// <param name="sum">注数</param>
        /// <param name="multiple">倍数</param>
        /// <param name="points">用于转换为赔率的点数</param>
        /// <param name="howToPlay">玩法</param>
        /// <param name="seats">位</param>
        /// <param name="bettings">投注记录</param>
        /// <param name="pay">投注总金额</param>
        /// <param name="endIfLotteryBeforeBegin">标识|如果在开始追号之前就开出号码 追号中止</param>
        /// <param name="endIfLotteryAtGoing">标识|如果在开始追号过程中中奖 追号中止</param>
        public Chasing(Author owner, int postpone, int continuance, int sum, double multiple, double points, HowToPlay howToPlay
            , List<ChasingSeat> seats, List<BettingForCgasing> bettings, double pay, bool endIfLotteryBeforeBegin
            , bool endIfLotteryAtGoing)
        {
            this.Owner = owner;
            this.Postpone = Postpone;
            this.Continuance = continuance;
            this.Sum = sum;
            this.Multiple = multiple;
            this.Points = points;
            this.HowToPlay = howToPlay;
            this.Seats = seats;
            this.Bettings = bettings;
            this.Status = ChasingStatus.未开始;
            this.Pay = pay;
            this.Bonus = 0;
            this.EndIfLotteryBeforeBegin = endIfLotteryBeforeBegin;
            this.EndIfLotteryAtGoing = endIfLotteryAtGoing;
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

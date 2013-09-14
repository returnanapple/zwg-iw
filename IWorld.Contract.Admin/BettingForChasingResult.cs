using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 投注（追号）信息
    /// </summary>
    [DataContract]
    public class BettingForChasingResult
    {
        /// <summary>
        /// 投注记录的存储指针
        /// </summary>
        [DataMember]
        public int BettingId { get; set; }

        /// <summary>
        /// 期数
        /// </summary>
        [DataMember]
        public string Phases { get; set; }

        /// <summary>
        /// 翻倍指数
        /// </summary>
        [DataMember]
        public double Exponent { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        [DataMember]
        public BettingStatus Status { get; set; }

        /// <summary>
        /// 投注金额
        /// </summary>
        [DataMember]
        public double Pay { get; set; }

        /// <summary>
        /// 中奖金额（未中奖时候为0）
        /// </summary>
        [DataMember]
        public double Bonus { get; set; }

        /// <summary>
        /// 所从属的追号记录的存储指针
        /// </summary>
        [DataMember]
        public int ChasingId { get; set; }

        /// <summary>
        /// 实例化一个投注（追号）信息
        /// </summary>
        /// <param name="betting">投注（追号）信息的数据封装</param>
        public BettingForChasingResult(BettingForCgasing betting)
        {
            this.BettingId = betting.Id;
            this.Phases = betting.Phases;
            this.Exponent = betting.Exponent;
            this.Status = betting.Status;
            this.Pay = betting.Pay;
            this.Bonus = betting.Bonus;
            this.ChasingId = betting.Chasing.Id;
        }
    }
}

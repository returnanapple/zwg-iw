using System.Linq;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 投注信息
    /// </summary>
    [DataContract]
    public class BettingResult
    {
        /// <summary>
        /// 投注记录的存储指针
        /// </summary>
        [DataMember]
        public int BettingId { get; set; }

        /// <summary>
        /// 投注人的存储指针
        /// </summary>
        [DataMember]
        public int OwnerId { get; set; }

        /// <summary>
        /// 投注人
        /// </summary>
        [DataMember]
        public string Owner { get; set; }

        /// <summary>
        /// 期数
        /// </summary>
        [DataMember]
        public string Phases { get; set; }

        /// <summary>
        /// 注数
        /// </summary>
        [DataMember]
        public int Sum { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        [DataMember]
        public double Multiple { get; set; }

        /// <summary>
        /// 用于转换为赔率的点数
        /// </summary>
        [DataMember]
        public double Points { get; set; }

        /// <summary>
        /// 所属彩票的存储指针
        /// </summary>
        [DataMember]
        public int TicketId { get; set; }

        /// <summary>
        /// 所属彩票
        /// </summary>
        [DataMember]
        public string Ticket { get; set; }

        /// <summary>
        /// 所属彩票标签的存储指针
        /// </summary>
        [DataMember]
        public int TagId { get; set; }

        /// <summary>
        /// 所属彩票标签
        /// </summary>
        [DataMember]
        public string Tag { get; set; }

        /// <summary>
        /// 所属玩法的存储指针
        /// </summary>
        [DataMember]
        public int HowToPlayId { get; set; }

        /// <summary>
        /// 所属玩法
        /// </summary>
        [DataMember]
        public string HowToPlay { get; set; }

        /// <summary>
        /// 位
        /// </summary>
        [DataMember]
        public string Seats { get; set; }

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
        /// 中奖金额
        /// </summary>
        [DataMember]
        public double Bonus { get; set; }

        /// <summary>
        /// 实例化一个新的投注信息
        /// </summary>
        /// <param name="betting">投注信息的数据封装</param>
        public BettingResult(Betting betting)
        {
            this.BettingId = betting.Id;
            this.Owner = betting.Owner.Username;
            this.OwnerId = betting.Owner.Id;
            this.Phases = betting.Phases;
            this.Sum = betting.Sum;
            this.Multiple = betting.Multiple;
            this.Points = betting.Points;
            this.Ticket = betting.HowToPlay.Tag.Ticket.Name;
            this.TicketId = betting.HowToPlay.Tag.Ticket.Id;
            this.Tag = betting.HowToPlay.Tag.Name;
            this.TagId = betting.HowToPlay.Tag.Id;
            this.HowToPlay = betting.HowToPlay.Name;
            this.HowToPlayId = betting.HowToPlay.Id;
            this.Seats = string.Join(",", betting.Seats.OrderBy(x => x.Order).ToList()
                .ConvertAll(x => string.Join("", x.ValueList)));
            this.Status = betting.Status;
            this.Pay = betting.Pay;
            this.Bonus = betting.Bonus;
        }
    }
}

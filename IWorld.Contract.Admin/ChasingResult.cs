using System.Linq;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 追号信息
    /// </summary>
    [DataContract]
    public class ChasingResult
    {
        /// <summary>
        /// 追号记录的存储指针
        /// </summary>
        [DataMember]
        public int ChasingId { get; set; }

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
        /// 顺延期数
        /// </summary>
        [DataMember]
        public int Postpone { get; set; }

        /// <summary>
        /// 持续期数
        /// </summary>
        [DataMember]
        public int Continuance { get; set; }

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
        public ChasingStatus Status { get; set; }

        /// <summary>
        /// 投注总金额
        /// </summary>
        [DataMember]
        public double Pay { get; set; }

        /// <summary>
        /// 中奖金额（未中奖时候为0）
        /// </summary>
        [DataMember]
        public double Bonus { get; set; }

        /// <summary>
        /// 标识|如果在开始追号之前就开出号码 追号中止
        /// </summary>
        [DataMember]
        public bool EndIfLotteryBeforeBegin { get; set; }

        /// <summary>
        /// 标识|如果在开始追号过程中中奖 追号中止
        /// </summary>
        [DataMember]
        public bool EndIfLotteryAtGoing { get; set; }

        /// <summary>
        /// 实例化一个新的追号信息
        /// </summary>
        /// <param name="chasing">追号信息的数据封装</param>
        public ChasingResult(Chasing chasing)
        {
            this.ChasingId = chasing.Id;
            this.Owner = chasing.Owner.Username;
            this.OwnerId = chasing.Owner.Id;
            this.Postpone = chasing.Postpone;
            this.Continuance = chasing.Continuance;
            this.Sum = chasing.Sum;
            this.Multiple = chasing.Multiple;
            this.Points = chasing.Points;
            this.Ticket = chasing.HowToPlay.Tag.Ticket.Name;
            this.TicketId = chasing.HowToPlay.Tag.Ticket.Id;
            this.Tag = chasing.HowToPlay.Tag.Name;
            this.TagId = chasing.HowToPlay.Tag.Id;
            this.HowToPlay = chasing.HowToPlay.Name;
            this.HowToPlayId = chasing.HowToPlay.Id;
            this.Seats = string.Join(",", chasing.Seats.OrderBy(x => x.Order).ToList()
                .ConvertAll(x => string.Join("", x.ValuesList)));
            this.Status = chasing.Status;
            this.Pay = chasing.Pay;
            this.Bonus = chasing.Bonus;
            this.EndIfLotteryBeforeBegin = chasing.EndIfLotteryBeforeBegin;
            this.EndIfLotteryAtGoing = chasing.EndIfLotteryAtGoing;
        }
    }
}

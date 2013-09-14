using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 玩法信息
    /// </summary>
    [DataContract]
    public class HowToPlayResult
    {
        /// <summary>
        /// 玩法的存储指针
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
        /// 所属彩票的存储指针
        /// </summary>
        [DataMember]
        public int TicketId { get; set; }

        /// <summary>
        /// 所属彩票的名称
        /// </summary>
        [DataMember]
        public string TicketName { get; set; }

        /// <summary>
        /// 玩法标签的存储指针
        /// </summary>
        [DataMember]
        public int TagId { get; set; }

        /// <summary>
        /// 玩法标签的名称
        /// </summary>
        [DataMember]
        public string TagName { get; set; }

        /// <summary>
        /// 赔率
        /// </summary>
        [DataMember]
        public double Odds { get; set; }

        /// <summary>
        /// 赔率/返点数转化率（如为0则采用系统参数）
        /// </summary>
        [DataMember]
        public double ConversionRates { get; set; }

        /// <summary>
        /// 返奖基数（如为0则采用系统参数）
        /// </summary>
        [DataMember]
        public double CardinalNumber { get; set; }

        /// <summary>
        /// 排序系数
        /// </summary>
        [DataMember]
        public int Order { get; set; }

        /// <summary>
        /// 一个布尔值 标识玩法是否被隐藏
        /// </summary>
        [DataMember]
        public bool Hide { get; set; }

        /// <summary>
        /// 实例化一个新的玩法信息
        /// </summary>
        /// <param name="howToPlay">玩法信息的数据封装</param>
        public HowToPlayResult(HowToPlay howToPlay)
        {
            this.HowToPlayId = howToPlay.Id;
            this.Name = howToPlay.Name;
            this.Description = howToPlay.Description;
            this.Rule = howToPlay.Rule;
            this.TicketId = howToPlay.Tag.Ticket.Id;
            this.TicketName = howToPlay.Tag.Ticket.Name;
            this.TagId = howToPlay.Tag.Id;
            this.TagName = howToPlay.Tag.Name;
            this.Odds = howToPlay.Odds;
            this.ConversionRates = howToPlay.ConversionRates;
            this.Order = howToPlay.Order;
            this.Hide = howToPlay.Hide;
        }
    }
}

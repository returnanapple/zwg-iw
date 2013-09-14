using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 玩法标签信息
    /// </summary>
    [DataContract]
    public class PlayTagResult
    {
        /// <summary>
        /// 玩法标签的名称
        /// </summary>
        [DataMember]
        public int PlayTagId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

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
        /// 排序系数
        /// </summary>
        [DataMember]
        public int Order { get; set; }

        /// <summary>
        /// 下辖玩法数量
        /// </summary>
        [DataMember]
        public int CountOfHowToPlay { get; set; }

        /// <summary>
        /// 一个布尔值 表示玩法标签是否被隐藏
        /// </summary>
        [DataMember]
        public bool Hide { get; set; }

        /// <summary>
        /// 实例化一个新的玩法标签信息
        /// </summary>
        /// <param name="tag">玩法标签信息的数据封装</param>
        /// <param name="countOfHowToPlay">下辖玩法数量</param>
        public PlayTagResult(PlayTag tag, int countOfHowToPlay)
        {
            this.PlayTagId = tag.Id;
            this.Name = tag.Name;
            this.TicketId = tag.Ticket.Id;
            this.TicketName = tag.Ticket.Name;
            this.Order = tag.Order;
            this.CountOfHowToPlay = countOfHowToPlay;
            this.Hide = tag.Hide;
        }
    }
}

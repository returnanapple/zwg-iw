using System.Collections.Generic;

namespace IWorld.Model
{
    /// <summary>
    /// 玩法标签
    /// </summary>
    public class PlayTag : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属彩票
        /// </summary>
        public virtual LotteryTicket Ticket { get; set; }

        /// <summary>
        /// 下辖玩法
        /// </summary>
        public virtual List<HowToPlay> HowToPlays { get; set; }

        /// <summary>
        /// 排序系数
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 标识 | 不在前台显示
        /// </summary>
        public bool Hide { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的玩法标签
        /// </summary>
        public PlayTag()
        {
        }

        /// <summary>
        /// 实例化一个新的玩法标签
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="ticket">所属彩票</param>
        /// <param name="order">排序系数</param>
        public PlayTag(string name, LotteryTicket ticket, int order)
        {
            this.Name = name;
            this.Ticket = ticket;
            this.HowToPlays = new List<HowToPlay>();
            this.Order = order;
            this.Hide = false;

            ticket.Tags.Add(this);
        }

        #endregion
    }
}

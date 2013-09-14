
namespace IWorld.Model
{
    /// <summary>
    /// 站内消息
    /// </summary>
    public class Message : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 发件人
        /// </summary>
        public virtual Author From { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public virtual Author To { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// 已经阅读的人的名单（[\uid][\uid]）
        /// </summary>
        public string Readed { get; set; }

        /// <summary>
        /// 已经删除的人的名单（[\uid][\uid]）
        /// </summary>
        public string Deleted { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的站内消息
        /// </summary>
        public Message()
        {
        }

        /// <summary>
        /// 实例化一个新的站内消息
        /// </summary>
        /// <param name="_from">发件人</param>
        /// <param name="_to">收件人</param>
        /// <param name="title">标题</param>
        /// <param name="context">正文</param>
        public Message(Author _from, Author _to, string title, string context)
        {
            this.From = _from;
            this.To = _to;
            this.Title = title;
            this.Context = context;
            this.Readed = "";
            this.Deleted = "";
        }

        #endregion
    }
}

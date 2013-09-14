
namespace IWorld.Model
{
    /// <summary>
    /// 通知
    /// </summary>
    public class Notice : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 接收人
        /// </summary>
        public virtual Author To { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// 一个布尔值 标识该通知是否已经被阅读
        /// </summary>
        public bool IsReaded { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        public NoticeType Type { get; set; }

        /// <summary>
        /// 目标对象的存储指针
        /// </summary>
        public int TargetId { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的通知
        /// </summary>
        public Notice()
        {
        }

        /// <summary>
        /// 实例化一个新的通知
        /// </summary>
        /// <param name="_to">接收人</param>
        /// <param name="context">正文</param>
        /// <param name="type">通知类型</param>
        /// <param name="targetId">目标对象的存储指针</param>
        public Notice(Author _to, string context, NoticeType type, int targetId)
        {
            this.To = _to;
            this.Context = context;
            this.IsReaded = false;
            this.Type = type;
            this.TargetId = targetId;
        }

        #endregion
    }
}

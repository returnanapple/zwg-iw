using System;

namespace IWorld.Model
{
    /// <summary>
    /// 公告
    /// </summary>
    public class Bulletin : RegularlyBase
    {
        #region 公开属性

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string Context { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的公告
        /// </summary>
        public Bulletin()
        {
        }

        /// <summary>
        /// 实例化一个新的公告
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="context">正文</param>
        /// <param name="beginTime">开始公示时间</param>
        /// <param name="days">持续天数</param>
        /// <param name="autoDelete">过期自动删除</param>
        public Bulletin(string title, string context, DateTime beginTime, int days, bool autoDelete)
            : base(beginTime, days, autoDelete)
        {
            this.Title = title;
            this.Context = context;
        }

        #endregion
    }
}

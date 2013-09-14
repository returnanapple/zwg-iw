using System;

namespace IWorld.Model
{
    /// <summary>
    /// 用于继承的定期活动模型的基础类
    /// </summary>
    public abstract class RegularlyBase : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 持续天数
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 暂停显示
        /// </summary>
        public bool Hide { get; set; }

        /// <summary>
        /// 过期自动删除
        /// </summary>
        public bool AutoDelete { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的定期活动模型
        /// </summary>
        public RegularlyBase()
        {
        }

        /// <summary>
        /// 实例化一个新的定期活动模型
        /// </summary>
        /// <param name="beginTime">开始公示时间</param>
        /// <param name="days">持续天数</param>
        /// <param name="autoDelete">过期自动删除</param>
        public RegularlyBase(DateTime beginTime, int days, bool autoDelete)
        {
            this.BeginTime = beginTime;
            this.Days = days;
            this.EndTime = beginTime.AddDays(days);
            this.Hide = false;
            this.AutoDelete = autoDelete;
        }

        #endregion
    }
}

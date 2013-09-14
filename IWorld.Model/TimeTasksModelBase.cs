using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 有时限的任务类数据模型
    /// </summary>
    public class TimeTasksModelBase : SolidModel
    {
        #region 属性

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

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

        /// <summary>
        /// 状态
        /// </summary>
        public TimeTasksStatus Status
        {
            get
            {
                DateTime now = DateTime.Now;

                if (this.BeginTime > now)
                {
                    return TimeTasksStatus.未开始;
                }
                else if (this.EndTime < now)
                {
                    return TimeTasksStatus.已过期;
                }
                else if (this.Hide == true)
                {
                    return TimeTasksStatus.暂停;
                }
                else
                {
                    return TimeTasksStatus.正常;
                }
            }
        }

        /// <summary>
        /// 持续天数
        /// </summary>
        public int Days
        {
            get { return (this.EndTime - this.BeginTime).Days; }
        }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的有时限的任务类数据模型
        /// </summary>
        public TimeTasksModelBase()
        {
        }

        /// <summary>
        /// 实例化一个新的有时限的任务类数据模型
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="hide">暂停</param>
        /// <param name="autoDelete">过期自动删除</param>
        public TimeTasksModelBase(DateTime beginTime, DateTime endTime, bool hide, bool autoDelete)
        {
            this.BeginTime = beginTime;
            this.EndTime = endTime;
            this.Hide = hide;
            this.AutoDelete = autoDelete;
        }

        #endregion
    }
}

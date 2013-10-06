using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 定期任务相关的数据模型的基类
    /// </summary>
    public class RegularModelBase : RecordingTimeModelBase
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
        /// 持续天数
        /// </summary>
        public int Days { get { return (EndTime - BeginTime).Days; } }

        /// <summary>
        /// 一个布尔值 标识活动是否暂停
        /// </summary>
        public bool Hide { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public RegularStatus Status
        {
            get { return this.GetStatus(); }
        }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的定期任务相关的数据模型
        /// </summary>
        public RegularModelBase()
        {
        }

        /// <summary>
        /// 实例化一个新的定期任务相关的数据模型
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="hide">一个布尔值 标识活动是否暂停（默认为 false）</param>
        public RegularModelBase(DateTime beginTime, DateTime endTime, bool hide = false)
        {
            if (beginTime >= endTime)
            {
                throw new Exception("定期活动的开始时候不小于结束时间 请检查输入");
            }
            this.BeginTime = beginTime;
            this.EndTime = endTime;
            this.Hide = hide;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取定时活动的当前状态
        /// </summary>
        /// <returns></returns>
        private RegularStatus GetStatus()
        {
            DateTime now = DateTime.Now;
            if (this.BeginTime > now)
            {
                return RegularStatus.未开始;
            }
            else if (this.EndTime < now)
            {
                return RegularStatus.已过期;
            }
            else if (this.Hide == true)
            {
                return RegularStatus.暂停;
            }

            return RegularStatus.正常;
        }

        #endregion
    }
}

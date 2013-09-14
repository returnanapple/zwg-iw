using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Admin.Framework
{
    /// <summary>
    /// 监听对象信息
    /// </summary>
    public class MonitorCondition
    {
        /// <summary>
        /// 所要监听的动作
        /// </summary>
        public ComAction ComAction { get; set; }

        /// <summary>
        /// 所要监听的状态
        /// </summary>
        public object Status { get; set; }

        /// <summary>
        /// 实例化一个新的监听对象信息
        /// </summary>
        /// <param name="comAction">所要监听的动作</param>
        /// <param name="status">所要监听的状态</param>
        public MonitorCondition(ComAction comAction, object status)
        {
            this.ComAction = comAction;
            this.Status = status;
        }
    }
}

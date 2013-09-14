using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Admin.Framework
{
    /// <summary>
    /// 标记监听动作
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MonitorAttribute : Attribute
    {
        /// <summary>
        /// 监听对象
        /// </summary>
        public virtual Type Sender { get; protected set; }

        /// <summary>
        /// 监听状态
        /// </summary>
        public object Status { get; protected set; }

        /// <summary>
        /// 标记监听动作
        /// </summary>
        /// <param name="sender">监听对象</param>
        /// <param name="status">监听状态</param>
        public MonitorAttribute(Type sender = null, object status = null)
        {
        }
    }
}

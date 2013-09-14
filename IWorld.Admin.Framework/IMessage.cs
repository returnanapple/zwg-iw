using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Admin.Framework
{
    /// <summary>
    /// 定义信息
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// 动作
        /// </summary>
        ComAction ComAction { get; }

        /// <summary>
        /// 主体
        /// </summary>
        object Content { get; }

        /// <summary>
        /// 检查该消息是否符合指定的监视条件
        /// </summary>
        /// <param name="condition">监视条件</param>
        /// <returns>返回一个布尔值 标识该消息是否符合指定的监视条件</returns>
        bool Licit(MonitorCondition condition);

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="newMessage">所要附加于消息的主体部分的新内容</param>
        void Handle(object newContent = null);

        /// <summary>
        /// 设置消息的命令状态
        /// </summary>
        /// <param name="newStatus">新状态</param>
        /// <param name="newMessage">所要附加于消息的主体部分的新内容</param>
        void SetStatus(object newStatus, object newContent = null);
    }

    /// <summary>
    /// 定义信息
    /// </summary>
    public interface IMessage<T> : IMessage
    {
        /// <summary>
        /// 状态
        /// </summary>
        T Status { get; }
    }
}

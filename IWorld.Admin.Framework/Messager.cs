using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Admin.Framework
{
    /// <summary>
    /// 信使
    /// </summary>
    public class Messager
    {
        #region 默认对象

        static Messager _Default = new Messager();

        /// <summary>
        /// 默认对象
        /// </summary>
        public static Messager Default
        {
            get
            {
                return _Default;
            }
        }

        #endregion

        #region 私有变量

        /// <summary>
        /// 监听者
        /// </summary>
        Dictionary<List<MonitorCondition>, RecipientDelegate> _registers
            = new Dictionary<List<MonitorCondition>, RecipientDelegate>();

        /// <summary>
        /// 临时监听者
        /// </summary>
        Dictionary<List<MonitorCondition>, RecipientDelegate> _temporarilyRegisters
            = new Dictionary<List<MonitorCondition>, RecipientDelegate>();

        #endregion

        #region 实例方法

        /// <summary>
        /// 注册监听信息
        /// </summary>
        /// <param name="conditions">监听条件</param>
        /// <param name="action">对应的动作</param>
        /// <param name="temporarily">一个布尔值 标识是否临时监听</param>
        public void RegisterRecipients(List<MonitorCondition> conditions, RecipientDelegate action, bool temporarily = false)
        {
            var registers = temporarily ? _temporarilyRegisters : _registers;
            registers.Add(conditions, action);
        }

        /// <summary>
        /// 清空临时监听者
        /// </summary>
        public void ClearTemporarilyRegisters()
        {
            _temporarilyRegisters.Clear();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public void Send(IMessage message)
        {

            _registers.Where(x => x.Key.Any(t => message.Licit(t)))
                .ToList().ForEach(x =>
                    {
                        x.Value(message);
                    });

            _temporarilyRegisters.Where(x => x.Key.Any(t => message.Licit(t)))
                .ToList().ForEach(x =>
                    {
                        x.Value(message);
                    });
        }

        /// <summary>
        /// 创建一个新的默认格式的消息
        /// </summary>
        /// <param name="action">动作</param>
        /// <param name="content">信息主体</param>
        /// <returns>返回一个新的默认格式的消息</returns>
        public IMessage CreateMessage<T>(ComAction action, object content = null)
        {
            return new Message<T>(action, content);
        }

        #endregion

        #region 内嵌类型

        /// <summary>
        /// 默认信息封装
        /// </summary>
        class Message<T> : IMessage, IMessage<T>
        {
            /// <summary>
            /// 状态标识
            /// </summary>
            int statusValue = 0;

            /// <summary>
            /// 动作
            /// </summary>
            public ComAction ComAction { get; protected set; }

            /// <summary>
            /// 主体
            /// </summary>
            public object Content { get; protected set; }

            /// <summary>
            /// 状态
            /// </summary>
            public T Status
            {
                get
                {
                    return (T)Enum.ToObject(typeof(T), statusValue);
                }
            }

            /// <summary>
            /// 实例化一个新的默认信息封装
            /// </summary>
            /// <param name="action">动作</param>
            /// <param name="content">主体</param>
            public Message(ComAction action, object content = null)
            {
                this.ComAction = action;
                this.Content = content;
            }

            /// <summary>
            /// 检查该消息是否符合指定的监视条件
            /// </summary>
            /// <param name="condition">监视条件</param>
            /// <returns>返回一个布尔值 标识该消息是否符合指定的监视条件</returns>
            public bool Licit(MonitorCondition condition)
            {
                return this.ComAction == condition.ComAction
                    && this.Status.Equals(condition.Status);
            }

            /// <summary>
            /// 处理
            /// </summary>
            /// <param name="newMessage">所要附加于消息的主体部分的新内容</param>
            public void Handle(object newContent = null)
            {
                this.Content = newContent;
                this.statusValue += 1;
            }

            /// <summary>
            /// 设置消息的命令状态
            /// </summary>
            /// <param name="newStatus">新状态</param>
            /// <param name="newMessage">所要附加于消息的主体部分的新内容</param>
            public void SetStatus(object newStatus, object newContent = null)
            {
                this.statusValue = (int)newStatus;
                this.Content = newContent;
            }
        }

        #endregion
    }
}

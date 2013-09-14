using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 站内短消息信息
    /// </summary>
    [DataContract]
    public class MessageResult
    {
        #region 公开属性

        /// <summary>
        /// 站内短消息的数据库存储指针
        /// </summary>
        [DataMember]
        public int MessageId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        /// <summary>
        /// 一个布尔值 标识是否已经阅读
        /// </summary>
        [DataMember]
        public bool Readed { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [DataMember]
        public DateTime Time { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的站内短消息信息
        /// </summary>
        /// <param name="message">站内短消息信息的数据封装</param>
        public MessageResult(Message message)
        {
            this.MessageId = message.Id;
            this.Title = message.Title;
            this.Context = message.Context;
            this.Readed = message.Readed.Contains(string.Format("[{0}]", message.To.Id));
        }

        #endregion
    }
}

using System;
using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 采集结果
    /// </summary>
    [DataContract]
    public class CollectionResult
    {
        /// <summary>
        /// 键
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [DataMember]
        public DateTime Time { get; set; }

        /// <summary>
        /// 实例化一个新的采集结果
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="message">信息</param>
        /// <param name="time">时间</param>
        public CollectionResult(string key, string message, DateTime time)
        {
            this.Key = key;
            this.Message = message;
            this.Time = time;
        }
    }
}

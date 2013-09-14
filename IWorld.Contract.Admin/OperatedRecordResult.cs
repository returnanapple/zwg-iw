using System;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 操作记录
    /// </summary>
    [DataContract]
    public class OperatedRecordResult
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string Username { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        [DataMember]
        public string Operated { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [DataMember]
        public DateTime OperatedTime { get; set; }

        /// <summary>
        /// 实例化一个新的操作记录
        /// </summary>
        /// <param name="record">操作记录的数据封装</param>
        public OperatedRecordResult(OperateRecord record)
        {
            this.Username = record.Owner.Username;
            this.Operated = record.Operated;
            this.OperatedTime = record.CreatedTime;
        }
    }
}

using System;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 公告信息
    /// </summary>
    [DataContract]
    public class BulletinResult
    {
        /// <summary>
        /// 公告的存储指针
        /// </summary>
        [DataMember]
        public int BulletinId { get; set; }

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
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 持续天数
        /// </summary>
        [DataMember]
        public int Days { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 暂停显示
        /// </summary>
        [DataMember]
        public bool Hide { get; set; }

        /// <summary>
        /// 过期自动删除
        /// </summary>
        [DataMember]
        public bool AutoDelete { get; set; }

        /// <summary>
        /// 实例化一个新的公告信息
        /// </summary>
        /// <param name="bulletin">公告信息的数据封装</param>
        public BulletinResult(Bulletin bulletin)
        {
            this.BulletinId = bulletin.Id;
            this.Title = bulletin.Title;
            this.Context = bulletin.Context;
            this.BeginTime = bulletin.BeginTime;
            this.Days = bulletin.Days;
            this.EndTime = bulletin.EndTime;
            this.Hide = bulletin.Hide;
            this.AutoDelete = bulletin.AutoDelete;
        }
    }
}

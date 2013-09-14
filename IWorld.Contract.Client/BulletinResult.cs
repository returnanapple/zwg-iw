using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 公告信息
    /// </summary>
    [DataContract]
    public class BulletinResult
    {
        /// <summary>
        /// 公告信息的数据库存储指针
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
        /// 发布时间
        /// </summary>
        [DataMember]
        public DateTime Time { get; set; }

        /// <summary>
        /// 实例化一个新的公告信息
        /// </summary>
        /// <param name="bulletin">公告信息的数据封装</param>
        public BulletinResult(Bulletin bulletin)
        {
            this.BulletinId = bulletin.Id;
            this.Title = bulletin.Title;
            this.Context = bulletin.Context;
            this.Time = bulletin.BeginTime;
        }
    }
}

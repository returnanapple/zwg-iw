using System;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 登陆记录
    /// </summary>
    [DataContract]
    public class LandingRecordResult
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string Username { get; set; }

        /// <summary>
        /// 网络地址
        /// </summary>
        [DataMember]
        public string Ip { get; set; }

        /// <summary>
        /// 登陆时间
        /// </summary>
        [DataMember]
        public DateTime LandingTime { get; set; }

        /// <summary>
        /// 实例化一个新的登陆记录
        /// </summary>
        /// <param name="landingRecord">登陆记录的数据封装</param>
        public LandingRecordResult(UserLandingRecord landingRecord)
        {
            this.Username = landingRecord.Owner.Username;
            this.Ip = landingRecord.Ip;
            this.LandingTime = landingRecord.CreatedTime;
        }

        /// <summary>
        /// 实例化一个新的登陆记录
        /// </summary>
        /// <param name="landingRecord">登陆记录的数据封装</param>
        public LandingRecordResult(AdministratorLandingRecord landingRecord)
        {
            this.Username = landingRecord.Owner.Username;
            this.Ip = landingRecord.Ip;
            this.LandingTime = landingRecord.CreatedTime;
        }
    }
}

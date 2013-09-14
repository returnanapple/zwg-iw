using System;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 管理员信息
    /// </summary>
    [DataContract]
    public class ManagerInfoResult
    {
        /// <summary>
        /// 用户的存储指针
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string Username { get; set; }

        /// <summary>
        /// 用户组的存储指针
        /// </summary>
        [DataMember]
        public int GroupId { get; set; }

        /// <summary>
        /// 用户组
        /// </summary>
        [DataMember]
        public string Group { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        [DataMember]
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 上次登录的网络地址
        /// </summary>
        [DataMember]
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 实例化一个新的管理员信息
        /// </summary>
        /// <param name="administrator">管理员信息的封装</param>
        public ManagerInfoResult(Administrator administrator)
        {
            this.UserId = administrator.Id;
            this.Username = administrator.Username;
            this.GroupId = administrator.Group.Id;
            this.Group = administrator.Group.Name;
            this.LastLoginTime = administrator.LastLoginTime;
            this.LastLoginIp = administrator.LastLoginIp;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 基础用户信息
    /// </summary>
    [DataContract]
    public class BasicUserInfoResult
    {
        #region 公开属性

        /// <summary>
        /// 该用户的数据库存储指针
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string Username { get; set; }

        /// <summary>
        /// 正常返点数
        /// </summary>
        [DataMember]
        public double NormalReturnPoints { get; set; }

        /// <summary>
        /// 不定位返点数
        /// </summary>
        [DataMember]
        public double UncertainReturnPoints { get; set; }

        /// <summary>
        /// 所属用户组
        /// </summary>
        [DataMember]
        public string Group { get; set; }

        /// <summary>
        /// 帐户余额
        /// </summary>
        [DataMember]
        public double Money { get; set; }

        /// <summary>
        /// 消费量
        /// </summary>
        [DataMember]
        public double Consumption { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        [DataMember]
        public UserStatus Status { get; set; }

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

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的基础用户信息
        /// </summary>
        /// <param name="user">用户信息的数据封装</param>
        public BasicUserInfoResult(Author user)
        {
            this.UserId = user.Id;
            this.Username = user.Username;
            this.Group = user.Group.Name;
            this.NormalReturnPoints = user.NormalReturnPoints;
            this.UncertainReturnPoints = user.UncertainReturnPoints;
            this.Money = user.Money;
            this.Consumption = user.Consumption;
            this.LastLoginTime = user.LastLoginTime;
            this.LastLoginIp = user.LastLoginIp;
            this.Status = user.Status;
        }

        #endregion
    }
}

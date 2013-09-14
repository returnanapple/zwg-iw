using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [DataContract]
    public class UserInfoResult : OperateResult
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
        /// 安全邮箱
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// 绑定安全邮箱
        /// </summary>
        [DataMember]
        public bool BindingEmail { get; set; }

        /// <summary>
        /// 代理（拥有创建下级用户的权限）
        /// </summary>
        [DataMember]
        public bool IsAgents { get; set; }

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
        public UserGroupResult Group { get; set; }

        /// <summary>
        /// 帐户余额
        /// </summary>
        [DataMember]
        public double Money { get; set; }

        /// <summary>
        /// 被冻结的金额（提现中）
        /// </summary>
        [DataMember]
        public double MoneyBeFrozen { get; set; }

        /// <summary>
        /// 消费量
        /// </summary>
        [DataMember]
        public double Consumption { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        [DataMember]
        public double Integral { get; set; }

        /// <summary>
        /// 开户人
        /// </summary>
        [DataMember]
        public string Holder { get; set; }

        /// <summary>
        /// 银行卡
        /// </summary>
        [DataMember]
        public string Card { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        [DataMember]
        public Bank Bank { get; set; }

        /// <summary>
        /// 绑定银行卡
        /// </summary>
        [DataMember]
        public bool BindingCard { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        [DataMember]
        public UserStatus Status { get; set; }

        /// <summary>
        /// 尚未使用的直属下级配额
        /// </summary>
        [DataMember]
        public int Quota { get; set; }

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
        /// 实例化一个新的用户信息（成功）
        /// </summary>
        /// <param name="user">用户信息的数据封装</param>
        public UserInfoResult(Author user)
        {
            this.UserId = user.Id;
            this.Username = user.Username;
            this.Group = new UserGroupResult(user.Group);
            this.Email = user.Email;
            this.BindingEmail = user.BindingEmail;
            this.Card = user.Card;
            this.Holder = user.Holder;
            this.Bank = user.Bank;
            this.BindingCard = user.BindingCard;
            this.NormalReturnPoints = user.NormalReturnPoints;
            this.UncertainReturnPoints = user.UncertainReturnPoints;
            this.Money = user.Money;
            this.MoneyBeFrozen = user.MoneyBeFrozen;
            this.Consumption = user.Consumption;
            this.Integral = user.Integral;
            this.Quota = user.MaxOfSubordinate - user.Subordinate;
            this.LastLoginTime = user.LastLoginTime;
            this.LastLoginIp = user.LastLoginIp;
            this.Status = user.Status;
        }

        /// <summary>
        /// 实例化一个新的用户信息（失败）
        /// </summary>
        /// <param name="error">错误信息</param>
        public UserInfoResult(string error)
            : base(error)
        {
        }

        #endregion
    }
}

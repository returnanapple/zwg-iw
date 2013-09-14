using System;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [DataContract]
    public class UserInfoResult
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
        /// 所属用户组的存储指针
        /// </summary>
        [DataMember]
        public int GroupId { get; set; }

        /// <summary>
        /// 所属用户组的名称
        /// </summary>
        [DataMember]
        public string GroupName { get; set; }

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
        public string Name { get; set; }

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
        /// 最多拥有直属下级数量限制
        /// </summary>
        [DataMember]
        public int MaxOfSubordinate { get; set; }

        /// <summary>
        /// 当前拥有直属下级数量
        /// </summary>
        [DataMember]
        public int Subordinate { get; set; }

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
        /// 实例化一个新的用户信息
        /// </summary>
        /// <param name="user">用户信息的数据封装</param>
        public UserInfoResult(Author user)
        {
            this.UserId = user.Id;
            this.Username = user.Username;
            this.Email = user.Email;
            this.BindingEmail = user.BindingEmail;
            this.IsAgents = user.IsAgents;
            this.NormalReturnPoints = user.NormalReturnPoints;
            this.UncertainReturnPoints = user.UncertainReturnPoints;
            this.GroupId = user.Group.Id;
            this.GroupName = user.Group.Name;
            this.Money = user.Money;
            this.MoneyBeFrozen = user.MoneyBeFrozen;
            this.Consumption = user.Consumption;
            this.Integral = user.Integral;
            this.Name = user.Holder;
            this.Card = user.Card;
            this.Bank = user.Bank;
            this.BindingCard = user.BindingCard;
            this.Status = user.Status;
            this.MaxOfSubordinate = user.MaxOfSubordinate;
            this.Subordinate = user.Subordinate;
            this.LastLoginTime = user.LastLoginTime;
            this.LastLoginIp = user.LastLoginIp;
        }
    }
}

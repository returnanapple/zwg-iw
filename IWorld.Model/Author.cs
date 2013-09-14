using System;

namespace IWorld.Model
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class Author : CategoryBase
    {
        #region 公开属性

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 安全码
        /// </summary>
        public string SafeCode { get; set; }

        /// <summary>
        /// 安全邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 绑定安全邮箱
        /// </summary>
        public bool BindingEmail { get; set; }

        /// <summary>
        /// 代理（拥有创建下级用户的权限）
        /// </summary>
        public bool IsAgents { get; set; }

        /// <summary>
        /// 正常返点数
        /// </summary>
        public double NormalReturnPoints { get; set; }

        /// <summary>
        /// 不定位返点数
        /// </summary>
        public double UncertainReturnPoints { get; set; }

        /// <summary>
        /// 所属用户组
        /// </summary>
        public virtual UserGroup Group { get; set; }

        /// <summary>
        /// 帐户余额
        /// </summary>
        public double Money { get; set; }

        /// <summary>
        /// 被冻结的金额（提现中）
        /// </summary>
        public double MoneyBeFrozen { get; set; }

        /// <summary>
        /// 消费量
        /// </summary>
        public double Consumption { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public double Integral { get; set; }

        /// <summary>
        /// 开户人
        /// </summary>
        public string Holder { get; set; }

        /// <summary>
        /// 银行卡
        /// </summary>
        public string Card { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        public Bank Bank { get; set; }

        /// <summary>
        /// 绑定银行卡
        /// </summary>
        public bool BindingCard { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserStatus Status { get; set; }

        /// <summary>
        /// 最多拥有直属下级数量限制
        /// </summary>
        public int MaxOfSubordinate { get; set; }

        /// <summary>
        /// 当前拥有直属下级数量
        /// </summary>
        public int Subordinate { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 上次登录的网络地址
        /// </summary>
        public string LastLoginIp { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的用户信息
        /// </summary>
        public Author()
        {
        }

        /// <summary>
        /// 实例化一个新的用户信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="isAgents">是否代理用户</param>
        /// <param name="normalReturnPoints">正常返点数</param>
        /// <param name="uncertainReturnPoints">不定位返点数</param>
        /// <param name="group">所属用户组</param>
        /// <param name="maxOfSubordinate">最多拥有直属下级数量限制</param>
        public Author(string username, string password, bool isAgents, double normalReturnPoints, double uncertainReturnPoints
            , UserGroup group, int maxOfSubordinate)
        {
            this.Username = username;
            this.Password = password;
            this.SafeCode = password;
            this.Email = "";
            this.BindingEmail = false;
            this.IsAgents = isAgents;
            this.NormalReturnPoints = normalReturnPoints;
            this.UncertainReturnPoints = uncertainReturnPoints;
            this.Group = group;
            this.Money = 0;
            this.MoneyBeFrozen = 0;
            this.Consumption = 0;
            this.Integral = 0;
            this.Card = "";
            this.Bank = Model.Bank.无;
            this.BindingCard = false;
            this.Holder = "";
            this.LeftKey = 1;
            this.RightKey = 2;
            this.Layer = 1;
            this.Status = UserStatus.未激活;
            this.MaxOfSubordinate = maxOfSubordinate;
            this.Subordinate = 0;
            this.LastLoginTime = DateTime.Now;
            this.LastLoginIp = "127.0.0.1";
        }

        #endregion
    }
}

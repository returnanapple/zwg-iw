
namespace IWorld.Model
{
    /// <summary>
    /// 系统邮件账户
    /// </summary>
    public class EmailAccount : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 索引字
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 应该使用的服务地址
        /// </summary>
        public virtual EmailClient Client { get; set; }

        /// <summary>
        /// 一个布尔值 标识该对象是否为默认展示对象
        /// </summary>
        public bool IsDefault { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的系统邮件账户
        /// </summary>
        public EmailAccount()
        {
        }

        /// <summary>
        /// 实例化一个新的系统邮件账户
        /// </summary>
        /// <param name="key">索引字</param>
        /// <param name="account">帐号</param>
        /// <param name="password">密码</param>
        /// <param name="remark">备注</param>
        /// <param name="client">应该使用的服务地址</param>
        /// <param name="isDefault">一个布尔值 标识该对象是否为默认展示对象</param>
        public EmailAccount(string key, string account, string password, string remark, EmailClient client, bool isDefault)
        {
            this.Key = key;
            this.Account = account;
            this.Password = password;
            this.Remark = remark;
            this.Client = client;
            this.IsDefault = isDefault;
        }

        #endregion
    }
}


namespace IWorld.Model
{
    /// <summary>
    /// 系统邮件服务地址
    /// </summary>
    public class EmailClient : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 索引字
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 服务器地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 一个布尔值 标识该对象是否为默认展示对象
        /// </summary>
        public bool IsDefault { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的系统邮件服务地址
        /// </summary>
        public EmailClient()
        {
        }

        /// <summary>
        /// 实例化一个新的系统邮件服务地址
        /// </summary>
        /// <param name="key">索引字</param>
        /// <param name="host">服务器地址</param>
        /// <param name="port">端口</param>
        /// <param name="remark">备注</param>
        /// <param name="isDefault">一个布尔值 标识该对象是否为默认展示对象</param>
        public EmailClient(string key, string host, int port, string remark, bool isDefault)
        {
            this.Key = key;
            this.Host = host;
            this.Port = port;
            this.Remark = remark;
            this.IsDefault = isDefault;
        }

        #endregion
    }
}

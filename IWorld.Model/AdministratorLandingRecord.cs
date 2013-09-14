
namespace IWorld.Model
{
    /// <summary>
    /// 管理员登录记录
    /// </summary>
    public class AdministratorLandingRecord : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 用户
        /// </summary>
        public virtual Administrator Owner { get; set; }

        /// <summary>
        /// 网络地址
        /// </summary>
        public string Ip { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的管理员登录记录
        /// </summary>
        public AdministratorLandingRecord()
        {
        }

        /// <summary>
        /// 实例化一个新的管理员登录记录
        /// </summary>
        /// <param name="owner">用户</param>
        /// <param name="ip">网络地址</param>
        public AdministratorLandingRecord(Administrator owner, string ip)
        {
            this.Owner = owner;
            this.Ip = ip;
        }

        #endregion
    }
}


namespace IWorld.Model
{
    /// <summary>
    /// 用户登陆记录
    /// </summary>
    public class UserLandingRecord : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 用户
        /// </summary>
        public virtual Author Owner { get; set; }

        /// <summary>
        /// 网络地址
        /// </summary>
        public string Ip { get; set; }
        
        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的用户登陆记录
        /// </summary>
        public UserLandingRecord()
        {
        }

        /// <summary>
        /// 实例化一个新的用户登陆记录
        /// </summary>
        /// <param name="owner">用户</param>
        /// <param name="ip">网络地址</param>
        public UserLandingRecord(Author owner, string ip)
        {
            this.Owner = owner;
            this.Ip = ip;
        }
        
        #endregion
    }
}

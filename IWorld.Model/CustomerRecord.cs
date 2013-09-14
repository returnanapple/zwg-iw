
namespace IWorld.Model
{
    /// <summary>
    /// 客户服务记录
    /// </summary>
    public class CustomerRecord : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 目标用户
        /// </summary>
        public virtual Author User { get; set; }

        /// <summary>
        /// 客服类型
        /// </summary>
        public CustomerType Type { get; set; }

        /// <summary>
        /// 标识 | 是否系统客服响应
        /// </summary>
        public bool IsService { get; set; }

        /// <summary>
        /// 聊天内容
        /// </summary>
        public string Message { get; set; }
        
        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的客户服务记录
        /// </summary>
        public CustomerRecord()
        {
        }

        /// <summary>
        /// 实例化一个新的客户服务记录
        /// </summary>
        /// <param name="user">目标用户</param>
        /// <param name="type">客服类型</param>
        /// <param name="isService">标识 | 是否系统客服响应</param>
        /// <param name="message">聊天内容</param>
        public CustomerRecord(Author user, CustomerType type, bool isService, string message)
        {
            this.User = user;
            this.Type = type;
            this.IsService = isService;
            this.Message = message;
        }
        
        #endregion
    }
}

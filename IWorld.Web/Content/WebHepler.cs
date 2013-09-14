using System.ServiceModel;
using System.ServiceModel.Channels;
using IWorld.BLL;

namespace IWorld.Web
{
    /// <summary>
    /// 站点信息的帮助着对象
    /// </summary>
    public class WebHepler
    {
        /// <summary>
        /// 获取客户端通信信息
        /// </summary>
        /// <returns>返回客户端通信信息的封装</returns>
        public static RemoteEndpointMessageProperty GetEndpoint()
        {
            //提供方法执行的上下文环境
            OperationContext context = OperationContext.Current;
            //获取传进的消息属性
            MessageProperties properties = context.IncomingMessageProperties;
            //获取消息发送的远程终结点IP和端口
            RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            return endpoint;
        }

        #region 用户

        /// <summary>
        /// 声明用户登陆
        /// </summary>
        /// <param name="userId">用户的存储指针</param>
        /// <returns>返回标识码</returns>
        public static string SetUserIn(int userId)
        {
            return CacheManager.SetUserIn(userId);
        }

        /// <summary>
        /// 声明用户退出登陆
        /// </summary>
        /// <param name="token">标识码</param>
        /// <returns>返回一个布尔值，表示该用户是否已经声明他登陆在系统中</returns>
        public static bool SetUserOut(string token)
        {
            return CacheManager.SetUserOut(token);
        }

        /// <summary>
        /// 获取登陆用户的存储指针
        /// </summary>
        /// <param name="token">标识码</param>
        /// <returns><返回用户的存储指针/returns>
        public static int GetUserId(string token)
        {
            return CacheManager.GetUserId(token);
        }
        
        #endregion

        #region 管理员

        /// <summary>
        /// 声明管理员登陆
        /// </summary>
        /// <param name="aministratorId">管理员的存储指针</param>
        /// <returns>返回标识码</returns>
        public static string SetAdministratorIn(int aministratorId)
        {
            return CacheManager.SetAdministratorIn(aministratorId);
        }

        /// <summary>
        /// 声明管理员退出登陆
        /// </summary>
        /// <param name="token">标识码</param>
        /// <returns>返回一个布尔值，表示该管理员是否已经声明他登陆在系统中</returns>
        public static bool SetAdministratorOut(string token)
        {
            return CacheManager.SetAdministratorOut(token);
        }

        /// <summary>
        /// 获取登陆管理员的存储指针
        /// </summary>
        /// <param name="token">标识码</param>
        /// <returns><返回管理员的存储指针/returns>
        public static int GetAdministratorId(string token)
        {
            return CacheManager.GetAdministratorId(token);
        }

        #endregion
    }
}
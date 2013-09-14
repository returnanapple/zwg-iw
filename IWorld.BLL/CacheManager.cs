using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using IWorld.Setting;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 缓存数据的管理者对象
    /// </summary>
    public class CacheManager
    {
        #region 用户信息缓存

        /// <summary>
        /// 用户信息的缓存池
        /// </summary>
        private static MemoryCache userCache = new MemoryCache("user");

        /// <summary>
        /// 用户信息的实体缓存区
        /// </summary>
        private static Dictionary<string, int> userCacheEntity = new Dictionary<string, int>();

        /// <summary>
        /// 声明用户登陆
        /// </summary>
        /// <param name="userId">用户的存储指针</param>
        /// <returns>返回标识码</returns>
        public static string SetUserIn(int userId)
        {
            #region 移除其余的同帐号登录
            if (userCacheEntity.Values.Any(x => x == userId))
            {
                List<string> _tokens = userCacheEntity.Where(x => x.Value == userId).Select(x => x.Key).ToList();
                _tokens.ForEach(x =>
                {
                    userCache.Remove(x);
                });
            }

            #endregion

            string token = Guid.NewGuid().ToString("N");
            WebSetting webSetting = new WebSetting();
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.SlidingExpiration = new TimeSpan(0, webSetting.UserInTime, 0);
            policy.Priority = CacheItemPriority.NotRemovable;
            policy.RemovedCallback = (arguments) =>
            {
                if (userCacheEntity.Keys.Any(x => x == arguments.CacheItem.Key))
                {
                    userCacheEntity.Remove(arguments.CacheItem.Key);
                }
            };
            userCache.Add(token, "", policy);
            userCacheEntity.Add(token, userId);

            return token;
        }

        /// <summary>
        /// 声明用户退出登陆
        /// </summary>
        /// <param name="token">标识码</param>
        /// <returns>返回一个布尔值，表示该用户是否已经声明他登陆在系统中</returns>
        public static bool SetUserOut(string token)
        {
            bool hadThisMan = userCache.Any(x => x.Key == token);
            if (!hadThisMan)
            {
                return false;
            }
            userCache.Remove(token);
            return true;
        }

        /// <summary>
        /// 获取登陆用户的存储指针
        /// </summary>
        /// <param name="token">标识码</param>
        /// <returns><返回用户的存储指针/returns>
        public static int GetUserId(string token)
        {
            bool hadThisMan = userCache.Any(x => x.Key == token);
            if (!hadThisMan)
            {
                return -1;
            }
            return userCacheEntity[token];
        }

        #endregion

        #region 管理员信息缓存

        /// <summary>
        /// 管理员信息的缓存池
        /// </summary>
        private static MemoryCache administratorCache = new MemoryCache("administrator");

        /// <summary>
        /// 管理员信息的实体缓存区
        /// </summary>
        private static Dictionary<string, int> administratorCacheEntity = new Dictionary<string, int>();

        /// <summary>
        /// 声明管理员登陆
        /// </summary>
        /// <param name="administratorId">管理员的存储指针</param>
        /// <returns>返回标识码</returns>
        public static string SetAdministratorIn(int administratorId)
        {
            #region 移除其余的同帐号登录
            if (administratorCacheEntity.Values.Any(x => x == administratorId))
            {
                List<string> _tokens = administratorCacheEntity.Where(x => x.Value == administratorId).Select(x => x.Key).ToList();
                _tokens.ForEach(x =>
                    {
                        administratorCache.Remove(x);
                    });
            }

            #endregion

            string token = Guid.NewGuid().ToString("N");
            WebSetting webSetting = new WebSetting();
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.SlidingExpiration = new TimeSpan(0, webSetting.UserInTime, 0);
            policy.Priority = CacheItemPriority.NotRemovable;
            policy.RemovedCallback = (arguments) =>
                {
                    if (administratorCacheEntity.Keys.Any(x => x == arguments.CacheItem.Key))
                    {
                        administratorCacheEntity.Remove(arguments.CacheItem.Key);
                    }
                };
            administratorCache.Add(token, "", policy);
            administratorCacheEntity.Add(token, administratorId);

            return token;
        }

        /// <summary>
        /// 声明管理员退出登陆
        /// </summary>
        /// <param name="token">标识码</param>
        /// <returns>返回一个布尔值，表示该管理员是否已经声明他登陆在系统中</returns>
        public static bool SetAdministratorOut(string token)
        {
            bool hadThisMan = administratorCache.Any(x => x.Key == token);
            if (!hadThisMan)
            {
                return false;
            }
            administratorCache.Remove(token);
            return true;
        }

        /// <summary>
        /// 获取登陆管理员的存储指针
        /// </summary>
        /// <param name="token">标识码</param>
        /// <returns><返回管理员的存储指针/returns>
        public static int GetAdministratorId(string token)
        {
            bool hadThisMan = administratorCache.Any(x => x.Key == token);
            if (!hadThisMan)
            {
                return -1;
            }
            return administratorCacheEntity[token];
        }

        #endregion

        #region 采集结果信息缓存

        /// <summary>
        /// 采集结果信息的缓存池
        /// </summary>
        private static MemoryCache collectionReslutCache = new MemoryCache("collectionReslut");

        /// <summary>
        /// 采集结果信息的实体缓存区
        /// </summary>
        private static Dictionary<string, CollectionResult> collectionReslutCacheEntity = new Dictionary<string, CollectionResult>();

        /// <summary>
        /// 将新的采集结果信息放入缓存池
        /// </summary>
        /// <param name="message">采集结果</param>
        public static void SetCollectionResultIn(string message)
        {
            string key = Guid.NewGuid().ToString("N");
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(10));
            policy.RemovedCallback = (arguments) =>
                {
                    if (collectionReslutCacheEntity.Keys.Any(x => x == arguments.CacheItem.Key))
                    {
                        collectionReslutCacheEntity.Remove(arguments.CacheItem.Key);
                    }
                };
            CollectionResult collectionResult = new CollectionResult(key, message);
            collectionReslutCache.Add(key, "", policy);
            collectionReslutCacheEntity.Add(key, collectionResult);
        }

        /// <summary>
        /// 获取采集结果信息的列表
        /// </summary>
        /// <param name="key">当前已读的信息的键</param>
        /// <returns>返回采集结果信息的列表</returns>
        public static List<ICollectionResult> GetCollectionResults(string key)
        {
            List<ICollectionResult> result = new List<ICollectionResult>();
            DateTime _time = collectionReslutCacheEntity.Any(x => x.Key == key)
                ? collectionReslutCacheEntity[key].Time : DateTime.Now.AddDays(-1);
            collectionReslutCacheEntity
                .Where(x => x.Value.Time > _time)
                .OrderBy(x => x.Value)
                .ToList()
                .ForEach(x =>
                    {
                        result.Add(x.Value);
                    });
            return result;
        }

        #endregion

        #region 客户服务信息缓存

        /// <summary>
        /// 客户服务信息的缓存池
        /// </summary>
        private static MemoryCache customerMessageCache = new MemoryCache("customerMessage");

        /// <summary>
        /// 客户服务信息的实体缓存区
        /// </summary>
        private static Dictionary<string, CustomerMessage> customerMessageCacheEntity
            = new Dictionary<string, CustomerMessage>();

        /// <summary>
        /// 将新的客户服务信息放入缓存池
        /// </summary>
        /// <param name="message">采集结果</param>
        public static void SetCustomerMessageIn(CustomerRecord recored)
        {
            string key = Guid.NewGuid().ToString("N");
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(10));
            policy.RemovedCallback = (arguments) =>
            {
                if (customerMessageCacheEntity.Keys.Any(x => x == arguments.CacheItem.Key))
                {
                    customerMessageCacheEntity.Remove(arguments.CacheItem.Key);
                }
            };
            CustomerMessage customerMessage = new CustomerMessage(key, recored);
            customerMessageCache.Add(key, "", policy);
            customerMessageCacheEntity.Add(key, customerMessage);
        }

        /// <summary>
        /// 获取客户服务信息的列表
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="type">客服类型</param>
        /// <param name="key">当前已读的信息的键</param>
        /// <returns>返回采集结果信息的列表</returns>
        public static List<ICustomerMessage> GetCustomerMessages(int userId, CustomerType type, string key)
        {
            List<ICustomerMessage> result = new List<ICustomerMessage>();
            DateTime _time = customerMessageCacheEntity.Any(x => x.Key == key)
                ? customerMessageCacheEntity[key].Time : DateTime.Now.AddDays(-1);
            customerMessageCacheEntity
                .Where(x => x.Value.Time > _time
                    && x.Value.UserId == userId
                    && x.Value.Type == type)
                .OrderBy(x => x.Value)
                .ToList()
                .ForEach(x =>
                {
                    result.Add(x.Value);
                });
            return result;
        }

        #region 附加方法

        /// <summary>
        /// 将新的客户服务信息放入缓存池
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void SetCustomerMessageIn(object sender, NEventArgs e)
        {
            CustomerRecord record = (CustomerRecord)e.State;
            SetCustomerMessageIn(record);
        }

        #endregion

        #endregion

        #region 内嵌接口

        /// <summary>
        /// 定义采集结果
        /// </summary>
        public interface ICollectionResult
        {
            /// <summary>
            /// 获取键
            /// </summary>
            /// <returns>返回键</returns>
            string GetKey();

            /// <summary>
            /// 获取采集结果信息
            /// </summary>
            /// <returns>返回采集结果信息</returns>
            string GetMessage();

            /// <summary>
            /// 获取采集时间
            /// </summary>
            /// <returns>返回采集时间</returns>
            DateTime GetTime();
        }

        /// <summary>
        /// 定义客服聊天记录 
        /// </summary>
        public interface ICustomerMessage
        {
            /// <summary>
            /// 获取键
            /// </summary>
            /// <returns>返回键</returns>
            string GetKey();

            /// <summary>
            /// 获取目标用户的存储指针
            /// </summary>
            /// <returns>返回目标用户的存储指针</returns>
            int GetUserId();

            /// <summary>
            /// 获取目标用户的用户名
            /// </summary>
            /// <returns>返回目标用户的用户名</returns>
            string GetUsername();

            /// <summary>
            /// 获取聊天信息内容
            /// </summary>
            /// <returns>返回聊天信息内容</returns>
            string GetMessage();

            /// <summary>
            /// 获取时间
            /// </summary>
            /// <returns>返回时间</returns>
            DateTime GetTime();

            /// <summary>
            /// 获取是否客服回应的标识
            /// </summary>
            /// <returns>返回是否客服回应的标识</returns>
            bool GetIsS();

            /// <summary>
            /// 获取客服类型
            /// </summary>
            /// <returns>返回客服类型</returns>
            CustomerType GetCustomerType();
        }

        #endregion

        #region 内嵌类型

        /// <summary>
        /// 采集结果
        /// </summary>
        private class CollectionResult : ICollectionResult
        {
            #region 公开属性

            /// <summary>
            /// 键
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// 信息
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 时间
            /// </summary>
            public DateTime Time { get; set; }

            #endregion

            #region 构造方法

            /// <summary>
            /// 实例化一个新的采集结果
            /// </summary>
            /// <param name="key">键</param>
            /// <param name="message">信息</param>
            public CollectionResult(string key, string message)
            {
                this.Key = key;
                this.Message = message;
                this.Time = DateTime.Now;
            }

            #endregion

            #region 实例方法

            /// <summary>
            /// 获取键
            /// </summary>
            /// <returns>返回键</returns>
            public string GetKey()
            {
                return this.Key;
            }

            /// <summary>
            /// 获取采集结果信息
            /// </summary>
            /// <returns>返回采集结果信息</returns>
            public string GetMessage()
            {
                return this.Message;
            }

            /// <summary>
            /// 获取采集时间
            /// </summary>
            /// <returns>返回采集时间</returns>
            public DateTime GetTime()
            {
                return Time;
            }

            #endregion
        }

        /// <summary>
        /// 客服聊天记录 
        /// </summary>
        private class CustomerMessage : ICustomerMessage
        {
            #region 公开属性

            /// <summary>
            /// 键
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// 目标用户的存储指针
            /// </summary>
            public int UserId { get; set; }

            /// <summary>
            /// 目标用户的用户名
            /// </summary>
            public string Username { get; set; }

            /// <summary>
            /// 聊天信息内容
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 时间
            /// </summary>
            public DateTime Time { get; set; }

            /// <summary>
            /// 是否客服回应的标识
            /// </summary>
            public bool IsService { get; set; }

            /// <summary>
            /// 客服类型
            /// </summary>
            public CustomerType Type { get; set; }

            #endregion

            #region 构造方法

            /// <summary>
            /// 实例化一个新的客服聊天记录
            /// </summary>
            /// <param name="key">键</param>
            /// <param name="record">客服聊天记录的数据封装</param>
            public CustomerMessage(string key, CustomerRecord record)
            {
                this.Key = key;
                this.UserId = record.User.Id;
                this.Username = record.User.Username;
                this.Message = record.Message;
                this.Time = record.CreatedTime;
                this.IsService = record.IsService;
                this.Type = record.Type;
            }

            #endregion

            #region 实例方法

            /// <summary>
            /// 获取键
            /// </summary>
            /// <returns>返回键</returns>
            public string GetKey()
            {
                return this.Key;
            }

            /// <summary>
            /// 获取目标用户的存储指针
            /// </summary>
            /// <returns>返回目标用户的存储指针</returns>
            public int GetUserId()
            {
                return this.UserId;
            }

            /// <summary>
            /// 获取目标用户的用户名
            /// </summary>
            /// <returns>返回目标用户的用户名</returns>
            public string GetUsername()
            {
                return this.Username;
            }

            /// <summary>
            /// 获取聊天信息内容
            /// </summary>
            /// <returns>返回聊天信息内容</returns>
            public string GetMessage()
            {
                return this.Message;
            }

            /// <summary>
            /// 获取时间
            /// </summary>
            /// <returns>返回时间</returns>
            public DateTime GetTime()
            {
                return this.Time;
            }

            /// <summary>
            /// 获取是否客服回应的标识
            /// </summary>
            /// <returns>返回是否客服回应的标识</returns>
            public bool GetIsS()
            {
                return this.IsService;
            }

            /// <summary>
            /// 获取客服类型
            /// </summary>
            /// <returns>返回客服类型</returns>
            public CustomerType GetCustomerType()
            {
                return this.Type;
            }

            #endregion
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 清空缓存池
        /// </summary>
        public static void ClearCache()
        {
            userCache = new MemoryCache("user");
            userCacheEntity = new Dictionary<string, int>();

            collectionReslutCache = new MemoryCache("collectionReslut");
            collectionReslutCacheEntity = new Dictionary<string, CollectionResult>();

            customerMessageCache = new MemoryCache("customerMessage");
            customerMessageCacheEntity = new Dictionary<string, CustomerMessage>();
        }

        #endregion
    }
}

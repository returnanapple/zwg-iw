using System;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 站内信的管理者对象
    /// </summary>
    public class MessageManager : SimplifyManagerBase<Message>, IManager<Message>, ISimplify<Message>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的站内信的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public MessageManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 声明目标站内信已经被目标用户所阅读
        /// </summary>
        /// <param name="messageId">目标站内信的存储指针</param>
        /// <param name="userId">目标用户的存储指针</param>
        public void Read(int messageId, int userId)
        {
            NChecker.CheckEntity<Message>(messageId, "站内信", db);
            NChecker.CheckEntity<Author>(userId, "用户", db);
            string token = string.Format("[{0}]", userId);
            Message message = db.Set<Message>().Find(messageId);
            if (message.Readed.Contains(token))
            {
                throw new Exception("该用户已经阅读过这个站内信");
            }

            message.Readed += message.Readed == "" ? token : "," + token;
            message.ModifiedTime = DateTime.Now;
            db.SaveChanges();
        }

        /// <summary>
        /// 声明目标站内信已经被目标用户所删除
        /// </summary>
        /// <param name="messageId">目标站内信的存储指针</param>
        /// <param name="userId">目标用户的存储指针</param>
        public void Delete(int messageId, int userId)
        {
            NChecker.CheckEntity<Message>(messageId, "站内信", db);
            NChecker.CheckEntity<Author>(userId, "用户", db);
            string token = string.Format("[{0}]", userId);
            Message message = db.Set<Message>().Find(messageId);
            if (message.Deleted.Contains(token))
            {
                throw new Exception("该用户已经删除这个站内信");
            }

            message.Deleted += message.Deleted == "" ? token : "," + token;
            message.ModifiedTime = DateTime.Now;
            db.SaveChanges();
        }

        #endregion

        #region 内嵌类型

        /// <summary>
        /// 内部工厂
        /// </summary>
        public class Fanctory
        {
            #region 静态方法

            /// <summary>
            /// 创建一个用于新建通知的数据集
            /// </summary>
            /// <param name="fromId">发件人的存储指针</param>
            /// <param name="toId">收件人的存储指针</param>
            /// <param name="title">标题</param>
            /// <param name="context">正文</param>
            /// <returns>返回用于新建通知的数据集</returns>
            public static ICreatePackage<Message> CreatePackageForCreate(int fromId, int toId, string title, string context)
            {
                return new PackageForCreate(fromId, toId, title, context);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建站内信的数据集
            /// </summary>
            private class PackageForCreate : IPackage<Message>, ICreatePackage<Message>
            {
                #region 公开属性

                /// <summary>
                /// 发件人的存储指针
                /// </summary>
                public int FromId { get; set; }

                /// <summary>
                /// 收件人的存储指针
                /// </summary>
                public int ToId { get; set; }

                /// <summary>
                /// 标题
                /// </summary>
                public string Title { get; set; }

                /// <summary>
                /// 正文
                /// </summary>
                public string Context { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建站内信的数据集
                /// </summary>
                /// <param name="fromId">发件人的存储指针</param>
                /// <param name="toId">收件人的存储指针</param>
                /// <param name="title">标题</param>
                /// <param name="context">正文</param>
                public PackageForCreate(int fromId, int toId, string title, string context)
                {
                    this.FromId = fromId;
                    this.ToId = toId;
                    this.Title = title;
                    this.Context = context;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    if (this.FromId != 0)
                    {
                        NChecker.CheckEntity<Author>(this.FromId, "用户", db);
                    }
                    if (this.ToId != 0)
                    {
                        NChecker.CheckEntity<Author>(this.ToId, "用户", db);
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public Message GetEntity(DbContext db)
                {
                    Author _from = this.FromId == 0 ? null : db.Set<Author>().Find(this.FromId);
                    Author to = this.ToId == 0 ? null : db.Set<Author>().Find(this.ToId);

                    return new Message(_from, to, this.Title, this.Context);
                }

                #endregion
            }

            #endregion
        }

        #endregion
    }
}

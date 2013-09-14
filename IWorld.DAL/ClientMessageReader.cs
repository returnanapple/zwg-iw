using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Contract.Client;
using IWorld.Helper;
using IWorld.Setting;

namespace IWorld.DAL
{
    /// <summary>
    /// 站内短消息的阅读者对象（前台）
    /// </summary>
    public class ClientMessageReader : ReaderBase
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的站内短消息的阅读者对象（前台）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public ClientMessageReader(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 读取站内短消息列表
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="page">页码</param>
        /// <returns>返回站内短消息的分页列表</returns>
        public PaginationList<MessageResult> ReadMessages(int userId, int page)
        {
            string token = string.Format("[{0}]", userId);
            Expression<Func<Message, bool>> predicate = x => x.To.Id == userId && !x.Deleted.Contains(token);

            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForClient);
            var mSet = db.Set<Message>();

            int tCount = mSet
                .Where(predicate)
                .Count();
            List<MessageResult> tList = mSet
                .Where(predicate)
                .OrderByDescending(x => x.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForClient)
                .ToList()
                .ConvertAll(x => new MessageResult(x));

            return new PaginationList<MessageResult>(page, webSetting.PageSizeForClient, tCount, tList);
        }

        #endregion
    }
}
